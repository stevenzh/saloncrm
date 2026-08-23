using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class AccountRecordMap : EntityTypeConfiguration<AccountRecord>
    {
        public AccountRecordMap()
        {
            // Primary Key
            this.HasKey(t => t.RecordID);

            // Properties
            this.Property(t => t.Remark)
                .HasMaxLength(200);

            this.Property(t => t.ClientID)
                .HasMaxLength(50);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(50);

            this.Property(t => t.PaymentType)
                .HasMaxLength(1);

            this.Property(t => t.SaleID)
                .HasMaxLength(50);

            this.Property(t => t.BeauticianID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("AccountRecords");
            this.Property(t => t.RecordID).HasColumnName("RecordID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.MemberID).HasColumnName("MemberID");
            this.Property(t => t.MemberCardId).HasColumnName("MemberCardId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.InAmount).HasColumnName("InAmount");
            this.Property(t => t.OutAmount).HasColumnName("OutAmount");
            this.Property(t => t.Balance).HasColumnName("Balance");
            this.Property(t => t.Debt).HasColumnName("Debt");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.ClientID).HasColumnName("ClientID");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.PaymentType).HasColumnName("PaymentType");
            this.Property(t => t.IsVaild).HasColumnName("IsVaild");
            this.Property(t => t.EventLogId).HasColumnName("EventLogID");

            // Relationships
            this.HasRequired(t => t.Member)
                .WithMany(t => t.AccountRecords)
                .HasForeignKey(d => d.MemberID);

            this.HasRequired(t => t.Branch)
                .WithMany(t => t.AccounRecords)
                .HasForeignKey(d => d.BranchId);

            this.HasMany(t => t.Splits)
                .WithRequired(t => t.Record)
                .HasForeignKey(d => d.RecordID);

            //this.HasRequired(t => t.EventLog)
            //    .WithMany(t => t.Records)
            //    .HasForeignKey(d => d.EventLogId);
        }
    }
}
