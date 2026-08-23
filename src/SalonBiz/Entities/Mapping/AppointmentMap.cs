using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SalonCRM.Models.Mapping
{
    public class AppointmentMap : EntityTypeConfiguration<Appointment>
    {
        public AppointmentMap()
        {
            // Primary Key
            this.HasKey(t => t.AppointmentID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Phone)
                .HasMaxLength(50);

            this.Property(t => t.Projects)
                .HasMaxLength(250);

            this.Property(t => t.ClientId)
                .HasMaxLength(50);

            this.Property(t => t.Wokers)
                .HasMaxLength(250);

            this.Property(t => t.BookRooms)
                .HasMaxLength(250);

            this.Property(t => t.BookStatus)
                .HasMaxLength(50);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(200);

            this.Property(t => t.Salesman)
                .HasMaxLength(50);
 
            // Table & Column Mappings
            this.ToTable("Appointment");
            this.Property(t => t.AppointmentID).HasColumnName("AppointmentID");
            this.Property(t => t.HostID).HasColumnName("HostID");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.MemberID).HasColumnName("MemberID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.BookDate).HasColumnName("BookDate");
            this.Property(t => t.Projects).HasColumnName("Projects");
            this.Property(t => t.Wokers).HasColumnName("Wokers");
            this.Property(t => t.BookRooms).HasColumnName("BookRooms");
            this.Property(t => t.BookStatus).HasColumnName("BookStatus");
            this.Property(t => t.ClientId).HasColumnName("ClientID");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.BookId).HasColumnName("BookId");
            this.Property(t => t.Approved).HasColumnName("Approved");
            this.Property(t => t.Salesman).HasColumnName("Salesman");

            // Relationships
            this.HasRequired(t => t.Member)
                .WithMany(t => t.Appointments)
                .HasForeignKey(d => d.MemberID);

        }
    }
}
