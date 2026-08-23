using System.Collections.Generic;

namespace SalonCRM.Models
{
    /// <summary>
    /// 客户卡模板
    /// </summary>
    public partial class CardTmpl
    {
        /// <summary>
        /// 
        /// </summary>
        public int TmplID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int HostID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public int IsVaild { get; set; }

        public string Remark { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public virtual ICollection<CardTmplProject> Projects { get; set; }
    }
}