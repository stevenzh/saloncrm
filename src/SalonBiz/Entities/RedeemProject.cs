using System;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    /// <summary>
    /// 退还项目
    /// </summary>
    public partial class RedeemProject
    {
        public int RedeemId { get; set; }
        public int HostId { get; set; }
        public int BranchId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long MemberId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long MemberProjectId { get; set; }
        /// <summary>
        /// 操作记录
        /// </summary>
        public long LogId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "单价")]
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "金额")]
        public decimal Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "次数")]
        public int Count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long CardLogId { get; set; }



        public virtual Member Member { get; set; }
    }
}