using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class BookProjectMap : EntityTypeConfiguration<BookProject>
    {
        public BookProjectMap()
        {
            // Primary Key
            this.HasKey(t => t.BookProjectID);

            // Properties
            this.Property(t => t.Appraisal)
                .HasMaxLength(500);

            this.Property(t => t.BeauticianId)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BookProjects");
            this.Property(t => t.BookProjectID).HasColumnName("BookProjectID");
            this.Property(t => t.BookID).HasColumnName("BookID");
            this.Property(t => t.MemberCardId).HasColumnName("MemberCardId");
            this.Property(t => t.MemberProjectId).HasColumnName("MemberProjectId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Points).HasColumnName("Points");
            this.Property(t => t.HandicraftFee).HasColumnName("HandicraftFee");
            this.Property(t => t.Satisfaction).HasColumnName("Satisfaction");
            this.Property(t => t.Appraisal).HasColumnName("Appraisal");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.BeauticianId).HasColumnName("BeauticianId");

            // Relationships
            this.HasRequired(t => t.Book)
                .WithMany(t => t.BookProjects)
                .HasForeignKey(d => d.BookID);

            this.HasRequired(t => t.Project)
                .WithMany(t => t.BookProjects)
                .HasForeignKey(d => d.ProjectID);

            this.HasMany(t => t.UserSplits)
                .WithRequired(t => t.BookProject)
                .HasForeignKey(d => d.BookProjectID);

        }
    }
}
