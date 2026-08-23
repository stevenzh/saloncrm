using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    /// <summary>
    /// 词典
    /// </summary>
    public partial class Dictionary
    {
        public long TypeId { get; set; }
        public int HostId { get; set; }
        [Display(Name = "标识")]
        public string Identifier { get; set; }
        [Display(Name = "编码")]
        public string KeyValue { get; set; }
        [Display(Name = "名词")]
        public string Contents { get; set; }
        public string Shell { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Display(Name = "是否有效")]
        public int IsVaild { get; set; }
        public int SortOrder { get; set; }
        public bool IsDefault { get; set; }
    }
}