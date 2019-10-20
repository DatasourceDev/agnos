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
    public class RenderTextbox : AppControl
    {
        public RenderTextbox()
            : base("input")
        {
            this.Attributes.Add("type", "text");
            this.Attributes.Add("class", "form-control");

            this.Control_Type = AppControlType.TextBox;
        }

       
        

    }
}
