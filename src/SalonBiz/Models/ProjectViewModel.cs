using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public class ProjectQModel : PagedModel
    {
        public ProjectQModel()
        {
            this.PagedIndex = 1;
            this.PagedSize = 20;
        }

        public int HostID { get; set; }
        public Int32 BranchID { get; set; }
        public string BrandCode { get; set; }
        public string Category { get; set; }
        public string ExtCategory { get; set; }
        public int? ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string CardType { get; set; }
        public string ProjectStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ProjectViewModel> ProjectList { get; set; }
        /// <summary>
        /// 用户项目销售、消耗一览表
        /// </summary>
        public List<ProjectSTAViewModel> StatList { get; set; }

        public List<ProjectSTAModel> SalesList { get; set; }

        public List<ProjectSTAModel> ServiceList { get; set; }
    }

    public class ProjectViewModel
    {
        public int ProjectID { get; set; }
        public int HostID { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        [Display(Name = "项目编码")]
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        public string Name { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        [Display(Name = "最低价")]
        public int MinUnit { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
        public string BrandValue { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        [Display(Name = "类别")]
        public string Category { get; set; }
        public string CategoryName { get; set; }
        /// <summary>
        /// 来源类型[基础类、合作类]
        /// </summary>
        [Display(Name = "来源类型")]
        public string ExtCategory { get; set; }
        public string ExtCategoryName { get; set; }
        [Display(Name = "扩展类别")]
        public int SecCategory { get; set; }
        public int HandicraftFee { get; set; }
        public decimal LowHandicraftFee { get; set; }

        /// <summary>
        /// 是否可以现消现耗， 1 可以 0 不可以
        /// </summary>
        [Display(Name = "即消即耗")]
        public int IsEntity { get; set; }
        /// <summary>
        /// 状态：上架，下架
        /// </summary>
        public string Status { get; set; }
        public string StatusValue { get; set; }
    }
}