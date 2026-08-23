using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public class ClientQModel : PagedModel
    {
        public IList<ClientViewModel> ClientList { get; set; }
    }
    public class ClientViewModel
    {
        public int ClientID { get; set; }
        public int HostID { get; set; }
        public Nullable<int> OrganID { get; set; }
        [Display(Name = "终端编号")]
        public string MobileGUID { get; set; }
        [Display(Name = "手机号码")]
        public string MobileNumber { get; set; }
        public string MobileModel { get; set; }
        /// <summary>
        /// 0 有效 1 无效
        /// </summary>
        [Display(Name = "是否有效")]
        public string IsVaild { get; set; }
        public string IsVaildValue { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "公司名称")]
        public string HostName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "门店名称")]
        public string BranchName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTime LastSignIn { get; set; }
    }

}