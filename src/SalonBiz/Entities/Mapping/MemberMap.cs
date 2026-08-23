using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class MemberMap : EntityTypeConfiguration<Member>
    {
        public MemberMap()
        {
            // Primary Key
            this.HasKey(t => t.MemberID);

            // Properties
            this.Property(t => t.Passwd)
                .HasMaxLength(50);

            this.Property(t => t.CardNo)
                .HasMaxLength(50);

            this.Property(t => t.Source)
                .HasMaxLength(50);

            this.Property(t => t.Level)
                .HasMaxLength(50);

            this.Property(t => t.Status)
                .HasMaxLength(50);

            this.Property(t => t.Type)
                .HasMaxLength(50);

            this.Property(t => t.Feedback)
                .HasMaxLength(50);

            this.Property(t => t.SalesmanId)
                .HasMaxLength(50);

            this.Property(t => t.BeauticianId)
                .HasMaxLength(50);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(2000);

            // 客户个人信息
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.MobileNumber)
                .HasMaxLength(50);

            this.Property(t => t.WebChat)
                .HasMaxLength(50);

            this.Property(t => t.TencentQQ)
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .HasMaxLength(50);

            this.Property(t => t.Sex)
                .HasMaxLength(1);

            this.Property(t => t.Address)
                .HasMaxLength(250);

            this.Property(t => t.CompanyAddress)
                .HasMaxLength(250);

            this.Property(t => t.Vocation)
                .HasMaxLength(50);

            this.Property(t => t.Position)
                .HasMaxLength(50);

            this.Property(t => t.Company)
                .HasMaxLength(50);

            this.Property(t => t.MaritalStatus)
                .HasMaxLength(50);

            this.Property(t => t.Conjugal)
                .HasMaxLength(200);

            this.Property(t => t.SkinType)
                .HasMaxLength(50);

            this.Property(t => t.SkinConditions)
                .HasMaxLength(100);

            this.Property(t => t.FacialDemand)
                .HasMaxLength(100);

            this.Property(t => t.BodyDemand)
                .HasMaxLength(100);

            this.Property(t => t.CustomerDemand)
                .HasMaxLength(100);

            this.Property(t => t.ConsumptionHabit)
                .HasMaxLength(100);

            this.Property(t => t.Personality)
                .HasMaxLength(100);

            // 微信信息
            this.Property(t => t.OpenID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Members");
            this.Property(t => t.MemberID).HasColumnName("MemberID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.CardNo).HasColumnName("CardNo");
            this.Property(t => t.Passwd).HasColumnName("Passwd");
            this.Property(t => t.JoinDate).HasColumnName("JoinDate");
            this.Property(t => t.JoinBranch).HasColumnName("JoinBranch");
            this.Property(t => t.Source).HasColumnName("Source");
            this.Property(t => t.Level).HasColumnName("Level");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.BookTime).HasColumnName("BookTime");
            this.Property(t => t.Points).HasColumnName("Points");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.Amt).HasColumnName("Amt");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.IsNew).HasColumnName("IsNew");
            this.Property(t => t.Feedback).HasColumnName("Feedback");
            this.Property(t => t.FeedbackDate).HasColumnName("FeedbackDate");
            this.Property(t => t.LastBirth).HasColumnName("LastBirth");
            this.Property(t => t.SalesmanId).HasColumnName("SalesmanId");

            // 以下客户个人信息
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.MobileNumber).HasColumnName("MobileNumber");
            this.Property(t => t.WebChat).HasColumnName("WebChat");
            this.Property(t => t.TencentQQ).HasColumnName("TencentQQ");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.CompanyAddress).HasColumnName("CompanyAddress");
            this.Property(t => t.WeddingDay).HasColumnName("WeddingDay");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.Vocation).HasColumnName("Vocation");
            this.Property(t => t.Position).HasColumnName("Position");
            this.Property(t => t.Company).HasColumnName("Company");
            this.Property(t => t.MaritalStatus).HasColumnName("MaritalStatus");
            this.Property(t => t.Conjugal).HasColumnName("Conjugal");
            this.Property(t => t.SkinType).HasColumnName("SkinType");
            this.Property(t => t.SkinConditions).HasColumnName("SkinConditions");
            this.Property(t => t.FacialDemand).HasColumnName("FacialDemand");
            this.Property(t => t.BodyDemand).HasColumnName("BodyDemand");
            this.Property(t => t.CustomerDemand).HasColumnName("CustomerDemand");
            this.Property(t => t.ConsumptionHabit).HasColumnName("ConsumptionHabit");
            this.Property(t => t.Personality).HasColumnName("Personality");

            // Relationships
            //this.HasRequired(t => t.Salesman)
            //    .WithMany(t => t.Members)
            //    .HasForeignKey(d => d.SalesmanId);
        }
    }
}
