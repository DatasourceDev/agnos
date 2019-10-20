using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class CMS_Charging_ControlMap : EntityTypeConfiguration<CMS_Charging_Control>
    {
        public CMS_Charging_ControlMap()
        {
            // Primary Key
            this.HasKey(t => t.Charging_Control_ID);

            // Properties
            this.Property(t => t.Action)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Product_Code)
                .HasMaxLength(150);

            this.Property(t => t.Drum_Code)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("CMS_Charging_Control");
            this.Property(t => t.Charging_Control_ID).HasColumnName("Charging_Control_ID");
            this.Property(t => t.Drum_Type_ID).HasColumnName("Drum_Type_ID");
            this.Property(t => t.Max_Of_Change).HasColumnName("Max_Of_Change");
            this.Property(t => t.Action).HasColumnName("Action");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Product_Code).HasColumnName("Product_Code");
            this.Property(t => t.Product_ID).HasColumnName("Product_ID");
            this.Property(t => t.Drum_Code).HasColumnName("Drum_Code");

            // Relationships
            this.HasOptional(t => t.CMS_Drum_Type)
                .WithMany(t => t.CMS_Charging_Control)
                .HasForeignKey(d => d.Drum_Type_ID);
            this.HasOptional(t => t.CMS_Product)
                .WithMany(t => t.CMS_Charging_Control)
                .HasForeignKey(d => d.Product_ID);

        }
    }
}
