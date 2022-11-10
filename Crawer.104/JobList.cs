namespace Crawer._104;

public class JobList
{
    public int status { get; set; }
    public Data? data { get; set; }
    public string? statusMsg { get; set; }
    public string? errorMsg { get; set; }
}

public class Data
{
    public Query? query { get; set; }
    public Filterdesc? filterDesc { get; set; }
    public Querydesc? queryDesc { get; set; }
    public List[]? list { get; set; }
    public string[]? count { get; set; }
    public int pageNo { get; set; }
    public int totalPage { get; set; }
    public int totalCount { get; set; }
}

public class Query
{
    public int ro { get; set; }
    public string? jobcat { get; set; }
    public string? isnew { get; set; }
    public int kwop { get; set; }
    public string? keyword { get; set; }
    public string? expansionType { get; set; }
    public string? excludeKeyword { get; set; }
    public string? area { get; set; }
    public string? indcat { get; set; }
    public string? expcate { get; set; }
    public string? edu { get; set; }
    public int order { get; set; }
    public int asc { get; set; }
    public string? zone { get; set; }
    public string? dep { get; set; }
    public string? dis_role { get; set; }
    public string? lang { get; set; }
    public string? sr { get; set; }
    public string? sctp { get; set; }
    public string? scmin { get; set; }
    public string? scmax { get; set; }
    public string? scstrict { get; set; }
    public string? scneg { get; set; }
    public string? excludeJobKeyword { get; set; }
    public string? excludeCompanyKeyword { get; set; }
    public string? excludeCompanyByCustno { get; set; }
    public string? excludeIndustryCat { get; set; }
    public string? s9 { get; set; }
    public string? s5 { get; set; }
    public string? wktm { get; set; }
    public string? startby { get; set; }
    public string? rostatus { get; set; }
    public int page { get; set; }
    public string? wf { get; set; }
    public string? jobexp { get; set; }
    public string? wt { get; set; }
    public string? mode { get; set; }
    public string? jobsource { get; set; }
    public string? utm_source { get; set; }
    public string? utm_medium { get; set; }
    public string? utm_campaign { get; set; }
    public string? c { get; set; }
    public string? custNo { get; set; }
    public string? jobNo { get; set; }
    public string? dist { get; set; }
    public string? lat { get; set; }
    public string? lon { get; set; }
    public string? mrt { get; set; }
    public string? indArea { get; set; }
    public string? newZone { get; set; }
    public string? searchTempExclude { get; set; }
    public string? manage { get; set; }
    public string? showLatLon { get; set; }
    public string? showDutyTime { get; set; }
    public int langFlag { get; set; }
    public int langStatus { get; set; }
    public int recommendJob { get; set; }
    public int hotJob { get; set; }
    public string? remoteWork { get; set; }
}

public class Filterdesc
{
    public Recommendindcat[]? recommendIndcat { get; set; }
    public Recommendmrt[]? recommendMrt { get; set; }
}

public class Recommendindcat
{
    public string? no { get; set; }
    public string? des { get; set; }
}

public class Recommendmrt
{
    public string? no { get; set; }
    public string? des { get; set; }
}

public class Querydesc
{
    public string? keyword { get; set; }
}

public class List
{
    public string? jobType { get; set; }
    public string? jobNo { get; set; }
    public string? jobName { get; set; }
    public string? jobNameSnippet { get; set; }
    public string? jobRole { get; set; }
    public string? jobRo { get; set; }
    public string? jobAddrNo { get; set; }
    public string? jobAddrNoDesc { get; set; }
    public string? jobAddress { get; set; }
    public string? description { get; set; }
    public string? optionEdu { get; set; }
    public string? period { get; set; }
    public string? periodDesc { get; set; }
    public string? applyCnt { get; set; }
    public string? applyDesc { get; set; }
    public string? custNo { get; set; }
    public string? custName { get; set; }
    public string? coIndustry { get; set; }
    public string? coIndustryDesc { get; set; }
    public string? salaryLow { get; set; }
    public string? salaryHigh { get; set; }
    public string? salaryDesc { get; set; }
    public string? s10 { get; set; }
    public string? appearDate { get; set; }
    public string? appearDateDesc { get; set; }
    public string? optionZone { get; set; }
    public string? isApply { get; set; }
    public string? applyDate { get; set; }
    public string? isSave { get; set; }
    public string? descSnippet { get; set; }
    public object? tags { get; set; }
    public string? landmark { get; set; }
    public Link? link { get; set; }
    public string? jobsource { get; set; }
    public string? jobNameRaw { get; set; }
    public string? custNameRaw { get; set; }
    public string? lon { get; set; }
    public string? lat { get; set; }
    public int remoteWorkType { get; set; }
    public string[]? major { get; set; }
}

public class Link
{
    public string? applyAnalyze { get; set; }
    public string? job { get; set; }
    public string? cust { get; set; }
}

public class Tags
{
    public Emp? emp { get; set; }
    public Zoneforeign? zoneForeign { get; set; }
    public Zone? zone { get; set; }
    public Remote? remote { get; set; }
}

public class Emp
{
    public string? desc { get; set; }
    public string? param { get; set; }
}

public class Zoneforeign
{
    public string? desc { get; set; }
    public int param { get; set; }
}

public class Remote
{
    public string? desc { get; set; }
}

public class Zone
{
    public string? desc { get; set; }
    public int param { get; set; }
}




