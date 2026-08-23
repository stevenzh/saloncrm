using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    /// <summary>
    /// Êê»ØÏîÄ¿
    /// </summary>
    public class RedeemProjectMap : EntityTypeConfiguration<RedeemProject>
    {
        public RedeemProjectMap()
        {
            // Primary Key
            this.HasKey(t => t.RedeemId);

            // Properties
            this.Property(t => t.ClientId)
                .HasMaxLength(50);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("RedeemProject");
            this.Property(t => t.RedeemId).HasColumnName("RedeemId");
            this.Property(t => t.HostId).HasColumnName("HostId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.MemberId).HasColumnName("MemberId");
            this.Property(t => t.MemberProjectId).HasColumnName("MemberProjectId");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.ProjectId).HasColumnName("ProjectId");
            this.Property(t => t.LogId).HasColumnName("LogId");
            this.Property(t => t.CardLogId).HasColumnName("CardLogId");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Count).HasColumnName("Count");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.Remark).HasColumnName("Remark");

            // Relationships
            this.HasRequired(t => t.Member)
                .WithMany(t => t.RedeemProjects)
                .HasForeignKey(d => d.MemberId);

        }
    }
}
