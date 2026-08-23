using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class ProjectGoodsMap : EntityTypeConfiguration<ProjectGoods>
    {
        public ProjectGoodsMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectGoodsID);

            // Properties

            // Table & Column Mappings
            this.ToTable("ProjectGoods");
            this.Property(t => t.ProjectGoodsID).HasColumnName("ProjectGoodsID");
            this.Property(t => t.GoodsID).HasColumnName("GoodsID");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.Quantity).HasColumnName("Quantity");

            // Relationships
            this.HasRequired(t => t.Project)
                .WithMany(t => t.ProjectGoods)
                .HasForeignKey(d => d.ProjectID);

            this.HasRequired(t => t.Goods)
                .WithMany(t => t.ProjectGoods)
                .HasForeignKey(d => d.GoodsID);
        }
    }
}
