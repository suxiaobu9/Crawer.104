using System;
using System.Collections.Generic;

namespace Crawer._104.Models.EF
{
    public partial class Company
    {
        public Company()
        {
            JobDescriptions = new HashSet<JobDescription>();
        }

        public long Id { get; set; }
        /// <summary>
        /// 公司編號
        /// </summary>
        public string CompanyNo { get; set; } = null!;
        /// <summary>
        /// 公司名稱
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 性質
        /// </summary>
        public string? Property { get; set; }
        /// <summary>
        /// 公司網址
        /// </summary>
        public string? CompanyUrl { get; set; }
        /// <summary>
        /// 福利
        /// </summary>
        public string? Welfare { get; set; }
        /// <summary>
        /// 忽視
        /// </summary>
        public bool? Ignore { get; set; }

        public virtual ICollection<JobDescription> JobDescriptions { get; set; }
    }
}
