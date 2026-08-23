using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public partial class CardTmplProject
    {
        /// <summary>
        /// 
        /// </summary>
        public int TmplProjectID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TmplID { get; set; }
        public int ProjectID { get; set; }
        public decimal Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public virtual CardTmpl Card { get; set; }
        public virtual Project Project { get; set; }
    }
}
