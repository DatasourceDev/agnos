using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Template_Group
    {
        public Template_Group()
        {
            this.Logsheet_Group = new List<Logsheet_Group>();
            this.Tmp_Log_Group = new List<Tmp_Log_Group>();
        }

        public int Group_ID { get; set; }
        public string Group_Name { get; set; }
        public Nullable<int> Group_Order { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Record_Status { get; set; }
        public Nullable<int> Role_ID { get; set; }
        public virtual ICollection<Logsheet_Group> Logsheet_Group { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Tmp_Log_Group> Tmp_Log_Group { get; set; }
    }
}
