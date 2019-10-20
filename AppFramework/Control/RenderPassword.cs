using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace AppFramework.Control
{


    public class RenderPassword : AppControl
    {
        public RenderPassword()
            : base("input")
        {
            this.Attributes.Add("type", "password");
            this.Attributes.Add("class", "form-control");
            this.Attributes.Add("autocomplete", "off");

            this.Control_Type = AppControlType.TextPassword;
        }
        
    }
}
