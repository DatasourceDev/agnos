using AppFramework.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AppFramework.Control
{
    public static class AppInput
    {
        #region Tag      
        public static MvcHtmlString AppBlank(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create("");
        }
        public static MvcHtmlString AppBlank(this HtmlHelper htmlHelper, string id)
        {
            return MvcHtmlString.Create("<div id=" + id+ "></div>");
        }
        public static MvcHtmlString AppGridData(this HtmlHelper htmlHelper, string id, List<GridHeader> columns, List<GridRow> rows = null, MvcHtmlString addButton = null, object htmlAttributes = null)
        {
            var control = new RenderGridData();
            control.GridColumnHeader = columns;
            control.GridRows = rows;
            control.ID = id;
            if (addButton != null)
                control.GridAddButton = addButton;


            HtmlAttributes(control, htmlAttributes);
            return MvcHtmlString.Create(control.ToString());
        }
        public static MvcHtmlString AppMultiButton(this HtmlHelper htmlHelper, MvcHtmlString[] buttons)
        {
            var allbtn = "";
            if (buttons != null)
            {
                var i = 0;
                foreach (var btn in buttons)
                {
                    allbtn += btn.ToString();
                    if (i < buttons.Length)
                        allbtn += "<span style='margin-left: 3px'></span>";
                    i++;
                }
            }
            return MvcHtmlString.Create(allbtn);
        }
        public static MvcHtmlString AppMultiControl(this HtmlHelper htmlHelper, MvcHtmlString[] controls, int colspan = 12)
        {
            var control = new RenderMultiControl();
            control.Controls = controls;
            control.ColSpan = colspan;
            return MvcHtmlString.Create(control.ToString());
        }
        public static MvcHtmlString AppSectionHeader(this HtmlHelper htmlHelper, string text)
        {
            var control = new RenderSectionHeader();
            control.Text = text;

            return MvcHtmlString.Create(control.ToString());
        }
        public static MvcHtmlString AppPageHeader(this HtmlHelper htmlHelper, string text)
        {
            var control = new RenderPageHeader();
            control.Text = text;

            return MvcHtmlString.Create(control.ToString());
        }
        public static MvcHtmlString AppButtonModal(this HtmlHelper htmlHelper, string text, string modalname, AppColor color = AppColor.none, object htmlAttributes = null)
        {
            var control = new RenderActionLink();
            control.Text = text;
            control.Color = color;
            control.IsModal = true;
            control.ForModal = "#" + modalname;
            control.Href = "#" + modalname;
            control.ID = "btn-" + modalname;

            HtmlAttributes(control, htmlAttributes);
            return MvcHtmlString.Create(control.ToString());
        }
        public static MvcHtmlString AppActionLink(this HtmlHelper htmlHelper, string text, string href, AppColor color = AppColor.none, object htmlAttributes = null)
        {
            var control = new RenderActionLink();
            control.Text = text;
            control.Name = text;
            control.Color = color;
            control.Href = href;
            control.ID = "a" + text;

            HtmlAttributes(control, htmlAttributes);
            return MvcHtmlString.Create(control.ToString());
        }
        public static MvcHtmlString AppGridAddButton(this HtmlHelper htmlHelper, string text, string href, AppColor color = AppColor.none, object htmlAttributes = null)
        {

            var control = new RenderActionLink();
            control.Text = text;
            control.Name = text;
            control.Color = color;
            control.Href = href;
            control.ID = "a" + text;
            control.Class = "btn-sm add-row";

            HtmlAttributes(control, htmlAttributes);
            return MvcHtmlString.Create(control.ToString());
        }
        public static MvcHtmlString AppGridAddModalButton(this HtmlHelper htmlHelper, string text, string modalname, AppColor color = AppColor.none, object htmlAttributes = null)
        {

            var control = new RenderActionLink();
            control.Text = text;
            control.Name = text;
            control.Color = color;
            control.Href = "#" + modalname;
            control.ID = "a" + text;
            control.Class = "btn-sm add-row";
            control.IsModal = true;
            control.ForModal = modalname;
            control.ID = "btn-" + modalname;
            HtmlAttributes(control, htmlAttributes);
            return MvcHtmlString.Create(control.ToString());
        }
        public static MvcHtmlString AppButton(this HtmlHelper htmlHelper, string text, AppButtonType type, AppColor color = AppColor.none, object htmlAttributes = null)
        {
            var control = new RenderButton();
            control.Text = text;
            control.Type = type;
            control.Color = color;
            HtmlAttributes(control, htmlAttributes);
            return MvcHtmlString.Create(control.ToString());
        }
        public static MvcHtmlString AppMessage(this HtmlHelper htmlHelper, ReturnCode code, string msg, object htmlAttributes = null)
        {
            var control = new RenderMessage();
            control.Message = msg;
            control.Return_Code = code;

            return MvcHtmlString.Create(control.ToString());
        }
        public static MvcHtmlString AppMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Object value = metadata.Model;

            var control = new RenderMessage();

            if (value != null)
                control.Message = value.ToString();

            return MvcHtmlString.Create(control.ToString());
        }
        public static MvcHtmlString AppLabel(this HtmlHelper htmlHelper, string text, object htmlAttributes = null)
        {
            var control = new RenderLabel();
            control.Text = text;

            HtmlAttributes(control, htmlAttributes);

            return MvcHtmlString.Create(control.ToString());

        }
        public static MvcHtmlString AppLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var control = new RenderLabel();
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            control.For = metadata.PropertyName;

            if (!string.IsNullOrEmpty(metadata.DisplayName))
                control.Text = metadata.DisplayName;
            else
                control.Text = metadata.PropertyName.Replace("_", " ");

            HtmlAttributes(control, htmlAttributes);

            return MvcHtmlString.Create(control.ToString());

        }
        #endregion

        #region Input

        #region Dropdown list

        public static MvcHtmlString AppDropDownList(this HtmlHelper htmlHelper, string name)
        {
            return AppDropDownList(htmlHelper, name, null, null, null);
        }
        public static MvcHtmlString AppDropDownList(this HtmlHelper htmlHelper, string name, List<ComboRow> options)
        {
            return AppDropDownList(htmlHelper, name, options, null, null);
        }
        public static MvcHtmlString AppDropDownList(this HtmlHelper htmlHelper, string name, List<ComboRow> options, object value)
        {
            return AppDropDownList(htmlHelper, name, options, value, null);
        }

        public static MvcHtmlString AppDropDownList(this HtmlHelper htmlHelper, string name, List<ComboRow> options, object value, object htmlAttributes)
        {
            var control = new RenderComboBox();
            control.Options = options;

            control.Name = name;
            control.ID = name;

            if (value != null)
                control.Selected = value;

            HtmlAttributes(control, htmlAttributes);

            ModelError modelError = htmlHelper.ViewData.ModelState.Where(w => w.Key == name).SelectMany(m => m.Value.Errors).FirstOrDefault();
            if (modelError != null)
                control.HtmlValidateString = MvcHtmlString.Create("<span class='validation_wrapper customValidation'><span>" + modelError.ErrorMessage + "</span></span>");

            return MvcHtmlString.Create(control.ToString());
        }

        public static MvcHtmlString AppDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, List<ComboRow> options)
        {
            return AppDropDownListFor(htmlHelper, expression, options, null);
        }
        public static MvcHtmlString AppDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, List<ComboRow> options, object htmlAttributes)
        {
            var control = new RenderComboBox();
            control.Options = options;

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            control.Name = metadata.PropertyName;
            control.ID = metadata.PropertyName;

            Object value = metadata.Model;
            if (value != null)
                control.Selected = value;

            HtmlAttributes(control, htmlAttributes);

            ModelError modelError = htmlHelper.ViewData.ModelState.Where(w => w.Key == metadata.PropertyName).SelectMany(m => m.Value.Errors).FirstOrDefault();
            if (modelError != null)
                control.HtmlValidateString = MvcHtmlString.Create("<span class='validation_wrapper customValidation'><span>" + modelError.ErrorMessage + "</span></span>");

            var result = MvcHtmlString.Create(control.ToString());
            return result;
        }

        #endregion

        #region File Upload
        public static MvcHtmlString AppFileUpload(this HtmlHelper htmlHelper, string id)
        {
            return AppFileUpload(htmlHelper, id, null);
        }
        public static MvcHtmlString AppFileUpload(this HtmlHelper htmlHelper, string id, object htmlAttributes)
        {
            var control = new RenderFileUpload();
            control.ID = id;

            HtmlAttributes(control, htmlAttributes);

            return MvcHtmlString.Create(control.ToString());
        }
        #endregion

        #region Check box
        public static MvcHtmlString AppCheckBox(this HtmlHelper htmlHelper, string name)
        {
            return AppCheckBox(htmlHelper, name, false, null);
        }
        public static MvcHtmlString AppCheckBox(this HtmlHelper htmlHelper, string name, bool isChecked)
        {
            return AppCheckBox(htmlHelper, name, isChecked, null);
        }

        public static MvcHtmlString AppCheckBox(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        {
            return AppCheckBox(htmlHelper, name, false, htmlAttributes);
        }


        public static MvcHtmlString AppCheckBox(this HtmlHelper htmlHelper, string name, bool isChecked, object htmlAttributes)
        {
            var control = new RenderCheckBox();
            control.Name = name;
            if (isChecked)
                control.Checked = isChecked.ToString();


            HtmlAttributes(control, htmlAttributes);

            ModelError modelError = htmlHelper.ViewData.ModelState.Where(w => w.Key == name).SelectMany(m => m.Value.Errors).FirstOrDefault();
            if (modelError != null)
                control.HtmlValidateString = MvcHtmlString.Create("<span class='validation_wrapper customValidation'><span>" + modelError.ErrorMessage + "</span></span>");
     
            return MvcHtmlString.Create(control.ToString());
        }

        public static MvcHtmlString AppCheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return AppCheckBoxFor(htmlHelper, expression, null);
        }

        public static MvcHtmlString AppCheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var control = new RenderCheckBox();
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            control.Name = metadata.PropertyName;
            control.ID = metadata.PropertyName;
            control.Value = true;

            Object value = metadata.Model;
            if (value != null && (bool)value == true)
                control.Checked = "true";

            if (!string.IsNullOrEmpty(metadata.DisplayName))
                control.Text = metadata.DisplayName;
            else
                control.Text = metadata.PropertyName.Replace("_", " ");

            HtmlAttributes(control, htmlAttributes);

            ModelError modelError = htmlHelper.ViewData.ModelState.Where(w => w.Key == metadata.PropertyName).SelectMany(m => m.Value.Errors).FirstOrDefault();
            if (modelError != null)
                control.HtmlValidateString = MvcHtmlString.Create("<span class='validation_wrapper customValidation'><span>" + modelError.ErrorMessage + "</span></span>");

            return MvcHtmlString.Create(control.ToString());
        }

        #endregion

        #region Radio box
        public static MvcHtmlString AppRadioButton(this HtmlHelper htmlHelper, string name, object value)
        {
            return AppRadioButton(htmlHelper, name, value, null);
        }
        public static MvcHtmlString AppRadioButton(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            var control = new RenderRadioButton();
            control.Name = name;
            if (value != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (attributes != null && attributes.ContainsKey("value"))
                {
                    var attrvalue = attributes.Where(w => w.Key == "value").FirstOrDefault();
                    if (value.ToString().ToLower() == attrvalue.Value.ToString().ToLower())
                        control.Checked = "checked";
                }
                control.Value = value;
            }

            HtmlAttributes(control, htmlAttributes);

            ModelError modelError = htmlHelper.ViewData.ModelState.Where(w => w.Key == name).SelectMany(m => m.Value.Errors).FirstOrDefault();
            if (modelError != null)
                control.HtmlValidateString = MvcHtmlString.Create("<span class='validation_wrapper customValidation'><span>" + modelError.ErrorMessage + "</span></span>");

            return MvcHtmlString.Create(control.ToString());
        }

        public static MvcHtmlString AppRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var control = new RenderRadioButton();
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Object value = metadata.Model;
            control.Name = metadata.PropertyName;
            if (value != null)
            {

                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (attributes != null && attributes.ContainsKey("value"))
                {
                    var attrvalue = attributes.Where(w => w.Key == "value").FirstOrDefault();
                    if (value.ToString().ToLower() == attrvalue.Value.ToString().ToLower())
                        control.Checked = "checked";
                }
                control.Value = value;
            }

            HtmlAttributes(control, htmlAttributes);

            ModelError modelError = htmlHelper.ViewData.ModelState.Where(w => w.Key == metadata.PropertyName).SelectMany(m => m.Value.Errors).FirstOrDefault();
            if (modelError != null)
                control.HtmlValidateString = MvcHtmlString.Create("<span class='validation_wrapper customValidation'><span>" + modelError.ErrorMessage + "</span></span>");

            return MvcHtmlString.Create(control.ToString());
        }

        #endregion

        #region Text Date
        public static MvcHtmlString AppTextDate(this HtmlHelper htmlHelper, string name)
        {
           return AppTextDate(htmlHelper, name, null, null);
        }
        public static MvcHtmlString AppTextDate(this HtmlHelper htmlHelper, string name, object value)
        {
           return AppTextDate(htmlHelper, name, value, null);
        }

        public static MvcHtmlString AppTextDate(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
           return AppText(htmlHelper, new RenderTextDate(), name, value, htmlAttributes);
        }

        public static MvcHtmlString AppTextDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return AppTextFor(htmlHelper, expression, new RenderTextDate(), 12, null);
        }
        public static MvcHtmlString AppTextDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int colspan)
        {
            return AppTextFor(htmlHelper, expression, new RenderTextDate(), colspan, null);
        }
        public static MvcHtmlString AppTextDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return AppTextFor(htmlHelper, expression, new RenderTextDate(), 12, htmlAttributes);
        }
        public static MvcHtmlString AppTextDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int colspan, object htmlAttributes)
        {
            return AppTextFor(htmlHelper, expression, new RenderTextDate(), colspan, htmlAttributes);
        }
        #endregion

        #region Text Area
        public static MvcHtmlString AppTextArea(this HtmlHelper htmlHelper, string name)
        {
            return AppText(htmlHelper, new RenderTextArea(), name, null, null);
        }
        public static MvcHtmlString AppTextArea(this HtmlHelper htmlHelper, string name, object value)
        {
            return AppText(htmlHelper, new RenderTextArea(), name, value, null);
        }
        public static MvcHtmlString AppTextArea(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            return AppText(htmlHelper, new RenderTextArea(), name, value, htmlAttributes);
        }

        public static MvcHtmlString AppTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return AppTextAreaFor(htmlHelper, expression, 12, null);
        }
        public static MvcHtmlString AppTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int colspan)
        {
            return AppTextAreaFor(htmlHelper, expression, colspan, null);
        }
        public static MvcHtmlString AppTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return AppTextAreaFor(htmlHelper, expression, 12, htmlAttributes);
        }
        public static MvcHtmlString AppTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int colspan, object htmlAttributes)
        {
            return AppTextFor(htmlHelper, expression, new RenderTextArea(), colspan, htmlAttributes);
        }
        #endregion

        #region Text Decimal
        public static MvcHtmlString AppTextboxDecimal(this HtmlHelper htmlHelper, string name)
        {
            return AppTextboxDecimal(htmlHelper, name, null, null);
        }
        public static MvcHtmlString AppTextboxDecimal(this HtmlHelper htmlHelper, string name, object value)
        {
            return AppTextboxDecimal(htmlHelper, name, value, null);
        }
        public static MvcHtmlString AppTextboxDecimal(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            if (value != null)
            {
                if (NumUtil.IsNumeric(value))
                    value = NumUtil.ParseDecimal(value.ToString());
            }

            return AppText(htmlHelper, new RenderTextboxDecimal(), name, value, htmlAttributes);
        }

        public static MvcHtmlString AppTextboxDecimalFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return AppTextboxDecimalFor(htmlHelper, expression, 12, null);
        }
        public static MvcHtmlString AppTextboxDecimalFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return AppTextboxDecimalFor(htmlHelper, expression, 12, htmlAttributes);
        }

        public static MvcHtmlString AppTextboxDecimalFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int colspan)
        {
            return AppTextboxDecimalFor(htmlHelper, expression, colspan, null);
        }

        private static MvcHtmlString AppTextboxDecimalFor<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int colspan, object htmlAttributes)
        {

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (metadata.Model != null)
            {
                if (NumUtil.IsNumeric(metadata.Model))
                    metadata.Model = NumUtil.FormatCurrency(NumUtil.ParseDecimal(metadata.Model.ToString()), false);
            }

            return AppTextFor(htmlHelper, expression, new RenderTextboxDecimal(), colspan, htmlAttributes);
        }



        #endregion

        #region Text Password
        public static MvcHtmlString AppPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return AppPasswordFor(htmlHelper, expression, 12, null);
        }
        public static MvcHtmlString AppPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int colspan)
        {
            return AppPasswordFor(htmlHelper, expression, colspan, null);
        }

        public static MvcHtmlString AppPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return AppPasswordFor(htmlHelper, expression, 12, htmlAttributes);
        }
        public static MvcHtmlString AppPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int colspan, object htmlAttributes)
        {
            return AppTextFor(htmlHelper, expression, new RenderPassword(), colspan, htmlAttributes);
        }
        #endregion

        #region Text Box
        public static MvcHtmlString AppTextbox(this HtmlHelper htmlHelper, string name)
        {
            return AppTextbox(htmlHelper, name, null, null);
        }
        public static MvcHtmlString AppTextbox(this HtmlHelper htmlHelper, string name, object value)
        {
            return AppTextbox(htmlHelper, name, value, null);
        }

        public static MvcHtmlString AppTextbox(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
        {
            return AppText(htmlHelper, new RenderTextbox(), name, value, htmlAttributes);
        }
        public static MvcHtmlString AppTextboxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return AppTextboxFor(htmlHelper, expression, 12, null);
        }
        public static MvcHtmlString AppTextboxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int colspan)
        {
            return AppTextboxFor(htmlHelper, expression, colspan, null);
        }
        public static MvcHtmlString AppTextboxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return AppTextboxFor(htmlHelper, expression, 12, htmlAttributes);
        }

        public static MvcHtmlString AppTextboxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int colspan, object htmlAttributes)
        {
            return AppTextFor(htmlHelper, expression, new RenderTextbox(), colspan, htmlAttributes);
        }

        #endregion

        #region Text Time
        public static MvcHtmlString AppTextTimeFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return AppTextFor(htmlHelper, expression, new RenderTextTime(), 12, null);
        }
        public static MvcHtmlString AppTextTimeFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int colspan)
        {
            return AppTextFor(htmlHelper, expression, new RenderTextTime(), colspan, null);
        }
        public static MvcHtmlString AppTextTimeFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return AppTextFor(htmlHelper, expression, new RenderTextTime(), 12, htmlAttributes);
        }
        public static MvcHtmlString AppTextTimeFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int colspan, object htmlAttributes)
        {
            return AppTextFor(htmlHelper, expression, new RenderTextTime(), colspan, htmlAttributes);
        }
        #endregion

        private static MvcHtmlString AppText(this HtmlHelper htmlHelper, AppControl control, string name, object value, object htmlAttributes)
        {
            control.Name = name;
            control.ID = name;
            control.Placeholder = name;
            control.Value = value;

            HtmlAttributes(control, htmlAttributes);

            ModelError modelError = htmlHelper.ViewData.ModelState.Where(w => w.Key == name).SelectMany(m => m.Value.Errors).FirstOrDefault();
            if (modelError != null)
                control.HtmlValidateString = MvcHtmlString.Create("<span class='validation_wrapper customValidation'><span>" + modelError.ErrorMessage + "</span></span>");

            return MvcHtmlString.Create(control.ToString());
        }

        private static MvcHtmlString AppTextFor<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, AppControl control, int colspan, object htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Object value = metadata.Model;
            control.Name = metadata.PropertyName;
            if (!string.IsNullOrEmpty(metadata.DisplayName))
                control.Placeholder = metadata.DisplayName;
            else
                control.Placeholder = metadata.PropertyName.Replace("_", " ");

            control.ID = metadata.PropertyName;
            control.ColSpan = colspan;

            if (value != null)
            {
                control.Value = value;
                control.Text = value.ToString();
            }

            HtmlAttributes(control, htmlAttributes);

            ModelError modelError = htmlHelper.ViewData.ModelState.Where(w => w.Key == metadata.PropertyName).SelectMany(m => m.Value.Errors).FirstOrDefault();
            if (modelError != null)
                control.HtmlValidateString = MvcHtmlString.Create("<span class='validation_wrapper customValidation'><span>" + modelError.ErrorMessage + "</span></span>");

            return MvcHtmlString.Create(control.ToString());
        }


        public static MvcHtmlString AppValidationMessage(this HtmlHelper htmlHelper, string name)
        {
           var control = new RenderValidateMessage();
           ModelError modelError = htmlHelper.ViewData.ModelState.Where(w => w.Key == name).SelectMany(m => m.Value.Errors).FirstOrDefault();
           if (modelError != null)
              control.HtmlValidateString = MvcHtmlString.Create("<span class='validation_wrapper customValidation'><span>" + modelError.ErrorMessage + "</span></span>");

           return MvcHtmlString.Create(control.ToString());
        }

        #endregion
        private static void HtmlAttributes(AppControl control, object htmlAttributes)
        {
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                foreach (var attr in attributes)
                {
                    try
                    {
                        if (attr.Value != null && attr.Key != null)
                        {
                            if (attr.Key.ToLower() == "id")
                                control.ID = attr.Value.ToString();
                            else if (attr.Key.ToLower() == "name")
                                control.Name = attr.Value.ToString();
                            else if (attr.Key.ToLower() == "readonly")
                                control.Readonly = (bool)attr.Value;
                            else if (attr.Key.ToLower() == "disabled")
                                control.Disabled = (bool)attr.Value;
                            else if (attr.Key.ToLower() == "text")
                                control.Text = attr.Value.ToString();
                            else if (attr.Key.ToLower() == "rows")
                                control.Rows = attr.Value.ToString();
                            else if (attr.Key.ToLower() == "style")
                                control.Style = attr.Value.ToString();
                            else if (attr.Key.ToLower() == "icon")
                                control.Icon = attr.Value.ToString();
                            else if (attr.Key.ToLower() == "checked")
                            {
                                if (attr.Value != null && (bool)attr.Value == true)
                                    control.Attributes.Add("checked", attr.Value.ToString());
                            }
                            else if (attr.Key.ToLower() == "value")
                                control.Value = attr.Value.ToString();
                            else
                                control.Attributes.Add(attr.Key.Replace("_", "-").ToString(), attr.Value.ToString());
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
