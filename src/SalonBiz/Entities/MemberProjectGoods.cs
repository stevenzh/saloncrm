using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonCRM.Models
{
    public class MemberProjectGoods
    {
        public long MemberProjectGoodsID { get; set; }
        public long MemberProjectId { get; set; }
        public int GoodsID { get; set; }
        public int ProjectID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }
        public virtual Goods Goods { get; set; }
        public virtual MemberProject MemberProject { get; set; }
    }
}