using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace AppFramework.Control
{
    public class RenderTextArea : AppControl
    {
        public RenderTextArea()
            : base("textarea")
        {
            this.Attributes.Add("rows", "6");
            this.Attributes.Add("class", "form-control");
            this.Control_Type = AppControlType.TextArea;
        }
       

        

    }
}
