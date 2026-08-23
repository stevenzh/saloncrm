using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "角色名称")]
        public string Name { get; set; }
        public int HostID { get; set; }
        [Display(Name = "总部角色")]
        public bool IsMajor { get; set; }
        public string MenuItems { get; set; }
        public List<RoleMenuViewModel> RoleMenus { get; set; }
    }

    public class RoleMenuViewModel
    {
        public RoleMenuViewModel() {
            open = true;
            IsActive = false;
        }
        public int MenuId { get; set; }
        public Nullable<int> ParentId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool open { get; set; }

    }
}