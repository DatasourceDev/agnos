using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Raw_Material_FormMap : EntityTypeConfiguration<Raw_Material_Form>
    {
        public Raw_Material_FormMap()
        {
            // Primary Key
            this.HasKey(t => t.Form_ID);

            // Properties
            this.Property(t => t.Status)
                .HasMaxLength(50);

            this.Property(t => t.File_Name)
                .HasMaxLength(300);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Raw_Material_Form");
            this.Property(t => t.Form_ID).HasColumnName("Form_ID");
            this.Property(t => t.Raw_Material_ID).HasColumnName("Raw_Material_ID");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.File).HasColumnName("File");
            this.Property(t => t.File_Name).HasColumnName("File_Name");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");

            // Relationships
            this.HasOptional(t => t.Raw_Material)
                .WithMany(t => t.Raw_Material_Form)
                .HasForeignKey(d => d.Raw_Material_ID);

        }
    }
}
