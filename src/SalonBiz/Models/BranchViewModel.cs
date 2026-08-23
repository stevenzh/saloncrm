using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public class BranchQModel
    {
        public int HostID { get; set; }
        public int BranchID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Sort { get; set; }
        public IList<BranchViewModel> BranchList { get; set; }
        public IList<BranchRankingViewModel> StatList { get; set; }


        // 统计使用
        public List<AccountRecordModel> IncomeList { get; set; }
        public List<AccountRecordModel> OutcomeList { get; set; }
        public List<BookModel> ServiceList { get; set; }
    }

    public class BranchViewModel
    {
        public int OrganID { get; set; }
        [Required]
        [Display(Name = "公司ID")]
        public int HostID { get; set; }
        public string HostName { get; set; }
        public Nullable<int> ParentID { get; set; }
        public int Level { get; set; }
        [Required]
        [Display(Name = "门店名称")]
        public string Name { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        [Display(Name = "负责人")]
        public string Manager { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "联系电话")]
        public string Phone { get; set; }
        [Display(Name = "省份")]
        public string Province { get; set; }
        [Display(Name = "城市")]
        public string City { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        public string Address { get; set; }
        [Display(Name = "是否有效")]
        public int IsVaild { get; set; }
        [Display(Name = "终端数量")]
        public Nullable<int> ClientNum { get; set; }

    }

    public class BranchRankingViewModel
    {
        public int BranchId { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string BranchName { get; set; }
        /// <summary>
        /// 业绩
        /// </summary>
        public decimal? Income { get; set; }
        /// <summary>
        /// 卡扣
        /// </summary>
        public decimal? SalesAmount { get; set; }
        public decimal? CashSalesAmount { get; set; }
        /// <summary>
        /// 实操
        /// </summary>
        public decimal? ExpenseAmount { get; set; }
        /// <summary>
        /// 即销即耗
        /// </summary>
        public decimal? EntityAmount { get; set; }

        public string StatDate { get; set; }
    }
}