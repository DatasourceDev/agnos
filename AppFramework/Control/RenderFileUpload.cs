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
    //Name
    //ID

   

    public class RenderFileUpload : AppControl
    {
        public RenderFileUpload()
            : base("input")
        {
            this.Attributes.Add("type", "file");
            this.Control_Type = AppControlType.FileUpload;
        }

      



    }
}
