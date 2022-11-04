using System;
using System.Collections.Generic;

namespace Crawer._104.Models.EF
{
    public partial class 職缺
    {
        public long Id { get; set; }
        public long 公司id { get; set; }
        public DateTime? 出現日期 { get; set; }
        public string? 工作地點 { get; set; }
        public string? 工作名稱 { get; set; }
        public string 工作編號 { get; set; } = null!;
        public string? 詳細內容網址 { get; set; }
        public string? 年資 { get; set; }
        public string? 薪水說明 { get; set; }
        public string? 最低薪 { get; set; }
        public string? 最高薪 { get; set; }
        public DateTime? 更新時間 { get; set; }
        public DateTime? 建立時間 { get; set; }
        public string? 標記 { get; set; }
        public string? 工作內容 { get; set; }
        public string? 上班時間 { get; set; }
        public string? 要求 { get; set; }
        public bool? 遠端工作 { get; set; }
        public bool? 被刪除 { get; set; }
        public bool? 已讀 { get; set; }

        public virtual 公司 公司 { get; set; } = null!;
    }
}
