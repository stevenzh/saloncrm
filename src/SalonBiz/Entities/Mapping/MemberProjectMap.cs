using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class MemberProjectMap : EntityTypeConfiguration<MemberProject>
    {
        public MemberProjectMap()
        {
            // Primary Key
            this.HasKey(t => t.MemberProjectId);

            // Properties
            this.Property(t => t.ClientId)
                .HasMaxLength(50);

            this.Property(t => t.Type)
                .HasMaxLength(20);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("MemberProjects");
            this.Property(t => t.MemberProjectId).HasColumnName("MemberProjectId");
            this.Property(t => t.MemberID).HasColumnName("MemberID");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.LogId).HasColumnName("LogId");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.ActualPrice).HasColumnName("ActualPrice");
            this.Property(t => t.BookTime).HasColumnName("BookTime");
            this.Property(t => t.UsedTime).HasColumnName("UsedTime");
            this.Property(t => t.LastCount).HasColumnName("LastCount");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.DebtFlag).HasColumnName("DebtFlag");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreateDate");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.GiveId).HasColumnName("GiveId");
            this.Property(t => t.IsVaild).HasColumnName("IsVaild");

            // Relationships
            this.HasRequired(t => t.Member)
                .WithMany(t => t.MemberProjects)
                .HasForeignKey(d => d.MemberID);

        }
    }
}
