using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Activation_LinkMap : EntityTypeConfiguration<Activation_Link>
    {
        public Activation_LinkMap()
        {
            // Primary Key
            this.HasKey(t => t.Activation_ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Activation_Link");
            this.Property(t => t.Activation_ID).HasColumnName("Activation_ID");
            this.Property(t => t.Profile_ID).HasColumnName("Profile_ID");
            this.Property(t => t.Activation_Code).HasColumnName("Activation_Code");
            this.Property(t => t.Time_Limit).HasColumnName("Time_Limit");

            // Relationships
            this.HasRequired(t => t.User_Profile)
                .WithMany(t => t.Activation_Link)
                .HasForeignKey(d => d.Profile_ID);

        }
    }
}
