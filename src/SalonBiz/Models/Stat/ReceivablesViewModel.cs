using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalonCRM.Models
{
    public class ReceivablesQModel
    {
        public int BranchId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Salesman { get; set; }
        public List<ReceivablesViewModel> ReceivablesList { get; set; }
    }

    /// <summary>
    /// 应收
    /// </summary>
    public class ReceivablesViewModel
    {
        public string BranchName { get; set; }
        public string MemberName { get; set; }
        public decimal Sales { get; set; }
        public decimal Cash { get; set; }
        public decimal CardMoney { get; set; }
        public decimal Transfer { get; set; }
        public ICollection<AccountRecordSplit> _Salesman { get; set; }
        public string Salesman { get; set; }
        public string PaymentType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}