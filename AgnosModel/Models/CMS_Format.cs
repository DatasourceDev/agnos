using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class CMS_Format
    {
        public int Format_ID { get; set; }
        public string Format_Code { get; set; }
        public Nullable<int> Lot_No_Length { get; set; }
        public Nullable<int> Product_Code_Length { get; set; }
        public string Record_Status { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
    }
}
