using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using AppFramework.Util;
using System.Text.RegularExpressions;

namespace AppFramework.Control
{
  

   public enum AppColor
   {
      primary,
      success,
      info,
      warning,
      danger,
      red,
      cyan,
      green,
      orange,
      amethyst,
      greensea,
      hotpink,
      drank,
      dutch,
      blue,
      redbrown,
      slategray,
      link,
      none
   }

   public enum AppControlType
   {
      TextBox,
      TextArea,
      TextDate,
      TextDecimal,
      Button,
      Label,
      TextPassword,
      ActionLink,
      RadioButton,
      CheckBox,
      FileUpload,
      ComboBox,
      GridData,
      TextTime,
      ValidateMessage,
      none
   }

   public abstract class AppControl
   {
      private AttributeCollection attributes;
      public AttributeCollection Attributes
      {
         get
         {
            if (attributes == null)
            {
               System.Web.UI.StateBag bag = new System.Web.UI.StateBag(true);
               attributes = new System.Web.UI.AttributeCollection(bag);
            }

            return attributes;
         }
      }

      private string _tagname;
      public string TagName
      {
         get { return _tagname; }
         set { _tagname = value; }
      }

      private AppControlType _controltype;
      public AppControlType Control_Type
      {
         get { return _controltype; }
         set { _controltype = value; }
      }

      private string _icon;
      public string Icon
      {
         get { return _icon; }
         set
         {
            _icon = value;
         }
      }

      private int _colSpan = 12;
      public int ColSpan
      {
         get { return _colSpan; }
         set
         {
            if (value > 12)
               value = 12;
            _colSpan = value;
         }
      }

      private MvcHtmlString _htmlvalidateString;
      public MvcHtmlString HtmlValidateString
      {
         get { return _htmlvalidateString; }
         set { _htmlvalidateString = value; }
      }

      public string ID
      {
         get { 
            return this.Attributes["id"]; 
         }
         set {
            if (!string.IsNullOrEmpty(value))
            {
               value = value.Replace("[", "_");
               value = value.Replace("]", "_");
               value = value.Replace(".", "_");
            }
            this.Attributes["id"] = value;
         }
      }

      public string Name
      {
         get { return this.Attributes["name"]; }
         set
         {
            this.Attributes["name"] = value;
            if (this.Attributes["placeholder"] != null)
               this.Attributes["placeholder"] = value.Replace("_", " ");
         }
      }
      private bool _disabled = false;
      public bool Disabled
      {
         get { return _disabled; }
         set
         {
            if (value == true)
            {
               this.Attributes["disabled"] = "disabled";
               _disabled = true;
            }
            _disabled = value;
         }
      }
     
      private bool _readonly = false;
      public bool Readonly
      {
         get { return _readonly; }
         set
         {
            if (value == true)
            {
               this.Attributes["readonly"] = value.ToString();
               _readonly = true;
            }
            _readonly = value;
         }
      }

      private object _selected;
      public object Selected
      {
         get { return _selected; }
         set { _selected = value; }
      }

      private object _value;
      public object Value
      {
         get { return this.Attributes["value"]; }
         set
         {
            if (value != null)
            {
               this.Attributes["value"] = value.ToString();
               _value = value.ToString();
            }
            else
            {
               this.Attributes["value"] = "";
               _value = "";
            }
         }
      }

      public string Text
      {
         get
         {
            if (string.IsNullOrEmpty(this.Attributes["Text"]))
               this.Attributes["Text"] = "";
            return this.Attributes["Text"];
         }
         set { this.Attributes["Text"] = value; }
      }

      public string Style
      {
         get { return this.Attributes["style"]; }
         set { this.Attributes["style"] = value; }
      }

      public string Rows
      {
         get
         {
            if (string.IsNullOrEmpty(this.Attributes["rows"]))
               this.Attributes["rows"] = "4";
            return this.Attributes["rows"];
         }
         set { this.Attributes["rows"] = value; }
      }


      public string Placeholder
      {
         get { return this.Attributes["placeholder"]; }
         set
         {
            this.Attributes["placeholder"] = value.Replace("_", " ");
         }
      }

      public string Class
      {
         get
         {
            return this.Attributes["class"];
         }
         set
         {
            if (this.Attributes["class"] != null)
            {
               this.Attributes["class"] += " " + value;
            }
            else
            {
               this.Attributes["class"] = value;
            }
         }
      }

