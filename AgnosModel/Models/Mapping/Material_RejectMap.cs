using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Material_RejectMap : EntityTypeConfiguration<Material_Reject>
    {
        public Material_RejectMap()
        {
            // Primary Key
            this.HasKey(t => t.Material_Reject_ID);

            // Properties
            this.Property(t => t.Reject_From)
                .HasMaxLength(50);

            this.Property(t => t.Reject_Supplier_Name)
                .HasMaxLength(300);

            this.Property(t => t.Reject_Inhouse_Location)
                .HasMaxLength(300);

            this.Property(t => t.QA_Staff)
                .HasMaxLength(150);

            this.Property(t => t.D8_No)
                .HasMaxLength(150);

            this.Property(t => t.Disposition_Others_Description)
                .HasMaxLength(300);

            this.Property(t => t.Instructions)
                .HasMaxLength(500);

            this.Property(t => t.Re_Inspection_On_Rework)
                .HasMaxLength(500);

            this.Property(t => t.Reject_Status)
                .HasMaxLength(50);

            this.Property(t => t.Carried_Out_By)
                .HasMaxLength(150);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Reject_Customer_Name)
                .HasMaxLength(300);

            this.Property(t => t.Overall_Status)
                .HasMaxLength(50);

            this.Property(t => t.Remarks)
                .HasMaxLength(500);

            this.Property(t => t.Disposition_Action_By)
                .HasMaxLength(150);

            this.Property(t => t.Product_Code)
                .HasMaxLength(300);

            this.Property(t => t.Product_Name)
                .HasMaxLength(500);

            this.Property(t => t.Lot_No)
                .HasMaxLength(150);

            this.Property(t => t.Project_Name)
                .HasMaxLength(50);

            this.Property(t => t.Defect_Description)
                .HasMaxLength(500);

            this.Property(t => t.RMR_No)
                .HasMaxLength(50);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Material_Reject");
            this.Property(t => t.Material_Reject_ID).HasColumnName("Material_Reject_ID");
            this.Property(t => t.Raw_Material_ID).HasColumnName("Raw_Material_ID");
            this.Property(t => t.Reject_From).HasColumnName("Reject_From");
            this.Property(t => t.Reject_Supplier_Name).HasColumnName("Reject_Supplier_Name");
            this.Property(t => t.Reject_Inhouse_Location).HasColumnName("Reject_Inhouse_Location");
            this.Property(t => t.QA_Staff).HasColumnName("QA_Staff");
            this.Property(t => t.D8_No).HasColumnName("D8_No");
            this.Property(t => t.Disposition_RTS).HasColumnName("Disposition_RTS");
            this.Property(t => t.Disposition_Rework).HasColumnName("Disposition_Rework");
            this.Property(t => t.Disposition_Scrap).HasColumnName("Disposition_Scrap");
            this.Property(t => t.Disposition_UAI).HasColumnName("Disposition_UAI");
            this.Property(t => t.Disposition_Others).HasColumnName("Disposition_Others");
            this.Property(t => t.Disposition_Others_Description).HasColumnName("Disposition_Others_Description");
            this.Property(t => t.Instructions).HasColumnName("Instructions");
            this.Property(t => t.PD).HasColumnName("PD");
            this.Property(t => t.QA).HasColumnName("QA");
            this.Property(t => t.Logistics).HasColumnName("Logistics");
            this.Property(t => t.Sales).HasColumnName("Sales");
            this.Property(t => t.PD_Date).HasColumnName("PD_Date");
            this.Property(t => t.QA_Date).HasColumnName("QA_Date");
            this.Property(t => t.Logistics_Date).HasColumnName("Logistics_Date");
            this.Property(t => t.Sales_Date).HasColumnName("Sales_Date");
            this.Property(t => t.GM_Approval_Date).HasColumnName("GM_Approval_Date");
            this.Property(t => t.Disposition_Date).HasColumnName("Disposition_Date");
            this.Property(t => t.Re_Inspection_On_Rework).HasColumnName("Re_Inspection_On_Rework");
            this.Property(t => t.Reject_Status).HasColumnName("Reject_Status");
            this.Property(t => t.Carried_Out_By).HasColumnName("Carried_Out_By");
            this.Property(t => t.Carried_Out_Date).HasColumnName("Carried_Out_Date");
            this.Property(t => t.Review_Date).HasColumnName("Review_Date");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Reject_Customer_Name).HasColumnName("Reject_Customer_Name");
            this.Property(t => t.Overall_Status).HasColumnName("Overall_Status");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.Disposition_Action_By).HasColumnName("Disposition_Action_By");
            this.Property(t => t.Product_Code).HasColumnName("Product_Code");
            this.Property(t => t.Product_Name).HasColumnName("Product_Name");
            this.Property(t => t.Lot_No).HasColumnName("Lot_No");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Project_Name).HasColumnName("Project_Name");
            this.Property(t => t.Defect_Description).HasColumnName("Defect_Description");
            this.Property(t => t.RMR_No).HasColumnName("RMR_No");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.GM_Approval).HasColumnName("GM_Approval");
            this.Property(t => t.UOM).HasColumnName("UOM");
            this.Property(t => t.Packaging).HasColumnName("Packaging");
            this.Property(t => t.PD_Approval).HasColumnName("PD_Approval");
            this.Property(t => t.QA_Approval).HasColumnName("QA_Approval");
            this.Property(t => t.Logistics_Approval).HasColumnName("Logistics_Approval");
            this.Property(t => t.Sales_Approval).HasColumnName("Sales_Approval");

            // Relationships
            this.HasOptional(t => t.Global_Lookup_Data)
                .WithMany(t => t.Material_Reject)
                .HasForeignKey(d => d.UOM);
            this.HasOptional(t => t.Global_Lookup_Data1)
                .WithMany(t => t.Material_Reject1)
                .HasForeignKey(d => d.Packaging);
            this.HasOptional(t => t.Raw_Material)
                .WithMany(t => t.Material_Reject)
                .HasForeignKey(d => d.Raw_Material_ID);
            this.HasOptional(t => t.User_Profile)
                .WithMany(t => t.Material_Reject)
                .HasForeignKey(d => d.PD_Approval);
            this.HasOptional(t => t.User_Profile1)
                .WithMany(t => t.Material_Reject1)
                .HasForeignKey(d => d.QA_Approval);
            this.HasOptional(t => t.User_Profile2)
                .WithMany(t => t.Material_Reject2)
                .HasForeignKey(d => d.Logistics_Approval);
            this.HasOptional(t => t.User_Profile3)
                .WithMany(t => t.Material_Reject3)
                .HasForeignKey(d => d.Sales_Approval);
            this.HasOptional(t => t.User_Profile4)
                .WithMany(t => t.Material_Reject4)
                .HasForeignKey(d => d.GM_Approval);

        }
    }
}
