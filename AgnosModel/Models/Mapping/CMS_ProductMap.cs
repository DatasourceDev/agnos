using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class CMS_ProductMap : EntityTypeConfiguration<CMS_Product>
    {
        public CMS_ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.CMS_Product_ID);

            // Properties
            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Product_Code)
                .HasMaxLength(150);

            this.Property(t => t.Product_Name)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("CMS_Product");
            this.Property(t => t.CMS_Product_ID).HasColumnName("CMS_Product_ID");
            this.Property(t => t.Filling_Station_ID).HasColumnName("Filling_Station_ID");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Product_Code).HasColumnName("Product_Code");
            this.Property(t => t.Product_Name).HasColumnName("Product_Name");

            // Relationships
            this.HasOptional(t => t.CMS_Filling_Station)
                .WithMany(t => t.CMS_Product)
                .HasForeignKey(d => d.Filling_Station_ID);

        }
    }
}
