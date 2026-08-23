using System;
using System.Collections.Generic;
using System.Data.Entity;
using SalonCRM.Models.Mapping;

namespace SalonCRM.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection") { }
        /// <summary>
        /// 用户，员工
        /// </summary>
        public DbSet<ApplicationUser> Users { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<ApplicationRole> Roles { get; set; }
        /// <summary>
        /// 功能菜单
        /// </summary>
        public DbSet<MenuItem> MenuItems { get; set; }
        /// <summary>
        /// 商户
        /// </summary>
        public DbSet<Host> Hosts { get; set; }
        public DbSet<HostProfile> HostProfiles {get; set;}
        /// <summary>
        /// 部门、门店
        /// </summary>
        public DbSet<Organ> Organs { get; set; }
        /// <summary>
        /// 终端
        /// </summary>
        public DbSet<Client> Clients { get; set; }
        /// <summary>
        /// 项目、产品
        /// </summary>
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectCategory> ProjectCategorys { get; set; }
        /// <summary>
        /// 客户、会员
        /// </summary>
        public DbSet<Member> Members { get; set; }
        /// <summary>
        /// 客户购买、赠送的项目
        /// </summary>
        public DbSet<MemberProject> MemberProjects { get; set; }
        /// <summary>
        /// 客户购买的卡片
        /// </summary>
        public DbSet<MemberCard> MemberCards { get; set; }
        public DbSet<MemberCardProject> MemberCardProjects { get; set; }
        public DbSet<MemberGive> MemberGives { get; set; }
        /// <summary>
        /// 预约
        /// </summary>
        public DbSet<Appointment> Appointments { get; set; }
        /// <summary>
        /// 充值/消费/退款
        /// </summary>
        public DbSet<AccountRecord> AccountRecords { get; set; }

        public DbSet<AccountRecordSplit> AccountRecordSplits { get; set; }
        /// <summary>
        /// 美容服务
        /// </summary>
        public DbSet<Book> Books { get; set; }
        /// <summary>
        /// 消费项目
        /// </summary>
        public DbSet<BookProject> BookProjects { get; set; }
        public DbSet<BookProjectSplit> BookProjectSplits { get; set; }
        /// <summary>
        /// 词典
        /// </summary>
        public DbSet<Dictionary> Dictionaries { get; set; }
        /// <summary>
        /// 目标
        /// </summary>
        public DbSet<Objective> Objectives { get; set; }
        /// <summary>
        /// 项目赎回
        /// </summary>
        public DbSet<RedeemProject> RedeemProjects { get; set; }
        /// <summary>
        /// 积分生成 消费
        /// </summary>
        public DbSet<PointBook> PointBooks { get; set; }
        /// <summary>
        /// 行政区域
        /// </summary>
        public DbSet<Region> Regions { get; set; }
        /// <summary>
        /// 操作日志 换卡
        /// </summary>
        public DbSet<EventLog> EventLogs { get; set; }
        /// <summary>
        /// 客户回访
        /// </summary>
        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<WxMember> WxMembers { get; set; }

        public DbSet<Goods> Goods { get; set; }

        public DbSet<ProjectGoods> ProjectGoods { get; set; }

        public DbSet<CardTmpl> CardTmpls { get; set; }
        public DbSet<CardTmplProject> CardTmplProjects { get; set; }
        public DbSet<BookGoods> BookGoods { get; set; }

        public DbSet<MemberProjectGoods> MemberProjectGoods { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new MenuItemMap());
            modelBuilder.Configurations.Add(new HostMap());
            modelBuilder.Configurations.Add(new HostProfileMap());
            modelBuilder.Configurations.Add(new OrganMap());
            modelBuilder.Configurations.Add(new ClientMap());
            modelBuilder.Configurations.Add(new ProjectMap());
            modelBuilder.Configurations.Add(new ProjectCategoryMap());
            modelBuilder.Configurations.Add(new MemberMap());
            modelBuilder.Configurations.Add(new MemberProjectMap());
            modelBuilder.Configurations.Add(new MemberCardMap());
            modelBuilder.Configurations.Add(new MemberCardProjectMap());
            modelBuilder.Configurations.Add(new MemberGiveMap());
            modelBuilder.Configurations.Add(new AccountRecordMap());
            modelBuilder.Configurations.Add(new AccountRecordSplitMap());
            modelBuilder.Configurations.Add(new AppointmentMap());
            modelBuilder.Configurations.Add(new BookMap());
            modelBuilder.Configurations.Add(new BookProjectMap());
            modelBuilder.Configurations.Add(new BookProjectSplitMap());
            modelBuilder.Configurations.Add(new DictionaryMap());
            modelBuilder.Configurations.Add(new ObjectiveMap());
            modelBuilder.Configurations.Add(new RedeemProjectMap());
            modelBuilder.Configurations.Add(new RegionMap());
            modelBuilder.Configurations.Add(new EventLogMap());
            modelBuilder.Configurations.Add(new WxMemberMap());
            modelBuilder.Configurations.Add(new FeedbackMap());

            modelBuilder.Configurations.Add(new GoodsMap());
            modelBuilder.Configurations.Add(new CardTmplMap());
            modelBuilder.Configurations.Add(new CardTmplProjectMap());
            modelBuilder.Configurations.Add(new ProjectGoodsMap());
            modelBuilder.Configurations.Add(new BookGoodsMap());
            modelBuilder.Configurations.Add(new MemberProjectGoodsMap());

        }
    }
}