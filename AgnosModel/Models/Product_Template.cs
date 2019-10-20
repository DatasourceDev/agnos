using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Product_Template
    {
        public int Product_Template_ID { get; set; }
        public string Product_Code { get; set; }
        public Nullable<int> Template_ID { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Product_Name { get; set; }
        public string From_No { get; set; }
        public string Revision { get; set; }
        public string Record_Status { get; set; }
        public string Dilution_Tank_No { get; set; }
        public virtual Template_Logsheet Template_Logsheet { get; set; }
    }
}
