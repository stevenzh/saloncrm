using System;
using System.Collections.Generic;

namespace SalonCRM.Models
{
    /// <summary>
    /// 操作日志
    /// 包含换卡\转卡
    /// </summary>
    public partial class EventLog
    {
        public long LogId { get; set; }
        public int HostId { get; set; }
        public int BranchId { get; set; }
        public string UserId { get; set; }
        public string ClientId { get; set; }
        public Nullable<long> MemberId { get; set; }
        /// <summary>
        /// 1:建卡 2:赠积分 3:送项目 4:换卡 5:转卡 6:终端登录 7:充值 8 :购卡 9:购买项目 10:消耗 99:其他
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Shell { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDate { get; set; }

    }


    /// <summary>
    /// 用于数据库保存Json的实例化
    /// </summary>
    public class EventLogShell
    {
        /// <summary>
        /// 原始卡号
        /// </summary>
        public string OriginalCardNo { get; set; }
        /// <summary>
        /// 新卡号
        /// </summary>
        public string NewCardNo { get; set; }

        /// <summary>
        /// 原始项目
        /// </summary>
        public string OriginalProjects { get; set; }
        /// <summary>
        /// 原始项目名称
        /// </summary>
        public string OriginalProjectNames { get; set; }
        /// <summary>
        /// 原始项目累计金额
        /// </summary>
        public decimal OriginalPrjAmt { get; set; }
        /// <summary>
        /// 新项目
        /// </summary>
        public long NewMemberProjectId { get; set; }
        /// <summary>
        /// 新项目名称
        /// </summary>
        public string NewProjects { get; set; }
        /// <summary>
        /// 新项目累计金额
        /// </summary>
        public decimal NewPrjAmt { get; set; }

    }
}