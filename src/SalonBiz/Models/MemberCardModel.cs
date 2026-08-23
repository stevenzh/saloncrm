using System;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    /// <summary>
    /// 客户卡账户
    /// </summary>
    public partial class MemberCardModel
    {
        public long MemberCardId { get; set; }
        public long MemberID { get; set; }

        /// <summary>
        /// 卡片类型 0:储值卡 1: 疗程卡 2: 单次卡 3: 体验卡 4: 综合限时卡 5: 综合限次卡 6: 拓客增值卡 7: 拓客优惠卡
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 卡标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 余额（可用金额 拓客卡 单次卡）
        /// </summary>
        public decimal Amt { get; set; }
        /// <summary>
        /// 有效期(综合卡)
        /// </summary>
        public Nullable<DateTime> ExpiryDate { get; set; }
        /// <summary>
        /// 0:默认,1:可用,2: 失效
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 购买次数（综合卡）
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

        #region 购卡数据

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 实收（金额-实收= 欠款）
        /// </summary>
        public decimal ActualPrice { get; set; }
        /// <summary>
        /// 欠款购买标记. 0:一次付清 1:欠款购买
        /// </summary>
        public int DebtFlag { get; set; }
        /// <summary>
        /// 0:默认,1:欠款,2: 完成
        /// </summary>
        public int DebtStatus { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = "购买时间")]
        public DateTime CreatedDate { get; set; }
        public int HostID { get; set; }
        /// <summary>
        /// 购买门店
        /// </summary>
        public int BranchID { get; set; }
        /// <summary>
        /// 购买终端
        /// </summary>
        public string ClientID { get; set; }
        /// <summary>
        /// 购买记录
        /// </summary>
        public AccountRecord Record { get; set; }
        #endregion


        public virtual Member Member { get; set; }
        public object TypeValue { get; set; }
    }
}
