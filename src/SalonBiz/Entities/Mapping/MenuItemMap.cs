using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class MenuItemMap : EntityTypeConfiguration<MenuItem>
    {
        public MenuItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.MenuPath)
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .HasMaxLength(500);

            this.Property(t => t.Icon)
                .HasMaxLength(20);

            this.Property(t => t.SiteNav)
                .HasMaxLength(20);

            this.Property(t => t.SiteNavNext)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("MenuItems");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.MenuPath).HasColumnName("MenuPath");
            this.Property(t => t.Icon).HasColumnName("Icon");
            this.Property(t => t.Level).HasColumnName("Level");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
            this.Property(t => t.SiteNav).HasColumnName("SiteNav");
            this.Property(t => t.SiteNavNext).HasColumnName("SiteNavNext");
        }
    }
}
