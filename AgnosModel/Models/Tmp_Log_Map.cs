using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Tmp_Log_Map
    {
        public int Tmp_Log_Map_ID { get; set; }
        public Nullable<int> Tmp_Log_Header_ID { get; set; }
        public Nullable<int> Field_ID { get; set; }
        public Nullable<int> Header_ID { get; set; }
        public string Option_Selected { get; set; }
        public string Option_Text { get; set; }
        public string Option_Data_Type { get; set; }
        public string Option_Field_From { get; set; }
        public string Option_Range_From { get; set; }
        public string Option_Range_To { get; set; }
        public string Lot_No { get; set; }
        public string Option_Dropdown_Type { get; set; }
        public Nullable<int> Map_Order { get; set; }
        public virtual Tmp_Log_Header Tmp_Log_Header { get; set; }
    }
}
