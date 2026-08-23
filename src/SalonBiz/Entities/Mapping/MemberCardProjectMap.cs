using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class MemberCardProjectMap : EntityTypeConfiguration<MemberCardProject>
    {
        public MemberCardProjectMap()
        {
            // Primary Key
            this.HasKey(t => t.MemberCardProjectId);

            // Properties

            // Table & Column Mappings
            this.ToTable("MemberCardProjects");
            this.Property(t => t.MemberCardProjectId).HasColumnName("MemberCardProjectId");
            this.Property(t => t.MemberCardId).HasColumnName("MemberCardId");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");


            // Relationships
            this.HasRequired(t => t.Card)
                .WithMany(t => t.Projects)
                .HasForeignKey(d => d.MemberCardId);

        }
    }
}
