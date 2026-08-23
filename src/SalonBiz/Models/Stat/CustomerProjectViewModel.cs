using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalonCRM.Models
{
    /// <summary>
    /// 客户消费 消耗统计
    /// </summary>
    public class CustomerProjectViewModel
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
        /// 充值金额
        /// </summary>
        public decimal? RechargeAmount { get; set; }
        /// <summary>
        /// 剩余金额
        /// </summary>
        public decimal? RemaindAmount { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal? ExpenseAmount { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目次数
        /// </summary>
        public int? ProjectNumber { get; set; }
        /// <summary>
        /// 到店频次
        /// </summary>
        public int? BookTime { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

}