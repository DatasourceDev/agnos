using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class CMS_Charging_Control
    {
        public int Charging_Control_ID { get; set; }
        public Nullable<int> Drum_Type_ID { get; set; }
        public Nullable<int> Max_Of_Change { get; set; }
        public string Action { get; set; }
        public string Record_Status { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Product_Code { get; set; }
        public Nullable<int> Product_ID { get; set; }
        public string Drum_Code { get; set; }
        public virtual CMS_Drum_Type CMS_Drum_Type { get; set; }
        public virtual CMS_Product CMS_Product { get; set; }
    }
}
