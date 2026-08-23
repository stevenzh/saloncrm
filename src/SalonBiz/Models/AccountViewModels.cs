using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "商户编码")]
        public string HostCode { get; set; }
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "下次自动登陆?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "姓名")]
        public string UserCnName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "输入的 {0} 长度不少于 {2} 个字符.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "两次输入的密码不相同.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "选择商户")]
        public int HostId { get; set; }
        public int OrganId { get; set; }
        public string Type { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子信箱")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "输入的 {0} 长度不少于 {2} 个字符.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "两次输入的密码不相同.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class UserAdminViewModel
    {
        public int HostId { get; set; }
        public int BranchId { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserCnName { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public IList<EditUserViewModel> UserList { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }
        public int HostId { get; set; }
        public string HostName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "用户名")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "门市")]
        public int OrganId { get; set; }
        [Display(Name = "门市")]
        public string OrganName { get; set; }
        [Display(Name = "电子邮件")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "姓名")]
        public string UserCnName { get; set; }
        [StringLength(100, ErrorMessage = "设置 {0} 字符长度不少于 {2} 个字符.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        public string Type { get; set; }
        public string TypeValue { get; set; }
        [Display(Name = "状态")]
        public string Status { get; set; }
        public string StatusValue { get; set; }
        [Display(Name = "职务")]
        public string Position { get; set; }
        [Display(Name = "手机号码")]
        public string MobileNumber { get; set; }
        [Display(Name = "是否总部")]
        public Boolean IsMajorOrgan { get; set; }
        public string IsAdminUser { get; set; }
        [Display(Name = "加入时间")]
        public System.DateTime? JoinDate { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        [Display(Name = "离职时间")]
        public Nullable<DateTime> ResignDate { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> RolesList { get; set; }
    }
}