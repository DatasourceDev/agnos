using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Logsheet_Header
    {
        public Logsheet_Header()
        {
            this.Logsheet_Map = new List<Logsheet_Map>();
        }

        public int Logsheet_Header_ID { get; set; }
        public Nullable<int> Header_ID { get; set; }
        public Nullable<int> Logsheet_Group_ID { get; set; }
        public virtual Logsheet_Group Logsheet_Group { get; set; }
        public virtual Template_Header Template_Header { get; set; }
        public virtual ICollection<Logsheet_Map> Logsheet_Map { get; set; }
    }
}
