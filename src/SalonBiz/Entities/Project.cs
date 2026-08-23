using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{

    /// <summary>
    /// 项目
    /// </summary>
    public class Project
    {
        public Project()
        {
            this.BookProjects = new List<BookProject>();
            this.MemberGives = new List<MemberGive>();
        }
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
        /// 手工费
        /// </summary>
        [Display(Name = "手工费")]
        public int HandicraftFee { get; set; }
        /// <summary>
        /// 手工费（最低）
        /// </summary>
        [Display(Name = "最低手工费")]
        public decimal LowHandicraftFee { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [Display(Name = "品牌")]
        public string Brand { get; set; }
        /// <summary>
        /// 类别 [面部、身体、仪器、家居产品、其他]
        /// </summary>
        [Display(Name = "类别")]
        public string Category { get; set; }
        /// <summary>
        /// 基础类、合作类
        /// </summary>
        [Display(Name = "来源类型")]
        public string ExtCategory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "扩展类别")]
        public int SecCategory { get; set; }
        /// <summary>
        /// 是否可以现消现耗， 1 是 0 否
        /// </summary>
        [Display(Name = "即消即耗")]
        public int IsEntity { get; set; }
        /// <summary>
        /// 状态：20:上架，30:下架
        /// </summary>
        [Display(Name = "状态")]
        public string Status { get; set; }
        /// <summary>
        /// 套装次数
        /// </summary>
        public int Count { get; set; }


        public virtual Host Host { get; set; }
        public virtual ICollection<BookProject> BookProjects { get; set; }
        public virtual ICollection<MemberGive> MemberGives { get; set; }
        public virtual ICollection<CardTmplProject> TmplProjects { get; set; }
        public virtual ICollection<ProjectGoods> ProjectGoods { get; set; }

    }
}