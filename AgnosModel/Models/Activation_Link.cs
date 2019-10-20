using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Activation_Link
    {
        public int Activation_ID { get; set; }
        public int Profile_ID { get; set; }
        public string Activation_Code { get; set; }
        public Nullable<System.DateTime> Time_Limit { get; set; }
        public virtual User_Profile User_Profile { get; set; }
    }
}
