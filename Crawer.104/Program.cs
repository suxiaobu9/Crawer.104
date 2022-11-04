using Crawer._104;
using Crawer._104.Models.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Linq;
using System.Diagnostics;
using Serilog;
using Serilog.Events;
using System;
using System.Web;
using System.Diagnostics.CodeAnalysis;

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

serviceCollection.AddDbContext<Crawer104Context>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

var httpClient = new HttpClient { BaseAddress = new Uri("https://www.104.com.tw") };

httpClient.DefaultRequestHeaders.Add("Referer", httpClient.BaseAddress.AbsoluteUri);

var services = serviceCollection.BuildServiceProvider();

var db = services.GetRequiredService<Crawer104Context>();

var compNoHash = new HashSet<string>();
var jobNoHash = new HashSet<string>();

var compDic = new Dictionary<string, 公司>();
var jobDic = new Dictionary<string, 職缺>();

var searchKey = new string[] { "C%23", ".net" };
var jobExps = new string[] { "1", "3", "5", "10" };

var sw = new Stopwatch();

Log.Information("Process Start !");

sw.Start();

// 取得所有條件的總頁數
var getTotalPageTasks = await Task.WhenAll(
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

if (getTotalPageTasks == null)
    return;

// 取得所有條件的所有列表資料
var jobTasks = getTotalPageTasks
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

            compDic.Add(job.custNo, new 公司
            {
                公司名稱 = job.custName,
                公司網址 = compDetailUrl,
                公司編號 = job.custNo,
            });
        }

        var addJobSuccess = jobNoHash.Add(job.jobNo);

        if (!addJobSuccess)
            continue;

        var detailLink = job.link?.job;

        detailLink = !string.IsNullOrWhiteSpace(detailLink) && detailLink.Contains('?') ?
                        detailLink[..detailLink.IndexOf("?")] : "";

        var transDateSuccess = DateTime.TryParseExact(job.appearDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var 出現日期);

        jobDic.Add(job.jobNo, new 職缺
        {
            出現日期 = transDateSuccess ? 出現日期 : null,
            工作名稱 = job.jobName,
            工作地點 = (job.jobAddrNoDesc ?? "") + job.jobAddress ?? "",
            工作編號 = job.jobNo,
            年資 = job.periodDesc,
            建立時間 = now,
            最低薪 = job.salaryLow?.TrimStart('0'),
            最高薪 = job.salaryHigh?.TrimStart('0'),
            薪水說明 = job.salaryDesc,
            標記 = job.tags == null ? null : string.Join(",", job.tags),
            詳細內容網址 = detailLink,
            被刪除 = false,
            已讀 = false
        });
    }
}

Log.Information($"Comp Count : {compDic.Count}");

// 處理公司資料進資料庫
foreach (var comp in compDic)
{
    var dbCompData = await db.公司s.FirstOrDefaultAsync(x => x.公司編號 == comp.Key);

    Log.Information($"{comp.Value.公司名稱}");

    if (dbCompData == null)
    {
        await db.公司s.AddAsync(comp.Value);
        continue;
    }

    dbCompData.公司名稱 = comp.Value.公司名稱;
    dbCompData.公司網址 = comp.Value.公司網址;
    dbCompData.公司編號 = comp.Value.公司編號;
    comp.Value.Id = dbCompData.Id;
}

await db.SaveChangesAsync();

Log.Information($"Job Count : {jobDic.Count}");
var jobCounter = 1;
// 取得所有職缺詳細資料，並且進資料庫
foreach (var job in jobDic)
{
    if (string.IsNullOrWhiteSpace(job.Value.詳細內容網址))
        continue;

    var startIdx = (job.Value.詳細內容網址.IndexOf("tw/job") + "tw/job".Length) + 1;

    var jobDetailCode = job.Value.詳細內容網址[startIdx..];

    var jobDetail = await httpClient.GetFromJsonAsync<JobDetail>($"/job/ajax/content/{jobDetailCode}");

    if (jobDetail == null ||
        jobDetail.data == null ||
        jobDetail.data.jobDetail == null)
        continue;

    var compData = compDic.FirstOrDefault(x => x.Key == jobDetail.data.custNo).Value;

    job.Value.公司id = compData.Id;

    job.Value.上班時間 = jobDetail.data.jobDetail.workPeriod;
    job.Value.工作內容 = jobDetail.data.jobDetail.jobDescription;
    job.Value.要求 = jobDetail.data.condition?.other;
    job.Value.遠端工作 = jobDetail.data.jobDetail.remoteWork != null;

    var dbCompData = await db.公司s.FirstOrDefaultAsync(x => x.Id == compData.Id);

    if (dbCompData == null)
        continue;

    dbCompData.福利 = jobDetail.data.welfare?.welfare;

    var dbJobData = await db.職缺s.FirstOrDefaultAsync(x => x.工作編號 == job.Key);

    Log.Information($"[{jobDic.Count} - {jobCounter++}] {dbCompData.公司名稱} - {job.Value.工作名稱}");

    if (dbJobData == null)
    {
        await db.職缺s.AddAsync(job.Value);
        continue;
    }

    dbJobData.出現日期 = job.Value.出現日期;
    dbJobData.工作名稱 = job.Value.工作名稱;
    dbJobData.工作地點 = job.Value.工作地點;
    dbJobData.工作編號 = job.Value.工作編號;
    dbJobData.年資 = job.Value.年資;
    dbJobData.最低薪 = job.Value.最低薪;
    dbJobData.最高薪 = job.Value.最高薪;
    dbJobData.薪水說明 = job.Value.薪水說明;
    dbJobData.標記 = job.Value.標記;
    dbJobData.詳細內容網址 = job.Value.詳細內容網址;
    dbJobData.被刪除 = job.Value.被刪除;
    dbJobData.已讀 = job.Value.已讀;

}

await db.SaveChangesAsync();

var allDbJobs = await db.職缺s.ToArrayAsync();
foreach (var job in allDbJobs)
{
    if (string.IsNullOrWhiteSpace(job.工作編號) ||
        jobNoHash.Contains(job.工作編號))
        continue;

    job.被刪除 = true;
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