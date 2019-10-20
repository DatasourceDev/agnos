using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class CMS_Delivery_Detail
    {
        public int CMS_Delivery_Detail_ID { get; set; }
        public Nullable<int> Delivery_ID { get; set; }
        public Nullable<System.DateTime> Date_Delivered { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Record_Status { get; set; }
        public Nullable<int> Product_ID { get; set; }
        public string Drum_Code { get; set; }
        public Nullable<int> No_Of_Containers { get; set; }
        public string Product_Code { get; set; }
        public virtual CMS_Delivery CMS_Delivery { get; set; }
        public virtual CMS_Product CMS_Product { get; set; }
    }
}
