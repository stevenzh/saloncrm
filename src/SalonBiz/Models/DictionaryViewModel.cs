using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public class DictionaryQModel : PagedModel
    {
        public DictionaryQModel()
        {
            this.PagedIndex = 1;
            this.PagedSize = 20;
        }
        public string Category { get; set; }
        public string FCategory { get; set; }
        public List<DictionaryViewModel> DictionayList { get; set; }
    }

    public class DictionaryViewModel
    {
        /// <summary>
        /// 用于参数传递
        /// </summary>
        public string Category { get; set; }
        public int HostId { get; set; }
        public long TypeId { get; set; }
        [Display(Name = "标识")]
        public string Identifier { get; set; }
        [Display(Name = "标识")]
        public string IdentifierName { get; set; }
        [Required]
        [Display(Name = "编码")]
        public string KeyValue { get; set; }
        [Required]
        [Display(Name = "名词")]
        public string Contents { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Display(Name = "是否有效")]
        public int IsVaild { get; set; }
        [Display(Name = "权重")]
        public int SortOrder { get; set; }
        [Display(Name = "默认值")]
        public bool IsDefault { get; set; }

        #region 客户等级
        public string MinAmount { get; set; }
        public string MaxAmount { get; set; }
        #endregion

        #region 客户状态
        public string FMonth { get; set; }
        public string SMonth { get; set; }
        public string MonthSet { get; set; }
        #endregion

    }
}