      private List<GridHeader> _gridcolumnHeader;
      public List<GridHeader> GridColumnHeader
      {
         get { return _gridcolumnHeader; }
         set { _gridcolumnHeader = value; }
      }
      private List<GridRow> _gridrows;
      public List<GridRow> GridRows
      {
         get { return _gridrows; }
         set { _gridrows = value; }
      }
      private MvcHtmlString _gridAddButton;
      public MvcHtmlString GridAddButton
      {
         get { return _gridAddButton; }
         set { _gridAddButton = value; }
      }


      public int Size
      {
         get { return System.Convert.ToInt32(this.Attributes["size"]); }
         set { this.Attributes["size"] = value.ToString(); }
      }

      public int MaxLength
      {
         get { return System.Convert.ToInt32(this.Attributes["maxlength"]); }
         set { this.Attributes["maxlength"] = value.ToString(); }
      }


      private List<ComboRow> _options;
      public List<ComboRow> Options
      {
         get
         {
            if (_options == null)
               _options = new List<ComboRow>();
            return _options;
         }
         set { _options = value; }
      }


      protected AppControl()
      {

      }

      protected AppControl(string tag)
      {
         this.TagName = tag;
      }


      public override string ToString()
      {
         StringBuilder builder = new StringBuilder();
         using (StringWriter writer = new StringWriter(builder))
         {
            using (HtmlTextWriter html = new HtmlTextWriter(writer))
            {
               if (_controltype != AppControlType.Button && _controltype != AppControlType.ActionLink)
               {
                  html.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                  html.RenderBeginTag(HtmlTextWriterTag.Div);//Tag Row
                  html.AddAttribute(HtmlTextWriterAttribute.Class, "col-md-" + _colSpan);
                  html.RenderBeginTag(HtmlTextWriterTag.Div);//Tag Colspan 
                  html.AddAttribute(HtmlTextWriterAttribute.Class, "form-group");
                  html.RenderBeginTag(HtmlTextWriterTag.Div);//Tag Form Group 

                  if (_controltype == AppControlType.CheckBox)
                  {
                     html.AddAttribute(HtmlTextWriterAttribute.Class, "checkbox");
                     html.RenderBeginTag(HtmlTextWriterTag.Div);
                  }

                  if (_controltype == AppControlType.RadioButton)
                  {
                     html.AddAttribute(HtmlTextWriterAttribute.Class, "radio");
                     html.RenderBeginTag(HtmlTextWriterTag.Div);
                  }

                  if (_controltype == AppControlType.FileUpload)
                  {
                     html.AddAttribute(HtmlTextWriterAttribute.Class, "input-group");
                     html.RenderBeginTag(HtmlTextWriterTag.Div);//Tag Fileupload 1
                     html.AddAttribute(HtmlTextWriterAttribute.Class, "input-group-btn");
                     html.RenderBeginTag(HtmlTextWriterTag.Span);//Tag Fileupload 2
                     html.AddAttribute(HtmlTextWriterAttribute.Class, "btn btn-primary btn-file");
                     html.RenderBeginTag(HtmlTextWriterTag.Span);//Tag Fileupload 3
                     html.AddAttribute(HtmlTextWriterAttribute.Class, "fa fa-upload");
                     html.RenderBeginTag(HtmlTextWriterTag.I);//Tag Fileupload 4
                     html.RenderEndTag();//End Tag Fileupload 4
                  }

                  if (_controltype == AppControlType.GridData)
                  {
                     html.AddAttribute(HtmlTextWriterAttribute.Class, "table-responsive");
                     html.RenderBeginTag(HtmlTextWriterTag.Div);//Tag Grid Data
                  }

                  if (_controltype == AppControlType.TextDate)
                  {
                     if (!_readonly)
                     {
                        html.RenderBeginTag(HtmlTextWriterTag.Script);
                        html.Write("$(function () { InitDatepickerByID('" + ID + "'); })");
                        html.RenderEndTag();
                     }
                  }
                  if (_controltype == AppControlType.TextTime)
                  {
                     if (!_readonly)
                     {
                        html.RenderBeginTag(HtmlTextWriterTag.Script);
                        html.Write("$(function () {  $('#" + ID + "').datetimepicker( {pickDate: false , format: 'HH:mm', pickSeconds: false, pick12HourFormat: false}) })");
                        html.RenderEndTag();
                     }
                  }
               }
               if (_controltype != AppControlType.ValidateMessage)
               {
                    this.Attributes.AddAttributes(html);
                     html.RenderBeginTag(_tagname);//Tag Control
                     this.RenderContents(html);
               }

             

               if (_controltype == AppControlType.ComboBox)
               {
                  if (_options != null && _options.Count > 0)
                  {
                     var i = 0;
                     foreach (var op in _options)
                     {
                        if (_selected != null)
                        {
                           if (op.Value == _selected.ToString())
                              html.AddAttribute(HtmlTextWriterAttribute.Selected, "selected");
                        }
                        else
                        {
                           if (i == 0)
                              html.AddAttribute(HtmlTextWriterAttribute.Selected, "selected");
                        }

                        html.AddAttribute(HtmlTextWriterAttribute.Value, op.Value.Trim());
                        html.RenderBeginTag(HtmlTextWriterTag.Option);//Tag Option
                        html.WriteEncodedText(op.Text.Trim());
                        html.RenderEndTag();//End Option
                        
                        i++;
                     }
                  }

               }
               else if (_controltype == AppControlType.GridData)
               {
                  if (_gridcolumnHeader != null)
                  {
                     html.RenderBeginTag(HtmlTextWriterTag.Thead);//Tag thead
                     html.RenderBeginTag(HtmlTextWriterTag.Tr);//Tag Tr
                     foreach (var col in _gridcolumnHeader)
                     {
                        if (col.Data_Type == GridDataType.none | col.Data_Type == GridDataType.usercontrol)
                        {
                           html.AddAttribute(HtmlTextWriterAttribute.Class, "no-sort");
                           if (!string.IsNullOrEmpty(col.Width))
                              html.AddStyleAttribute(HtmlTextWriterStyle.Width, col.Width);
                           else
                              html.AddStyleAttribute(HtmlTextWriterStyle.Width, "150px");
                           html.RenderBeginTag(HtmlTextWriterTag.Th);//Tag Th
                        }
                        else if (col.Data_Type == GridDataType.index)
                        {
                           html.AddAttribute(HtmlTextWriterAttribute.Class, "sort-numeric");
                           if (!string.IsNullOrEmpty(col.Width))
                              html.AddStyleAttribute(HtmlTextWriterStyle.Width, col.Width);
                           else
                              html.AddStyleAttribute(HtmlTextWriterStyle.Width, "70px");
                           html.RenderBeginTag(HtmlTextWriterTag.Th);//Tag Th
                        }
                        else
                        {
                           html.AddAttribute(HtmlTextWriterAttribute.Class, "sort-" + col.Data_Type.ToString());
                           if (!string.IsNullOrEmpty(col.Width))
                              html.AddStyleAttribute(HtmlTextWriterStyle.Width, col.Width);
                           html.RenderBeginTag(HtmlTextWriterTag.Th);//Tag Th
                        }
                        html.WriteEncodedText(col.Column_Name.Replace("_", " "));
                        html.RenderEndTag();//End Option
                     }
                     html.RenderEndTag();//End Tag Tr
                     html.RenderEndTag();//End Tag thead

                     html.RenderBeginTag(HtmlTextWriterTag.Tbody);//Tag Th;//Tag tbody
                     if (_gridrows != null)
                     {
                        foreach (var row in _gridrows)
                        {
                           html.RenderBeginTag(HtmlTextWriterTag.Tr);//Tag Tr
                           for (var i = 0; i < row.Item.Count; i++)
                           {
                              var item = row.Item[i];
                              if (_gridcolumnHeader[i].Data_Type == GridDataType.amount)
                              {
                                 html.AddAttribute(HtmlTextWriterAttribute.Class, "text-right");
                                 html.RenderBeginTag(HtmlTextWriterTag.Td);//Tag Td
                                 if (item.Value != null)
                                 {
                                    var amount = NumUtil.ParseDecimal(item.Value.ToString());
                                    html.WriteEncodedText(NumUtil.FormatCurrency(amount));
                                 }
                                 else
                                    html.WriteEncodedText("0");

                                 html.RenderEndTag();//End Tag Td
                              }
                              else if (_gridcolumnHeader[i].Data_Type == GridDataType.index)
                              {
                                 html.AddAttribute(HtmlTextWriterAttribute.Class, "text-center");
                                 html.RenderBeginTag(HtmlTextWriterTag.Td);//Tag Td
                                 if (item.Value != null)
                                 {
                                    var amount = NumUtil.ParseInteger(item.Value.ToString());
                                    html.WriteEncodedText(amount.ToString("n0"));
                                 }
                                 html.RenderEndTag();//End Tag Td
                              }
                              else if (_gridcolumnHeader[i].Data_Type == GridDataType.numeric)
                              {
                                 html.AddAttribute(HtmlTextWriterAttribute.Class, "text-right");
                                 html.RenderBeginTag(HtmlTextWriterTag.Td);//Tag Td
                                 if (item.Value != null)
                                 {
                                    var amount = NumUtil.ParseInteger(item.Value.ToString());
                                    html.WriteEncodedText(amount.ToString("n0"));
                                 }
                                 html.RenderEndTag();//End Tag Td
                              }
                              else if (_gridcolumnHeader[i].Data_Type == GridDataType.usercontrol)
                              {
                                 html.RenderBeginTag(HtmlTextWriterTag.Td);//Tag Td
                                 if (item.Value != null)
                                    html.Write(item.Value.ToString());
                                 html.RenderEndTag();//End Tag Td
                              }
                              else
                              {
                                 html.RenderBeginTag(HtmlTextWriterTag.Td);//Tag Td
                                 if (item.Value != null)
                                    html.WriteEncodedText(item.Value.ToString());
                                 html.RenderEndTag();//End Tag Td
                              }

                           }
                           html.RenderEndTag();//End Tag Tr
                        }
                     }

                     html.RenderEndTag();//End Tag tbody

                  }

               }
               if (_controltype == AppControlType.Button || _controltype == AppControlType.ActionLink || _controltype == AppControlType.Label)
               {
                  if (!string.IsNullOrEmpty(_icon))
                  {
                     html.AddAttribute(HtmlTextWriterAttribute.Class, _icon);
                     html.RenderBeginTag(HtmlTextWriterTag.I);//Tag i
                     html.RenderEndTag();//End Tag i
                  }
                  if (Text != null)
                     html.WriteEncodedText(Text);
               }



               if (_controltype == AppControlType.TextArea)
                  if (Value != null)
                     html.WriteEncodedText(Value.ToString());

               if (_controltype != AppControlType.ValidateMessage)
               {
                  html.RenderEndTag();//End Tag Control
               }

              

               if (_controltype == AppControlType.RadioButton | _controltype == AppControlType.CheckBox)
               {
                  html.AddAttribute(HtmlTextWriterAttribute.For, ID);
                  html.RenderBeginTag(HtmlTextWriterTag.Label);//Tag Radio or Check box label
                  html.WriteEncodedText(Text);
                  html.RenderEndTag();//End Tag Radio or Check box label
               }


               if (this.HtmlValidateString != null)
               {
                  html.AddAttribute(HtmlTextWriterAttribute.Class, "validation-error");
                  html.RenderBeginTag(HtmlTextWriterTag.Div);//Tag validation-error
                  html.Write(_htmlvalidateString.ToHtmlString());
                  html.RenderEndTag();//End Tag validation-error
               }

               if (_controltype != AppControlType.Button && _controltype != AppControlType.ActionLink)
               {
                  if (_controltype == AppControlType.GridData)
                  {
                     html.RenderEndTag();//Tag Grid data

                     html.RenderBeginTag(HtmlTextWriterTag.Script);//Tag script
                     var addbutton = "null";
                     if (_gridAddButton != null)
                        addbutton = _gridAddButton.ToString();
                     if (!string.IsNullOrEmpty(addbutton) && addbutton != "null")
                        html.Write("$(function () { InitDatatable('" + ID + "', '" + addbutton + "', null); })");
                     else
                        html.Write("$(function () { InitDatatable('" + ID + "', null, null); })");
                     html.RenderEndTag(); //End script;
                  }

                  if (_controltype == AppControlType.FileUpload)
                  {
                     html.RenderEndTag();//Tag Fileupload 1
                     html.RenderEndTag();//Tag Fileupload 2
                     html.RenderBeginTag("input class='form-control'  type='text'  readonly=''");//Tag Fileupload Text
                     html.RenderEndTag();//End Tag Fileupload Text
                     html.RenderEndTag();//Tag Fileupload 3
                  }

                  if (_controltype == AppControlType.RadioButton)
                     html.RenderEndTag();//End Tag Radio

                  if (_controltype == AppControlType.CheckBox)
                     html.RenderEndTag();//End Tag Check box



                  html.RenderEndTag();//End Tag Form Group 
                  html.RenderEndTag();//End Tag Colspan 
                  html.RenderEndTag();//End Row 
               }


            }
         }
         var str = builder.ToString();
         if (_controltype == AppControlType.ComboBox)
         {
            Regex RegexBetweenTags = new Regex(@">(?! )\s+", RegexOptions.Compiled);
            Regex RegexLineBreaks = new Regex(@"([\n\s])+?(?<= {2,})<", RegexOptions.Compiled);
            str = RegexBetweenTags.Replace(str, ">");
            str = RegexLineBreaks.Replace(str, "<");
            str = Regex.Replace(str, @"\t|\n|\r", "");
            str = str.Trim();
         }
         return str;
      }

      public virtual void RenderContents(System.Web.UI.HtmlTextWriter writer)
      {

      }

   }
}
