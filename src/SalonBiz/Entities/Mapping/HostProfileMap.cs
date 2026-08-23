using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class HostProfileMap : EntityTypeConfiguration<HostProfile>
    {
        public HostProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.ProfileID);

            // Properties
            this.Property(t => t.PropertyText)
                .HasMaxLength(50);

            this.Property(t => t.PropertyValue)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("HostProfiles");
            this.Property(t => t.ProfileID).HasColumnName("ProfileID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.PropertyText).HasColumnName("PropertyText");
            this.Property(t => t.PropertyValue).HasColumnName("PropertyValue");
        }
    }
}
