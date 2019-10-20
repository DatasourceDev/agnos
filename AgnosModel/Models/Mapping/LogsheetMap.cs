using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class LogsheetMap : EntityTypeConfiguration<Logsheet>
    {
        public LogsheetMap()
        {
            // Primary Key
            this.HasKey(t => t.Logsheet_ID);

            // Properties
            this.Property(t => t.Product_Code)
                .HasMaxLength(150);

            this.Property(t => t.Lot_No)
                .HasMaxLength(150);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Work_Order_No)
                .HasMaxLength(150);

            this.Property(t => t.Lotgsheet_Status)
                .HasMaxLength(50);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.UAI)
                .HasMaxLength(300);

            this.Property(t => t.RTS)
                .HasMaxLength(300);

            this.Property(t => t.Rework)
                .HasMaxLength(300);

            this.Property(t => t.Scrap)
                .HasMaxLength(300);

            this.Property(t => t.RMR_No)
                .HasMaxLength(150);

            this.Property(t => t.Reason_If_Reject)
                .HasMaxLength(500);

            this.Property(t => t.Dispose)
                .HasMaxLength(100);

            this.Property(t => t.Status)
                .HasMaxLength(100);

            this.Property(t => t.Authorized_By)
                .HasMaxLength(150);

            this.Property(t => t.Dilution_Tank_No)
                .HasMaxLength(100);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.Product_Name)
                .HasMaxLength(500);

            this.Property(t => t.Form_No)
                .HasMaxLength(50);

            this.Property(t => t.Product_Status)
                .HasMaxLength(50);

            this.Property(t => t.Product_Remarks)
                .HasMaxLength(1000);

            this.Property(t => t.Remarks)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Logsheet");
            this.Property(t => t.Logsheet_ID).HasColumnName("Logsheet_ID");
            this.Property(t => t.Product_Code).HasColumnName("Product_Code");
            this.Property(t => t.Template_ID).HasColumnName("Template_ID");
            this.Property(t => t.Lot_No).HasColumnName("Lot_No");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Work_Order_No).HasColumnName("Work_Order_No");
            this.Property(t => t.Lotgsheet_Status).HasColumnName("Lotgsheet_Status");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Expiry_Date).HasColumnName("Expiry_Date");
            this.Property(t => t.UAI).HasColumnName("UAI");
            this.Property(t => t.RTS).HasColumnName("RTS");
            this.Property(t => t.Rework).HasColumnName("Rework");
            this.Property(t => t.Scrap).HasColumnName("Scrap");
            this.Property(t => t.RMR_No).HasColumnName("RMR_No");
            this.Property(t => t.Reason_If_Reject).HasColumnName("Reason_If_Reject");
            this.Property(t => t.Authorized_Date).HasColumnName("Authorized_Date");
            this.Property(t => t.Dispose).HasColumnName("Dispose");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Authorized_By).HasColumnName("Authorized_By");
            this.Property(t => t.UOM).HasColumnName("UOM");
            this.Property(t => t.Packaging).HasColumnName("Packaging");
            this.Property(t => t.PD_Issue).HasColumnName("PD_Issue");
            this.Property(t => t.PD_Approval).HasColumnName("PD_Approval");
            this.Property(t => t.QA_Approval).HasColumnName("QA_Approval");
            this.Property(t => t.Display_Product_Form_Field).HasColumnName("Display_Product_Form_Field");
            this.Property(t => t.Transferring_Date).HasColumnName("Transferring_Date");
            this.Property(t => t.Revision_No).HasColumnName("Revision_No");
            this.Property(t => t.Dilution_Tank_No).HasColumnName("Dilution_Tank_No");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Product_Name).HasColumnName("Product_Name");
            this.Property(t => t.Form_No).HasColumnName("Form_No");
            this.Property(t => t.PD_Issue_Date).HasColumnName("PD_Issue_Date");
            this.Property(t => t.PD_Approval_Date).HasColumnName("PD_Approval_Date");
            this.Property(t => t.QA_Approval_Date).HasColumnName("QA_Approval_Date");
            this.Property(t => t.Product_Status).HasColumnName("Product_Status");
            this.Property(t => t.Product_Remarks).HasColumnName("Product_Remarks");
            this.Property(t => t.Remarks).HasColumnName("Remarks");

            // Relationships
            this.HasOptional(t => t.Global_Lookup_Data)
                .WithMany(t => t.Logsheets)
                .HasForeignKey(d => d.UOM);
            this.HasOptional(t => t.Global_Lookup_Data1)
                .WithMany(t => t.Logsheets1)
                .HasForeignKey(d => d.Packaging);
            this.HasOptional(t => t.Template_Logsheet)
                .WithMany(t => t.Logsheets)
                .HasForeignKey(d => d.Template_ID);
            this.HasOptional(t => t.User_Profile)
                .WithMany(t => t.Logsheets)
                .HasForeignKey(d => d.PD_Issue);
            this.HasOptional(t => t.User_Profile1)
                .WithMany(t => t.Logsheets1)
                .HasForeignKey(d => d.PD_Approval);
            this.HasOptional(t => t.User_Profile2)
                .WithMany(t => t.Logsheets2)
                .HasForeignKey(d => d.QA_Approval);

        }
    }
}
