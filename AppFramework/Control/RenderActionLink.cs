using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace AppFramework.Control
{
    public enum ActionTarget
    {
        _blank,
        _self,
        _parent,
        _top
    }

    public class RenderActionLink : AppControl
    {
        public RenderActionLink()
            : base("a")
        {
            this.Control_Type = AppControlType.ActionLink;
        }

        public AppColor Color
        {
            set
            {
                if (value == AppColor.link)
                    this.Class = "color-blue";
                else
                {
                    var color = value.ToString();
                    if (value == AppColor.none)
                        color = "default";
                    this.Class = "btn btn-" + color;
                }
            }
        }

        public ActionTarget Target
        {
            set { this.Attributes["target"] = value.ToString(); }
        }

        public string Href
        {
            get { return this.Attributes["href"]; }
            set { this.Attributes["href"] = value; }
        }

        public bool IsModal
        {
            set
            {
                if (value)
                {
                    this.Attributes["role"] = "button";
                    this.Attributes["data-toggle"] = "modal";
                }

            }
        }

        public string ForModal
        {
            set
            {
                if (!value.Contains("#"))
                    value = "#" + value;
                this.Attributes["href"] = value;
            }
        }

    }
}
