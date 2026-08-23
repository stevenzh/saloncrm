using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public class BookQModel : PagedModel
    {
        public BookQModel()
        {
            this.PagedIndex = 1;
            this.PagedSize = 20;
        }

        public int BookID { get; set; }
        public int BranchId { get; set; }
        /// <summary>
        /// 美容顾问
        /// </summary>
        public string SalesId { get; set; }
        /// <summary>
        /// 美容师
        /// </summary>
        public string BeauticianId { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string MemberName { get; set; }
        public string CardNo { get; set; }
        public string Category { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IList<BookModel> BookList { get; set; }
    }

    public class BookModel 
    {
        [Display(Name = "订单号")]
        public long BookID { get; set; }
        public int HostID { get; set; }
        public int BranchId { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public long MemberID { get; set; }
        public string MemberName { get; set; }
        public string MemberCardNo;

        /// <summary>
        /// 充值关联卡
        /// </summary>
        public Nullable<long> MemberCardId { get; set; }
        /// <summary>
        /// 消耗金额（计算）
        /// </summary>
        [Display(Name = "消耗金额")]
        public decimal Amount { get; set; }
        /// <summary>
        /// 满意度
        /// </summary>
        public Nullable<int> Satisfaction { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 终端ID
        /// </summary>
        public string ClientID { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = "创建时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 顾问 对应销售
        /// </summary>
        [Display(Name = "美容顾问")]
        public string SalesmanID { get; set; }
        public ApplicationUser Salesman { get; set; }
        /// <summary>
        /// 下单流程 0 下单 10 进行中  20 已结算  30 已取消
        /// </summary>
        [Display(Name = "状态")]
        public string State { get; set; }
        /// <summary>
        /// 服务项目
        /// </summary>
        public ICollection<BookProject> BookProjects { get; set; }
        public Member Member { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        public Organ Branch { get; set; }
        /// <summary>
        /// 下单流程 0 下单 10 进行中  20 已结算 
        /// </summary>
        public string StateValue { get; set; }
        public DateTime? PayTime { get; set; }
    }
    public class BookProjectModel
    {
        public BookProjectModel()
        {
            this.Splits = new List<BookProjectSplit>();
        }
        public long BookProjectID { get; set; }
        public long BookID { get; set; }
        /// <summary>
        /// 消耗关联卡
        /// </summary>
        public Nullable<long> MemberCardId { get; set; }
        public string MemberCardTitle { get; set; }
        /// <summary>
        /// 关联ID
        /// </summary>
        public Nullable<long> MemberProjectId { get; set; }
        public Nullable<long> MemberGiveId { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectID { get; set; }
        /// <summary>
        /// 1:实操（扣次数） 2:卡扣（现金） 3:现消现耗（购买产品，不进消费）
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 是否现消现耗 商品 1 是 0 不是
        /// </summary>
        public int IsEntity { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public Nullable<int> Points { get; set; }
        /// <summary>
        /// 满意度
        /// </summary>
        public Nullable<int> Satisfaction { get; set; }
        public decimal HandicraftFee { get; set; }

        public BookModel Book { get; set; }
        public Project Project { get; set; }
        [Display(Name = "服务员工")]
        public ICollection<BookProjectSplit> Splits { get; set; }
        public string BranchName { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}