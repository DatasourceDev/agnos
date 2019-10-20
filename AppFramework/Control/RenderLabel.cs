using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace AppFramework.Control
{

    
    public class RenderLabel : AppControl
    {
        public RenderLabel()
            : base("label")
        {
            this.Attributes.Add("type", "text");
            this.Attributes.Add("class", "control-label");

            this.Control_Type = AppControlType.Label;
        }

        public string For
        {
            get { return this.Attributes["for"]; }
            set { this.Attributes["for"] = value; }
        }
        

    }
}
