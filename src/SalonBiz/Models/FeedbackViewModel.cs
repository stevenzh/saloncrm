using System;
using System.Collections.Generic;

namespace SalonCRM.Models
{
    public class FeedbackViewModel
    {
        public int HostId { get; set; }
        public long FeedbackId { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public long MemberId { get; set; }
        public string MemberName { get; set; }
        public string CardNo { get; set; }
        public string MobileNumber { get; set; }
        public string TypeValue { get; set; }
        public string LevelValue { get; set; }
        public string Status { get; set; }
        public string StatusValue { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        /// <summary>
        /// 类型 - 沟通意图
        /// 投诉解决、休眠唤醒、服务邀约、节日问候……
        /// </summary>
        public string Purpose { get; set; }
        /// <summary>
        /// 结果
        /// 成功，失败
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 下次沟通时间
        /// </summary>
        public Nullable<System.DateTime> NextDate { get; set; }
        /// <summary>
        /// 回访时间
        /// </summary>
        public Nullable<DateTime> CreatedDate { get; set; }
        /// <summary>
        /// 回访人ID
        /// </summary>
        public string CallUserId { get; set; }
        public string CallUserName { get; set; }
        /// <summary>
        /// 回访内容
        /// </summary>
        public string Centent { get; set; }
        /// <summary>
        /// 跟进时间
        /// </summary>
        public Nullable<System.DateTime> FeedbackDate { get; set; }

    }

    public class FeedbackQModel
    {
        public FeedbackQModel()
        {
        }
        public int BranchId { get; set; }
        public string Name { get; set; }
        public string CardNo { get; set; }
        public string Mobile { get; set; }
        public string Purpose { get; set; }
        public Nullable<System.DateTime> FeedbackDate { get; set; }

        public IList<FeedbackViewModel> FeedbackList { get; set; }
        public string SalesmanId { get; set; }
        public string BeauticianId { get; set; }
    }
}