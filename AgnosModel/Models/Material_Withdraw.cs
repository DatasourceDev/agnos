using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Material_Withdraw
    {
        public int Withdraw_ID { get; set; }
        public string Product_Code { get; set; }
        public string Product_Name { get; set; }
        public string Lot_No { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> Total_Receiving { get; set; }
        public Nullable<System.DateTime> Receiving_Date { get; set; }
        public Nullable<decimal> Qty_Withdraw { get; set; }
        public Nullable<System.DateTime> Withdraw_Date { get; set; }
        public Nullable<System.TimeSpan> From_Time { get; set; }
        public Nullable<System.TimeSpan> To_Time { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Finished_Goods { get; set; }
        public string Finished_Goods_Lot_No { get; set; }
        public Nullable<int> PLC { get; set; }
        public string Record_Status { get; set; }
        public Nullable<int> UOM { get; set; }
        public Nullable<int> Packaging { get; set; }
        public string Transaction_Type { get; set; }
        public Nullable<System.DateTime> Transferring_Date { get; set; }
        public string Remarks { get; set; }
        public string Withdraw_From { get; set; }
        public string Withdraw_To { get; set; }
        public string Location { get; set; }
        public Nullable<decimal> Qty_Balance { get; set; }
        public virtual Global_Lookup_Data Global_Lookup_Data { get; set; }
        public virtual Global_Lookup_Data Global_Lookup_Data1 { get; set; }
        public virtual User_Profile User_Profile { get; set; }
    }
}
