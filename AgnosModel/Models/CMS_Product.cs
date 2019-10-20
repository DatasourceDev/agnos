using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class CMS_Product
    {
        public CMS_Product()
        {
            this.CMS_Charge = new List<CMS_Charge>();
            this.CMS_Charging_Control = new List<CMS_Charging_Control>();
            this.CMS_Delivery_Detail = new List<CMS_Delivery_Detail>();
            this.CMS_Drum_Control = new List<CMS_Drum_Control>();
            this.CMS_Skip_Purging = new List<CMS_Skip_Purging>();
        }

        public int CMS_Product_ID { get; set; }
        public Nullable<int> Filling_Station_ID { get; set; }
        public string Record_Status { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Product_Code { get; set; }
        public string Product_Name { get; set; }
        public virtual ICollection<CMS_Charge> CMS_Charge { get; set; }
        public virtual ICollection<CMS_Charging_Control> CMS_Charging_Control { get; set; }
        public virtual ICollection<CMS_Delivery_Detail> CMS_Delivery_Detail { get; set; }
        public virtual ICollection<CMS_Drum_Control> CMS_Drum_Control { get; set; }
        public virtual CMS_Filling_Station CMS_Filling_Station { get; set; }
        public virtual ICollection<CMS_Skip_Purging> CMS_Skip_Purging { get; set; }
    }
}
