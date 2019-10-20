using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace AppFramework.Control
{
    public class RenderMultiControl
    {
        private MvcHtmlString[] _controls;
        public MvcHtmlString[] Controls
        {
            get
            {
                return _controls;
            }
            set
            {
                _controls = value;
            }
        }

        private int _colSpan = 12;
        public int ColSpan
        {
            get
            {
                return _colSpan;
            }
            set
            {
                if (value > 12)
                    value = 12;

                _colSpan = value;
            }
        }

        public override string ToString()
        {
            
            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter(builder))
            {
                using (HtmlTextWriter html = new HtmlTextWriter(writer))
                {
                    html.RenderBeginTag("div class='row'");//Tag Row
                    foreach (MvcHtmlString control in _controls)
                    {
                        html.RenderBeginTag("div class='col-md-" + _colSpan + "'");//Tag Colspan 
                        html.RenderBeginTag("div class='form-group'");//Tag Form Group 
                        html.Write(control.ToString());
                        //if (control.Control_Type != AppControlType.none)
                        //{
                        //    if (control.Control_Type == AppControlType.CheckBox)
                        //        html.RenderBeginTag("div class='checkbox'");//Tag Check box

                        //    if (control.Control_Type == AppControlType.RadioButton)
                        //        html.RenderBeginTag("div class='radio'"); //Tag Radio

                        //    if (control.Control_Type == AppControlType.FileUpload)
                        //    {
                        //        html.RenderBeginTag("div class='input-group'");//Tag Fileupload 1
                        //        html.RenderBeginTag("span class='input-group-btn'");//Tag Fileupload 2
                        //        html.RenderBeginTag("span class='btn btn-primary btn-file'");//Tag Fileupload 3
                        //        html.RenderBeginTag("i class='fa fa-upload'");//Tag Fileupload 4
                        //        html.RenderEndTag();//End Tag Fileupload 4

                        //    }

                        //    control.Attributes.AddAttributes(html);
                        //    html.RenderBeginTag(control.TagName);//Tag Control
                        //    control.RenderContents(html);
                        //    if (control.Control_Type == AppControlType.ComboBox)
                        //    {
                        //        if (control.Options != null)
                        //        {
                        //            foreach (var op in control.Options)
                        //            {
                        //                html.RenderBeginTag("option value='" + op.Value + "'");//Tag Option
                        //                html.WriteEncodedText(op.Text);
                        //                html.RenderEndTag();//End Option
                        //            }
                        //        }

                        //    }

                        //    if (control.Control_Type == AppControlType.Button || control.Control_Type == AppControlType.ActionLink || control.Control_Type == AppControlType.Label)
                        //        if (control.Text != null)
                        //            html.WriteEncodedText(control.Text);

                        //    if (control.Control_Type == AppControlType.TextArea)
                        //        if (control.Value != null)
                        //            html.WriteEncodedText(control.Value.ToString());

                        //    html.RenderEndTag();//End Tag Control

                        //    if (control.Control_Type == AppControlType.RadioButton | control.Control_Type == AppControlType.CheckBox)
                        //    {
                        //        html.RenderBeginTag("label for='" + control.ID + "'");//Tag Radio or Check box label
                        //        html.WriteEncodedText(control.Text);
                        //        html.RenderEndTag();//End Tag Radio or Check box label
                        //    }


                        //    if (control.HtmlValidateString != null)
                        //    {
                        //        html.RenderBeginTag("div class='validation-error'");//Tag validation-error
                        //        html.Write(control.HtmlValidateString.ToHtmlString());
                        //        html.RenderEndTag();//End Tag validation-error
                        //    }

                        //    if (control.Control_Type != AppControlType.Button && control.Control_Type != AppControlType.ActionLink)
                        //    {
                        //        if (control.Control_Type == AppControlType.FileUpload)
                        //        {

                        //            html.RenderEndTag();//Tag Fileupload 1
                        //            html.RenderEndTag();//Tag Fileupload 2
                        //            html.RenderBeginTag("input class='form-control'  type='text'  readonly=''");//Tag Fileupload Text
                        //            html.RenderEndTag();//End Tag Fileupload Text
                        //            html.RenderEndTag();//Tag Fileupload 3
                        //        }

                        //        if (control.Control_Type == AppControlType.RadioButton)
                        //            html.RenderEndTag();//End Tag Radio

                        //        if (control.Control_Type == AppControlType.CheckBox)
                        //            html.RenderEndTag();//End Tag Check box


                        //    }

                        //}


                        html.RenderEndTag();//End Tag Form Group 
                        html.RenderEndTag();//End Tag Colspan 

                    }

                    html.RenderEndTag();//End Tag Row

                }
            }

            return builder.ToString();
        }


    }
} 
