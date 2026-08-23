using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{

    /// <summary>
    /// 货品
    /// </summary>
    public partial class Goods
    {
        public int GoodsID { get; set; }
        public int HostID { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string GoodsCode { get; set; }
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "计量单位")]
        public string Unit { get; set; }
        public string Category { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [Display(Name = "品牌")]
        public string Brand { get; set; }
        /// <summary>
        /// 1：有效 0 无效
        /// </summary>
        [Display(Name = "是否有效")]
        public int IsVaild { get; set; }

        public virtual ICollection<BookGoods> BookGoods { get; set; }
        public virtual ICollection<MemberProjectGoods> MemberProjectGoods { get; set; }

        public virtual ICollection<ProjectGoods> ProjectGoods { get; set; }
    }
}