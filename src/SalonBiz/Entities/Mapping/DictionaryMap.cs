using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class DictionaryMap : EntityTypeConfiguration<Dictionary>
    {
        public DictionaryMap()
        {
            // Primary Key
            this.HasKey(t => t.TypeId);

            // Properties
            this.Property(t => t.Identifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.KeyValue)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Shell)
                .HasMaxLength(500);

            this.Property(t => t.Contents)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(500);


            // Table & Column Mappings
            this.ToTable("Dictionary");
            this.Property(t => t.TypeId).HasColumnName("TypeId");
            this.Property(t => t.HostId).HasColumnName("HostId");
            this.Property(t => t.Identifier).HasColumnName("Identifier");
            this.Property(t => t.KeyValue).HasColumnName("KeyValue");
            this.Property(t => t.Shell).HasColumnName("Shell");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.IsVaild).HasColumnName("IsVaild");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");

        }
    }
}
