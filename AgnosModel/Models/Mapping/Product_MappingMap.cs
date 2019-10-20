using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Product_MappingMap : EntityTypeConfiguration<Product_Mapping>
    {
        public Product_MappingMap()
        {
            // Primary Key
            this.HasKey(t => t.Map_ID);

            // Properties
            this.Property(t => t.Product_Code)
                .HasMaxLength(300);

            this.Property(t => t.Product_Name)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Product_Mapping");
            this.Property(t => t.Map_ID).HasColumnName("Map_ID");
            this.Property(t => t.Product_Code).HasColumnName("Product_Code");
            this.Property(t => t.Product_Name).HasColumnName("Product_Name");
        }
    }
}
