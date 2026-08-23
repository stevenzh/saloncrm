using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    /// <summary>
    /// 角色[超级用户、管理员除外]
    /// </summary>
    public class ApplicationRole
    {
        public ApplicationRole() { }
        public ApplicationRole(string name)
        {
            this.Name = name;
        }
        public virtual string Id { get; set; }
        [Display(Name = "名称")]
        public virtual string Name { get; set; }
        [Display(Name = "说明")]
        public virtual string Description { get; set; }
        /// <summary>
        /// 总公司用户角色
        /// </summary>
        public virtual bool IsMajor { get; set; }
        public virtual Int32 HostID { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<MenuItem> Menus { get; set; }
    }
}