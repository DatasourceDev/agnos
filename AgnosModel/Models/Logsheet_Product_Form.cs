using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Logsheet_Product_Form
    {
        public System.Guid Form_ID { get; set; }
        public Nullable<int> Logsheet_ID { get; set; }
        public byte[] File { get; set; }
        public string File_Name { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public virtual Logsheet Logsheet { get; set; }
    }
}
