using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Logsheet_Group
    {
        public Logsheet_Group()
        {
            this.Logsheet_Field = new List<Logsheet_Field>();
            this.Logsheet_Header = new List<Logsheet_Header>();
        }

        public int Logsheet_Group_ID { get; set; }
        public Nullable<int> Logsheet_ID { get; set; }
        public Nullable<int> Group_ID { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public virtual Logsheet Logsheet { get; set; }
        public virtual ICollection<Logsheet_Field> Logsheet_Field { get; set; }
        public virtual Template_Group Template_Group { get; set; }
        public virtual ICollection<Logsheet_Header> Logsheet_Header { get; set; }
    }
}
