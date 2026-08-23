using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalonCRM.Models
{
    /// <summary>
    /// 客户信息
    /// </summary>
    public partial class WxMember
    {
        #region 客户信息
        public long WxMemberID { get; set; }
        public int HostID { get; set; }
        /// <summary>
        /// 微信最后消息时间
        /// </summary>
        public Nullable<System.DateTime> LastMessageTime { get; set; }
        public string Binding { get; set; }
        public string EmployeeID { get; set; }
        /// <summary>
        /// 关联编号
        /// </summary>
        public long MemberID { get; set; }
        #endregion

        #region 微信信息
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
        public Nullable<System.DateTime> UnsubscribeTime { get; set; }

        #endregion
    }
}