using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    /// <summary>
    /// 购买项目
    /// </summary>
    public partial class MemberProject
    {
        public long MemberProjectId { get; set; }
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
        /// <summary>
        /// 消费流水
        /// </summary>
        public Nullable<long> AccountRecordID { get; set; }
        /// <summary>
        /// 消费关联卡
        /// </summary>
        public Nullable<long> MemberCardId { get; set; }
        public int ProjectID { get; set; }
        /// <summary>
        /// 0:购买,1:赠送,2:转卡
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 欠款购买标记. 0:一次付清 1:欠款购买
        /// </summary>
        public int DebtFlag { get; set; }
        [Display(Name = "单价")]
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
        public decimal Amount { get; set; }
        /// <summary>
        /// 实收
        /// eg:200元现金购买2000元套餐 实收200 金额2000
        /// </summary>
        public decimal ActualPrice { get; set; }
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
        /// <summary>
        /// 是否现消现耗， 1 是 0 否
        /// </summary>
        public int IsEntity { get; set; }
        [Display(Name = "购买时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 购买终端
        /// </summary>
        public string ClientId { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = " 有效期")]
        public Nullable<DateTime> ExpiryDate { get; set; }
        /// <summary>
        /// 是否有效  0 无效  1 有效
        /// </summary>
        public int IsVaild { get; set; }
        /// <summary>
        /// 0:默认,1:欠款,2: 完成
        /// </summary>
        public int status { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        public Nullable<long> GiveId { get; set; }
        public virtual Member Member { get; set; }
        public virtual Project Project { get; set; }

        public virtual ICollection<MemberProjectGoods> MemberProjectGoods { get; set; }

    }
}