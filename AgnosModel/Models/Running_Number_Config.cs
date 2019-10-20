using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Running_Number_Config
    {
        public int Running_Number_Config_ID { get; set; }
        public string Running_Number_Type { get; set; }
        public Nullable<int> Number_Of_Digit { get; set; }
        public string Prefix_Ref_No { get; set; }
        public Nullable<int> Ref_Count { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public Nullable<int> Running_Year { get; set; }
        
    }
}
