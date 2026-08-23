using System;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{

    /// <summary>
    /// 回访
    /// </summary>
    public class Feedback
    {
        public int HostId { get; set; }

        public long FeedbackId { get; set; }
        public long MemberId { get; set; }
        /// <summary>
        /// 类型 - 沟通意图
        /// 投诉解决、休眠唤醒、服务邀约、节日问候……
        /// </summary>
        [Display(Name = "回访类型")]
        public string Purpose { get; set; }
        /// <summary>
        /// 联系方式 电话|微信
        /// </summary>
        public string LinkWay { get; set; }
        /// <summary>
        /// 结果 成功|失败
        /// </summary>
        [Display(Name = "回访结果")]
        public string Result { get; set; }
        /// <summary>
        /// 下次沟通时间
        /// </summary>
        [Display(Name = "下次回访时间")]
        public Nullable<System.DateTime> NextDate { get; set; }
        /// <summary>
        /// 回访时间
        /// </summary>
        [Display(Name = "回访时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 回访人ID
        /// </summary>
        [Display(Name = "回访人")]
        public string CallUserId { get; set; }
        /// <summary>
        /// 回访内容
        /// </summary>
        [Display(Name = "回访内容")]
        public string Centent { get; set; }
        public int BranchId { get; set; }


        public virtual Member Member { get; set; }
    }
}