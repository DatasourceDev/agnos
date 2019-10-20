using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Material_Reject
    {
        public Material_Reject()
        {
            this.Material_Reject_Form = new List<Material_Reject_Form>();
        }

        public int Material_Reject_ID { get; set; }
        public Nullable<int> Raw_Material_ID { get; set; }
        public string Reject_From { get; set; }
        public string Reject_Supplier_Name { get; set; }
        public string Reject_Inhouse_Location { get; set; }
        public string QA_Staff { get; set; }
        public string D8_No { get; set; }
        public Nullable<bool> Disposition_RTS { get; set; }
        public Nullable<bool> Disposition_Rework { get; set; }
        public Nullable<bool> Disposition_Scrap { get; set; }
        public Nullable<bool> Disposition_UAI { get; set; }
        public Nullable<bool> Disposition_Others { get; set; }
        public string Disposition_Others_Description { get; set; }
        public string Instructions { get; set; }
        public Nullable<bool> PD { get; set; }
        public Nullable<bool> QA { get; set; }
        public Nullable<bool> Logistics { get; set; }
        public Nullable<bool> Sales { get; set; }
        public Nullable<System.DateTime> PD_Date { get; set; }
        public Nullable<System.DateTime> QA_Date { get; set; }
        public Nullable<System.DateTime> Logistics_Date { get; set; }
        public Nullable<System.DateTime> Sales_Date { get; set; }
        public Nullable<System.DateTime> GM_Approval_Date { get; set; }
        public Nullable<System.DateTime> Disposition_Date { get; set; }
        public string Re_Inspection_On_Rework { get; set; }
        public string Reject_Status { get; set; }
        public string Carried_Out_By { get; set; }
        public Nullable<System.DateTime> Carried_Out_Date { get; set; }
        public Nullable<System.DateTime> Review_Date { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Reject_Customer_Name { get; set; }
        public string Overall_Status { get; set; }
        public string Remarks { get; set; }
        public string Disposition_Action_By { get; set; }
        public string Product_Code { get; set; }
        public string Product_Name { get; set; }
        public string Lot_No { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public string Project_Name { get; set; }
        public string Defect_Description { get; set; }
        public string RMR_No { get; set; }
        public string Record_Status { get; set; }
        public Nullable<int> GM_Approval { get; set; }
        public Nullable<int> UOM { get; set; }
        public Nullable<int> Packaging { get; set; }
        public Nullable<int> PD_Approval { get; set; }
        public Nullable<int> QA_Approval { get; set; }
        public Nullable<int> Logistics_Approval { get; set; }
        public Nullable<int> Sales_Approval { get; set; }
        public virtual Global_Lookup_Data Global_Lookup_Data { get; set; }
        public virtual Global_Lookup_Data Global_Lookup_Data1 { get; set; }
        public virtual ICollection<Material_Reject_Form> Material_Reject_Form { get; set; }
        public virtual Raw_Material Raw_Material { get; set; }
        public virtual User_Profile User_Profile { get; set; }
        public virtual User_Profile User_Profile1 { get; set; }
        public virtual User_Profile User_Profile2 { get; set; }
        public virtual User_Profile User_Profile3 { get; set; }
        public virtual User_Profile User_Profile4 { get; set; }
    }
}
