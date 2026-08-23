using System;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    /// <summary>
    /// 赠送记录
    /// </summary>
    public partial class MemberGive
    {
        public long GiveId { get; set; }
        public int HostID { get; set; }
        /// <summary>
        /// 购买门店
        /// </summary>
        public int BranchId { get; set; }
        public long MemberID { get; set; }
        /// <summary>
        /// 操作记录
        /// </summary>
        public long LogId { get; set; }
        public Nullable<int> ProjectID { get; set; }
        /// <summary>
        /// 0:积分 1:项目
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
        public int InPoints { get; set; }
        public int RemainPoints { get; set; }
        /// <summary>
        /// 购买次数
        /// </summary>
        [Display(Name = "购买次数")]
        public int BookTime { get; set; }
        /// <summary>
        /// 已用次数
        /// </summary>
        [Display(Name = "已用次数")]
        public int UsedTime { get; set; }
        /// <summary>
        /// 可用次数
        /// </summary>
        public int LastCount { get; set; }
        public string Salesman { get; set; }
        [Display(Name = "购买时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 购买终端
        /// </summary>
        public string ClientId { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = " 有效期")]
        public Nullable<DateTime> ExpiryDate { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        public virtual Member Member { get; set; }
        public virtual Project Project { get; set; }
        [Display(Name = "是否有效")]
        public int IsVaild { get; set; }
    }
}