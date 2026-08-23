using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class BookGoodsMap : EntityTypeConfiguration<BookGoods>
    {
        public BookGoodsMap()
        {
            // Primary Key
            this.HasKey(t => t.BookGoodsID);

            // Properties
            this.Property(t => t.BookGoodsID)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("BookGoods");
            this.Property(t => t.BookGoodsID).HasColumnName("BookGoodsID");
            this.Property(t => t.BookProjectID).HasColumnName("BookProjectID");
            this.Property(t => t.BookID).HasColumnName("BookID");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.GoodsID).HasColumnName("GoodsID");

            // Relationships
            this.HasRequired(t => t.Goods)
                .WithMany(t => t.BookGoods)
                .HasForeignKey(d => d.GoodsID);


            this.HasRequired(t => t.BookProject)
                .WithMany(t => t.BookGoods)
                .HasForeignKey(d => d.BookProjectID);

        }
    }
}
