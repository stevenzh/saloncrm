using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class HostMap : EntityTypeConfiguration<Host>
    {
        public HostMap()
        {
            // Primary Key
            this.HasKey(t => t.HostID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.HostCode)
                .HasMaxLength(50);

            this.Property(t => t.Industry)
                .HasMaxLength(50);

            this.Property(t => t.Province)
               .HasMaxLength(50);

            this.Property(t => t.City)
              .HasMaxLength(50);

            this.Property(t => t.Url)
              .HasMaxLength(50);

            this.Property(t => t.Address)
                .HasMaxLength(255);

            this.Property(t => t.Manager)
               .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Hosts");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ClientNum).HasColumnName("ClientNum");
            this.Property(t => t.HostCode).HasColumnName("HostCode");
            this.Property(t => t.BranchNum).HasColumnName("BranchNum");
            this.Property(t => t.Industry).HasColumnName("Industry");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Manager).HasColumnName("Manager");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.IsVaild).HasColumnName("IsVaild");
        }
    }
}
