using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class RoleMap : EntityTypeConfiguration<ApplicationRole>
    {
        public RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("ApplicationRole");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IsMajor).HasColumnName("IsMajor");

            this.HasMany(u => u.Menus)
                .WithMany(r => r.Roles)
                .Map(m =>
                {
                    m.ToTable("RoleMenus");
                    m.MapLeftKey("RoleId");
                    m.MapRightKey("MenuId");
                });
        }
    }
}
