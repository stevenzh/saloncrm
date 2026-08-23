using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalonCRM.Models
{
    /// <summary>
    /// 项目销售 消耗统计
    /// </summary>
    public class ProjectSTAViewModel
    {
        public int BranchID { get; set; }
        public int ProjectId { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string ProjectName { get; set; }
        public decimal Amount { get; set; }
        public int SaleTimes { get; set; }
        public decimal SalesPercent { get; set; }
        public decimal UsedAmount { get; set; }
        public int? UsedCount { get; set; }
        /// <summary>
        /// 人头数
        /// </summary>
        public int PersonCount { get; set; }
        public int ServiceCount { get; set; }

    }

    public class ProjectSTAModel
    {
        public long MemberID { get; set; }
        public string MemberName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 美容师
        /// </summary>
        public AccountRecord UserSplits { get; set; }
        public decimal Amount { get; set; }
        public ICollection<BookProjectSplit> BookUserSplits { get; set; }
    }
}