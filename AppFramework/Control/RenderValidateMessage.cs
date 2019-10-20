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
    public class RenderValidateMessage : AppControl
    {
       public RenderValidateMessage()
        {
            this.Control_Type = AppControlType.ValidateMessage;
        }
    }
}
