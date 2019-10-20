using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Logsheet
    {
        public Logsheet()
        {
            this.Logsheet_Group = new List<Logsheet_Group>();
            this.Logsheet_Product_Form = new List<Logsheet_Product_Form>();
            this.Upload_Attachment = new List<Upload_Attachment>();
        }

        public int Logsheet_ID { get; set; }
        public string Product_Code { get; set; }
        public Nullable<int> Template_ID { get; set; }
        public string Lot_No { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Work_Order_No { get; set; }
        public string Lotgsheet_Status { get; set; }
        public string Record_Status { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<System.DateTime> Expiry_Date { get; set; }
        public string UAI { get; set; }
        public string RTS { get; set; }
        public string Rework { get; set; }
        public string Scrap { get; set; }
        public string RMR_No { get; set; }
        public string Reason_If_Reject { get; set; }
        public Nullable<System.DateTime> Authorized_Date { get; set; }
        public string Dispose { get; set; }
        public string Status { get; set; }
        public string Authorized_By { get; set; }
        public Nullable<int> UOM { get; set; }
        public Nullable<int> Packaging { get; set; }
        public Nullable<int> PD_Issue { get; set; }
        public Nullable<int> PD_Approval { get; set; }
        public Nullable<int> QA_Approval { get; set; }
        public Nullable<bool> Display_Product_Form_Field { get; set; }
        public Nullable<System.DateTime> Transferring_Date { get; set; }
        public Nullable<int> Revision_No { get; set; }
        public string Dilution_Tank_No { get; set; }
        public string Note { get; set; }
        public string Product_Name { get; set; }
        public string Form_No { get; set; }
        public Nullable<System.DateTime> PD_Issue_Date { get; set; }
        public Nullable<System.DateTime> PD_Approval_Date { get; set; }
        public Nullable<System.DateTime> QA_Approval_Date { get; set; }
        public string Product_Status { get; set; }
        public string Product_Remarks { get; set; }
        public string Remarks { get; set; }
        public virtual Global_Lookup_Data Global_Lookup_Data { get; set; }
        public virtual Global_Lookup_Data Global_Lookup_Data1 { get; set; }
        public virtual Template_Logsheet Template_Logsheet { get; set; }
        public virtual ICollection<Logsheet_Group> Logsheet_Group { get; set; }
        public virtual ICollection<Logsheet_Product_Form> Logsheet_Product_Form { get; set; }
        public virtual User_Profile User_Profile { get; set; }
        public virtual User_Profile User_Profile1 { get; set; }
        public virtual User_Profile User_Profile2 { get; set; }
        public virtual ICollection<Upload_Attachment> Upload_Attachment { get; set; }
    }
}
