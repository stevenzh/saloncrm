using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public class HostQModel
    {
        public List<HostViewModel> HostList { get; set; }
    }
    public class HostViewModel
    {
        public int HostID { get; set; }
        /// <summary>
        /// 商家编号
        /// </summary>
        [Required]
        [Display(Name = "商家英文编码")]
        public string HostCode { get; set; }
        [Required]
        [Display(Name = "商家名称")]
        public string Name { get; set; }
        /// <summary>
        /// 店铺数量
        /// </summary>
        [Display(Name = "店铺数量")]
        public Nullable<int> BranchNum { get; set; }
        /// <summary>
        /// 终端数量
        /// </summary> 
        [Display(Name = "终端数量")]
        public Nullable<int> ClientNum { get; set; }
        /// <summary>
        /// 行业
        /// </summary>
        [Display(Name = "行业")]
        public string Industry { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        [Display(Name = "省份")]
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [Display(Name = "城市")]
        public string City { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        [Display(Name = "URL")]
        public string Url { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        [Display(Name = "负责人")]
        public string Manager { get; set; }
        /// <summary>
        /// 起始日期
        /// </summary>
        [Display(Name = "起始日期")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        [Display(Name = "结束日期")]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        public string Address { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        [Display(Name = "是否有效")]
        public int IsVaild { get; set; }

        public List<HostProfileModel> Profiles { get; set; }
    }

    public class HostProfileModel
    {
        public int ProfileID { get; set; }
        public int HostID { get; set; }
        public string PropertyValue { get; set; }
        public string PropertyText { get; set; }
    }
}