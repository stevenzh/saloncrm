namespace SalonCRM.Models.Post
{
    public class MemberByCardModel
    {
        /// <summary>
        ///  0:储值卡 1: 疗程卡 2: 单次卡 3: 留客卡 4: 综合限时卡 5:综合限次卡 6:拓客增值卡 7:拓客优惠卡
        /// </summary>
        public int route { get; set; }
        public int? TmplID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public long MemberId { get; set; }
        /// <summary>
        /// 项目ID（，分隔）
        /// </summary>
        public int Project { get; set; }
        /// <summary>
        /// 购买金额
        /// </summary>
        public decimal Payment { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Points { get; set; }
        /// <summary>
        /// 支付现金（剩余为欠款）
        /// </summary>
        public string BookPrice { get; set; }
        /// <summary>
        /// 增值金额（拓客增值卡\储值卡增值）
        /// </summary>
        public string IncreasePrice { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 客户密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 终端用户
        /// </summary>
        public string user { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 终端门店ＩＤ
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// 终端ＩＤ
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 付款方式 1现金 2刷卡 3转账 4储值卡
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 美容顾问
        /// </summary>
        public string Salesman { get; set; }
        /// <summary>
        /// 美容师（，分隔）
        /// </summary>
        public string Beautician { get; set; }
        /// <summary>
        /// 有效期（综合卡用）
        /// </summary>
        public string ExpiryDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? MemberCardId { get; set; }
        /// <summary>
        /// 顾问分成
        /// </summary>
        public decimal SalesRadix { get; set; }
        /// <summary>
        /// 美容师分成
        /// </summary>
        public decimal WorkerRadix { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProjectList { get; set; }

        public string OtherPay { get; set; }
    }
}
