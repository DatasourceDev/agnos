using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Material_Reject_FormMap : EntityTypeConfiguration<Material_Reject_Form>
    {
        public Material_Reject_FormMap()
        {
            // Primary Key
            this.HasKey(t => t.Form_ID);

            // Properties
            this.Property(t => t.File_Name)
                .HasMaxLength(300);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Material_Reject_Form");
            this.Property(t => t.Form_ID).HasColumnName("Form_ID");
            this.Property(t => t.Material_Reject_ID).HasColumnName("Material_Reject_ID");
            this.Property(t => t.File).HasColumnName("File");
            this.Property(t => t.File_Name).HasColumnName("File_Name");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");

            // Relationships
            this.HasOptional(t => t.Material_Reject)
                .WithMany(t => t.Material_Reject_Form)
                .HasForeignKey(d => d.Material_Reject_ID);

        }
    }
}
