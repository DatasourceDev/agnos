using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Template_Field
    {
        public Template_Field()
        {
            this.Logsheet_Field = new List<Logsheet_Field>();
            this.Logsheet_Map = new List<Logsheet_Map>();
            this.Tmp_Log_Field = new List<Tmp_Log_Field>();
        }

        public int Field_ID { get; set; }
        public string Field_Name { get; set; }
        public Nullable<int> Field_Order { get; set; }
        public string Data_Type { get; set; }
        public string Field_From { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Record_Status { get; set; }
        public virtual ICollection<Logsheet_Field> Logsheet_Field { get; set; }
        public virtual ICollection<Logsheet_Map> Logsheet_Map { get; set; }
        public virtual ICollection<Tmp_Log_Field> Tmp_Log_Field { get; set; }
    }
}
