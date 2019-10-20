using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Page_Role
    {
        public int Page_Role_ID { get; set; }
        public Nullable<int> Page_ID { get; set; }
        public Nullable<int> Role_ID { get; set; }
        public Nullable<bool> Modify { get; set; }
        public Nullable<bool> View { get; set; }
        public Nullable<bool> Close { get; set; }
        public string Record_Status { get; set; }
        public virtual Page Page { get; set; }
        public virtual Role Role { get; set; }
    }
}
