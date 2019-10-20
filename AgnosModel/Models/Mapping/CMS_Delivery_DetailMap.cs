using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class CMS_Delivery_DetailMap : EntityTypeConfiguration<CMS_Delivery_Detail>
    {
        public CMS_Delivery_DetailMap()
        {
            // Primary Key
            this.HasKey(t => t.CMS_Delivery_Detail_ID);

            // Properties
            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Drum_Code)
                .HasMaxLength(1000);

            this.Property(t => t.Product_Code)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("CMS_Delivery_Detail");
            this.Property(t => t.CMS_Delivery_Detail_ID).HasColumnName("CMS_Delivery_Detail_ID");
            this.Property(t => t.Delivery_ID).HasColumnName("Delivery_ID");
            this.Property(t => t.Date_Delivered).HasColumnName("Date_Delivered");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Product_ID).HasColumnName("Product_ID");
            this.Property(t => t.Drum_Code).HasColumnName("Drum_Code");
            this.Property(t => t.No_Of_Containers).HasColumnName("No_Of_Containers");
            this.Property(t => t.Product_Code).HasColumnName("Product_Code");

            // Relationships
            this.HasOptional(t => t.CMS_Delivery)
                .WithMany(t => t.CMS_Delivery_Detail)
                .HasForeignKey(d => d.Delivery_ID);
            this.HasOptional(t => t.CMS_Product)
                .WithMany(t => t.CMS_Delivery_Detail)
                .HasForeignKey(d => d.Product_ID);

        }
    }
}
