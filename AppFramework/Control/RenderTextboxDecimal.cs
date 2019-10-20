using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace AppFramework.Control
{
    public class RenderTextboxDecimal : AppControl
    {
        public RenderTextboxDecimal()
            : base("input")
        {
            this.Attributes.Add("type", "text");
            this.Attributes.Add("class", "form-control text-right");

            this.Control_Type = AppControlType.TextDecimal;
        }
        

    }
}
