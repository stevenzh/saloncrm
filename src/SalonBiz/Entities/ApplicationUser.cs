using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public class ApplicationUser
    {
        public virtual string Id { get; set; }
        /// <summary>
        /// 所属商户
        /// </summary>
        public virtual Int32 HostId { get; set; }
        /// <summary>
        /// 所属门店
        /// </summary>
        public virtual Int32 OrganId { get; set; }
        [Required]
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        [Required]
        public virtual string Password { get; set; }

        /// <summary>
        /// 用户类型  1：美容师，2：账户，3：顾问，4：店长
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 特殊用户 0：普通 1：管理员 2：超级用户
        /// </summary>
        public virtual Int32 IsAdminUser { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string UserCnName { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public virtual string Rank { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        public virtual Nullable<DateTime> JoinDate { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        public virtual Nullable<DateTime> ResignDate { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public virtual string Position { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public virtual string MobileNumber { get; set; }
        /// <summary>
        /// 隶属总部还是门店
        /// </summary>
        public virtual Boolean IsMajorOrgan { get; set; }
        public virtual Boolean IsActive { get; set; }
        public virtual DateTime CreateDate { get; set; }
        /// <summary>
        /// 离入职状态 1:在职 2:离职
        /// </summary>
        public virtual string Status { get; set; }
        public virtual string OpenID { get; set; }
        public virtual ICollection<ApplicationRole> Roles { get; set; }
        public virtual ICollection<AccountRecordSplit> AccountRecordSplits { get; set; }
        public virtual ICollection<BookProjectSplit> BookProjectSplits { get; set; }

    }
}