namespace SalonCRM.Models
{
    /// <summary>
    /// 目标
    /// </summary>
    public partial class Objective
    {
        public Objective()
        {
            Level = 1;
        }
        public long ObjectiveId { get; set; }
        /// <summary>
        /// 1、门店 2 、团队 3美容师
        /// </summary>
        public int Level { get; set; }
        public int OrganId { get; set; }
        /// <summary>
        /// 团队
        /// </summary>
        public int TeamId { get; set; }
        /// <summary>
        /// 美容师
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 年
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 业绩保底指标
        /// </summary>
        public int Accounts { get; set; }
        /// <summary>
        /// 业绩挑战目标
        /// </summary>
        public int TopObjective { get; set; }

        /// <summary>
        /// 销售指标
        /// </summary>
        public int SalesObjective { get; set; }
        /// <summary>
        /// 消耗指标
        /// </summary>
        public int ServiceObjective { get; set; }
    }
}