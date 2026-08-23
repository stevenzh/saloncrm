using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class OrganMap : EntityTypeConfiguration<Organ>
    {
        public OrganMap()
        {
            // Primary Key
            this.HasKey(t => t.OrganID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Manager)
                .HasMaxLength(50);

            this.Property(t => t.Phone)
                .HasMaxLength(50);

            this.Property(t => t.Province)
               .HasMaxLength(50);

            this.Property(t => t.City)
              .HasMaxLength(50);

            this.Property(t => t.Address)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Organ");
            this.Property(t => t.OrganID).HasColumnName("OrganID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.ParentID).HasColumnName("ParentID");
            this.Property(t => t.Level).HasColumnName("Level");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ClientNum).HasColumnName("ClientNum");
            this.Property(t => t.Manager).HasColumnName("Manager");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.IsVaild).HasColumnName("IsVaild");

            // Relationships
            this.HasRequired(t => t.Host)
                .WithMany(t => t.Organs)
                .HasForeignKey(d => d.HostID);

        }
    }
}
