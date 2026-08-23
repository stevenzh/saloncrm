using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalonCRM.Models
{
    public class MemberQModel : PagedModel
    {
        public MemberQModel()
        {
            this.PagedIndex = 1;
            this.PagedSize = 20;
            this.IsNew = -1;
        }
        public int BranchId { get; set; }
        public string Name { get; set; }
        public string CardNo { get; set; }
        public string Mobile { get; set; }
        public string Type { get; set; }
        public string Level { get; set; }

        public string Status { get; set; }
        public int IsNew { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 美容顾问
        /// </summary>
        public string SalesmanId { get; set; }
        /// <summary>
        /// 美容师
        /// </summary>
        public string BeauticianId { get; set; }
        public PagedList<MemberViewModel> MemberPageList { get; set; }
        public IList<MemberViewModel> MemberList { get; set; }
    }

    public class MemberViewModel
    {
        public MemberViewModel()
        {
            this.Sex = "F";
            this.JoinBranch = 2;
        }

        // 会员信息
        public long MemberID { get; set; }
        [Display(Name = "密码")]
        public string Passwd { get; set; }
        private DateTime _JoinDate;
        [Display(Name = "入会时间")]
        public string JoinDateStr { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "入会时间")]
        public DateTime JoinDate
        {
            get { return _JoinDate; }
            set
            {
                _JoinDate = value;
                JoinDateStr = (value == null) ? "" : string.Format("{0:yyyy-MM-dd}", value);
            }
        }
        [Display(Name = "入会门店")]
        public Nullable<int> JoinBranch { get; set; }
        [Display(Name = "入会门店")]
        public string JoinBranchStr { get; set; }
        /// <summary>
        /// 会员类型 正式|体验
        /// </summary>
        [Required]
        [Display(Name = "类型")]
        public string Type { get; set; }
        [Display(Name = "状态")]
        public string Status { get; set; }
        [Display(Name = "状态")]
        public string StatusValue { get; set; }
        [Display(Name = "来源")]
        public string Source { get; set; }
        public string SourceValue { get; set; }
        [Display(Name = "级别")]
        public string Level { get; set; }
        [Display(Name = "级别")]
        public string LevelValue { get; set; }
        [Display(Name = "到店频次")]
        public int? BookTime { get; set; }
        [Display(Name = "最后到店时间")]
        public Nullable<DateTime> LastServiceDate { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Display(Name = "余额")]
        public Nullable<decimal> Amt { get; set; }
        [Display(Name = "积分")]
        public Nullable<int> Points { get; set; }
        public DateTime CreatedDate { get; set; }
        [Display(Name = "美容顾问")]
        public string SalesmanId { get; set; }
        /// <summary>
        /// 客户对应美容师
        /// </summary>
        [Display(Name = "美容师")]
        public string BeauticianId { get; set; }

        /// 个人信息
        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "手机号码")]
        public string MobileNumber { get; set; }
        [Display(Name = "微信")]
        public string WebChat { get; set; }
        [Display(Name = "QQ")]
        public string TencentQQ { get; set; }
        [Display(Name = "电子信箱")]
        public string Email { get; set; }
        [Display(Name = "联系地址")]
        public string Address { get; set; }
        [Display(Name = "公司地址")]
        public string CompanyAddress { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        [Display(Name = "职业")]
        public string Vocation { get; set; }
        [Display(Name = "职务")]
        public string Position { get; set; }
        [Display(Name = "所属公司")]
        public string Company { get; set; }
        private DateTime? _Birthday;
        [Display(Name = "生日")]
        public string BirthdayStr { get; set; }
        public string TypeValue { get; set; }
        [Display(Name = "生日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Birthday
        {
            get { return _Birthday; }
            set
            {
                _Birthday = value;
                BirthdayStr = (value == null) ? "" : string.Format("{0:yyyy-MM-dd}", value);
            }
        }
        [Display(Name = "卡号")]
        public string CardNo { get; set; }
        [Required]
        [Display(Name = "性别")]
        public string Sex { get; set; }
        [Display(Name = "性别")]
        public string SexValue { get; set; }
        //2014-12-11添加
        [Display(Name = "结婚纪念日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> WeddingDay { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        [Display(Name = "婚姻状况")]
        public string MaritalStatus { get; set; }
        [Display(Name = "夫妻关系")]
        public string Conjugal { get; set; }

        /// <summary>
        /// 皮肤类型
        /// </summary>
        public string SkinType { get; set; }
        /// <summary>
        /// 肌肤状况
        /// </summary>
        public string SkinConditions { get; set; }
        /// <summary>
        /// 面部需求
        /// </summary>
        public string FacialDemand { get; set; }
        /// <summary>
        /// 身体需求
        /// </summary>
        public string BodyDemand { get; set; }
        /// <summary>
        /// 客户需求
        /// </summary>
        public string CustomerDemand { get; set; }
        /// <summary>
        /// 消费习惯
        /// </summary>
        public string ConsumptionHabit { get; set; }
        /// <summary>
        /// 性格
        /// </summary>
        public string Personality { get; set; }

        /// <summary>
        /// 油性；干性；混合性；中性；敏感性
        /// </summary>
        [Display(Name = "皮肤类型")]
        public IEnumerable<string> SkinTypeE { get; set; }
        /// <summary>
        /// 干燥缺水；肤色暗沉；肌肤松弛；肌肤粗糙；红血丝；面疱粉刺；敏感；毛孔粗大；
        /// 眼袋；代谢紊乱；疤痕痘痕；黑眼圈；斑；油脂分泌过盛 
        /// </summary>
        [Display(Name = "肌肤状况")]
        public IEnumerable<string> SkinConditionE { get; set; }
        /// <summary>
        /// 美白；祛皱；淡斑；提升；脱敏；补水；其它
        /// </summary>
        [Display(Name = "面部需求")]
        public IEnumerable<string> FacialDemandE { get; set; }
        /// <summary>
        /// 肩颈部；胸部；手臂；腹部；大腿；小腿；内分泌 ；臀部；其它
        /// </summary>
        [Display(Name = "身体需求")]
        public IEnumerable<string> BodyDemandE { get; set; }
        /// <summary>
        /// 面部；身体；抗衰；家居；美胸/塑形；眼部；私密；仪器；其它
        /// </summary>
        [Display(Name = "客户需求")]
        public IEnumerable<string> CustomerDemandE { get; set; }
        /// <summary>
        /// 高额消费；高频消费；活动消费；新项目消费 
        /// </summary>
        [Display(Name = "消费习惯")]
        public IEnumerable<string> ConsumptionHabitE { get; set; }
        /// <summary>
        /// 敏感型；完美型；目标型；差异型；思维型；6：权威型；高傲型；虚荣型；务实型；自卑型
        /// </summary>
        [Display(Name = "性格")]
        public IEnumerable<string> PersonalityE { get; set; }



        public IList<MemberCardModel> Cards { get; set; }
        public List<MemberProjectViewModel> Projects { get; set; }
        /// <summary>
        /// 消费记录
        /// </summary>
        public IList<AccountRecordModel> ExpenseRecords { get; set; }
        public IList<Book> ExpenseBooks { get; set; }
        /// <summary>
        /// 消费记录(项目)
        /// </summary>
        public IList<BookProjectModel> ExpenseProjects { get; set; }
        /// <summary>
        /// 充值记录
        /// </summary>
        public IList<AccountRecord> RechargeRecords { get; set; }
        /// <summary>
        /// 欠款记录
        /// </summary>
        public List<DebtViewModel> DebtRecord { get; set; }
        /// <summary>
        /// 可用项目
        /// </summary>
        public IList<MemberProjectViewModel> UsableProjects { get; set; }
        /// <summary>
        /// 回访
        /// </summary>
        public IList<FeedbackViewModel> Feedbacks { get; set; }
        public Feedback Feedback { get; set; }

        public ApplicationUser Salesman { get; set; }
        public ApplicationUser Beautician { get; set; }
        #region 统计显示数据
        /// <summary>
        /// 剩余项目数
        /// </summary>
        public int RemainedProject { get; set; }
        /// <summary>
        /// 今年卡扣（购买疗程）
        /// </summary>
        public decimal ConsumptionThisYear { get; set; }
        /// <summary>
        /// 今年卡扣频率  N次/年
        /// </summary>
        public decimal ConsumptionHzThisYear { get; set; }
        /// <summary>
        /// 去年卡扣金额
        /// </summary>
        public decimal ConsumptionLastYear { get; set; }
        /// <summary>
        /// 去年卡扣频率  N次/年
        /// </summary>
        public decimal ConsumptionHzLastYear { get; set; }
        public string OpenID { get; set; }
        public WxMember wxMember { get; set; }
        public int IsNew { get; set; }

        #endregion
    }

    public class MemberProjectViewModel
    {
        public long MemberProjectId { get; set; }
        public long MemberID { get; set; }
        [Display(Name = "客户名称")]
        public string MemberName { get; set; }
        public int ProjectID { get; set; }
        public string ProjectCode { get; set; }
        [Display(Name = "单价")]
        public Nullable<decimal> UnitPrice { get; set; }
        [Display(Name = "应收金额")]
        public Nullable<decimal> Amount { get; set; }
        [Display(Name = "实收金额")]
        public decimal ActualPrice { get; set; }
        [Display(Name = "欠款金额")]
        public decimal Payment { get; set; }
        [Display(Name = "购买次数")]
        public int BookTime { get; set; }
        [Display(Name = "已用次数")]
        public int UsedTime { get; set; }
        [Display(Name = "可用次数")]
        public int LastCount { get; set; }
        [Display(Name = "购买时间")]
        public Nullable<System.DateTime> CreatedDate { get; set; }
        [Display(Name = "项目名称")]
        public string ProjectName { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public ICollection<AccountRecordSplit> Sales { get; set; }
        /// <summary>
        /// 顾问
        /// </summary>
        [Display(Name = "顾问")]
        public string SalesStr { get; set; }
        public decimal SalesRadix { get; set; }
        public string CardType { get; set; }
        /// <summary>
        /// 是否可以现消现耗 1 可以  0 不可以
        /// </summary>
        public int IsEntity { get; set; }

        #region 用于还款
        /// <summary>
        /// 还款金额
        /// </summary>
        [Display(Name = "还款金额")]
        public decimal Repayment { get; set; }
        public long? MemberCardId { get; set; }
        [Display(Name = "卡标题")]
        public string CardTitle { get; set; }
        [Display(Name = "销售")]
        public IEnumerable<string> Beautician { get; set; }

        public string Workers { get; set; }
        public decimal WorkerRadix { get; set; }
        public long? AccountRecordID { get; set; }

        public string BranchName { get; set; }
        #endregion
    }
}