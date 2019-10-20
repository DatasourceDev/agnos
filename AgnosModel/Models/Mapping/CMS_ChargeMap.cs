using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class CMS_ChargeMap : EntityTypeConfiguration<CMS_Charge>
    {
        public CMS_ChargeMap()
        {
            // Primary Key
            this.HasKey(t => t.Charge_ID);

            // Properties
            this.Property(t => t.Drum_Code)
                .HasMaxLength(150);

            this.Property(t => t.Lot_No)
                .HasMaxLength(150);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Product_Code)
                .HasMaxLength(150);

            this.Property(t => t.Delivery_Status)
                .HasMaxLength(50);

            this.Property(t => t.Delivery_Order_No)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("CMS_Charge");
            this.Property(t => t.Charge_ID).HasColumnName("Charge_ID");
            this.Property(t => t.Drum_Code).HasColumnName("Drum_Code");
            this.Property(t => t.Lot_No).HasColumnName("Lot_No");
            this.Property(t => t.Quantity_Scanned).HasColumnName("Quantity_Scanned");
            this.Property(t => t.Filling_Station_ID).HasColumnName("Filling_Station_ID");
            this.Property(t => t.Initial_Weight).HasColumnName("Initial_Weight");
            this.Property(t => t.Final_Weight).HasColumnName("Final_Weight");
            this.Property(t => t.No_Of_Charging).HasColumnName("No_Of_Charging");
            this.Property(t => t.Max_No_Of_Charging).HasColumnName("Max_No_Of_Charging");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Product_Code).HasColumnName("Product_Code");
            this.Property(t => t.Product_ID).HasColumnName("Product_ID");
            this.Property(t => t.Delivery_Status).HasColumnName("Delivery_Status");
            this.Property(t => t.Delivery_Order_No).HasColumnName("Delivery_Order_No");
            this.Property(t => t.Date_Delivered).HasColumnName("Date_Delivered");
            this.Property(t => t.Delivery_ID).HasColumnName("Delivery_ID");
            this.Property(t => t.Profile_ID).HasColumnName("Profile_ID");

            // Relationships
            this.HasOptional(t => t.CMS_Product)
                .WithMany(t => t.CMS_Charge)
                .HasForeignKey(d => d.Product_ID);
            this.HasOptional(t => t.User_Profile)
                .WithMany(t => t.CMS_Charge)
                .HasForeignKey(d => d.Profile_ID);
            this.HasOptional(t => t.CMS_Filling_Station)
                .WithMany(t => t.CMS_Charge)
                .HasForeignKey(d => d.Filling_Station_ID);

        }
    }
}
