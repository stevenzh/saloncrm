using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalonCRM.Models
{
    /// <summary>
    /// 员工排名
    /// </summary>
    public class WorkerRankModel
    {
        public string Worker { get; set; }
        public string WorkerName { get; set; }
        /// <summary>
        /// 用户类型  1：美容师， 2：账户， 3：顾问
        /// </summary>
        public string Type { get; set; }
        public string Branch { get; set; }

        public DateTime TheDay { get; set; }
        /// <summary>
        /// 业绩（所有现金入账）
        /// </summary>
        public decimal? InCome { get; set; }
        /// <summary>
        /// 虚耗（所有购买的服务项目）
        /// </summary>
        public decimal? Expend { get; set; }
        /// <summary>
        /// 现消现耗（实物直接购买）
        /// </summary>
        public decimal? EntityExpend { get; set; }
        /// <summary>
        /// 实操
        /// </summary>
        public decimal? ServiceExpend { get; set; }
        /// <summary>
        /// 顾问消耗业绩
        /// </summary>
        public decimal? SalesServiceExpend { get; set; }
        /// <summary>
        /// 服务人次
        /// </summary>
        public int? ServiceRC { get; set; }
        /// <summary>
        /// 服务人头数
        /// </summary>
        public int? ServiceRT { get; set; }
        /// <summary>
        /// 服务项目量
        /// </summary>
        public int? ServiceXC { get; set; }
        /// <summary>
        /// 顾问接待人数
        /// </summary>
        public int? SaleRS { get; set; }
    }

    /// <summary>
    /// 员工排名详细
    /// </summary>
    public class WorkerRankDetailModel
    {
        public string Worker { get; set; }
        public string MemberName { get; set; }

        public DateTime TheTime { get; set; }
        /// <summary>
        /// 销售业绩（充值）
        /// </summary>
        public decimal? InCome { get; set; }
        public DateTime? IncomeTime { get; set; }

        /// <summary>
        /// 卡扣（购买项目）
        /// </summary>
        public decimal? InSales { get; set; }
        public DateTime? SalesTime { get; set; }
        /// <summary>
        /// 服务项目
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 消耗业绩
        /// </summary>
        public decimal? Expend { get; set; }
        public decimal? SalesExpend { get; set; }
        public string ProjetSaleType { get; set; }
        public DateTime? ExpendTime { get; set; }
        /// <summary>
        /// 服务项目量
        /// </summary>
        public int? ServiceXC { get; set; }
        //public int IsEntity { get; set; }
        public decimal? HandicraftFee { get; set; }
    }
}
