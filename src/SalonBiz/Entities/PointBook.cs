using System;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    /// <summary>
    /// 积分生成\消费记录
    /// </summary>
    public partial class PointBook
    {
        public long PointBookId { get; set; }
        public int HostId { get; set; }
        public int BranchId { get; set; }
        public long MemberId { get; set; }
        /// <summary>
        /// 操作记录
        /// </summary>
        public long LogId { get; set; }
        public int OutPoints { get; set; }
        public int InPoints { get; set; }
        public int RemainPoints { get; set; }
        public string ClientId { get; set; }
        /// <summary>
        /// 消费赠积分:1,消费积分:2
        /// </summary>
        public int InOut { get; set; }
        public Nullable<long> MemberCardId { get; set; }
        [Display(Name = " 有效期")]
        public Nullable<DateTime> ExpiryDate { get; set; }
        public string Salesman { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<long> GiveId { get; set; }
        public virtual Member Member { get; set; }
    }
}