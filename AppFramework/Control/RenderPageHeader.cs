using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace AppFramework.Control
{
    public class RenderPageHeader 
    {

        private string _text ;
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
                    html.RenderBeginTag("div class='row'");
                    html.RenderBeginTag("div class='col-md-12'");
                    html.RenderBeginTag("section class='tile'");
                    html.RenderBeginTag("div class='tile-header'");
                    html.RenderBeginTag("div class='pageheader white'");

                    html.RenderBeginTag("h2");
                    html.WriteEncodedText(_text);
                    html.RenderEndTag();

                    html.RenderBeginTag("div class='breadcrumbs'");
                    html.RenderBeginTag("ol class='breadcrumb'");
                    html.RenderBeginTag("li");
                    html.WriteEncodedText("You are here");
                    html.RenderEndTag();
                    html.RenderBeginTag("li class='active'");
                    html.WriteEncodedText(_text);
                    html.RenderEndTag();

                    html.RenderEndTag();
                    html.RenderEndTag();

                    html.RenderEndTag();
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
