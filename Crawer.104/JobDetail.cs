using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawer._104;

public class JobDetail
{
    public Data? data { get; set; }
    public Metadata? metadata { get; set; }

    public class Data
    {
        public Corpimageright? corpImageRight { get; set; }
        public Header? header { get; set; }
        public Contact? contact { get; set; }
        public Environmentpic? environmentPic { get; set; }
        public Condition? condition { get; set; }
        public Welfare? welfare { get; set; }
        public Jobdetail? jobDetail { get; set; }
        public string? _switch { get; set; }
        public string? custLogo { get; set; }
        public string? postalCode { get; set; }
        public string? closeDate { get; set; }
        public string? industry { get; set; }
        public string? custNo { get; set; }
        public string? reportUrl { get; set; }
        public string? industryNo { get; set; }
        public string? employees { get; set; }
        public bool chinaCorp { get; set; }
    }

    public class Corpimageright
    {
        public Corpimageright1? corpImageRight { get; set; }
    }

    public class Corpimageright1
    {
        public string? imageUrl { get; set; }
        public string? link { get; set; }
    }

    public class Header
    {
        public Corpimagetop? corpImageTop { get; set; }
        public string? jobName { get; set; }
        public string? appearDate { get; set; }
        public string? custName { get; set; }
        public string? custUrl { get; set; }
        public string? applyDate { get; set; }
        public int analysisType { get; set; }
        public string? analysisUrl { get; set; }
        public bool isSaved { get; set; }
        public bool isApplied { get; set; }
    }

    public class Corpimagetop
    {
        public string? imageUrl { get; set; }
        public string? link { get; set; }
    }

    public class Contact
    {
        public string? hrName { get; set; }
        public string? email { get; set; }
        public string? visit { get; set; }
        public object[]? phone { get; set; }
        public string? other { get; set; }
        public string? reply { get; set; }
        public bool suggestExam { get; set; }
    }

    public class Environmentpic
    {
        public Environmentpic1[]? environmentPic { get; set; }
        public Corpimagebottom? corpImageBottom { get; set; }
    }

    public class Corpimagebottom
    {
        public string? imageUrl { get; set; }
        public string? link { get; set; }
    }

    public class Environmentpic1
    {
        public string? thumbnailLink { get; set; }
        public string? link { get; set; }
        public string? description { get; set; }
    }

    public class Condition
    {
        public Acceptrole? acceptRole { get; set; }
        public string? workExp { get; set; }
        public string? edu { get; set; }
        public object[]? major { get; set; }
        public object[]? language { get; set; }
        public object[]? localLanguage { get; set; }
        public object[]? specialty { get; set; }
        public object[]? skill { get; set; }
        public object[]? certificate { get; set; }
        public object[]? driverLicense { get; set; }
        public string? other { get; set; }
    }

    public class Acceptrole
    {
        public Role[]? role { get; set; }
        public Disrole? disRole { get; set; }
    }

    public class Disrole
    {
        public bool needHandicapCompendium { get; set; }
        public object[]? disability { get; set; }
    }

    public class Role
    {
        public int code { get; set; }
        public string? description { get; set; }
    }

    public class Welfare
    {
        public object[]? tag { get; set; }
        public string? welfare { get; set; }
        public object[]? legalTag { get; set; }
    }

    public class Jobdetail
    {
        public string? jobDescription { get; set; }
        public Jobcategory[]? jobCategory { get; set; }
        public string? salary { get; set; }
        public int salaryMin { get; set; }
        public int salaryMax { get; set; }
        public int salaryType { get; set; }
        public int jobType { get; set; }
        public object[]? workType { get; set; }
        public string? addressNo { get; set; }
        public string? addressRegion { get; set; }
        public string? addressArea { get; set; }
        public string? addressDetail { get; set; }
        public string? industryArea { get; set; }
        public string? longitude { get; set; }
        public string? latitude { get; set; }
        public string? manageResp { get; set; }
        public string? businessTrip { get; set; }
        public string? workPeriod { get; set; }
        public string? vacationPolicy { get; set; }
        public string? startWorkingDay { get; set; }
        public int hireType { get; set; }
        public string? delegatedRecruit { get; set; }
        public string? needEmp { get; set; }
        public string? landmark { get; set; }
        public object? remoteWork { get; set; }
    }

    public class Jobcategory
    {
        public string? code { get; set; }
        public string? description { get; set; }
    }

    public class Metadata
    {
        public bool enableHTML { get; set; }
        public bool hiddenBanner { get; set; }
        public Seo? seo { get; set; }
    }

    public class Seo
    {
        public bool noindex { get; set; }
    }
}
