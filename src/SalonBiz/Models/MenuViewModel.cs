using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public class MenuQModel
    {
        public List<MenuViewModel> MenuList { get; set; }
        public int BranchId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class MenuViewModel
    {
        public int MenuId { get; set; }
        public Nullable<int> ParentId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string MenuPath { get; set; }
        public int Level { get; set; }
        public int SortOrder { get; set; }
        public Boolean IsActive { get; set; }
        public string Icon { get; set; }
        public string SiteNav { get; set; }
        public string SiteNavNext { get; set; }
    }

}