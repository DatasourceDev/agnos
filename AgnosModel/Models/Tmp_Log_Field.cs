using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Tmp_Log_Field
    {
        public int Tmp_Log_Field_ID { get; set; }
        public Nullable<int> Tmp_Log_Group_ID { get; set; }
        public Nullable<int> Field_ID { get; set; }
        public Nullable<int> Field_Order { get; set; }
        public virtual Template_Field Template_Field { get; set; }
        public virtual Tmp_Log_Group Tmp_Log_Group { get; set; }
    }
}
