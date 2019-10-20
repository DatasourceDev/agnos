using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class CMS_Drum_ControlMap : EntityTypeConfiguration<CMS_Drum_Control>
    {
        public CMS_Drum_ControlMap()
        {
            // Primary Key
            this.HasKey(t => t.Drum_Control_ID);

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
            this.ToTable("CMS_Drum_Control");
            this.Property(t => t.Drum_Control_ID).HasColumnName("Drum_Control_ID");
            this.Property(t => t.Product_ID).HasColumnName("Product_ID");
            this.Property(t => t.Drum_Type_ID).HasColumnName("Drum_Type_ID");
            this.Property(t => t.Running_Number).HasColumnName("Running_Number");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Product_Code).HasColumnName("Product_Code");

            // Relationships
            this.HasOptional(t => t.CMS_Drum_Type)
                .WithMany(t => t.CMS_Drum_Control)
                .HasForeignKey(d => d.Drum_Type_ID);
            this.HasOptional(t => t.CMS_Product)
                .WithMany(t => t.CMS_Drum_Control)
                .HasForeignKey(d => d.Product_ID);

        }
    }
}
