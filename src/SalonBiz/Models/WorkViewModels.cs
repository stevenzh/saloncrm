using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SalonCRM.Models
{

    public class WorkerQModel
    {
        public int HostID { get; set; }
        public Int32 BranchID { get; set; }
        public string WorkerType { get; set; }
        public string WorkerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MemberType { get; set; }
        public string MemberNewType { get; set; }
        public string Sort { get; set; }
        public int WorkerID { get; set; }
        public List<WorkerRankModel>  WorkerList { get; set; }
    }

    public class WorkerViewModel
    {
        public string Id { get; set; }
        [Display(Name = "用户名")]
        public string UserName { get; set; }
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "姓名")]
        public string UserCnName { get; set; }
        [StringLength(100, ErrorMessage = "设置 {0} 字符长度不少于 {2} 个字符.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "选择公司")]
        public int HostId { get; set; }
        [Required]
        [Display(Name = "门市")]
        public int OrganId { get; set; }
        /// <summary>
        /// 用户类型 2:账户 1:美容师 3：顾问
        /// </summary>
        [Display(Name = "工种")]
        public string Type { get; set; }
        /// <summary>
        /// 离入职状态 V01在职 V02离职
        /// </summary>
        [Display(Name = "状态")]
        public string Status { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public virtual string Rank { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        [Display(Name = "入职时间")]
        public Nullable<DateTime> JoinDate { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        [Display(Name = "离职时间")]
        public Nullable<DateTime> ResignDate { get; set; }
        [Display(Name = "职务")]
        public string Position { get; set; }
        [Display(Name = "手机号码")]
        public string MobileNumber { get; set; }
        [Display(Name = "是否总部")]
        public Boolean IsMajorOrgan { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }

}