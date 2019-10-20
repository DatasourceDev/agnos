using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace AppFramework.Control
{
    public class ComboRow
    {
        public String Value { get; set; }
        public String Text { get; set; }
        public String Desc { get; set; }
        public String HidValue { get; set; }
    }
    
    public class RenderComboBox : AppControl
    {
        public RenderComboBox()
            : base("select")
        {
            this.Attributes.Add("class", "chosen-select form-control");
            this.Control_Type = AppControlType.ComboBox;
        }

      

    }
}
