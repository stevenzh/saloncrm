using System;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{

    /// <summary>
    /// 终端
    /// </summary>
    public partial class Client
    {
        public int ClientID { get; set; }
        public int HostID { get; set; }
        [Display(Name = "门店")]
        public Nullable<int> OrganID { get; set; }
        [Display(Name = "终端编号")]
        public string MobileGUID { get; set; }
        [Display(Name = "手机号码")]
        public string MobileNumber { get; set; }
        [Display(Name = "终端说明")]
        public string MobileModel { get; set; }
        /// <summary>
        /// 0：有效 1 无效
        /// </summary>
        [Display(Name = "是否有效")]
        public string IsVaild { get; set; }
        public string UserId { get; set; }
        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTime LastSignIn { get; set; }

        public virtual Host Host { get; set; }
    }
}