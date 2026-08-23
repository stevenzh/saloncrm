using System;
using System.Collections.Generic;

namespace SalonCRM.Models
{
    public class ObjectiveQModel
    {
        public List<ObjectiveViewModel> ObjectiveList { get; set; }
        public int BranchId { get; set; }
        public int DYear { get; set; }
        public int DMonth { get; set; }
    }

    public class ObjectiveViewModel
    {
        public long? ObjectiveId { get; set; }
        /// <summary>
        /// 1:门店 2:团队 3:顾问美容师
        /// </summary>
        public int Level { get; set; }
        public int OrganId { get; set; }

        public string OrganName { get; set; }
        /// <summary>
        /// 团队
        /// </summary>
        public int TeamId { get; set; }
        /// <summary>
        /// 美容师
        /// </summary>
        public string UserId { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 业绩保底指标（充值）
        /// </summary>
        public int? Accounts { get; set; }
        /// <summary>
        /// 业绩挑战指标
        /// </summary>
        public int? TopObjective { get; set; }
        /// <summary>
        /// 销售指标（项目销售）
        /// </summary>
        public int? SalesObjective { get; set; }
        /// <summary>
        /// 消耗指标（美容服务）
        /// </summary>
        public int? ServiceObjective { get; set; }
        public string ID { get; set; }
        public string ParentID { get; set; }
        // public List<ObjectiveViewModel> Workers { get; set; }

    }

}