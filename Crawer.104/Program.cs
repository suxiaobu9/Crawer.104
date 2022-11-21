using Crawer._104;
using JD._104.Model.Models.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Web;

var now = DateTime.UtcNow.AddHours(8);

var serviceCollection = new ServiceCollection();

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(string.Format(config.GetSection("SerilogPath").Value, now.ToString("yyyyMMdd")))
    .CreateLogger();

var options = new JsonSerializerOptions
{
    PropertyNamingPolicy = null,
    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs)
};

serviceCollection.AddDbContext<Crawer104Context>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

var httpClient = new HttpClient { BaseAddress = new Uri("https://www.104.com.tw") };

httpClient.DefaultRequestHeaders.Add("Referer", httpClient.BaseAddress.AbsoluteUri);

var services = serviceCollection.BuildServiceProvider();

var db = services.GetRequiredService<Crawer104Context>();



var compNoHash = new HashSet<string>();
var jobNoHash = new HashSet<string>();

var compDic = new Dictionary<string, Company>();
var jobDic = new Dictionary<string, JobDescription>();

var searchKey = new string[] { "C%23", ".net" };
var jobExps = new string[] { "1", "3", "5", "10" };

var sw = new Stopwatch();

Log.Information("Process Start !");

sw.Start();

// 取得所有條件的總頁數
var getTotalPages = await Task.WhenAll(
searchKey.SelectMany(key =>
{
    return jobExps.Select(jobExp =>
     {
         var jobListParams = GetJobListParams();

         jobListParams["keyword"] = key;
         jobListParams["jobexp"] = jobExp;
         jobListParams["page"] = "1";

         var queryString = string.Join("&", jobListParams.Select(x => x.Key + "=" + x.Value));

         var url = $"/jobs/search/list?{queryString}";

         return httpClient.GetFromJsonAsync<JobList>(url);
     });
}).ToArray());

if (getTotalPages == null)
    return;

// 取得所有條件的所有列表資料
var jobTasks = getTotalPages
.Where(x => x != null && x.data != null)
.SelectMany(jobList =>
{
    var jobListParams = GetJobListParams();
    jobListParams["keyword"] = HttpUtility.UrlEncode(jobList?.data?.query?.keyword?.ToString() ?? "");
    jobListParams["jobexp"] = jobList?.data?.query?.jobexp?.ToString() ?? "";

    return Enumerable.Range(1, jobList?.data?.totalPage ?? 1).Select(page =>
    {
        jobListParams["page"] = page.ToString();

        var queryString = string.Join("&", jobListParams.Select(x => x.Key + "=" + x.Value));

        var url = $"/jobs/search/list?{queryString}";

        return httpClient.GetFromJsonAsync<JobList>(url);
    });
}).ToArray();

// 取得所有公司資料
foreach (var jobTask in jobTasks)
{
    var jobList = await jobTask;

    if (jobList == null ||
        jobList.data == null ||
        jobList.data.list == null)
        continue;

    foreach (var job in jobList.data.list)
    {
        if (string.IsNullOrWhiteSpace(job.custNo) ||
            string.IsNullOrWhiteSpace(job.jobNo))
            continue;

        var addCompSuccess = compNoHash.Add(job.custNo);

        if (addCompSuccess)
        {
            var compDetailUrl = job.link?.cust;
            compDetailUrl = !string.IsNullOrWhiteSpace(compDetailUrl) && compDetailUrl.Contains('?') ?
                            compDetailUrl[..compDetailUrl.IndexOf("?")] : "";

            compDic.Add(job.custNo, new Company
            {
                CompanyName = job.custName,
                CompanyUrl = compDetailUrl,
                CompanyNo = job.custNo,
            });
        }

        var addJobSuccess = jobNoHash.Add(job.jobNo);

        if (!addJobSuccess)
            continue;

        var detailLink = job.link?.job;

        detailLink = !string.IsNullOrWhiteSpace(detailLink) && detailLink.Contains('?') ?
                        detailLink[..detailLink.IndexOf("?")] : "";

        var transDateSuccess = DateTime.TryParseExact(job.appearDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var 出現日期);

        jobDic.Add(job.jobNo, new JobDescription
        {
            AppearDate = transDateSuccess ? 出現日期 : null,
            JobName = job.jobName,
            JobPlace = (job.jobAddrNoDesc ?? "") + job.jobAddress ?? "",
            JobNo = job.jobNo,
            Seniority = job.periodDesc,
            CreatedDate = now,
            MinimunSalary = job.salaryLow?.TrimStart('0'),
            HighestSalary = job.salaryHigh?.TrimStart('0'),
            SalaryDescription = job.salaryDesc,
            Tags = job.tags == null ? null : JsonSerializer.Serialize(job.tags, options: options),
            DetailUrl = detailLink,
            IsDeleted = false,
        });
    }
}

