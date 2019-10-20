using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Page
    {
        public Page()
        {
            this.Page_Role = new List<Page_Role>();
        }

        public int Page_ID { get; set; }
        public string Page_Code { get; set; }
        public string Page_Name { get; set; }
        public virtual ICollection<Page_Role> Page_Role { get; set; }
    }
}
