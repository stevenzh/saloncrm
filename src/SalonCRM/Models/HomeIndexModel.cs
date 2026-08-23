using SalonCRM.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalonCRM.Models
{
    public class HomeIndexModel
    {
        public int BranchID { get; set; }
        /// <summary>
        /// 本月业绩指标
        /// </summary>
        public int MonthObjective { get; set; }
        /// <summary>
        /// 本月业绩
        /// </summary>
        public decimal MonthsIncome { get; set; }
        /// <summary>
        /// 本月销售
        /// </summary>
        public decimal MonthSales { get; set; }
        /// <summary>
        /// 本月消耗
        /// </summary>
        public decimal MonthsService { get; set; }
        /// <summary>
        /// 今日预约 到店客户
        /// </summary>
        public decimal MonthWvie { get; set; }
        /// <summary>
        /// 明天预约人数
        /// </summary>
        public int NextDayAp { get; set; }
        /// <summary>
        /// 今天新会员
        /// </summary>
        public int NewMember { get; set; }
        /// <summary>
        /// 今日回访数量
        /// </summary>
        public int TodayFeedback { get; set; }
        ///// <summary>
        ///// 当月项目量
        ///// </summary>
        //public int MonthProject { get; set; }
        /// <summary>
        /// 当天储值卡购买
        /// </summary>
        public decimal DayCardAmount { get; set; }
        /// <summary>
        /// 当天储值卡购买
        /// </summary>
        public decimal DayZhCardAmount { get; set; }
        /// <summary>
        /// 客户总人数
        /// </summary>
        public int MemberCount { get; set; }


        public DailyViewModel DayStat { get; set; }
        public IList<Appointment> Appointment { get; set; }

        public List<CommonSample> MemberType { get; set; }
        public List<CommonSample> MemberStatus { get; set; }
        public IList<Member> BirtAlert { get; set; }
        public MonthViewModel MonthStat { get; set; }
    }
}