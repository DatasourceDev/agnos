﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace AppFramework.Control
{

    //Require Proproty
    //Name
    //HtmlValidateString

    public class RenderTextTime : AppControl
    {
        public RenderTextTime()
            : base("input")
        {
            this.Attributes.Add("type", "text");
            this.Attributes.Add("class", "form-control input-timepicker");

            this.Control_Type = AppControlType.TextTime;
        }





    }
}