Log.Information($"Comp Count : {compDic.Count}");

// 處理公司資料進資料庫
foreach (var comp in compDic)
{
    var dbCompData = await db.Companies.FirstOrDefaultAsync(x => x.CompanyNo == comp.Key);

    Log.Information($"{comp.Value.CompanyName}");

    if (dbCompData == null)
    {
        await db.Companies.AddAsync(comp.Value);
        continue;
    }

    dbCompData.CompanyName = comp.Value.CompanyName;
    dbCompData.CompanyUrl = comp.Value.CompanyUrl;
    dbCompData.CompanyNo = comp.Value.CompanyNo;
    comp.Value.Id = dbCompData.Id;
}

await db.SaveChangesAsync();

Log.Information($"Job Count : {jobDic.Count}");
var jobCounter = 1;
// 取得所有職缺詳細資料，並且進資料庫
foreach (var job in jobDic)
{
    if (string.IsNullOrWhiteSpace(job.Value.DetailUrl))
        continue;

    var startIdx = (job.Value.DetailUrl.IndexOf("tw/job") + "tw/job".Length) + 1;

    var jobDetailCode = job.Value.DetailUrl[startIdx..];

    var jobDetail = await httpClient.GetFromJsonAsync<JobDetail>($"/job/ajax/content/{jobDetailCode}");

    if (jobDetail == null ||
        jobDetail.data == null ||
        jobDetail.data.jobDetail == null)
        continue;

    var compData = compDic.FirstOrDefault(x => x.Key == jobDetail.data.custNo).Value;

    job.Value.CompanyId = compData.Id;

    job.Value.WorkingHour = jobDetail.data.jobDetail.workPeriod;
    job.Value.WorkContent = jobDetail.data.jobDetail.jobDescription;
    job.Value.Request = jobDetail.data.condition?.other;
    job.Value.RemoteWork = jobDetail.data.jobDetail.remoteWork != null;

    var dbCompData = await db.Companies.FirstOrDefaultAsync(x => x.Id == compData.Id);

    if (dbCompData == null)
        continue;

    dbCompData.Welfare = jobDetail.data.welfare?.welfare;

    var dbJobData = await db.JobDescriptions.FirstOrDefaultAsync(x => x.JobNo == job.Key);

    Log.Information($"[{jobDic.Count} - {jobCounter++}] {dbCompData.CompanyName} - {job.Value.JobName}");

    if (dbJobData == null)
    {
        await db.JobDescriptions.AddAsync(job.Value);
        continue;
    }

    dbJobData.AppearDate = job.Value.AppearDate;
    dbJobData.JobName = job.Value.JobName;
    dbJobData.JobPlace = job.Value.JobPlace;
    dbJobData.JobNo = job.Value.JobNo;
    dbJobData.Seniority = job.Value.Seniority;
    dbJobData.MinimunSalary = job.Value.MinimunSalary;
    dbJobData.HighestSalary = job.Value.HighestSalary;
    dbJobData.SalaryDescription = job.Value.SalaryDescription;
    dbJobData.Tags = job.Value.Tags;
    dbJobData.DetailUrl = job.Value.DetailUrl;
    dbJobData.IsDeleted = job.Value.IsDeleted;

}

await db.SaveChangesAsync();

var allDbJobs = await db.JobDescriptions.ToArrayAsync();
foreach (var job in allDbJobs)
{
    if (string.IsNullOrWhiteSpace(job.JobNo) ||
        jobNoHash.Contains(job.JobNo))
        continue;

    job.IsDeleted = true;
}

await db.SaveChangesAsync();

sw.Stop();

Log.Information(@$"


{sw.Elapsed.Hours}:{sw.Elapsed.Minutes}:{sw.Elapsed.Seconds}.{sw.Elapsed.Milliseconds}

----------

");

static Dictionary<string, string> GetJobListParams() => new()
{
    { "ro", "1" },
    { "order", "17" },
    { "keyword", "" },
    { "jobexp", "" },
    { "page", "" },
    { "mode", "l" },
    { "s9", "1" },
};

// dotnet ef dbcontext scaffold "Server=(localdb)\Projects;Database=Crawer-104;Trusted_Connection=True" Microsoft.EntityFrameworkCore.SqlServer -o Models\EF\ --force --no-onconfiguring --context Crawer104Context
