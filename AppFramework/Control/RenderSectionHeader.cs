using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace AppFramework.Control
{
    public class RenderSectionHeader
    {

        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }


        public override string ToString()
        {

            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter(builder))
            {
                using (HtmlTextWriter html = new HtmlTextWriter(writer))
                {
                    html.RenderBeginTag("div class='tile-header'");
                    html.RenderBeginTag("h1");
                    html.WriteEncodedText(_text);
                    html.RenderEndTag();
                    html.RenderBeginTag("div class='controls'");
                    html.RenderBeginTag("a href='#'  class='minimize'");
                    html.RenderBeginTag("i class='fa fa-chevron-down color-blue'");
                    html.RenderEndTag();
                    html.RenderEndTag();
                    html.RenderEndTag();
                    html.RenderEndTag();
                }
            }

            return builder.ToString();
        }

        public virtual void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {

        }
    }
}
