using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Lot_Number
    {
        public int Lot_Number_ID { get; set; }
        public string Product_Code { get; set; }
        public Nullable<System.DateTime> Lot_Number_Date { get; set; }
        public Nullable<int> Template_ID { get; set; }
        public string Lot_No { get; set; }
        public string Record_Status { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public Nullable<int> No_Count { get; set; }
        public virtual Template_Logsheet Template_Logsheet { get; set; }
    }
}
