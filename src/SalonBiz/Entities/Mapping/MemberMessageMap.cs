using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class MemberMessageMap : EntityTypeConfiguration<MemberMessage>
    {
        public MemberMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.MessageID);

            // Properties
            this.Property(t => t.OpenID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.MsgType)
                .HasMaxLength(50);

            this.Property(t => t.FileUrl)
                .HasMaxLength(250);

            this.Property(t => t.Content)
                .HasMaxLength(500);

            this.Property(t => t.IsCallBack)
                .HasMaxLength(10);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("MemberMessage");
            this.Property(t => t.MessageID).HasColumnName("MessageID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.OpenID).HasColumnName("OpenID");
            this.Property(t => t.MsgType).HasColumnName("MsgType");
            this.Property(t => t.FileUrl).HasColumnName("FileUrl");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.InOut).HasColumnName("InOut");
            this.Property(t => t.IsCallBack).HasColumnName("IsCallBack");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
        }
    }
}
