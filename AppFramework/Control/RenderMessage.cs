using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace AppFramework.Control
{
    public class RenderMessage
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private ReturnCode _returnCode = ReturnCode.SUCCESS;
        public ReturnCode Return_Code
        {
            get { return _returnCode; }
            set { _returnCode = value; }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter(builder))
            {
                using (HtmlTextWriter html = new HtmlTextWriter(writer))
                {
                    if (!string.IsNullOrEmpty(_message) && _message.Trim().Length > 0)
                    {
                        if (_returnCode == ReturnCode.SUCCESS)
                            html.RenderBeginTag("div class='alert alert-green'");
                        else
                            html.RenderBeginTag("div class='alert alert-red'");
                       
                        html.WriteEncodedText(_message);
                        this.RenderContents(html);
                        html.RenderEndTag();
                    }
                }
            }

            return builder.ToString();
        }

        public virtual void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {

        }
    }
}
