using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class AccountRecordSplitMap : EntityTypeConfiguration<AccountRecordSplit>
    {
        public AccountRecordSplitMap()
        {
            // Primary Key
            this.HasKey(t => t.SplitID);

            // Properties
            this.Property(t => t.Remark)
                .HasMaxLength(200);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.Position)
                .HasMaxLength(10);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("AccountRecordSplits");
            this.Property(t => t.SplitID).HasColumnName("SplitID");
            this.Property(t => t.RecordID).HasColumnName("RecordID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.Position).HasColumnName("Position");
            this.Property(t => t.Percentage).HasColumnName("Percentage");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.ModifiedTime).HasColumnName("ModifiedTime");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.AccountRecordSplits)
                .HasForeignKey(d => d.UserID);


        }
    }
}
