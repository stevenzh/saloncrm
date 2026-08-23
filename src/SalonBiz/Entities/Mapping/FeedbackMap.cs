using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class FeedbackMap : EntityTypeConfiguration<Feedback>
    {
        public FeedbackMap()
        {
            // Primary Key
            this.HasKey(t => t.FeedbackId);

            // Properties
            this.Property(t => t.Purpose)
                .HasMaxLength(50);

            this.Property(t => t.LinkWay)
                .HasMaxLength(50);

            this.Property(t => t.Result)
                .HasMaxLength(50);

            this.Property(t => t.CallUserId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Centent)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("Feedback");
            this.Property(t => t.FeedbackId).HasColumnName("FeedbackId");
            this.Property(t => t.HostId).HasColumnName("HostId");
            this.Property(t => t.MemberId).HasColumnName("MemberId");
            this.Property(t => t.Purpose).HasColumnName("Purpose");
            this.Property(t => t.LinkWay).HasColumnName("LinkWay");
            this.Property(t => t.Result).HasColumnName("Result");
            this.Property(t => t.NextDate).HasColumnName("NextDate");
            this.Property(t => t.Centent).HasColumnName("Centent");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CallUserId).HasColumnName("CallUserId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");

            // Relationships
            this.HasRequired(t => t.Member)
                .WithMany(t => t.Feedbacks)
                .HasForeignKey(d => d.MemberId);
        }
    }
}
