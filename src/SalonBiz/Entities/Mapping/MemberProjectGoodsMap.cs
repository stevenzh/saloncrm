using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class MemberProjectGoodsMap : EntityTypeConfiguration<MemberProjectGoods>
    {
        public MemberProjectGoodsMap()
        {
            // Primary Key
            this.HasKey(t => t.MemberProjectGoodsID);

            // Properties
            this.Property(t => t.MemberProjectGoodsID)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("MemberProjectGoods");
            this.Property(t => t.MemberProjectGoodsID).HasColumnName("MemberProjectGoodsID");
            this.Property(t => t.MemberProjectId).HasColumnName("MemberProjectId");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.GoodsID).HasColumnName("GoodsID");

            // Relationships
            this.HasRequired(t => t.Goods)
                .WithMany(t => t.MemberProjectGoods)
                .HasForeignKey(d => d.GoodsID);


            this.HasRequired(t => t.MemberProject)
                .WithMany(t => t.MemberProjectGoods)
                .HasForeignKey(d => d.MemberProjectId);

        }
    }
}
