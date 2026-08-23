using SalonCRM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalonBiz.Models.Stat
{
    public class CardQModel
    {
        public int HostID { get; set; }
        public Int32 BranchID { get; set; }
        public string CardType { get; set; }
        public string CardTmplID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MemberType { get; set; }
        public string MemberNewType { get; set; }
        public int WorkerID { get; set; }
        public List<CardViewModel> StatList { get; set; }

        public List<MemberCard> CardList { get; set; }

    }
    public class CardViewModel
    {
        public int OrganID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string TypeValue { get; set;  }
        public Nullable<int> Quantity { get; set; }
        /// <summary>
        /// 购卡金额合计
        /// </summary>
        public Nullable<decimal> Amount { get; set; }
        public int Person { get; set; }
        /// <summary>
        /// 储值卡金额合计
        /// </summary>
        public decimal Amt { get; set; }
        /// <summary>
        /// 项目购买次数
        /// </summary>
        public int BookTime { get; set; }
        /// <summary>
        /// 项目剩余次数
        /// </summary>
        public int LastCount { get; set; }

        //public int T1 { get; set; }
        //public int T2 { get; set; }
        //public int T3 { get; set; }
        //public int T4 { get; set; }
        //public int T5 { get; set; }
        //public int T6 { get; set; }
        //public int T7 { get; set; }
        //public int T8 { get; set; }
        //public int T9 { get; set; }


        //public decimal M1 { get; set; }
        //public decimal M2 { get; set; }
        //public decimal M3 { get; set; }
        //public decimal M4 { get; set; }
        //public decimal M5 { get; set; }
        //public decimal M6 { get; set; }
        //public decimal M7 { get; set; }
        //public decimal M8 { get; set; }
        //public decimal M9 { get; set; }

    }
}
