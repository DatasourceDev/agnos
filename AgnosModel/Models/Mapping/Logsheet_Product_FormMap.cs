using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Logsheet_Product_FormMap : EntityTypeConfiguration<Logsheet_Product_Form>
    {
        public Logsheet_Product_FormMap()
        {
            // Primary Key
            this.HasKey(t => t.Form_ID);

            // Properties
            this.Property(t => t.File_Name)
                .HasMaxLength(300);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Logsheet_Product_Form");
            this.Property(t => t.Form_ID).HasColumnName("Form_ID");
            this.Property(t => t.Logsheet_ID).HasColumnName("Logsheet_ID");
            this.Property(t => t.File).HasColumnName("File");
            this.Property(t => t.File_Name).HasColumnName("File_Name");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");

            // Relationships
            this.HasOptional(t => t.Logsheet)
                .WithMany(t => t.Logsheet_Product_Form)
                .HasForeignKey(d => d.Logsheet_ID);

        }
    }
}
