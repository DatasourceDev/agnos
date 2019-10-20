using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class CMS_Purge
    {
        public int Purge_ID { get; set; }
        public Nullable<int> Filling_Station_ID { get; set; }
        public string Drum_Code { get; set; }
        public Nullable<decimal> Initial_Weight { get; set; }
        public Nullable<decimal> Final_Weight { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Record_Status { get; set; }
        public Nullable<int> Delivery_ID { get; set; }
        public string Delivery_Status { get; set; }

        public virtual CMS_Filling_Station CMS_Filling_Station { get; set; }

    }
}
