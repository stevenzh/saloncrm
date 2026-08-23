using System;
using System.Linq;
using System.Data;
using Common.Logging;
using Quartz;
using SalonCRM.Models;
using SalonCRM.Manager;

namespace SalonCRM.Job
{

    /// <summary>
    /// 5分钟
    /// </summary>
    public class DayJob : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(FiveJob));

        static ApplicationDbContext dbcontent = new ApplicationDbContext();

        public void Execute(IJobExecutionContext context)
        {
            logger.Info("DayJob running...");

            // 积分失效扣除

            // 赠送项目过期扣除

            // 综合限时卡失效

            // dbcontent.SaveChanges();
            logger.Info("DayJob run finished.");
        }
    }
}