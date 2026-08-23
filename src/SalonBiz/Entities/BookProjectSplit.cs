using System;
using System.Collections.Generic;

namespace SalonCRM.Models
{
    /// <summary>
    /// 实耗分割
    /// </summary>
    public class BookProjectSplit
    {
        public long SplitID { get; set; }
        public long BookProjectID { get; set; }

        /// <summary>
        /// 位置  1 顾问 2 美容师 3助理美容师
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        /// <summary>
        /// 手工费
        /// </summary>
        public decimal HandicraftFee { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string ModifiedBy { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual BookProject BookProject { get; set; }
    }
}