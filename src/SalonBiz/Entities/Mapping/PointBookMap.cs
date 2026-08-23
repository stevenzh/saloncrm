using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    /// <summary>
    /// 积分流水
    /// </summary>
    public class PointBookMap : EntityTypeConfiguration<PointBook>
    {
        public PointBookMap()
        {
            // Primary Key
            this.HasKey(t => t.PointBookId);

            // Properties

            this.Property(t => t.Remark)
                .HasMaxLength(500);

            this.Property(t => t.ClientId)
                .HasMaxLength(50);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PointBooks");
            this.Property(t => t.PointBookId).HasColumnName("PointBookId");
            this.Property(t => t.HostId).HasColumnName("HostId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.MemberId).HasColumnName("MemberId");
            this.Property(t => t.LogId).HasColumnName("LogId");
            this.Property(t => t.OutPoints).HasColumnName("OutPoints");
            this.Property(t => t.InPoints).HasColumnName("InPoints");
            this.Property(t => t.RemainPoints).HasColumnName("RemainPoints");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.InOut).HasColumnName("InOut");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.Salesman).HasColumnName("Salesman");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.MemberCardId).HasColumnName("MemberCardId");
            this.Property(t => t.GiveId).HasColumnName("GiveId");


            // Relationships
            this.HasRequired(t => t.Member)
                .WithMany(t => t.PointBooks)
                .HasForeignKey(d => d.MemberId);

        }
    }
}
