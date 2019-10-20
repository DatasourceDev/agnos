using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Raw_Material
    {
        public Raw_Material()
        {
            this.Material_Reject = new List<Material_Reject>();
            this.Raw_Material_Attachment = new List<Raw_Material_Attachment>();
            this.Raw_Material_Form = new List<Raw_Material_Form>();
        }

        public int Raw_Material_ID { get; set; }
        public string Product_Code { get; set; }
        public string Product_Name { get; set; }
        public Nullable<bool> Dented { get; set; }
        public Nullable<bool> Hole { get; set; }
        public string Lot_No { get; set; }
        public Nullable<decimal> Total_Receiving { get; set; }
        public Nullable<System.DateTime> Receiving_Date { get; set; }
        public string COA { get; set; }
        public Nullable<bool> Status_Pass { get; set; }
        public Nullable<bool> Status_Pending { get; set; }
        public Nullable<bool> Status_Reject { get; set; }
        public Nullable<decimal> Qty_Pass { get; set; }
        public Nullable<decimal> Qty_Pending { get; set; }
        public Nullable<decimal> Qty_Reject { get; set; }
        public string Reject_Reason { get; set; }
        public string Reject_Remarks { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public Nullable<System.DateTime> Expiring_Date { get; set; }
        public string Remarks_Pass { get; set; }
        public string Remarks_Pending { get; set; }
        public string Record_Status { get; set; }
        public Nullable<int> UOM { get; set; }
        public Nullable<int> Packaging { get; set; }
        public Nullable<int> Record_By { get; set; }
        public Nullable<System.DateTime> Report_Date { get; set; }
        public string Analysis_Type { get; set; }
        public Nullable<int> Authorized_By { get; set; }
        public Nullable<System.DateTime> Authorized_Date { get; set; }
        public virtual Global_Lookup_Data Global_Lookup_Data { get; set; }
        public virtual Global_Lookup_Data Global_Lookup_Data1 { get; set; }
        public virtual ICollection<Material_Reject> Material_Reject { get; set; }
        public virtual ICollection<Raw_Material_Attachment> Raw_Material_Attachment { get; set; }
        public virtual User_Profile User_Profile { get; set; }
        public virtual ICollection<Raw_Material_Form> Raw_Material_Form { get; set; }
        public virtual User_Profile User_Profile1 { get; set; }
    }
}
