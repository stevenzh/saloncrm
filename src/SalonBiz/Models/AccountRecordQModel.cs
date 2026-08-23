using System;
using System.Collections.Generic;

namespace SalonCRM.Models
{
    public class AccountRecordQModel : PagedModel
    {
        public AccountRecordQModel()
        {
            this.PagedIndex = 1;
            this.PagedSize = 20;
        }
        public int BranchId { get; set; }
        public string Type { get; set; }
        public string SalesmanId { get; set; }
        public string BeauticianId { get; set; }
        /// <summary>
        /// 客户卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 交易卡项类型
        /// </summary>
        public string CardType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IList<AccountRecordModel> RecordList { get; set; }
    }


    public class AccountRecordModel
    {
        public long RecordID { get; set; }
        public int HostID { get; set; }
        public int BranchId { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public long MemberID { get; set; }
        /// <summary>
        /// 目标卡
        /// </summary>
        public Nullable<long> MemberCardId { get; set; }
        /// <summary>
        /// 1:充值,2:购卡,3:购买项目,4:转出,5:转入,6:退项目,7:退款
        /// </summary>
        public string Type { get; set; }
        public string TypeValue { get; set; }
        /// <summary>
        /// 付款方式 1:现金 2:刷卡 3:转账
        /// </summary>
        public string PaymentType { get; set; }
        public string PaymentTypeValue { get; set; }
        /// <summary>
        /// 支出
        /// </summary>
        public decimal OutAmount { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        public decimal InAmount { get; set; }
        /// <summary>
        /// 账户余额
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



        public virtual Member Member { get; set; }
        public MemberCardModel MemberCard { get; set; }
        public string MemberCardType { get; set; }
        public virtual Organ Branch { get; set; }
        /// <summary>
        /// 美容顾问 
        /// </summary>
        public string SaleId { get; set; }
        public ApplicationUser Salesman { get; set; }
        /// <summary>
        /// 美容顾问分成（例如：0.5）
        /// </summary>
        public decimal SalesPercentage { get; set; }
        /// <summary>
        /// 美容师
        /// </summary>
        public string BeauticianId { get; set; }
        /// <summary>
        /// 美容师分成（例如：0.3）
        /// </summary>
        public decimal BeauticianPercentage { get; set; }
        /// <summary>
        /// 辅助美容师
        /// </summary>
        public virtual ICollection<AccountRecordSplit> Splits { get; set; }
        /// <summary>
        /// 辅助销售
        /// </summary>
        public string SalesmanStr { get; set; }
        public List<MemberProject> MemberProjects { get; set; }
        /// <summary>
        /// 1 卡扣 2 现金购买
        /// </summary>
        public int SalesType { get; set; }
        public int IsVaild { get; set; }
    }
}