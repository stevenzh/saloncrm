using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonCRM.Models
{
    /// <summary>
    /// 美容服务记录(消耗)
    /// </summary>
    public class Book
    {
        public long BookID { get; set; }
        public int HostID { get; set; }
        public int BranchId { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public long MemberID { get; set; }
        /// <summary>
        /// 操作记录
        /// </summary>
        public long LogId { get; set; }
        /// <summary>
        /// 消耗金额（计算）
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 满意度
        /// </summary>
        public Nullable<int> Satisfaction { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 终端ID
        /// </summary>
        public string ClientID { get; set; }
        public string CreatedBy { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 美容顾问
        /// </summary>
        public string SalesmanID { get; set; }
        /// <summary>
        /// 下单流程 0 下单 10 进行中  20 已结算 30 已取消
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 收款人
        /// </summary>
        public string PaymentID { get; set; }

        /// <summary>
        /// 付款时间
        /// </summary>
        public Nullable<DateTime> PayTime { get; set; }
        /// <summary>
        /// 服务项目
        /// </summary>
        public virtual ICollection<BookProject> BookProjects { get; set; }
        //public virtual ICollection<BookGoods> BookGoods { get; set; }
        public virtual Member Member { get; set; }

    }
}