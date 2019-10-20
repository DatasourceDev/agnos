using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Role
    {
        public Role()
        {
            this.Page_Role = new List<Page_Role>();
            this.Template_Group = new List<Template_Group>();
            this.User_Profile = new List<User_Profile>();
        }

        public int Role_ID { get; set; }
        public string Role_Name { get; set; }
        public virtual ICollection<Page_Role> Page_Role { get; set; }
        public virtual ICollection<Template_Group> Template_Group { get; set; }
        public virtual ICollection<User_Profile> User_Profile { get; set; }
    }
}
