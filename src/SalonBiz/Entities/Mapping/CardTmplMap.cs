using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class CardTmplMap : EntityTypeConfiguration<CardTmpl>
    {
        public CardTmplMap()
        {
            // Primary Key
            this.HasKey(t => t.TmplID);

            // Properties
            this.Property(t => t.CardType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Title)
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("CardTmpl");
            this.Property(t => t.TmplID).HasColumnName("TmplID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.CardType).HasColumnName("CardType");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.IsVaild).HasColumnName("IsVaild");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
        }
    }
}
