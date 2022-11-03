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

var searchKey = new string[]
{
    "C%23",
    ".net"
};

var sw = new Stopwatch();

sw.Start();

var doneCompTable = new Dictionary<string, 公司>();
var doneJobTable = new Dictionary<string, 職缺>();
var jobexps = new string[] { "1", "3", "5", "10" };

foreach (var key in searchKey)
{
    foreach (var jobexp in jobexps)
    {
        var totalPage = 1;

        for (var currentPage = 1; currentPage <= totalPage; currentPage++)
        {
            var jobList = await httpClient.GetFromJsonAsync<JobList>($"/jobs/search/list?order=17&keyword={key}&jobexp={jobexp}&page={currentPage}&mode=l");

            if (jobList == null ||
                jobList.data == null ||
                jobList.data.list == null)
                break;

            totalPage = jobList.data.totalPage;

            foreach (var job in jobList.data.list)
            {
                公司? dbCompData;
                職缺? dbJobData;

                if (string.IsNullOrWhiteSpace(job.custNo))
                    continue;

                Log.Information($"{job.custName}");
                Log.Information($"{job.jobName}");

                if (doneCompTable.ContainsKey(job.custNo))
                    dbCompData = doneCompTable.FirstOrDefault(x => x.Key == job.custNo).Value;
                else
                {
                    dbCompData = await db.公司s.FirstOrDefaultAsync(x => x.公司編號 == job.custNo);

                    var compDetailUrl = job.link?.cust;
                    compDetailUrl = !string.IsNullOrWhiteSpace(compDetailUrl) && compDetailUrl.Contains('?') ?
                                    compDetailUrl[..compDetailUrl.IndexOf("?")] : "";

                    if (dbCompData == null)
                    {
                        dbCompData = new 公司
                        {
                            公司名稱 = job.custName,
                            公司網址 = compDetailUrl,
                            公司編號 = job.custNo,
                        };
                        await db.公司s.AddAsync(dbCompData);
                    }
                    else
                    {
                        dbCompData.公司名稱 = job.custName;
                        dbCompData.公司網址 = compDetailUrl;
                        dbCompData.公司編號 = job.custNo;
                    }
                    await db.SaveChangesAsync();
                    doneCompTable.Add(job.custNo, dbCompData);
                }

                if (string.IsNullOrWhiteSpace(job.jobNo) || doneJobTable.ContainsKey(job.jobNo))
                    continue;

                dbJobData = await db.職缺s.FirstOrDefaultAsync(x => x.工作編號 == job.jobNo);

                var transDateSuccess = DateTime.TryParseExact(job.appearDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var 出現日期);

                var detailLink = job.link?.job;

                detailLink = !string.IsNullOrWhiteSpace(detailLink) && detailLink.Contains('?') ?
                                detailLink[..detailLink.IndexOf("?")] : "";

                if (dbJobData == null)
                {
                    dbJobData = new 職缺
                    {
                        公司id = dbCompData.Id,
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
                    };

                    await db.職缺s.AddAsync(dbJobData);
                }
                else
                {
                    dbJobData.公司id = dbCompData.Id;
                    dbJobData.出現日期 = transDateSuccess ? 出現日期 : null;
                    dbJobData.工作名稱 = job.jobName;
                    dbJobData.工作地點 = (job.jobAddrNoDesc ?? "") + job.jobAddress ?? "";
                    dbJobData.工作編號 = job.jobNo;
                    dbJobData.年資 = job.periodDesc;
                    dbJobData.最低薪 = job.salaryLow?.TrimStart('0');
                    dbJobData.最高薪 = job.salaryHigh?.TrimStart('0');
                    dbJobData.薪水說明 = job.salaryDesc;
                    dbJobData.標記 = job.tags == null ? null : string.Join(",", job.tags);
                    dbJobData.詳細內容網址 = detailLink;
                    dbJobData.被刪除 = false;
                    dbJobData.已讀 = false;
                }
                await db.SaveChangesAsync();
                doneJobTable.Add(job.jobNo, dbJobData);


                if (string.IsNullOrWhiteSpace(dbJobData.詳細內容網址))
                    continue;

                var startIdx = (dbJobData.詳細內容網址.IndexOf("tw/job") + "tw/job".Length) + 1;

                var jobDetailCode = dbJobData.詳細內容網址[startIdx..];

                var jobDetail = await httpClient.GetFromJsonAsync<JobDetail>($"/job/ajax/content/{jobDetailCode}");

                if (jobDetail == null ||
                    jobDetail.data == null ||
                    jobDetail.data.jobDetail == null)
                    continue;

                dbJobData.上班時間 = jobDetail.data.jobDetail.workPeriod;
                dbJobData.工作內容 = jobDetail.data.jobDetail.jobDescription;
                dbJobData.要求 = jobDetail.data.condition?.other;
                dbJobData.遠端工作 = jobDetail.data.jobDetail.remoteWork != null;
                dbCompData.福利 = jobDetail.data.welfare?.welfare;

                await db.SaveChangesAsync();

                Log.Information($"----------");

            }

        }
    }
}

var allDbJobs = db.職缺s.Where(x => x.被刪除 == false);
foreach (var job in allDbJobs)
{
    if (string.IsNullOrWhiteSpace(job.工作編號))
        continue;

    if (!doneJobTable.ContainsKey(job.工作編號))
        job.被刪除 = true;
}

await db.SaveChangesAsync();

sw.Stop();

Log.Information(@$"


{sw.Elapsed.Minutes}:{sw.Elapsed.Seconds}.{sw.Elapsed.Milliseconds}

----------

");

// dotnet ef dbcontext scaffold "Server=(localdb)\Projects;Database=Crawer-104;Trusted_Connection=True" Microsoft.EntityFrameworkCore.SqlServer -o Models\EF\ --force --no-onconfiguring --context Crawer104Context