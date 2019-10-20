using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class User_ProfileMap : EntityTypeConfiguration<User_Profile>
    {
        public User_ProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.Profile_ID);

            // Properties
            this.Property(t => t.ApplicationUser_Id)
                .HasMaxLength(128);

            this.Property(t => t.Name)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(100);

            this.Property(t => t.Phone)
                .HasMaxLength(150);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.LDAP_Username)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("User_Profile");
            this.Property(t => t.Profile_ID).HasColumnName("Profile_ID");
            this.Property(t => t.Email_Address).HasColumnName("Email_Address");
            this.Property(t => t.PWD).HasColumnName("PWD");
            this.Property(t => t.Login_Attempt).HasColumnName("Login_Attempt");
            this.Property(t => t.Activated).HasColumnName("Activated");
            this.Property(t => t.ApplicationUser_Id).HasColumnName("ApplicationUser_Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Latest_Connection).HasColumnName("Latest_Connection");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Role_ID).HasColumnName("Role_ID");
            this.Property(t => t.LDAP_Username).HasColumnName("LDAP_Username");
            this.Property(t => t.Email_Notification).HasColumnName("Email_Notification");

            // Relationships
            this.HasOptional(t => t.Role)
                .WithMany(t => t.User_Profile)
                .HasForeignKey(d => d.Role_ID);

        }
    }
}
