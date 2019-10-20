using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Template_GroupMap : EntityTypeConfiguration<Template_Group>
    {
        public Template_GroupMap()
        {
            // Primary Key
            this.HasKey(t => t.Group_ID);

            // Properties
            this.Property(t => t.Group_Name)
                .HasMaxLength(500);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Template_Group");
            this.Property(t => t.Group_ID).HasColumnName("Group_ID");
            this.Property(t => t.Group_Name).HasColumnName("Group_Name");
            this.Property(t => t.Group_Order).HasColumnName("Group_Order");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Role_ID).HasColumnName("Role_ID");

            // Relationships
            this.HasOptional(t => t.Role)
                .WithMany(t => t.Template_Group)
                .HasForeignKey(d => d.Role_ID);

        }
    }
}
