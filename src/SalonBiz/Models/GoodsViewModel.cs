using SalonCRM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalonCRM.Models
{
    public class GoodsQModel : PagedModel
    {
        public GoodsQModel()
        {
            this.PagedIndex = 1;
            this.PagedSize = 20;
        }

        public int HostID { get; set; }
        public string GoodsName { get; set; }
        public string BrandCode { get; set; }
        public string Category { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<GoodsViewModel> GoodsList { get; set; }

        public List<GoodsViewModel> GoodsDetailList { get; set; }
        public int? BranchID { get; set; }
        public int? ProjectID { get; set; }
        public long GoodsID { get; set; }
    }

    public partial class GoodsViewModel
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
        public string CategoryText { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [Display(Name = "品牌")]
        public string Brand { get; set; }
        /// <summary>
        /// 0：有效 1 无效
        /// </summary>
        [Display(Name = "是否有效")]
        public int IsVaild { get; set; }

        public virtual ICollection<ProjectGoods> ProjectGoods { get; set; }

        public decimal Quantity { get; set; }
    }
}
