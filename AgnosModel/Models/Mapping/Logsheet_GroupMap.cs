using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Logsheet_GroupMap : EntityTypeConfiguration<Logsheet_Group>
    {
        public Logsheet_GroupMap()
        {
            // Primary Key
            this.HasKey(t => t.Logsheet_Group_ID);

            // Properties
            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Logsheet_Group");
            this.Property(t => t.Logsheet_Group_ID).HasColumnName("Logsheet_Group_ID");
            this.Property(t => t.Logsheet_ID).HasColumnName("Logsheet_ID");
            this.Property(t => t.Group_ID).HasColumnName("Group_ID");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");

            // Relationships
            this.HasOptional(t => t.Logsheet)
                .WithMany(t => t.Logsheet_Group)
                .HasForeignKey(d => d.Logsheet_ID);
            this.HasOptional(t => t.Template_Group)
                .WithMany(t => t.Logsheet_Group)
                .HasForeignKey(d => d.Group_ID);

        }
    }
}
