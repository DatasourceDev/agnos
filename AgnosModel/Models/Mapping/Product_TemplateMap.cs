using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Product_TemplateMap : EntityTypeConfiguration<Product_Template>
    {
        public Product_TemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.Product_Template_ID);

            // Properties
            this.Property(t => t.Product_Code)
                .HasMaxLength(150);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Product_Name)
                .HasMaxLength(500);

            this.Property(t => t.From_No)
                .HasMaxLength(150);

            this.Property(t => t.Revision)
                .HasMaxLength(50);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Dilution_Tank_No)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Product_Template");
            this.Property(t => t.Product_Template_ID).HasColumnName("Product_Template_ID");
            this.Property(t => t.Product_Code).HasColumnName("Product_Code");
            this.Property(t => t.Template_ID).HasColumnName("Template_ID");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Product_Name).HasColumnName("Product_Name");
            this.Property(t => t.From_No).HasColumnName("From_No");
            this.Property(t => t.Revision).HasColumnName("Revision");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Dilution_Tank_No).HasColumnName("Dilution_Tank_No");

            // Relationships
            this.HasOptional(t => t.Template_Logsheet)
                .WithMany(t => t.Product_Template)
                .HasForeignKey(d => d.Template_ID);

        }
    }
}
