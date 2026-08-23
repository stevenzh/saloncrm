using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class MemberCardMap : EntityTypeConfiguration<MemberCard>
    {
        public MemberCardMap()
        {
            // Primary Key
            this.HasKey(t => t.MemberCardId);

            // Properties
            this.Property(t => t.Type)
                .HasMaxLength(1);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(50);

            this.Property(t => t.Title)
                .HasMaxLength(200);

            this.Property(t => t.ClientID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MemberCards");
            this.Property(t => t.MemberCardId).HasColumnName("MemberCardId");
            this.Property(t => t.MemberID).HasColumnName("MemberID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.BranchID).HasColumnName("BranchID");
            this.Property(t => t.ClientID).HasColumnName("ClientID");
            this.Property(t => t.LogId).HasColumnName("LogId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.ActualPrice).HasColumnName("ActualPrice");
            this.Property(t => t.BookTime).HasColumnName("BookTime");
            this.Property(t => t.UsedTime).HasColumnName("UsedTime");
            this.Property(t => t.LastCount).HasColumnName("LastCount");
            this.Property(t => t.DebtFlag).HasColumnName("DebtFlag");
            this.Property(t => t.DebtStatus).HasColumnName("DebtStatus");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.CreatedDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Amt).HasColumnName("Amt");
            this.Property(t => t.TmplID).HasColumnName("TmplID");

            // Relationships
            this.HasRequired(t => t.Member)
                .WithMany(t => t.MemberCards)
                .HasForeignKey(d => d.MemberID);

        }
    }
}
