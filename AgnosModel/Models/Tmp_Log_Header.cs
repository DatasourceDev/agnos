using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Tmp_Log_Header
    {
        public Tmp_Log_Header()
        {
            this.Tmp_Log_Map = new List<Tmp_Log_Map>();
        }

        public int Tmp_Log_Header_ID { get; set; }
        public Nullable<int> Tmp_Log_Group_ID { get; set; }
        public Nullable<int> Header_ID { get; set; }
        public Nullable<int> Header_Order { get; set; }
        public Nullable<bool> Sumary_Report_Display { get; set; }
        public virtual Template_Header Template_Header { get; set; }
        public virtual Tmp_Log_Group Tmp_Log_Group { get; set; }
        public virtual ICollection<Tmp_Log_Map> Tmp_Log_Map { get; set; }
    }
}
