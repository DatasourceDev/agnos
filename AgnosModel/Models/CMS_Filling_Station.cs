using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class CMS_Filling_Station
    {
        public CMS_Filling_Station()
        {
            this.CMS_Charge = new List<CMS_Charge>();
            this.CMS_Product = new List<CMS_Product>();
        }

        public int Filling_Station_ID { get; set; }
        public string Station_Code { get; set; }
        public string Record_Status { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public virtual ICollection<CMS_Charge> CMS_Charge { get; set; }
        public virtual ICollection<CMS_Product> CMS_Product { get; set; }
    }
}
