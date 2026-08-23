using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public partial class ProjectGoods
    {
        public int ProjectGoodsID { get; set; }
        public int GoodsID { get; set; }
        public int ProjectID { get; set; }
        public decimal Quantity { get; set; }
        public virtual Goods Goods { get; set; }
        public virtual Project Project { get; set; }
    }
}
