using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class CMS_Delivery
    {
        public CMS_Delivery()
        {
            this.CMS_Delivery_Detail = new List<CMS_Delivery_Detail>();
        }

        public int Delivery_ID { get; set; }
        public string Delivery_Order_No { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Record_Status { get; set; }
        public Nullable<bool> Completed { get; set; }
        public virtual ICollection<CMS_Delivery_Detail> CMS_Delivery_Detail { get; set; }
    }
}
