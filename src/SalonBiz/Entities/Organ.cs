using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonCRM.Models
{
    /// <summary>
    /// 机构组织（公司、门店）
    /// </summary>
    public partial class Organ
    {
        public int OrganID { get; set; }
        public int HostID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public int Level { get; set; }
        [Display(Name = "门店名称")]
        public string Name { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Manager { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        public int IsVaild { get; set; }
        public Nullable<int> ClientNum { get; set; }
        public virtual Host Host { get; set; }
        public virtual ICollection<AccountRecord> AccounRecords { get; set; }

        [NotMapped]
        public virtual ICollection<Book> Books { get; set; }
    }
}