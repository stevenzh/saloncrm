using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class CardTmplProjectMap : EntityTypeConfiguration<CardTmplProject>
    {
        public CardTmplProjectMap()
        {
            // Primary Key
            this.HasKey(t => t.TmplProjectID);

            // Properties

            // Table & Column Mappings
            this.ToTable("CardTmplProject");
            this.Property(t => t.TmplProjectID).HasColumnName("TmplProjectID");
            this.Property(t => t.TmplID).HasColumnName("TmplID");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Quantity).HasColumnName("Quantity");

            // Relationships
            this.HasRequired(t => t.Project)
                .WithMany(t => t.TmplProjects)
                .HasForeignKey(d => d.ProjectID);

            this.HasRequired(t => t.Card)
                .WithMany(t => t.Projects)
                .HasForeignKey(d => d.TmplID);
        }
    }
}
