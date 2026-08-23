using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalonCRM.Models
{

    /// <summary>
    /// 日报表
    /// </summary>
    public class DailyViewModel
    {
        public DateTime TheDay { get; set; }
        public int? RowNum { get; set; }
        public long MemberId { get; set; }
        public string MemberName { get; set; }
        public string CardNo { get; set; }
        public string Worker { get; set; }
        public string Type { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int? Flow { get; set; }
        /// <summary>
        /// 服务项目
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 消耗项目次数
        /// </summary>
        public int? ProjectNum { get; set; }
        public decimal? ExpenseAmount { get; set; }
        public int? ddType { get; set; }
        public DateTime? ExpenseTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<BookProjectSplit> Splits { get; set; }

        /// <summary>
        /// 实操
        /// </summary>
        public decimal? A1 { get; set; }
        /// <summary>
        /// 即销即耗
        /// </summary>
        public decimal? A2 { get; set; }
        /// <summary>
        /// 卡扣
        /// </summary>
        public decimal? A3 { get; set; }
        /// <summary>
        /// 面部
        /// </summary>
        public decimal? S1 { get; set; }
        /// <summary>
        /// 身体
        /// </summary>
        public decimal? S2 { get; set; }
        /// <summary>
        /// 仪器
        /// </summary>
        public decimal? S3 { get; set; }
        /// <summary>
        /// 家居产品
        /// </summary>
        public decimal? S4 { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public decimal? S5 { get; set; }
        /// <summary>
        /// 卡项
        /// </summary>
        public decimal? S6 { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal? T1 { get; set; }
        /// <summary>
        /// 刷卡
        /// </summary>
        public decimal? T2 { get; set; }
        /// <summary>
        /// 转账
        /// </summary>
        public decimal? T3 { get; set; }
        /// <summary>
        /// 欠款
        /// </summary>
        public decimal? T4 { get; set; }
        /// <summary>
        /// 老客户
        /// </summary>
        public decimal? N1 { get; set; }
        /// <summary>
        /// 新客户
        /// </summary>
        public decimal? N2 { get; set; }
    }


    /// <summary>
    /// 日报详细
    /// </summary>
    public class CustomerDailyViewModel
    {
        public DateTime TheDay { get; set; }
        public long MemberId { get; set; }
        public string MemberName { get; set; }
        public string CardNo { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string Type { get; set; }

        /// <summary>
        /// 消费项目
        /// </summary>
        public string ServiceProjectName { get; set; }
        public int? ServiceProjectNum { get; set; }
        public string Worker { get; set; }
        public ICollection<BookProjectSplit> Workers { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal? ExpenseAmount { get; set; }
        public DateTime? ExpenseTime { get; set; }


        /// <summary>
        /// 销售项目
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目次数
        /// </summary>
        public int? ProjectNum { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Debt { get; set; }
        public string ProjectSales { get; set; }
        public ICollection<AccountRecordSplit> Sales { get; set; }
        public DateTime? SalesTime { get; set; }

        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal? RechargeAmount { get; set; }
        public string RechangeType { get; set; }
        public string RechangeSaleman { get; set; }
        public ICollection<AccountRecordSplit> RechangeSales { get; set; }

        public DateTime? RechangeTime { get; set; }

    }


    /// <summary>
    /// 月度报表
    /// </summary>
    public class MonthViewModel
    {
        public DateTime TheMonth { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }

        /// <summary>
        /// 项目次数
        /// </summary>
        public int? ProjectNum { get; set; }

        /// <summary>
        /// 实操
        /// </summary>
        public decimal? A1 { get; set; }
        /// <summary>
        /// 即销即耗
        /// </summary>
        public decimal? A2 { get; set; }
        /// <summary>
        /// 卡扣
        /// </summary>
        public decimal? A3 { get; set; }
        /// <summary>
        /// 面部
        /// </summary>
        public decimal? S1 { get; set; }
        /// <summary>
        /// 身体
        /// </summary>
        public decimal? S2 { get; set; }
        /// <summary>
        /// 仪器
        /// </summary>
        public decimal? S3 { get; set; }
        /// <summary>
        /// 家居产品
        /// </summary>
        public decimal? S4 { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public decimal? S5 { get; set; }
        /// <summary>
        /// 卡项
        /// </summary>
        public decimal? S6 { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal? T1 { get; set; }
        /// <summary>
        /// 刷卡
        /// </summary>
        public decimal? T2 { get; set; }
        /// <summary>
        /// 转账
        /// </summary>
        public decimal? T3 { get; set; }
        /// <summary>
        /// 欠款
        /// </summary>
        public decimal? T4 { get; set; }
        /// <summary>
        /// 老客户
        /// </summary>
        public decimal? N1 { get; set; }
        /// <summary>
        /// 新客户
        /// </summary>
        public decimal? N2 { get; set; }
        /// <summary>
        /// 体验客户
        /// </summary>
        public decimal? N3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Flow { get;  set; }
    }


}