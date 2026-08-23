using System;
using System.Collections.Generic;

namespace SalonCRM.Models
{
    public class MenuItem
    {
        public virtual int Id { get; set; }
        public virtual Nullable<int> ParentId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string MenuPath { get; set; }
        public virtual string Icon { get; set; }
        public virtual int Level { get; set; }
        public virtual int SortOrder { get; set; }
        public virtual Boolean IsActive { get; set; }
        public virtual ICollection<ApplicationRole> Roles { get; set; }

        /// <summary>
        /// 大分类
        /// </summary>
        public virtual string SiteNav { get; set; }
        /// <summary>
        /// 二级分类
        /// </summary>
        public virtual string SiteNavNext { get; set; }
    }
}