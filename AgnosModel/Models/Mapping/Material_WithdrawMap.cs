using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Material_WithdrawMap : EntityTypeConfiguration<Material_Withdraw>
    {
        public Material_WithdrawMap()
        {
            // Primary Key
            this.HasKey(t => t.Withdraw_ID);

            // Properties
            this.Property(t => t.Product_Code)
                .HasMaxLength(300);

            this.Property(t => t.Product_Name)
                .HasMaxLength(500);

            this.Property(t => t.Lot_No)
                .HasMaxLength(150);

            this.Property(t => t.Unit)
                .HasMaxLength(150);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Finished_Goods)
                .HasMaxLength(150);

            this.Property(t => t.Finished_Goods_Lot_No)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Transaction_Type)
                .HasMaxLength(50);

            this.Property(t => t.Remarks)
                .HasMaxLength(500);

            this.Property(t => t.Withdraw_From)
                .HasMaxLength(50);

            this.Property(t => t.Withdraw_To)
                .HasMaxLength(50);

            this.Property(t => t.Location)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Material_Withdraw");
            this.Property(t => t.Withdraw_ID).HasColumnName("Withdraw_ID");
            this.Property(t => t.Product_Code).HasColumnName("Product_Code");
            this.Property(t => t.Product_Name).HasColumnName("Product_Name");
            this.Property(t => t.Lot_No).HasColumnName("Lot_No");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.Total_Receiving).HasColumnName("Total_Receiving");
            this.Property(t => t.Receiving_Date).HasColumnName("Receiving_Date");
            this.Property(t => t.Qty_Withdraw).HasColumnName("Qty_Withdraw");
            this.Property(t => t.Withdraw_Date).HasColumnName("Withdraw_Date");
            this.Property(t => t.From_Time).HasColumnName("From_Time");
            this.Property(t => t.To_Time).HasColumnName("To_Time");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Finished_Goods).HasColumnName("Finished_Goods");
            this.Property(t => t.Finished_Goods_Lot_No).HasColumnName("Finished_Goods_Lot_No");
            this.Property(t => t.PLC).HasColumnName("PLC");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.UOM).HasColumnName("UOM");
            this.Property(t => t.Packaging).HasColumnName("Packaging");
            this.Property(t => t.Transaction_Type).HasColumnName("Transaction_Type");
            this.Property(t => t.Transferring_Date).HasColumnName("Transferring_Date");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.Withdraw_From).HasColumnName("Withdraw_From");
            this.Property(t => t.Withdraw_To).HasColumnName("Withdraw_To");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.Qty_Balance).HasColumnName("Qty_Balance");

            // Relationships
            this.HasOptional(t => t.Global_Lookup_Data)
                .WithMany(t => t.Material_Withdraw)
                .HasForeignKey(d => d.UOM);
            this.HasOptional(t => t.Global_Lookup_Data1)
                .WithMany(t => t.Material_Withdraw1)
                .HasForeignKey(d => d.Packaging);
            this.HasOptional(t => t.User_Profile)
                .WithMany(t => t.Material_Withdraw)
                .HasForeignKey(d => d.PLC);

        }
    }
}
