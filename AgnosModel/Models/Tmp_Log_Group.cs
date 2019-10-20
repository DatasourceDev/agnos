using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Tmp_Log_Group
    {
        public Tmp_Log_Group()
        {
            this.Tmp_Log_Field = new List<Tmp_Log_Field>();
            this.Tmp_Log_Header = new List<Tmp_Log_Header>();
        }

        public int Tmp_Log_Group_ID { get; set; }
        public Nullable<int> Template_ID { get; set; }
        public Nullable<int> Group_ID { get; set; }
        public Nullable<int> Group_Order { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Sub_Group_Name { get; set; }
        public virtual Template_Group Template_Group { get; set; }
        public virtual Template_Logsheet Template_Logsheet { get; set; }
        public virtual ICollection<Tmp_Log_Field> Tmp_Log_Field { get; set; }
        public virtual ICollection<Tmp_Log_Header> Tmp_Log_Header { get; set; }
    }
}
