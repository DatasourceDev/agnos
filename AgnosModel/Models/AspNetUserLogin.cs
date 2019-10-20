using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class AspNetUserLogin
    {
        public string UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}
