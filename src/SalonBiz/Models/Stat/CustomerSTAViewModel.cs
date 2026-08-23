using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalonCRM.Models
{
    /// <summary>
    /// 客户统计
    /// </summary>
    public class CustomerSTAViewModel
    {
        public int? BranchId { get; set; }

        public string BranchName { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        public long MemberId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string Type { get; set; }
        public string Status { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 入会日期
        /// </summary>
        public DateTime JoinDate { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal? RechargeAmount { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal? ExpenseAmount { get; set; }
        /// <summary>
        /// 剩余金额
        /// </summary>
        public decimal RemainingAmount { get; set; }
        /// <summary>
        /// 项目次数
        /// </summary>
        public int? ProjectNumber { get; set; }
        /// <summary>
        /// 剩余次数
        /// </summary>
        public int? RemainingNumber { get; set; }
        /// <summary>
        /// 最后消费日期
        /// </summary>
        public DateTime? LastService { get; set; }
        /// <summary>
        /// 到店频次
        /// </summary>
        public int? BookTime { get; set; }
        /// <summary>
        /// 最后到店日期
        /// </summary>
        public DateTime? LastToDate { get; set; }
    }
}