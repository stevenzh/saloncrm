using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class ProjectMap : EntityTypeConfiguration<Project>
    {
        public ProjectMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectID);

            // Properties
            this.Property(t => t.Code)
                //.IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Category)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Status)
                .HasMaxLength(50);

            this.Property(t => t.Brand)
                .HasMaxLength(50);

            this.Property(t => t.ExtCategory)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Projects");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.MinUnit).HasColumnName("MinUnit");
            this.Property(t => t.HandicraftFee).HasColumnName("HandicraftFee");
            this.Property(t => t.LowHandicraftFee).HasColumnName("LowHandicraftFee");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.ExtCategory).HasColumnName("ExtCategory");
            this.Property(t => t.SecCategory).HasColumnName("SecCategory");
            this.Property(t => t.IsEntity).HasColumnName("IsEntity");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Brand).HasColumnName("Brand");
            this.Property(t => t.Count).HasColumnName("Count");

            // Relationships
            this.HasRequired(t => t.Host)
                .WithMany(t => t.Projects)
                .HasForeignKey(d => d.HostID);

        }
    }
}
