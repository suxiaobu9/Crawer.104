using System;
using System.Collections.Generic;

namespace JD._104.Model.Models.EF
{
    public partial class JobDescription
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        /// <summary>
        /// 出現日期
        /// </summary>
        public DateTime? AppearDate { get; set; }
        /// <summary>
        /// 工作地點
        /// </summary>
        public string? JobPlace { get; set; }
        /// <summary>
        /// 工作名稱
        /// </summary>
        public string? JobName { get; set; }
        /// <summary>
        /// 工作編號
        /// </summary>
        public string JobNo { get; set; } = null!;
        /// <summary>
        /// 詳細內容網址
        /// </summary>
        public string? DetailUrl { get; set; }
        /// <summary>
        /// 年資
        /// </summary>
        public string? Seniority { get; set; }
        /// <summary>
        /// 薪水說明
        /// </summary>
        public string? SalaryDescription { get; set; }
        /// <summary>
        /// 最低薪
        /// </summary>
        public string? MinimunSalary { get; set; }
        /// <summary>
        /// 最高薪
        /// </summary>
        public string? HighestSalary { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// 標記
        /// </summary>
        public string? Tags { get; set; }
        /// <summary>
        /// 工作內容
        /// </summary>
        public string? WorkContent { get; set; }
        /// <summary>
        /// 上班時間
        /// </summary>
        public string? WorkingHour { get; set; }
        /// <summary>
        /// 要求
        /// </summary>
        public string? Request { get; set; }
        /// <summary>
        /// 遠端工作
        /// </summary>
        public bool? RemoteWork { get; set; }
        /// <summary>
        /// 被刪除
        /// </summary>
        public bool? IsDeleted { get; set; }
        /// <summary>
        /// 已讀
        /// </summary>
        public bool? HaveRead { get; set; }

        public virtual Company Company { get; set; } = null!;
    }
}
