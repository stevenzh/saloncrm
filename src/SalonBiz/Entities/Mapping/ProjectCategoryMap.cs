using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class ProjectCategoryMap : EntityTypeConfiguration<ProjectCategory>
    {
        public ProjectCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .HasMaxLength(500);


            // Table & Column Mappings
            this.ToTable("ProjectCategorys");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.Level).HasColumnName("Level");
        }
    }
}
