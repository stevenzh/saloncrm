namespace SalonCRM.Models.Post
{
    /// <summary>
    /// 
    /// </summary>
    public class MemberByProjectModel
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public long MemberId { get; set; }
        public long? MemberCardId { get; set; }
        /// <summary>
        /// 所购项目ID
        /// </summary>
        public int ProjectID { get; set; }
        /// <summary>
        /// 总应付
        /// </summary>
        public decimal Payment { get; set; }
        /// <summary>
        /// 项目数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 客户密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string user { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 操作门店
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// 操作终端
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 0:购买,1:赠送,2:转卡
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 1 现金 2 刷卡 3 转账 4 储值卡
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        /// 美容顾问
        /// </summary>
        public string Salesman { get; set; }
        /// <summary>
        /// 美容师
        /// </summary>
        public string Workers { get; set; }
        /// <summary>
        /// 使用期限
        /// </summary>
        public string ExpiryDate { get; set; }
        /// <summary>
        /// 使用积分
        /// </summary>
        public int Points { get; set; }
        /// <summary>
        /// 现付定金
        /// </summary>
        public int? BookPrice { get; set; }

        public int IsEntity { get; set; }

        public decimal SalesRadix { get; set; }
        public decimal WorkerRadix { get; set; }

        /// <summary>
        /// 支付总金额
        /// </summary>
        public decimal AllPay { get; set; }
        /// <summary>
        /// 其他支付{'type', 'amount', 'card'}
        /// </summary>
        public string OtherPay { get; set; }
    }
}
