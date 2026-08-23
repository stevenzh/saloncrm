using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    /// <summary>
    /// 综合卡使用
    /// </summary>
    public partial class MemberCardProject
    {
        public long MemberCardProjectId { get; set; }
        public long MemberCardId { get; set; }
        public int ProjectID { get; set; }
        [Display(Name = "单价")]
        public decimal UnitPrice { get; set; }

        public virtual MemberCard Card { get; set; }
        public virtual Project Project { get; set; }
    }
}
