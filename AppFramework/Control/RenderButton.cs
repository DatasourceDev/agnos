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

    public enum AppButtonType
    {
        submit,
        button,
        reset,
        none
    }

    public class RenderButton : AppControl
    {
        public RenderButton()
            : base("button")
        {
            //this.Attributes.Add("type", "text");
            this.Attributes.Add("class", "btn");
            this.Control_Type = AppControlType.Button;
        }

        public AppColor Color
        {
            set
            {
                var color = value.ToString();
                if (value == AppColor.none)
                    color = "default";
                this.Class = "btn-" + color;
            }

        }

        public AppButtonType Type
        {
            set
            {
                var type = value.ToString();
                this.Attributes["type"] = type;
            }
        }



    }
}
