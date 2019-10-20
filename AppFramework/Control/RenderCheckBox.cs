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

    public class RenderCheckBox : AppControl
    {
        public RenderCheckBox()
            : base("input")
        {
            this.Attributes.Add("type", "checkbox");
            this.Control_Type = AppControlType.CheckBox;
        }

        public string Checked
        {
            get { return this.Attributes["checked"]; }
            set { this.Attributes["checked"] = value; }
        }
      

    }
}
