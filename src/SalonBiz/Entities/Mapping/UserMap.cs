using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<ApplicationUser>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Password)
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .HasMaxLength(500);

            this.Property(t => t.Type)
                .HasMaxLength(500);

            this.Property(t => t.UserCnName)
                .HasMaxLength(500);

            this.Property(t => t.Rank)
                .HasMaxLength(50);

            this.Property(t => t.Status)
                 .HasMaxLength(200);

            this.Property(t => t.OpenID)
                 .HasMaxLength(200);

            this.Property(t => t.MobileNumber)
                 .HasMaxLength(200);

            this.Property(t => t.Position)
                 .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("ApplicationUser");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.HostId).HasColumnName("HostId");
            this.Property(t => t.OrganId).HasColumnName("OrganId");
            this.Property(t => t.UserCnName).HasColumnName("UserCnName");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.IsAdminUser).HasColumnName("IsAdminUser");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.OpenID).HasColumnName("OpenID");
            this.Property(t => t.Rank).HasColumnName("Rank");
            this.Property(t => t.Position).HasColumnName("Position");
            this.Property(t => t.MobileNumber).HasColumnName("MobileNumber");
            this.Property(t => t.IsMajorOrgan).HasColumnName("IsMajorOrgan");
            this.Property(t => t.JoinDate).HasColumnName("JoinDate");
            this.Property(t => t.ResignDate).HasColumnName("ResignDate");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");


            this.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(m =>
                {
                    m.ToTable("UserRoles");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("RoleId");
                });
        }
    }
}
