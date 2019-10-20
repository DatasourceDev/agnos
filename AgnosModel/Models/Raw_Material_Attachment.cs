using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Raw_Material_Attachment
    {
        public System.Guid Attachment_ID { get; set; }
        public Nullable<int> Raw_Material_ID { get; set; }
        public byte[] Attachment_File { get; set; }
        public string Attachment_Field { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Attachment_File_Name { get; set; }
        public string Record_Status { get; set; }
        public virtual Raw_Material Raw_Material { get; set; }
    }
}
