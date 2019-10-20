using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Raw_MaterialMap : EntityTypeConfiguration<Raw_Material>
    {
        public Raw_MaterialMap()
        {
            // Primary Key
            this.HasKey(t => t.Raw_Material_ID);

            // Properties
            this.Property(t => t.Product_Code)
                .HasMaxLength(300);

            this.Property(t => t.Product_Name)
                .HasMaxLength(500);

            this.Property(t => t.Lot_No)
                .HasMaxLength(150);

            this.Property(t => t.COA)
                .HasMaxLength(50);

            this.Property(t => t.Reject_Reason)
                .HasMaxLength(500);

            this.Property(t => t.Reject_Remarks)
                .HasMaxLength(500);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Remarks_Pass)
                .HasMaxLength(300);

            this.Property(t => t.Remarks_Pending)
                .HasMaxLength(300);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Analysis_Type)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Raw_Material");
            this.Property(t => t.Raw_Material_ID).HasColumnName("Raw_Material_ID");
            this.Property(t => t.Product_Code).HasColumnName("Product_Code");
            this.Property(t => t.Product_Name).HasColumnName("Product_Name");
            this.Property(t => t.Dented).HasColumnName("Dented");
            this.Property(t => t.Hole).HasColumnName("Hole");
            this.Property(t => t.Lot_No).HasColumnName("Lot_No");
            this.Property(t => t.Total_Receiving).HasColumnName("Total_Receiving");
            this.Property(t => t.Receiving_Date).HasColumnName("Receiving_Date");
            this.Property(t => t.COA).HasColumnName("COA");
            this.Property(t => t.Status_Pass).HasColumnName("Status_Pass");
            this.Property(t => t.Status_Pending).HasColumnName("Status_Pending");
            this.Property(t => t.Status_Reject).HasColumnName("Status_Reject");
            this.Property(t => t.Qty_Pass).HasColumnName("Qty_Pass");
            this.Property(t => t.Qty_Pending).HasColumnName("Qty_Pending");
            this.Property(t => t.Qty_Reject).HasColumnName("Qty_Reject");
            this.Property(t => t.Reject_Reason).HasColumnName("Reject_Reason");
            this.Property(t => t.Reject_Remarks).HasColumnName("Reject_Remarks");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Expiring_Date).HasColumnName("Expiring_Date");
            this.Property(t => t.Remarks_Pass).HasColumnName("Remarks_Pass");
            this.Property(t => t.Remarks_Pending).HasColumnName("Remarks_Pending");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.UOM).HasColumnName("UOM");
            this.Property(t => t.Packaging).HasColumnName("Packaging");
            this.Property(t => t.Record_By).HasColumnName("Record_By");
            this.Property(t => t.Report_Date).HasColumnName("Report_Date");
            this.Property(t => t.Analysis_Type).HasColumnName("Analysis_Type");
            this.Property(t => t.Authorized_By).HasColumnName("Authorized_By");
            this.Property(t => t.Authorized_Date).HasColumnName("Authorized_Date");

            // Relationships
            this.HasOptional(t => t.Global_Lookup_Data)
                .WithMany(t => t.Raw_Material)
                .HasForeignKey(d => d.UOM);
            this.HasOptional(t => t.Global_Lookup_Data1)
                .WithMany(t => t.Raw_Material1)
                .HasForeignKey(d => d.Packaging);
            this.HasOptional(t => t.User_Profile)
                .WithMany(t => t.Raw_Material)
                .HasForeignKey(d => d.Authorized_By);
            this.HasOptional(t => t.User_Profile1)
                .WithMany(t => t.Raw_Material1)
                .HasForeignKey(d => d.Record_By);

        }
    }
}
