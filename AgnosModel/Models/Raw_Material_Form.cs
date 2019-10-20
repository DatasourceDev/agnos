using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Raw_Material_Form
    {
        public System.Guid Form_ID { get; set; }
        public Nullable<int> Raw_Material_ID { get; set; }
        public string Status { get; set; }
        public byte[] File { get; set; }
        public string File_Name { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public virtual Raw_Material Raw_Material { get; set; }
    }
}
