using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Logsheet_Map
    {
        public int Logsheet_Map_ID { get; set; }
        public Nullable<int> Logsheet_Header_ID { get; set; }
        public Nullable<int> Field_ID { get; set; }
        public Nullable<int> Header_ID { get; set; }
        public string Option_Selected { get; set; }
        public string Text_Display { get; set; }
        public virtual Logsheet_Header Logsheet_Header { get; set; }
        public virtual Template_Header Template_Header { get; set; }
        public virtual Template_Field Template_Field { get; set; }
    }
}
