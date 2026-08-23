using System;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    /// <summary>
    /// 赠送记录
    /// </summary>
    public partial class GiveModel
    {
        public long GiveId { get; set; }
        public int HostID { get; set; }
        /// <summary>
        /// 购买门店
        /// </summary>
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public long MemberID { get; set; }
        public string MemberName { get; set; }
        public string MemberCardNo { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 0:积分 1:项目
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 获得积分
        /// </summary>
        [Display(Name = "金额")]
        public int InPoints { get; set; }
        /// <summary>
        /// 剩余积分
        /// </summary>
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
        public Member Member { get; set; }
        public Project Project { get; set; }
        /// <summary>
        /// 最终项目名称（用于赠送次数不限项目）
        /// </summary>
        public string FinalProject { get; set; }
        public Nullable< DateTime> ServiceDate { get; set; }

    }
}