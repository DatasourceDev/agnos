using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Lot_NumberMap : EntityTypeConfiguration<Lot_Number>
    {
        public Lot_NumberMap()
        {
            // Primary Key
            this.HasKey(t => t.Lot_Number_ID);

            // Properties
            this.Property(t => t.Product_Code)
                .HasMaxLength(150);

            this.Property(t => t.Lot_No)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Lot_Number");
            this.Property(t => t.Lot_Number_ID).HasColumnName("Lot_Number_ID");
            this.Property(t => t.Product_Code).HasColumnName("Product_Code");
            this.Property(t => t.Lot_Number_Date).HasColumnName("Lot_Number_Date");
            this.Property(t => t.Template_ID).HasColumnName("Template_ID");
            this.Property(t => t.Lot_No).HasColumnName("Lot_No");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.No_Count).HasColumnName("No_Count");

            // Relationships
            this.HasOptional(t => t.Template_Logsheet)
                .WithMany(t => t.Lot_Number)
                .HasForeignKey(d => d.Template_ID);

        }
    }
}
