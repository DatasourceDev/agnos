using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace AppFramework.Control
{
    //Require Proproty
    //Text
    //Value
    //ID
    //Name

    public class RenderRadioButton : AppControl
    {
        public RenderRadioButton()
            : base("input")
        {
            this.Attributes.Add("type", "radio");
            this.Control_Type = AppControlType.RadioButton;
        }

        public string Checked
        {
            get { return this.Attributes["checked"]; }
            set { this.Attributes["checked"] = value; }
        }
      

    }
}
