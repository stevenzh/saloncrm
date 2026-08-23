using System;
using System.Collections.Generic;

namespace SalonCRM.Models
{
    /// <summary>
    /// 账户流水   [充值|转账|消费|退款]
    /// </summary>
    public class AccountRecord
    {
        public long RecordID { get; set; }
        public int HostID { get; set; }
        public int BranchId { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public long MemberID { get; set; }
        /// <summary>
        /// 操作记录
        /// </summary>
        public long EventLogId { get; set; }
        /// <summary>
        /// 目标卡
        /// </summary>
        public long MemberCardId { get; set; }
        /// <summary>
        /// 1:充值,2:购卡,3:购买项目,4:转出,5:转入,6:退项目,7:退款
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 购买项目方式 1:卡扣(储值卡扣费)  2: 现金(现金、转账、刷卡直接购买项目)
        /// </summary>
        public int SalesType { get; set; }
        /// <summary>
        /// 付款方式 1:现金 2:刷卡 3:转账（仅在1充值 2 购卡是有意义）
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        /// 支出
        /// </summary>
        public decimal OutAmount { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        public decimal InAmount { get; set; }
        /// <summary>
        /// 账户余额(储值卡)
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 购卡欠款
        /// </summary>
        public decimal Debt { get; set; }
        /// <summary>
        /// 转账起始卡
        /// </summary>
        public Nullable<long> FromCardId { get; set; }
        /// <summary>
        /// 消耗服务ID
        /// </summary>
        public Nullable<long> BookID { get; set; }
        /// <summary>
        /// 退项目ID
        /// </summary>
        public Nullable<int> RedeemId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 终端ID
        /// </summary>
        public string ClientID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 是否有效  0 无效  1 有效
        /// </summary>
        public int IsVaild { get; set; }
        /// <summary>
        /// 美容顾问 
        /// </summary>
        public string SaleID { get; set; }
        /// <summary>
        /// 美容师
        /// </summary>
        public string BeauticianID { get; set; }

        public virtual Member Member { get; set; }
        public virtual Organ Branch { get; set; }
        /// <summary>
        /// 业绩分割
        /// </summary>
        public virtual ICollection<AccountRecordSplit> Splits { get; set; }
    }
}