using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public partial class MemberMessage
    {
        public int MessageID { get; set; }
        public int HostID { get; set; }
        public string OpenID { get; set; }
        public string MsgType { get; set; }
        public string Content { get; set; }
        public string FileUrl { get; set; }
        /// <summary>
        /// 微信外发 0:客户发送到服务号, 1:外发给客户,2:自动模板外发
        /// </summary>
        public int InOut { get; set; }
        /// <summary>
        /// 是否回复
        /// </summary>
        public string IsCallBack { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
