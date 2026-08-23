using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class ObjectiveMap : EntityTypeConfiguration<Objective>
    {
        public ObjectiveMap()
        {
            // Primary Key
            this.HasKey(t => t.ObjectiveId);

            // Properties
            this.Property(t => t.UserId)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("Objective");
            this.Property(t => t.ObjectiveId).HasColumnName("ObjectiveId");
            this.Property(t => t.Level).HasColumnName("Level");
            this.Property(t => t.OrganId).HasColumnName("OrganId");
            this.Property(t => t.TeamId).HasColumnName("TeamId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.Month).HasColumnName("Month");
            this.Property(t => t.Accounts).HasColumnName("Accounts");
            this.Property(t => t.TopObjective).HasColumnName("TopObjective");
        }
    }
}
