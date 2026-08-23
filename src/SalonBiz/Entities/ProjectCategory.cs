using System;
using System.Collections.Generic;

namespace SalonCRM.Models
{
    public class ProjectCategory
    {
        public virtual int Id { get; set; }
        public virtual Nullable<int> ParentId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int Level { get; set; }
        public virtual Boolean IsActive { get; set; }
    }
}