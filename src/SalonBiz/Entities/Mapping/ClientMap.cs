using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class ClientMap : EntityTypeConfiguration<Client>
    {
        public ClientMap()
        {
            // Primary Key
            this.HasKey(t => t.ClientID);

            // Properties
            this.Property(t => t.MobileGUID)
                .HasMaxLength(50);

            this.Property(t => t.MobileNumber)
                .HasMaxLength(50);

            this.Property(t => t.UserId)
                .HasMaxLength(200);

            this.Property(t => t.MobileModel)
                .HasMaxLength(200);

            this.Property(t => t.IsVaild)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Clients");
            this.Property(t => t.ClientID).HasColumnName("ClientID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.OrganID).HasColumnName("OrganID");
            this.Property(t => t.MobileGUID).HasColumnName("MobileGUID");
            this.Property(t => t.MobileModel).HasColumnName("MobileModel");
            this.Property(t => t.MobileNumber).HasColumnName("MobileNumber");
            this.Property(t => t.IsVaild).HasColumnName("IsVaild");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.LastSignIn).HasColumnName("LastSignIn");

            // Relationships
            this.HasRequired(t => t.Host)
                .WithMany(t => t.Clients)
                .HasForeignKey(d => d.HostID);

        }
    }
}
