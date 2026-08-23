using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonCRM.Models
{
    /// <summary>
    /// 客户
    /// </summary>
    public partial class Member
    {
        public Member()
        {
            this.Appointments = new List<Appointment>();
            this.Books = new List<Book>();
            this.MemberProjects = new List<MemberProject>();
            this.Feedbacks = new List<Feedback>();
        }

        #region 客户信息
        /// <summary>
        /// 编号
        /// </summary>
        public long MemberID { get; set; }
        public int HostID { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 入会时间
        /// </summary>
        public System.DateTime JoinDate { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        public Nullable<int> JoinBranch { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Passwd { get; set; }
        /// <summary>
        /// 会员类型  L01:体验|L02:正式|
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 状态：有效|准有效|休眠|流失
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 累计到店频次
        /// </summary>
        public int BookTime { get; set; }
        /// <summary>
        /// 账户余额(储值卡总和 )
        /// </summary>
        public decimal Amt { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Points { get; set; }
        /// <summary>
        /// 0:老客户|1:新客户
        /// </summary>
        public int IsNew { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 跟进时间
        /// </summary>
        public Nullable<System.DateTime> FeedbackDate { get; set; }
        /// <summary>
        /// 跟进状态 默认为空， 不为空代表需要跟进 
        /// 例如 F001 休眠唤醒、服务邀约、节日问候…
        /// </summary>
        public string Feedback { get; set; }
        /// <summary>
        /// 用于标记生日 辅助字段
        /// </summary>
        public Nullable<DateTime> LastBirth { get; set; }
        /// <summary>
        /// 美容顾问
        /// </summary>
        public string SalesmanId { get; set; }
        /// <summary>
        /// 美容师
        /// </summary>
        public string BeauticianId { get; set; }

        #endregion

        #region 个人信息
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string WebChat { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string TencentQQ { get; set; }
        /// <summary>
        /// 电子信箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
        public string CompanyAddress { get; set; }
        /// <summary>
        /// 结婚纪念日
        /// </summary>
        public Nullable<System.DateTime> WeddingDay { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public Nullable<System.DateTime> Birthday { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        public string Vocation { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string MaritalStatus { get; set; }
        /// <summary>
        /// 夫妻关系
        /// </summary>
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

        #endregion

        public string OpenID { get; set; }
        /// <summary>
        /// 预约
        /// </summary>
        public virtual ICollection<Appointment> Appointments { get; set; }
        /// <summary>
        /// 账户流水
        /// </summary>
        public virtual ICollection<AccountRecord> AccountRecords { get; set; }
        /// <summary>
        /// 消费
        /// </summary>
        public virtual ICollection<Book> Books { get; set; }
        /// <summary>
        /// 积分记录
        /// </summary>
        public virtual ICollection<PointBook> PointBooks { get; set; }
        /// <summary>
        /// 会员项目
        /// </summary>
        public virtual ICollection<MemberProject> MemberProjects { get; set; }
        /// <summary>
        /// 卡项
        /// </summary>
        public virtual ICollection<MemberCard> MemberCards { get; set; }
        /// <summary>
        /// 项目赎回记录
        /// </summary>
        public virtual ICollection<RedeemProject> RedeemProjects { get; set; }
        /// <summary>
        /// 回访记录
        /// </summary>
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<MemberGive> MemberGives { get; set; }

        [NotMapped]
        public virtual string ClientId { get; set; }

        public virtual ApplicationUser Salesman { get; set; }
    }

}