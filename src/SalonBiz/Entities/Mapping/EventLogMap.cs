using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class EventLogMap : EntityTypeConfiguration<EventLog>
    {
        public EventLogMap()
        {
            // Primary Key
            this.HasKey(t => t.LogId);

            // Properties
            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ClientId)
                .HasMaxLength(50);

            this.Property(t => t.Shell)
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("EventLog");
            this.Property(t => t.LogId).HasColumnName("LogId");
            this.Property(t => t.HostId).HasColumnName("HostId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.MemberId).HasColumnName("MemberId");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
            this.Property(t => t.TypeId).HasColumnName("TypeId");
            this.Property(t => t.Level).HasColumnName("Level");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
        }
    }
}
