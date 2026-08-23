using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class MemberGiveMap : EntityTypeConfiguration<MemberGive>
    {
        public MemberGiveMap()
        {
            // Primary Key
            this.HasKey(t => t.GiveId);

            // Properties
            this.Property(t => t.ClientId)
                .HasMaxLength(50);

            this.Property(t => t.Salesman)
              .HasMaxLength(50);

            this.Property(t => t.Type)
              .HasMaxLength(20);

            this.Property(t => t.Remark)
              .HasMaxLength(500);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MemberGives");
            this.Property(t => t.GiveId).HasColumnName("GiveId");
            this.Property(t => t.MemberID).HasColumnName("MemberID");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.LogId).HasColumnName("LogId");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.InPoints).HasColumnName("InPoints");
            this.Property(t => t.RemainPoints).HasColumnName("RemainPoints");
            this.Property(t => t.BookTime).HasColumnName("BookTime");
            this.Property(t => t.UsedTime).HasColumnName("UsedTime");
            this.Property(t => t.LastCount).HasColumnName("LastCount");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.Salesman).HasColumnName("Salesman");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreateDate");
            this.Property(t => t.IsVaild).HasColumnName("IsVaild");

            // Relationships
            this.HasRequired(t => t.Member)
                .WithMany(t => t.MemberGives)
                .HasForeignKey(d => d.MemberID);

            //this.HasRequired(t => t.Project)
            //    .WithMany(t => t.MemberGives)
            //    .HasForeignKey(d => d.ProjectID);

        }
    }
}
