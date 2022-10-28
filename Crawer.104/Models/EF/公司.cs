using System;
using System.Collections.Generic;

namespace Crawer._104.Models.EF
{
    public partial class 公司
    {
        public 公司()
        {
            職缺s = new HashSet<職缺>();
        }

        public long Id { get; set; }
        public string? 公司編號 { get; set; }
        public string? 公司名稱 { get; set; }
        public string? 性質 { get; set; }
        public string? 公司網址 { get; set; }
        public string? 福利 { get; set; }

        public virtual ICollection<職缺> 職缺s { get; set; }
    }
}
