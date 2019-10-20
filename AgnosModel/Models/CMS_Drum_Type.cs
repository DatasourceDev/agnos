using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class CMS_Drum_Type
    {
        public CMS_Drum_Type()
        {
            this.CMS_Charging_Control = new List<CMS_Charging_Control>();
            this.CMS_Drum_Control = new List<CMS_Drum_Control>();
            this.CMS_Skip_Purging = new List<CMS_Skip_Purging>();
        }

        public int Drum_Type_ID { get; set; }
        public string Drum_Type { get; set; }
        public string Record_Status { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public virtual ICollection<CMS_Charging_Control> CMS_Charging_Control { get; set; }
        public virtual ICollection<CMS_Drum_Control> CMS_Drum_Control { get; set; }
        public virtual ICollection<CMS_Skip_Purging> CMS_Skip_Purging { get; set; }
    }
}
