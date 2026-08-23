using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalonCRM.Models
{
    /// <summary>
    /// 欠款查询
    /// </summary>
    public class DebtQModel
    {
        public int HostId { get; set; }
        public int BranchId { get; set; }
        public string MemberName { get; set; }
        public string CardNo { get; set; }
        public string Salesman { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 是否结清 0:未结清 1 结清
        /// </summary>
        public string Payment { get; set; }

        public IList<DebtViewModel> DebtList { get; set; }
    }

    /// <summary>
    /// 欠款记录
    /// </summary>
    public class DebtViewModel
    {
        public long MemberCardId { get; set; }
        public long MemberProjectId { get; set; }
        public string BranchName { get; set; }
        public string MemberName { get; set; }
        public string CardNo { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 购买次数
        /// </summary>
        public int Quantity { get; set; }

        public string CardTitle { get; set; }
        public ICollection<AccountRecordSplit> _Salesman { get; set; }
        /// <summary>
        /// 姓名列表
        /// </summary>
        public string Salesman { get; set; }
        /// <summary>
        /// 还款使用
        /// </summary>
        public string SalesId { get; set; }
        /// <summary>
        /// 还款使用
        /// </summary>
        public string BeauticianId { get; set; }

        public string SalesRadix { get; set; }
        public string BeauticianRadix { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Debt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }
    }
}