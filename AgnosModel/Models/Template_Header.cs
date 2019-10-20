using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Template_Header
    {
        public Template_Header()
        {
            this.Logsheet_Header = new List<Logsheet_Header>();
            this.Logsheet_Map = new List<Logsheet_Map>();
            this.Tmp_Log_Header = new List<Tmp_Log_Header>();
        }

        public int Header_ID { get; set; }
        public string Header_Name { get; set; }
        public Nullable<int> Header_Order { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Record_Status { get; set; }
        public virtual ICollection<Logsheet_Header> Logsheet_Header { get; set; }
        public virtual ICollection<Logsheet_Map> Logsheet_Map { get; set; }
        public virtual ICollection<Tmp_Log_Header> Tmp_Log_Header { get; set; }
    }
}
