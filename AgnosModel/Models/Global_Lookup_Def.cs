using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Global_Lookup_Def
    {
        public Global_Lookup_Def()
        {
            this.Global_Lookup_Data = new List<Global_Lookup_Data>();
        }

        public int Def_ID { get; set; }
        public string Def_Code { get; set; }
        public string Name { get; set; }
        public string Record_Status { get; set; }
        public virtual ICollection<Global_Lookup_Data> Global_Lookup_Data { get; set; }
    }
}
