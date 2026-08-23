using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class WxMemberMap : EntityTypeConfiguration<WxMember>
    {
        public WxMemberMap()
        {
            // Primary Key
            this.HasKey(t => t.MemberID);

            // Properties
            this.Property(t => t.OpenID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NickName)
                .HasMaxLength(500)
                .IsUnicode(true);

            this.Property(t => t.Language)
                .HasMaxLength(20);

            this.Property(t => t.City)
                .HasMaxLength(50);

            this.Property(t => t.Province)
                .HasMaxLength(50);

            this.Property(t => t.Country)
                .HasMaxLength(50);

            this.Property(t => t.HeadImgUrl)
                .HasMaxLength(500);

            this.Property(t => t.Subscribe)
               .HasMaxLength(1);

            this.Property(t => t.Binding)
                .HasMaxLength(5);

            this.Property(t => t.EmployeeID)
                .HasMaxLength(200);


            // Table & Column Mappings
            this.ToTable("WxMembers");
            this.Property(t => t.MemberID).HasColumnName("MemberID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.OpenID).HasColumnName("OpenID");
            this.Property(t => t.NickName).HasColumnName("NickName");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.Language).HasColumnName("Language");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.Country).HasColumnName("Country");
            this.Property(t => t.HeadImgUrl).HasColumnName("HeadImgUrl");
            this.Property(t => t.SubscribeTime).HasColumnName("SubscribeTime");
            this.Property(t => t.UnsubscribeTime).HasColumnName("UnsubscribeTime");
            this.Property(t => t.Subscribe).HasColumnName("Subscribe");
            this.Property(t => t.LastMessageTime).HasColumnName("LastMessageTime");
        }

    }
}