using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Material_Reject_Form
    {
        public System.Guid Form_ID { get; set; }
        public Nullable<int> Material_Reject_ID { get; set; }
        public byte[] File { get; set; }
        public string File_Name { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public virtual Material_Reject Material_Reject { get; set; }
    }
}
