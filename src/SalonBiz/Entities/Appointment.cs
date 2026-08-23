using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonCRM.Models
{
    /// <summary>
    /// 预约, 多人服务
    /// </summary>
    public partial class Appointment
    {
        public long AppointmentID { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public int HostID { get; set; }
        /// <summary>
        /// 门店ID
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// 终端ID
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public long MemberID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        [NotMapped]
        public string CardNo { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime BookDate { get; set; }
        /// <summary>
        /// 预约项目 ,ProjectId 逗号分隔
        /// </summary>
        public string Projects { get; set; }
        [NotMapped]
        public string ProjectNames { get; set; }
        /// <summary>
        /// 顾问 单人
        /// </summary>
        public string Salesman { get; set; }
        /// <summary>
        /// 服务人员  多人 逗号分割
        /// </summary>
        public string Wokers { get; set; }
        /// <summary>
        /// 预约房间
        /// </summary>
        public string BookRooms { get; set; }
        /// <summary>
        /// 状态 0：预约 1：成交 2：取消 3:改约
        /// </summary>
        public string BookStatus { get; set; }
        // 成功消费ID
        public Nullable<long> BookId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 审核状态  0 未审核  1 已审核
        /// </summary>
        public int Approved { get; set; }


        public virtual Member Member { get; set; }
    }
}