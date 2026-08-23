using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class BookMap : EntityTypeConfiguration<Book>
    {
        public BookMap()
        {
            // Primary Key
            this.HasKey(t => t.BookID);

            // Properties
            this.Property(t => t.ClientID)
                .HasMaxLength(50);

            this.Property(t => t.SalesmanID)
                .HasMaxLength(200);

            this.Property(t => t.Remark)
                .HasMaxLength(2000);

            this.Property(t => t.State)
                .HasMaxLength(20);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(200);

            this.Property(t => t.PaymentID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Books");
            this.Property(t => t.BookID).HasColumnName("BookID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.MemberID).HasColumnName("MemberID");
            this.Property(t => t.LogId).HasColumnName("LogId");
            this.Property(t => t.SalesmanID).HasColumnName("SalesmanID");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Satisfaction).HasColumnName("Satisfaction");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.ClientID).HasColumnName("ClientID");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.PaymentID).HasColumnName("PaymentID");
            this.Property(t => t.PayTime).HasColumnName("PayTime");

            // Relationships
            this.HasRequired(t => t.Member)
                .WithMany(t => t.Books)
                .HasForeignKey(d => d.MemberID);

            //this.HasRequired(t => t.Branch)
            //    .WithMany(t => t.Books)
            //    .HasForeignKey(d => d.BranchId);
        }
    }
}
