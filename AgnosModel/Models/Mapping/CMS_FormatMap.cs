using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class CMS_FormatMap : EntityTypeConfiguration<CMS_Format>
    {
        public CMS_FormatMap()
        {
            // Primary Key
            this.HasKey(t => t.Format_ID);

            // Properties
            this.Property(t => t.Format_Code)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("CMS_Format");
            this.Property(t => t.Format_ID).HasColumnName("Format_ID");
            this.Property(t => t.Format_Code).HasColumnName("Format_Code");
            this.Property(t => t.Lot_No_Length).HasColumnName("Lot_No_Length");
            this.Property(t => t.Product_Code_Length).HasColumnName("Product_Code_Length");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
        }
    }
}
