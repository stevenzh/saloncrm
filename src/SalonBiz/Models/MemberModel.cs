using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalonCRM.Models
{
    public partial class MemberModel
    {
        public long MemberID { get; set; }
        public int HostID { get; set; }

        #region 微信客户信息
        public string OpenID { get; set; }
        public string NickName { get; set; }
        public Nullable<int> Sex { get; set; }
        public string Language { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string HeadImgUrl { get; set; }
        public System.DateTime SubscribeTime { get; set; }
        public string Subscribe { get; set; }
        public string SubscribeValue { get; set; }
        #endregion

        /// <summary>
        /// 0:默认 1:客户申请 2:绑定通过审核 3:申请未通过
        /// </summary>
        [Display(Name = "淘宝绑定")]
        public string Binding { get; set; }
        public string BindingValue { get; set; }
        public int? SalesID { get; set; }
        [Display(Name = "美容顾问")]
        public string Sales { get; set; }
        public Nullable<System.DateTime> UnsubscribeTime { get; set; }
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }
        [Display(Name = "审核")]
        public int Approved { get; set; }
        /// <summary>
        /// 淘宝订单交易号
        /// </summary>
        public string Tid { get; set; }
        public string EmployeeID { get; set; }

        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string LogoUrl { get; set; }
        public bool HideShared { get; set; }
        public int? QrID { get; set; }
        public DateTime? LastMessageTime { get; set; }
        ///// <summary>
        ///// 微信消息
        ///// </summary>
        //public IList<MemberMessageModel> Messages { get; set; }
        ///// <summary>
        ///// 微信消息
        ///// </summary>
        //public PagedList<MemberMessageModel> MessagePageList { get; set; }

    }
}
