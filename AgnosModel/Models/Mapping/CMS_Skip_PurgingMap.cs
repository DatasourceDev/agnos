using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class CMS_Skip_PurgingMap : EntityTypeConfiguration<CMS_Skip_Purging>
    {
        public CMS_Skip_PurgingMap()
        {
            // Primary Key
            this.HasKey(t => t.Skip_Purging_ID);

            // Properties
            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Product_Code)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("CMS_Skip_Purging");
            this.Property(t => t.Skip_Purging_ID).HasColumnName("Skip_Purging_ID");
            this.Property(t => t.Drum_Type_ID).HasColumnName("Drum_Type_ID");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Product_Code).HasColumnName("Product_Code");
            this.Property(t => t.Product_ID).HasColumnName("Product_ID");

            // Relationships
            this.HasOptional(t => t.CMS_Drum_Type)
                .WithMany(t => t.CMS_Skip_Purging)
                .HasForeignKey(d => d.Drum_Type_ID);
            this.HasOptional(t => t.CMS_Product)
                .WithMany(t => t.CMS_Skip_Purging)
                .HasForeignKey(d => d.Product_ID);

        }
    }
}
