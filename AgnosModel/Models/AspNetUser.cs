using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            this.AspNetUserClaims = new List<AspNetUserClaim>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Discriminator { get; set; }
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
    }
}
