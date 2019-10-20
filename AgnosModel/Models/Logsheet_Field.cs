using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Logsheet_Field
    {
        public int Logsheet_Field_ID { get; set; }
        public Nullable<int> Logsheet_Group_ID { get; set; }
        public Nullable<int> Field_ID { get; set; }
        public virtual Logsheet_Group Logsheet_Group { get; set; }
        public virtual Template_Field Template_Field { get; set; }
    }
}
