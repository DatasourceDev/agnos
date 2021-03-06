using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class CMS_Charge
    {
        public int Charge_ID { get; set; }
        public string Drum_Code { get; set; }
        public string Lot_No { get; set; }
        public Nullable<int> Quantity_Scanned { get; set; }
        public Nullable<int> Filling_Station_ID { get; set; }
        public Nullable<decimal> Initial_Weight { get; set; }
        public Nullable<decimal> Final_Weight { get; set; }
        public Nullable<int> No_Of_Charging { get; set; }
        public Nullable<int> Max_No_Of_Charging { get; set; }
        public string Create_By { get; set; }
        public string Create_By2 { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Record_Status { get; set; }
        public string Product_Code { get; set; }
        public Nullable<int> Product_ID { get; set; }
        public string Delivery_Status { get; set; }
        public string Delivery_Order_No { get; set; }
        public Nullable<System.DateTime> Date_Delivered { get; set; }
        public Nullable<int> Delivery_ID { get; set; }
        public Nullable<int> Profile_ID { get; set; }
        public virtual CMS_Product CMS_Product { get; set; }
        public virtual User_Profile User_Profile { get; set; }
        public virtual CMS_Filling_Station CMS_Filling_Station { get; set; }
    }
}
