using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class GoodsMap : EntityTypeConfiguration<Goods>
    {
        public GoodsMap()
        {
            // Primary Key
            this.HasKey(t => t.GoodsID);

            // Properties
            this.Property(t => t.GoodsCode)
                //.IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Unit)
                .HasMaxLength(50);

            this.Property(t => t.Category)
                .HasMaxLength(50);

            this.Property(t => t.Brand)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Goods");
            this.Property(t => t.GoodsID).HasColumnName("GoodsID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.GoodsCode).HasColumnName("GoodsCode");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.Brand).HasColumnName("Brand");
            this.Property(t => t.IsVaild).HasColumnName("IsVaild");

            // Relationships
            //this.HasRequired(t => t.Host)
            //    .WithMany(t => t.Goods)
            //    .HasForeignKey(d => d.HostID);

        }
    }
}
