using Agnos.Models;
using AgnosModel.Models;
using AgnosModel.Service;
using AppFramework;
using AppFramework.Control;
using AppFramework.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SBSResourceAPI;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using Agnos.Common;

namespace Agnos.Controllers
{
   public class TemplateController : ControllerBase
   {
      #region Components
      [HttpGet]
      public ActionResult Components(ComponentsViewModels model, ServiceResult msgresult)
      {
         var uService = new UserService();
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0004);
         if (prole == null)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
         if (prole.View == null || prole.View == false)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

         ModelState.Clear();
         if (model.operation == Operation.D)
            return Components(model);

         if (model.operation == Operation.U)
            return Components(model);

         model.Modify = prole.Modify;
         model.View = prole.View;
         model.result = msgresult; // return result from http post
         var tService = new TemplateService();

         var gresult = tService.GetTempateGroup();
         if (gresult.Code == ReturnCode.SUCCESS)
            model.grouplist = gresult.Object as List<Template_Group>;

         var hresult = tService.GetTempateHeader();
         if (hresult.Code == ReturnCode.SUCCESS)
            model.headerlist = hresult.Object as List<Template_Header>;

         var fresult = tService.GetTempateField();
         if (fresult.Code == ReturnCode.SUCCESS)
            model.fieldlist = fresult.Object as List<Template_Field>;

         var cService = new ComboService();
         model.cDatatypelist = cService.LstFieldDataType();
         model.cFieldformlist = cService.LstFieldFrom();
         model.cRolelist = cService.LstRole(true);
         return View(model);
      }

      [HttpPost]
      public ActionResult Components(ComponentsViewModels model)
      {
         var uService = new UserService();
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0004);
         if (prole != null)
            model.Modify = prole.Modify;

         var tService = new TemplateService(User.Identity.GetUserId());
         if (model.tabAction == "group")
         {
            if (model.Group_ID.HasValue)
            {
               if (model.Up.HasValue && model.Up.Value)
               {
                  var result = tService.MoveUpTempateGroup(model.Group_ID);
                  if (result.Code == ReturnCode.SUCCESS)
                     return RedirectToAction("Components", new AppRouteValueDictionary(model));
               }
               else if (model.Down.HasValue && model.Down.Value)
               {
                  var result = tService.MoveDownTempateGroup(model.Group_ID);
                  if (result.Code == ReturnCode.SUCCESS)
                     return RedirectToAction("Components", new AppRouteValueDictionary(model));
               }
            }

            foreach (var key in ModelState.Keys.ToList().Where(key => ModelState.ContainsKey(key) && (!key.Contains("Group"))))
               ModelState[key].Errors.Clear();

            if (!string.IsNullOrEmpty(model.Group_Name))
            {
               var dupcri = new GroupCriteria();
               dupcri.Duplicat_Name = model.Group_Name;
               var dupresult = tService.GetTempateGroup(dupcri);
               if (dupresult.Code == ReturnCode.SUCCESS)
               {
                  var dup = new Template_Group();
                  var dups = dupresult.Object as List<Template_Group>;
                  if (dups != null && dups.Count() != 0)
                     if (model.operation == Operation.C)
                        ModelState.AddModelError("Group_Name", Resource.The + " " + Resource.Group_Name + " " + Resource.Field + " " + Resource.Is_Duplicated);
                     else if (model.operation == Operation.U)
                     {
                        dup = dups.FirstOrDefault();
                        if (dup.Group_ID != model.Group_ID)
                           ModelState.AddModelError("Group_Name", Resource.The + " " + Resource.Group_Name + " " + Resource.Field + " " + Resource.Is_Duplicated);
                     }
               }
            }

            if (ModelState.IsValid)
            {
               var group = new Template_Group();
               if (model.operation == Operation.U)
               {
                  var cri = new GroupCriteria();
                  cri.Group_ID = model.Group_ID;
                  var result = tService.GetTempateGroup(cri);
                  if (result.Code == ReturnCode.SUCCESS)
                  {
                     var groups = result.Object as List<Template_Group>;
                     if (groups != null && groups.Count() == 1)
                        group = groups.FirstOrDefault();
                  }
               }

               if (model.operation != Operation.D)
               {
                  group.Group_Name = model.Group_Name;
                  group.Role_ID = model.Role_ID;
               }


               if (model.operation == Operation.C)
                  model.result = tService.InsertTempateGroup(group);
               else if (model.operation == Operation.U)
                  model.result = tService.UpdateTempateGroup(group);
               else if (model.operation == Operation.D)
                  model.result = tService.DeleteTempateGroup(model.Group_ID);

               return RedirectToAction("Components", new AppRouteValueDictionary(model));
            }
         }
         else if (model.tabAction == "hdr")
         {
            if (model.Header_ID.HasValue)
            {
               if (model.Up.HasValue && model.Up.Value)
               {
                  var result = tService.MoveUpTempateHeader(model.Header_ID);
                  if (result.Code == ReturnCode.SUCCESS)
                     return RedirectToAction("Components", new AppRouteValueDictionary(model));
               }
               else if (model.Down.HasValue && model.Down.Value)
               {
                  var result = tService.MoveDownTempateHeader(model.Header_ID);
                  if (result.Code == ReturnCode.SUCCESS)
                     return RedirectToAction("Components", new AppRouteValueDictionary(model));
               }
            }

            foreach (var key in ModelState.Keys.ToList().Where(key => ModelState.ContainsKey(key) && (!key.Contains("Header"))))
               ModelState[key].Errors.Clear();

            if (!string.IsNullOrEmpty(model.Header_Name))
            {
               var dupcri = new HeaderCriteria();
               dupcri.Duplicat_Name = model.Header_Name;
               var dupresult = tService.GetTempateHeader(dupcri);
               if (dupresult.Code == ReturnCode.SUCCESS)
               {
                  var dup = new Template_Header();
                  var dups = dupresult.Object as List<Template_Header>;
                  if (dups != null && dups.Count() != 0)
                     if (model.operation == Operation.C)
                        ModelState.AddModelError("Header_Name", Resource.The + " " + Resource.Header_Name + " " + Resource.Field + " " + Resource.Is_Duplicated);
                     else if (model.operation == Operation.U)
                     {
                        dup = dups.FirstOrDefault();
                        if (dup.Header_ID != model.Header_ID)
                           ModelState.AddModelError("Header_Name", Resource.The + " " + Resource.Header_Name + " " + Resource.Field + " " + Resource.Is_Duplicated);
                     }
               }
            }

            if (ModelState.IsValid)
            {
               var hdr = new Template_Header();
               if (model.operation == Operation.U)
               {
                  var cri = new HeaderCriteria();
                  cri.Header_ID = model.Header_ID;
                  var result = tService.GetTempateHeader(cri);
                  if (result.Code == ReturnCode.SUCCESS)
                  {
                     var hdrs = result.Object as List<Template_Header>;
                     if (hdrs != null && hdrs.Count() == 1)
                        hdr = hdrs.FirstOrDefault();
                  }
               }

               if (model.operation != Operation.D)
               {
                  hdr.Header_Name = model.Header_Name;
               }

               if (model.operation == Operation.C)
                  model.result = tService.InsertTempateHeader(hdr);

               else if (model.operation == Operation.U)
                  model.result = tService.UpdateTempateHeader(hdr);

               else if (model.operation == Operation.D)
                  model.result = tService.DeleteTempateHeader(model.Header_ID);


               return RedirectToAction("Components", new AppRouteValueDictionary(model));
            }
         }
         else if (model.tabAction == "field")
         {
            if (model.Field_ID.HasValue)
            {
               if (model.Up.HasValue && model.Up.Value)
               {
                  var result = tService.MoveUpTempateField(model.Field_ID);
                  if (result.Code == ReturnCode.SUCCESS)
                     return RedirectToAction("Components", new AppRouteValueDictionary(model));
               }
               else if (model.Down.HasValue && model.Down.Value)
               {
                  var result = tService.MoveDownTempateField(model.Field_ID);
                  if (result.Code == ReturnCode.SUCCESS)
                     return RedirectToAction("Components", new AppRouteValueDictionary(model));
               }
            }

            foreach (var key in ModelState.Keys.ToList().Where(key => ModelState.ContainsKey(key) && (!key.Contains("Field"))))
               ModelState[key].Errors.Clear();

            if (!string.IsNullOrEmpty(model.Field_Name))
            {
               var dupcri = new FieldCriteria();
               dupcri.Duplicat_Name = model.Field_Name;
               var dupresult = tService.GetTempateField(dupcri);
               if (dupresult.Code == ReturnCode.SUCCESS)
               {
                  var dup = new Template_Field();
                  var dups = dupresult.Object as List<Template_Field>;
                  if (dups != null && dups.Count() != 0)
                     if (model.operation == Operation.C)
                        ModelState.AddModelError("Field_Name", Resource.The + " " + Resource.Field_Name + " " + Resource.Field + " " + Resource.Is_Duplicated);
                     else if (model.operation == Operation.U)
                     {
                        dup = dups.FirstOrDefault();
                        if (dup.Field_ID != model.Field_ID)
                           ModelState.AddModelError("Field_Name", Resource.The + " " + Resource.Field_Name + " " + Resource.Field + " " + Resource.Is_Duplicated);
                     }
               }
            }

            if (ModelState.IsValid)
            {
               var field = new Template_Field();
               if (model.operation == Operation.U)
               {
                  var cri = new FieldCriteria();
                  cri.Field_ID = model.Field_ID;
                  var result = tService.GetTempateField(cri);
                  if (result.Code == ReturnCode.SUCCESS)
                  {
                     var fields = result.Object as List<Template_Field>;
                     if (fields != null && fields.Count() == 1)
                        field = fields.FirstOrDefault();
                  }
               }
               if (model.operation != Operation.D)
               {
                  field.Field_Name = model.Field_Name;
                  field.Field_From = model.Field_From;
                  field.Data_Type = model.Field_Data_Type;
               }

               if (model.operation == Operation.C)
                  model.result = tService.InsertTempateField(field);
               else if (model.operation == Operation.U)
                  model.result = tService.UpdateTempateField(field);
               else if (model.operation == Operation.D)
                  model.result = tService.DeleteTempateField(model.Field_ID);

               return RedirectToAction("Components", new AppRouteValueDictionary(model));
            }
         }

         var gresult = tService.GetTempateGroup();
         if (gresult.Code == ReturnCode.SUCCESS)
            model.grouplist = gresult.Object as List<Template_Group>;

         var hresult = tService.GetTempateHeader();
         if (hresult.Code == ReturnCode.SUCCESS)
            model.headerlist = hresult.Object as List<Template_Header>;

         var fresult = tService.GetTempateField();
         if (fresult.Code == ReturnCode.SUCCESS)
            model.fieldlist = fresult.Object as List<Template_Field>;

         var cService = new ComboService();
         model.cDatatypelist = cService.LstFieldDataType();
         model.cFieldformlist = cService.LstFieldFrom();
         model.cRolelist = cService.LstRole(true);
         return View(model);

      }
      #endregion

      #region Template Logsheet
      [HttpGet]
      public ActionResult TemplateLogsheet(TemplateLogsheetViewModels model, ServiceResult msgresult)
      {
         var uService = new UserService();
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0005);
         if (prole == null)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
         if (prole.View == null || prole.View == false)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

         ModelState.Clear();
         model.Modify = prole.Modify;
         model.View = prole.View;
         model.result = msgresult; // return result from http post
         var tService = new TemplateService();

         var cri = new TemplateLogsheetCriteria();
         cri.Template_Code = model.search_Template_Code;
         cri.Template_Name = model.search_Template_Name;
         var tresult = tService.GetTempateLogsheet(cri);
         if (tresult.Code == ReturnCode.SUCCESS)
            model.Tmplogsheetlist = tresult.Object as List<Template_Logsheet>;

         return View(model);
      }
      [HttpGet]
      public ActionResult TemplateLogsheetInfo(TemplateLogsheetViewModels pmodel, ServiceResult msgresult, bool pPrint = false)
      {
         var uService = new UserService();
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0005);
         if (prole == null)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
         if (prole.View == null || prole.View == false)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

         ModelState.Clear();
         var model = new TemplateLogsheetInfoViewModels();
         model.operation = pmodel.operation;
         model.Template_ID = pmodel.Template_ID;
         model.Modify = prole.Modify;
         model.View = prole.View;
         model.Print = pPrint;
         model.result = msgresult;
         var tService = new TemplateService(User.Identity.GetUserId());
         if (model.operation == Operation.C)
         {
            model.Tmp_Log_Group_Rows = (new List<Tmp_Log_Group_Row>()).ToArray();
         }
         else if (model.operation == Operation.U)
         {
            var cri = new TemplateLogsheetCriteria();
            cri.Template_ID = model.Template_ID;
            cri.include = false;
            var result = tService.GetTempateLogsheet(cri);
            if (result.Code == ReturnCode.SUCCESS)
            {
               var tmps = result.Object as List<Template_Logsheet>;
               if (tmps != null && tmps.Count() == 1)
               {
                  var tmp = tmps.FirstOrDefault();
                  model.Template_Code = tmp.Template_Code;
                  model.Template_Name = tmp.Template_Name;
                  model.Template_Description = tmp.Template_Description;

                  var i = 0;
                  var tmpGs = new List<Tmp_Log_Group_Row>();
                  foreach (var group in tmp.Tmp_Log_Group)
                  {
                     var tmpG = new Tmp_Log_Group_Row();
                     tmpG.Group_ID = group.Group_ID;
                     tmpG.Tmp_Log_Group_ID = group.Tmp_Log_Group_ID;
                     tmpG.Template_ID = group.Template_ID;
                     tmpG.Group_Order = group.Group_Order;
                     tmpG.Sub_Group_Name = group.Sub_Group_Name;
                     tmpG.RowAction = RAction.Edit;
                     var h = 0;
                     var tmphdrs = new List<Tmp_Log_Header_Row>();
                     foreach (var hdr in group.Tmp_Log_Header)
                     {
                        var tmphdr = new Tmp_Log_Header_Row();
                        tmphdr.Tmp_Log_Header_ID = hdr.Tmp_Log_Header_ID;
                        tmphdr.Tmp_Log_Group_ID = hdr.Tmp_Log_Group_ID;
                        tmphdr.Header_ID = hdr.Header_ID;
                        tmphdr.Header_Order = hdr.Header_Order;
                        tmphdr.Sumary_Report_Display = hdr.Sumary_Report_Display.HasValue ? hdr.Sumary_Report_Display.Value : true;
                        tmphdr.RowAction = RAction.Edit;
                        tmphdr.i = i;
                        tmphdr.h = h;

                        var m = 0;
                        var tmpMaps = new List<Tmp_Log_Map_Row>();
                        foreach (var field in hdr.Tmp_Log_Map)
                        {
                           var tmpmap = new Tmp_Log_Map_Row();
                           tmpmap.Tmp_Log_Header_ID = field.Tmp_Log_Header_ID;
                           tmpmap.Tmp_Log_Map_ID = field.Tmp_Log_Map_ID;
                           tmpmap.Field_ID = field.Field_ID;
                           tmpmap.Option_Text = field.Option_Text;
                           tmpmap.Option_Selected = field.Option_Selected;
                           tmpmap.Option_Data_Type = field.Option_Data_Type;
                           tmpmap.Option_Field_From = field.Option_Field_From;
                           tmpmap.Option_Range_From = NumUtil.ParseDecimal(field.Option_Range_From);
                           tmpmap.Option_Range_To = NumUtil.ParseDecimal(field.Option_Range_To);
                           tmpmap.Option_Dropdown_Type = field.Option_Dropdown_Type;
                           tmpmap.Lot_No = field.Lot_No;
                           tmpmap.RowAction = RAction.Edit;
                           tmpmap.i = i;
                           tmpmap.h = h;
                           tmpmap.m = m;
                           tmpMaps.Add(tmpmap);
                           m++;
                        }
                        tmphdr.Tmp_Log_Map_Rows = tmpMaps.ToArray();
                        if (tmphdr.Tmp_Log_Map_Rows == null)
                           tmphdr.Tmp_Log_Map_Rows = (new List<Tmp_Log_Map_Row>()).ToArray();

                        tmphdrs.Add(tmphdr);
                        h++;
                     }
                     tmpG.Tmp_Log_Header_Rows = tmphdrs.ToArray();
                     if (tmpG.Tmp_Log_Header_Rows == null)
                        tmpG.Tmp_Log_Header_Rows = (new List<Tmp_Log_Header_Row>()).ToArray();


                     var f = 0;
                     var tmpfields = new List<Tmp_Log_Field_Row>();
                     foreach (var field in group.Tmp_Log_Field)
                     {
                        var tmpfield = new Tmp_Log_Field_Row();
                        tmpfield.Tmp_Log_Group_ID = field.Tmp_Log_Group_ID;
                        tmpfield.Tmp_Log_Field_ID = field.Tmp_Log_Field_ID;
                        tmpfield.Field_ID = field.Field_ID;
                        tmpfield.Field_Order = field.Field_Order;
                        tmpfield.RowAction = RAction.Edit;
                        tmpfield.i = i;
                        tmpfield.f = f;
                        tmpfields.Add(tmpfield);
                        f++;
                     }
                     tmpG.Tmp_Log_Field_Rows = tmpfields.ToArray();
                     if (tmpG.Tmp_Log_Field_Rows == null)
                        tmpG.Tmp_Log_Field_Rows = (new List<Tmp_Log_Field_Row>()).ToArray();

                     tmpGs.Add(tmpG);
                     i++;
                  }
                  model.Tmp_Log_Group_Rows = tmpGs.ToArray();
                  if (model.Tmp_Log_Group_Rows == null)
                     model.Tmp_Log_Group_Rows = (new List<Tmp_Log_Group_Row>()).ToArray();
               }
            }
         }
         else if (model.operation == Operation.D)
         {
            if (model.Template_ID.HasValue)
            {
               model.result = tService.DeleteTempateLogsheet(model.Template_ID);
               if (model.result.Code == ReturnCode.SUCCESS)
                  model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Template_Logsheet };
               else
                  model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Template_Logsheet };
            }
            return RedirectToAction("TemplateLogsheet", new AppRouteValueDictionary(model));
         }
         var cbService = new ComboService();
         model.cDatatypelist = cbService.LstFieldDataType();
         model.cFieldformlist = cbService.LstFieldFrom();
         model.cHeaderlist = cbService.LstTmpHeader();
         model.cFieldlist = cbService.LstTmpField();
         model.cGrouplist = cbService.LstTmpGroup();
         model.cDropdowntypelist = cbService.LstDropdownListType();
         return View(model);
      }

      [HttpPost]
      public ActionResult TemplateLogsheetInfo(TemplateLogsheetInfoViewModels model, string savemode, string HMove, string FMove)
      {
         if (string.IsNullOrEmpty(savemode))
            ModelState.Clear();

         var tService = new TemplateService(User.Identity.GetUserId());
         if (!string.IsNullOrEmpty(HMove))
         {
            var maction = HMove.Split('-');
            if (maction.Length == 3)
            {
               var moveaction = maction[0];
               var i = NumUtil.ParseInteger(maction[1]);
               var h = NumUtil.ParseInteger(maction[2]);

               var iindex = 0;
               foreach (var tmpgroup in model.Tmp_Log_Group_Rows)
               {
                  if (i == iindex)
                  {
                     var newHdrs = new List<Tmp_Log_Header_Row>();
                     var hindex = 0;
                     Tmp_Log_Header_Row temp = null;
                     if (moveaction == "Left")
                     {
                        foreach (var tmphdr in tmpgroup.Tmp_Log_Header_Rows)
                        {
                           if (h == hindex && h != 0)
                           {
                              newHdrs.Add(tmphdr);
                              newHdrs.Add(temp);
                           }
                           else if (h - 1 >= 0 && hindex == h - 1)
                              temp = tmphdr;
                           else
                              newHdrs.Add(tmphdr);
                           hindex++;
                        }
                     }
                     else
                     {
                        foreach (var tmphdr in tmpgroup.Tmp_Log_Header_Rows)
                        {
                           if (hindex == h && h + 1 < tmpgroup.Tmp_Log_Header_Rows.Length)
                              temp = tmphdr;
                           else if (hindex == h + 1)
                           {
                              newHdrs.Add(tmphdr);
                              newHdrs.Add(temp);
                           }
                           else
                              newHdrs.Add(tmphdr);
                           hindex++;
                        }
                     }
                     tmpgroup.Tmp_Log_Header_Rows = newHdrs.ToArray();
                     break;
                  }
                  iindex++;
               }
            }
         }
         else if (!string.IsNullOrEmpty(FMove))
         {
            var maction = FMove.Split('-');
            if (maction.Length == 3)
            {
               var moveaction = maction[0];
               var i = NumUtil.ParseInteger(maction[1]);
               var f = NumUtil.ParseInteger(maction[2]);
               var iindex = 0;
               foreach (var tmpgroup in model.Tmp_Log_Group_Rows)
               {
                  if (i == iindex)
                  {
                     var newFields = new List<Tmp_Log_Field_Row>();
                     var findex = 0;
                     Tmp_Log_Field_Row temp = null;
                     if (moveaction == "Up")
                     {
                        foreach (var tmpField in tmpgroup.Tmp_Log_Field_Rows)
                        {
                           if (f == findex && f != 0)
                           {
                              newFields.Add(tmpField);
                              newFields.Add(temp);
                           }
                           else if (f - 1 >= 0 && findex == f - 1)
                              temp = tmpField;
                           else
                              newFields.Add(tmpField);
                           findex++;
                        }
                     }
                     else
                     {
                        foreach (var tmpField in tmpgroup.Tmp_Log_Field_Rows)
                        {
                           if (findex == f && f + 1 < tmpgroup.Tmp_Log_Field_Rows.Length)
                              temp = tmpField;
                           else if (findex == f + 1)
                           {
                              newFields.Add(tmpField);
                              newFields.Add(temp);
                           }
                           else
                              newFields.Add(tmpField);
                           findex++;
                        }
                     }
                     tmpgroup.Tmp_Log_Field_Rows = newFields.ToArray();
                     break;
                  }
                  iindex++;
               }
            }
         }

         if (!string.IsNullOrEmpty(model.Template_Code))
         {
            var dupcri = new TemplateLogsheetCriteria();
            dupcri.Template_Code = model.Template_Code;
            var dupresult = tService.GetTempateLogsheet(dupcri);
            if (dupresult.Code == ReturnCode.SUCCESS)
            {
               var dup = new Template_Logsheet();
               var dups = dupresult.Object as List<Template_Logsheet>;
               if (dups != null && dups.Count() != 0)
                  if (model.operation == Operation.C)
                     ModelState.AddModelError("Template_Code", Resource.The + " " + Resource.Template_Code + " " + Resource.Field + " " + Resource.Is_Duplicated);
                  else if (model.operation == Operation.U)
                  {
                     if (dups.Count() == 1)
                     {
                        dup = dups.FirstOrDefault();
                        if (dup.Template_ID != model.Template_ID)
                           ModelState.AddModelError("Template_Code", Resource.The + " " + Resource.Template_Code + " " + Resource.Field + " " + Resource.Is_Duplicated);
                     }
                  }
            }
         }
         if (!string.IsNullOrEmpty(model.Template_Name))
         {
            var dupcri = new TemplateLogsheetCriteria();
            dupcri.Template_Name = model.Template_Name;
            var dupresult = tService.GetTempateLogsheet(dupcri);
            if (dupresult.Code == ReturnCode.SUCCESS)
            {
               var dup = new Template_Logsheet();
               var dups = dupresult.Object as List<Template_Logsheet>;
               if (dups != null && dups.Count() != 0)
                  if (model.operation == Operation.C)
                     ModelState.AddModelError("Template_Name", Resource.The + " " + Resource.Template_Name + " " + Resource.Field + " " + Resource.Is_Duplicated);
                  else if (model.operation == Operation.U)
                  {
                     if (dups.Count() == 1)
                     {
                        dup = dups.FirstOrDefault();
                        if (dup.Template_ID != model.Template_ID)
                           ModelState.AddModelError("Template_Name", Resource.The + " " + Resource.Template_Name + " " + Resource.Field + " " + Resource.Is_Duplicated);
                     }
                  }
            }
         }
         foreach (var tmpgroup in model.Tmp_Log_Group_Rows)
         {
            var existhdrs = new List<int>();
            if (tmpgroup.Tmp_Log_Header_Rows != null)
               tmpgroup.Tmp_Log_Header_Rows = tmpgroup.Tmp_Log_Header_Rows.Where(w => w.RowAction != RAction.Delete).ToArray();
            else
               tmpgroup.Tmp_Log_Header_Rows = (new List<Tmp_Log_Header_Row>()).ToArray();

            var existfields = new List<int>();
            if (tmpgroup.Tmp_Log_Field_Rows != null)
               tmpgroup.Tmp_Log_Field_Rows = tmpgroup.Tmp_Log_Field_Rows.Where(w => w.RowAction != RAction.Delete).ToArray();
            else
               tmpgroup.Tmp_Log_Field_Rows = (new List<Tmp_Log_Field_Row>()).ToArray();

            if (tmpgroup.Tmp_Log_Header_Rows.Length == 0)
               tmpgroup.Tmp_Log_Field_Rows = (new List<Tmp_Log_Field_Row>()).ToArray();

            if (tmpgroup.Tmp_Log_Field_Rows.Length == 0)
               tmpgroup.Tmp_Log_Header_Rows = (new List<Tmp_Log_Header_Row>()).ToArray();

            var h = 0;
            foreach (var tmphdr in tmpgroup.Tmp_Log_Header_Rows)
            {
               tmphdr.h = h;
               if (!tmphdr.Header_ID.HasValue)
               {
                  ModelState.AddModelError(NameUtil.GenTmpLogsheet.GenHeaderName(tmphdr.i, tmphdr.h, "Header_ID"), Resource.The + " " + Resource.Header + " " + Resource.Is_Required);
                  h++;
                  continue;
               }

               if (existhdrs.Contains(tmphdr.Header_ID.Value))
               {
                  ModelState.AddModelError(NameUtil.GenTmpLogsheet.GenHeaderName(tmphdr.i, tmphdr.h, "Header_ID"), Resource.The + " " + Resource.Header + " " + Resource.Is_Duplicated);
                  h++;
                  continue;
               }


               tmphdr.Tmp_Log_Map_Rows = tmphdr.Tmp_Log_Map_Rows.Where(w => w.RowAction != RAction.Delete).ToArray();
               var m = 0;
               foreach (var tmpmap in tmphdr.Tmp_Log_Map_Rows)
               {
                  tmpmap.h = h;
                  tmpmap.m = m;
                  m++;
               }
               existhdrs.Add(tmphdr.Header_ID.Value);
               h++;
            }



            var f = 0;
            foreach (var tmpfield in tmpgroup.Tmp_Log_Field_Rows)
            {
               tmpfield.f = f;
               if (!tmpfield.Field_ID.HasValue)
               {
                  ModelState.AddModelError(NameUtil.GenTmpLogsheet.GenFieldName(tmpfield.i, tmpfield.f, "Field_ID"), Resource.The + " " + Resource.Field + " " + Resource.Is_Required);
                  f++;
                  continue;
               }

               if (existfields.Contains(tmpfield.Field_ID.Value))
               {
                  ModelState.AddModelError(NameUtil.GenTmpLogsheet.GenFieldName(tmpfield.i, tmpfield.f, "Field_ID"), Resource.The + " " + Resource.Field + " " + Resource.Is_Duplicated);
                  f++;
                  continue;
               }
               existfields.Add(tmpfield.Field_ID.Value);
               f++;
            }
         }

         if (!string.IsNullOrEmpty(savemode))
         {
            if (ModelState.IsValid)
            {
               var tmp = new Template_Logsheet();
               if (model.operation == Operation.C)
               {
               }
               else if (model.operation == Operation.U)
               {
                  var cri = new TemplateLogsheetCriteria();
                  cri.Template_ID = model.Template_ID;
                  cri.include = true;
                  var result = tService.GetTempateLogsheet(cri);
                  if (result.Code == ReturnCode.SUCCESS)
                  {
                     var tmps = result.Object as List<Template_Logsheet>;
                     if (tmps != null && tmps.Count() == 1)
                        tmp = tmps.FirstOrDefault();
                  }
               }

               tmp.Template_Code = model.Template_Code;
               tmp.Template_Name = model.Template_Name;
               tmp.Template_Description = model.Template_Description;
               var i = 0;
               if (model.Tmp_Log_Group_Rows != null)
               {
                  var groups = new List<Tmp_Log_Group>();
                  foreach (var grow in model.Tmp_Log_Group_Rows)
                  {
                     if (grow.RowAction == RAction.Delete)
                        continue;

                     if (grow.Tmp_Log_Header_Rows == null || grow.Tmp_Log_Field_Rows == null)
                        continue;

                     var group = new Tmp_Log_Group();
                     group.Tmp_Log_Group_ID = grow.Tmp_Log_Group_ID.HasValue ? grow.Tmp_Log_Group_ID.Value : 0;
                     group.Group_ID = grow.Group_ID;
                     group.Template_ID = model.Template_ID;
                     group.Sub_Group_Name = grow.Sub_Group_Name;
                     group.Update_On = tmp.Update_On;
                     group.Create_On = tmp.Create_On;
                     group.Group_Order = i;
                     group.Tmp_Log_Header = new List<Tmp_Log_Header>();

                     var f = 0;
                     var fIDs = new List<int?>();
                     foreach (var frow in grow.Tmp_Log_Field_Rows)
                     {
                        if (frow.RowAction == RAction.Delete)
                           continue;

                        var field = new Tmp_Log_Field();
                        field.Tmp_Log_Field_ID = frow.Tmp_Log_Field_ID.HasValue ? frow.Tmp_Log_Field_ID.Value : 0;
                        field.Field_ID = frow.Field_ID;
                        field.Tmp_Log_Group_ID = grow.Tmp_Log_Group_ID;
                        field.Field_Order = f;
                        fIDs.Add(field.Field_ID);
                        group.Tmp_Log_Field.Add(field);
                        f++;
                     }

                     var h = 0;
                     foreach (var hdrow in grow.Tmp_Log_Header_Rows)
                     {
                        if (hdrow.RowAction == RAction.Delete)
                           continue;

                        var hdr = new Tmp_Log_Header();
                        hdr.Tmp_Log_Header_ID = hdrow.Tmp_Log_Header_ID.HasValue ? hdrow.Tmp_Log_Header_ID.Value : 0;
                        hdr.Header_ID = hdrow.Header_ID;
                        hdr.Tmp_Log_Group_ID = grow.Tmp_Log_Group_ID;
                        hdr.Header_Order = hdrow.Header_Order;
                        hdr.Sumary_Report_Display = hdrow.Sumary_Report_Display;
                        hdr.Tmp_Log_Map = new List<Tmp_Log_Map>();
                        hdr.Header_Order = h;
                        if (hdrow.Tmp_Log_Map_Rows == null)
                           hdrow.Tmp_Log_Map_Rows = new List<Tmp_Log_Map_Row>().ToArray();

                        var m = 0;
                        foreach (var mrow in hdrow.Tmp_Log_Map_Rows)
                        {
                           if (mrow.RowAction == RAction.Delete)
                              continue;

                           var map = new Tmp_Log_Map();
                           map.Tmp_Log_Map_ID = mrow.Tmp_Log_Map_ID.HasValue ? mrow.Tmp_Log_Map_ID.Value : 0;
                           map.Field_ID = fIDs[m];
                           map.Header_ID = hdrow.Header_ID;
                           map.Tmp_Log_Header_ID = hdrow.Tmp_Log_Header_ID;
                           map.Option_Data_Type = mrow.Option_Data_Type;
                           map.Option_Field_From = mrow.Option_Field_From;
                           if (mrow.Option_Field_From == "Lot No.")
                              map.Lot_No = mrow.Lot_No;
                           else
                              map.Lot_No = null;

                           map.Option_Range_From = mrow.Option_Range_From.HasValue ? mrow.Option_Range_From.Value.ToString() : "0.00000";
                           map.Option_Range_To = mrow.Option_Range_To.HasValue ? mrow.Option_Range_To.Value.ToString() : "0.00000";
                           map.Option_Selected = mrow.Option_Selected;
                           map.Option_Text = mrow.Option_Text;
                           map.Option_Dropdown_Type = mrow.Option_Dropdown_Type;
                           map.Map_Order = m + 1;
                           hdr.Tmp_Log_Map.Add(map);
                           m++;
                        }
                        group.Tmp_Log_Header.Add(hdr);
                        h++;
                     }



                     groups.Add(group);
                  }
                  tmp.Tmp_Log_Group = groups;
                  i++;
               }


               if (model.operation == Operation.C)
               {
                  model.result = tService.InsertTempateLogsheet(tmp);
                  if (model.result.Code == ReturnCode.SUCCESS)
                  {
                     if (model.savemode == "save")
                        return RedirectToAction("TemplateLogsheet", new AppRouteValueDictionary(model));
                     else
                     {
                        var route = new AppRouteValueDictionary(model);
                        route.Add("Template_ID", tmp.Template_ID);
                        route.Add("pPrint", true);
                        route.Add("operation", Operation.U);
                        return RedirectToAction("TemplateLogsheetInfo", route);
                     }
                  }
               }
               else if (model.operation == Operation.U)
               {
                  model.result = tService.UpdateTempateLogsheet(tmp);
                  if (model.result.Code == ReturnCode.SUCCESS)
                  {
                     if (model.savemode == "save")
                        return RedirectToAction("TemplateLogsheet", new AppRouteValueDictionary(model));
                     else
                     {
                        var route = new AppRouteValueDictionary(model);
                        route.Add("Template_ID", tmp.Template_ID);
                        route.Add("pPrint", true);
                        route.Add("operation", Operation.U);
                        return RedirectToAction("TemplateLogsheetInfo", route);
                     }

                  }
               }
            }
         }
         var cbService = new ComboService();
         model.cDatatypelist = cbService.LstFieldDataType();
         model.cFieldformlist = cbService.LstFieldFrom();
         model.cHeaderlist = cbService.LstTmpHeader();
         model.cFieldlist = cbService.LstTmpField();
         model.cGrouplist = cbService.LstTmpGroup();
         model.cDropdowntypelist = cbService.LstDropdownListType();
         var uService = new UserService();
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0005);
         if (prole != null)
            model.Modify = prole.Modify;


         return View(model);
      }

      public ActionResult TmpLogNewHdr(int i, int h)
      {
         var model = new Tmp_Log_Header_Row();
         var cbService = new ComboService();
         model.cHeaderlist = cbService.LstTmpHeader();
         model.i = i;
         model.h = h;
         model.Header_Order = h;
         model.RowAction = RAction.Add;
         return PartialView("TmpLogsheetHdr", model);
      }

      public ActionResult TmpLogNewField(int i, int f)
      {
         var model = new Tmp_Log_Field_Row();
         var cbService = new ComboService();
         model.cFieldlist = cbService.LstTmpField();
         model.i = i;
         model.f = f;
         model.Field_Order = f;
         model.RowAction = RAction.Add;
         return PartialView("TmpLogsheetField", model);
      }

      public ActionResult TmpLogNewMap(int i, int h, int m, int fID, int hID, int? tfID, int? thID)
      {
         var model = new Tmp_Log_Map_Row();
         var cbService = new ComboService();
         model.i = i;
         model.h = h;
         model.m = m;
         model.Field_ID = fID;
         model.Header_ID = hID;
         model.Tmp_Log_Header_ID = tfID;
         model.RowAction = RAction.Add;
         model.Option_Selected = Logsheet_Control_Type.Text;
         model.cDropdownTypeList = cbService.LstDropdownListType();
         return PartialView("TmpLogsheetMap", model);
      }

      [HttpGet]
      public ActionResult TemplateLogsheetPrint(TemplateLogsheetInfoViewModels pmodel)
      {
         var uService = new UserService();
         ModelState.Clear();
         var model = new TemplateLogsheetInfoViewModels();
         model.operation = pmodel.operation;
         model.Template_ID = pmodel.Template_ID;
         var tmp = new Template_Logsheet();
         var tService = new TemplateService();
         var cri = new TemplateLogsheetCriteria();
         cri.Template_ID = model.Template_ID;
         cri.include = true;
         var result = tService.GetTempateLogsheet(cri);
         if (result.Code == ReturnCode.SUCCESS)
         {
            var tmps = result.Object as List<Template_Logsheet>;
            if (tmps != null && tmps.Count() == 1)
            {
               tmp = tmps.FirstOrDefault();
               model.Template_Code = tmp.Template_Code;
               model.Template_Name = tmp.Template_Name;
               model.Template_Description = tmp.Template_Description;

               var i = 0;
               var tmpGs = new List<Tmp_Log_Group_Row>();
               foreach (var group in tmp.Tmp_Log_Group)
               {
                  var tmpG = new Tmp_Log_Group_Row();
                  tmpG.Group_ID = group.Group_ID;
                  tmpG.Tmp_Log_Group_ID = group.Tmp_Log_Group_ID;
                  tmpG.Template_ID = group.Template_ID;
                  tmpG.Group_Order = group.Group_Order;
                  tmpG.Sub_Group_Name = group.Sub_Group_Name;
                  tmpG.RowAction = RAction.Edit;
                  var h = 0;
                  var tmphdrs = new List<Tmp_Log_Header_Row>();
                  foreach (var hdr in group.Tmp_Log_Header)
                  {
                     var tmphdr = new Tmp_Log_Header_Row();
                     tmphdr.Tmp_Log_Header_ID = hdr.Tmp_Log_Header_ID;
                     tmphdr.Tmp_Log_Group_ID = hdr.Tmp_Log_Group_ID;
                     tmphdr.Header_ID = hdr.Header_ID;
                     tmphdr.Header_Order = hdr.Header_Order;
                     tmphdr.Sumary_Report_Display = hdr.Sumary_Report_Display.HasValue ? hdr.Sumary_Report_Display.Value : true;
                     tmphdr.RowAction = RAction.Edit;
                     tmphdr.i = i;
                     tmphdr.h = h;

                     var m = 0;
                     var tmpMaps = new List<Tmp_Log_Map_Row>();
                     foreach (var field in hdr.Tmp_Log_Map)
                     {
                        var tmpmap = new Tmp_Log_Map_Row();
                        tmpmap.Tmp_Log_Header_ID = field.Tmp_Log_Header_ID;
                        tmpmap.Tmp_Log_Map_ID = field.Tmp_Log_Map_ID;
                        tmpmap.Field_ID = field.Field_ID;
                        tmpmap.Option_Text = field.Option_Text;
                        tmpmap.Option_Selected = field.Option_Selected;
                        tmpmap.Option_Data_Type = field.Option_Data_Type;
                        tmpmap.Option_Field_From = field.Option_Field_From;
                        tmpmap.Option_Range_From = NumUtil.ParseDecimal(field.Option_Range_From);
                        tmpmap.Option_Range_To = NumUtil.ParseDecimal(field.Option_Range_To);
                        tmpmap.Option_Dropdown_Type = field.Option_Dropdown_Type;
                        tmpmap.Lot_No = field.Lot_No;
                        tmpmap.RowAction = RAction.Edit;
                        tmpmap.i = i;
                        tmpmap.h = h;
                        tmpmap.m = m;
                        tmpMaps.Add(tmpmap);
                        m++;
                     }
                     tmphdr.Tmp_Log_Map_Rows = tmpMaps.ToArray();
                     if (tmphdr.Tmp_Log_Map_Rows == null)
                        tmphdr.Tmp_Log_Map_Rows = (new List<Tmp_Log_Map_Row>()).ToArray();

                     tmphdrs.Add(tmphdr);
                     h++;
                  }
                  tmpG.Tmp_Log_Header_Rows = tmphdrs.ToArray();
                  if (tmpG.Tmp_Log_Header_Rows == null)
                     tmpG.Tmp_Log_Header_Rows = (new List<Tmp_Log_Header_Row>()).ToArray();


                  var f = 0;
                  var tmpfields = new List<Tmp_Log_Field_Row>();
                  foreach (var field in group.Tmp_Log_Field)
                  {
                     var tmpfield = new Tmp_Log_Field_Row();
                     tmpfield.Tmp_Log_Group_ID = field.Tmp_Log_Group_ID;
                     tmpfield.Tmp_Log_Field_ID = field.Tmp_Log_Field_ID;
                     tmpfield.Field_ID = field.Field_ID;
                     tmpfield.Field_Order = field.Field_Order;
                     tmpfield.RowAction = RAction.Edit;
                     tmpfield.i = i;
                     tmpfield.f = f;
                     tmpfields.Add(tmpfield);
                     f++;
                  }
                  tmpG.Tmp_Log_Field_Rows = tmpfields.ToArray();
                  if (tmpG.Tmp_Log_Field_Rows == null)
                     tmpG.Tmp_Log_Field_Rows = (new List<Tmp_Log_Field_Row>()).ToArray();

                  tmpGs.Add(tmpG);
                  i++;
               }
               model.Tmp_Log_Group_Rows = tmpGs.ToArray();
               if (model.Tmp_Log_Group_Rows == null)
                  model.Tmp_Log_Group_Rows = (new List<Tmp_Log_Group_Row>()).ToArray();
            }

         }
         var cbService = new ComboService();
         model.cHeaderlist = cbService.LstTmpHeader();
         model.cFieldlist = cbService.LstTmpField();
         model.cGrouplist = cbService.LstTmpGroup();

         //-----------------------Start Product Template -----------------//
         if (pmodel.Product_Template_ID.HasValue && pmodel.Product_Template_ID.Value > 0)
         {
            var cri2 = new ProductTempateCriteria();
            cri2.Product_Template_ID = pmodel.Product_Template_ID;
            var result2 = tService.GetProductTempate(cri2);
            if (result2.Code == ReturnCode.SUCCESS)
            {
               var prodtmp = new Product_Template();
               var prodtmps = result2.Object as List<Product_Template>;
               if (prodtmps != null && prodtmps.Count() == 1)
               {
                  prodtmp = prodtmps.FirstOrDefault();
                  if (prodtmp != null)
                  {
                     model.Product_Name = prodtmp.Product_Name;
                     model.From_No = prodtmp.From_No;
                     model.Revision = prodtmp.Revision;
                     model.Dilution_Tank_No = prodtmp.Dilution_Tank_No;
                  }
               }
            }
         }
         //-----------------------End Product Template -----------------//

         var htmlToConvert = "";// RenderPartialViewAsString("LogsheetPrint", model);
         Response.Clear();
         Response.ClearContent();
         Response.ClearHeaders();
         Response.ContentType = "application/pdf";
         Response.Charset = Encoding.UTF8.ToString();
         Response.HeaderEncoding = Encoding.UTF8;
         Response.ContentEncoding = Encoding.UTF8;
         Response.Buffer = true;
         StringReader sr = new StringReader(htmlToConvert);
         Document pdfDoc = new Document(PageSize.A4);
         pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());


         HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
         var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
         var action = new PdfAction(PdfAction.PRINTDIALOG);
         writer.SetOpenAction(action);

         var pageevent = new PDFPageEvent();
         pageevent.Report_Code = Page_Code.P0007;
         writer.PageEvent = pageevent;
         pdfDoc.Open();
         htmlparser.Parse(sr);

         var bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
         Font fontH3 = new Font(bf, 9, Font.BOLD);
         Font fontNormal = new Font(bf, 9, Font.NORMAL);
         BaseColor bgColor = iTextSharp.text.html.WebColors.GetRGBColor("#eee");
         Rectangle pageSize = pdfDoc.PageSize;

         string path = HttpContext.Server.MapPath("~/image/agnos_logo.jpeg");

         var imageFile = System.IO.File.ReadAllBytes(path);
         Image img = Image.GetInstance(imageFile);

         PdfPTable hdrtable = new PdfPTable(5);
         hdrtable.TotalWidth = pageSize.Width;
         hdrtable.SetWidthPercentage(new float[] { (float)(hdrtable.TotalWidth * 0.20), (float)(hdrtable.TotalWidth * 0.30), (float)(hdrtable.TotalWidth * 0.20), (float)(hdrtable.TotalWidth * 0.15), (float)(hdrtable.TotalWidth * 0.15) }, pageSize);

         CustomITextSharp.Cell(hdrtable, Resource.Product_Name, fontNormal, PdfPCellBorderType.TopLeft, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, model.Product_Name, fontH3, PdfPCellBorderType.TopCenter, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, Resource.From_No, fontNormal, PdfPCellBorderType.TopCenter, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, model.From_No, fontNormal, PdfPCellBorderType.TopCenter, settextdefault: false);
         CustomITextSharp.CellImage(hdrtable, img, fontNormal, PdfPCellBorderType.FullBorder, 3);

         /*************************************/


         CustomITextSharp.Cell(hdrtable, Resource.Lot_No, fontNormal, PdfPCellBorderType.MidLeft, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, "", fontNormal, PdfPCellBorderType.MidCenter, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, Resource.Revision_No, fontNormal, PdfPCellBorderType.MidCenter, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, model.Revision, fontNormal, PdfPCellBorderType.MidCenter, settextdefault: false);
         /*************************************/
         CustomITextSharp.Cell(hdrtable, Resource.Dilution_Tank_No, fontNormal, PdfPCellBorderType.ButtomLeft, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, model.Dilution_Tank_No, fontNormal, PdfPCellBorderType.ButtomCenter, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, "", fontNormal, PdfPCellBorderType.ButtomCenter, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, "", fontNormal, PdfPCellBorderType.ButtomCenter, settextdefault: false);
         pdfDoc.Add(hdrtable);
         pdfDoc.Add(new Paragraph("\n"));

         if (model.Tmp_Log_Group_Rows != null && model.Tmp_Log_Group_Rows != null)
         {
            foreach (var tmpG in model.Tmp_Log_Group_Rows)
            {
               var group = model.cGrouplist.Where(w => w.Value == tmpG.Group_ID.Value.ToString()).FirstOrDefault();
               if (tmpG.Tmp_Log_Header_Rows != null && tmpG.Tmp_Log_Field_Rows != null && tmpG.Tmp_Log_Header_Rows.Count() > 0 && tmpG.Tmp_Log_Field_Rows.Count() > 0)
               {
                  {
                     PdfPTable table = new PdfPTable(1);
                     table.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                     table.TotalWidth = pageSize.Width;
                     table.SetWidthPercentage(new float[] { (float)(table.TotalWidth * 1) }, pageSize);

                     CustomITextSharp.Cell(table, group.Text, fontH3, PdfPCellBorderType.TopRight);
                     pdfDoc.Add(table);
                  }

                  List<float> witdthpercens = new List<float>();
                  if (tmpG.Tmp_Log_Field_Rows != null && tmpG.Tmp_Log_Field_Rows.Count() > 0 && tmpG.Tmp_Log_Header_Rows != null && tmpG.Tmp_Log_Header_Rows.Count() > 0)
                  {
                     if (tmpG.Tmp_Log_Header_Rows != null && tmpG.Tmp_Log_Header_Rows.Count() > 0)
                     {
                        {

                           PdfPTable table = new PdfPTable(tmpG.Tmp_Log_Header_Rows.Length + 1);
                           table.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                           table.TotalWidth = pageSize.Width;

                           float colwidth = (float)table.TotalWidth * (1f / (tmpG.Tmp_Log_Header_Rows.Length + 1));
                           for (var i = 0; i < tmpG.Tmp_Log_Header_Rows.Length + 1; i++)
                           {
                              witdthpercens.Add(colwidth);
                           }
                           table.SetWidthPercentage(witdthpercens.ToArray(), pageSize);

                           var hcnt = 1;
                           CustomITextSharp.Cell(table, tmpG.Sub_Group_Name, fontNormal, PdfPCellBorderType.TopLeft);
                           foreach (var tmphdr in tmpG.Tmp_Log_Header_Rows.OrderBy(o => o.Header_Order))
                           {
                              var hdr = model.cHeaderlist.Where(w => w.Value == tmphdr.Header_ID.Value.ToString()).FirstOrDefault();
                              if (hcnt < tmpG.Tmp_Log_Header_Rows.Length)
                                 CustomITextSharp.Cell(table, hdr.Text, fontNormal, PdfPCellBorderType.TopCenter);
                              else
                                 CustomITextSharp.Cell(table, hdr.Text, fontNormal, PdfPCellBorderType.TopRight);
                              hcnt++;
                           }
                           pdfDoc.Add(table);
                        }

                     }
                     if (tmpG.Tmp_Log_Field_Rows != null && tmpG.Tmp_Log_Field_Rows.Count() > 0)
                     {
                        {
                           PdfPTable table = new PdfPTable(tmpG.Tmp_Log_Header_Rows.Length + 1);
                           table.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                           table.TotalWidth = pageSize.Width;
                           table.SetWidthPercentage(witdthpercens.ToArray(), pageSize);
                           foreach (var tmpfield in tmpG.Tmp_Log_Field_Rows.OrderBy(o => o.Field_Order))
                           {
                              var field = model.cFieldlist.Where(w => w.Value == tmpfield.Field_ID.Value.ToString()).FirstOrDefault();
                              var hcnt = 1;
                              CustomITextSharp.Cell(table, field.Text, fontNormal, PdfPCellBorderType.MidLeft);
                              foreach (var tmphdr in tmpG.Tmp_Log_Header_Rows.OrderBy(o => o.Header_Order))
                              {
                                 var textdisplay = "-";
                                 var tmpmap = tmphdr.Tmp_Log_Map_Rows.Where(w => w.Field_ID == tmpfield.Field_ID).FirstOrDefault();
                                 if (tmpmap != null)
                                 {
                                    if (tmpmap.Option_Selected == Logsheet_Control_Type.Text)
                                    {
                                       textdisplay = tmpmap.Option_Text;
                                    }
                                    else
                                    {
                                       textdisplay = "-";
                                    }
                                 }

                                 if (hcnt < tmpG.Tmp_Log_Header_Rows.Length)
                                    CustomITextSharp.Cell(table, textdisplay, fontNormal, PdfPCellBorderType.MidCenter);
                                 else
                                    CustomITextSharp.Cell(table, textdisplay, fontNormal, PdfPCellBorderType.MidRight);
                                 hcnt++;
                              }
                           }
                           pdfDoc.Add(table);
                        }


                     }
                  }
               }
            }
         }

         /* footer*/
         {
            PdfPTable table = new PdfPTable(4);
            table.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            table.TotalWidth = pageSize.Width;
            table.SetWidthPercentage(new float[] { (float)(table.TotalWidth * 0.4), (float)(table.TotalWidth * 0.2), (float)(table.TotalWidth * 0.2), (float)(table.TotalWidth * 0.2) }, pageSize);

            CustomITextSharp.Cell(table, "Notes", fontNormal, PdfPCellBorderType.ButtomLeft, 3);
            CustomITextSharp.Cell(table, "", fontNormal, PdfPCellBorderType.TopCenter);
            CustomITextSharp.Cell(table, "", fontNormal, PdfPCellBorderType.TopCenter);
            CustomITextSharp.Cell(table, "", fontNormal, PdfPCellBorderType.TopRight);

            CustomITextSharp.Cell(table, "", fontNormal, PdfPCellBorderType.MidCenter);
            CustomITextSharp.Cell(table, "", fontNormal, PdfPCellBorderType.MidCenter);
            CustomITextSharp.Cell(table, "", fontNormal, PdfPCellBorderType.MidRight);

            CustomITextSharp.Cell(table, Resource.PD_Issue, fontH3, PdfPCellBorderType.ButtomCenter);
            CustomITextSharp.Cell(table, Resource.PD_Approval, fontH3, PdfPCellBorderType.ButtomCenter);
            CustomITextSharp.Cell(table, Resource.QA_Approval, fontH3, PdfPCellBorderType.ButtomRight);
            pdfDoc.Add(table);
         }


         pdfDoc.Close();
         Response.End();
         return View(model);
      }

      [HttpPost]
      public ActionResult TemplateLogsheet(TemplateLogsheetViewModels model)
      {
         var tService = new TemplateService();
         if (!string.IsNullOrEmpty(model.Template_Code))
         {
            var dupcri = new TemplateLogsheetCriteria();
            dupcri.Template_Code = model.Template_Code;
            var dupresult = tService.GetTempateLogsheet(dupcri);
            if (dupresult.Code == ReturnCode.SUCCESS)
            {
               var dup = new Template_Logsheet();
               var dups = dupresult.Object as List<Template_Logsheet>;
               if (dups != null && dups.Count() != 0)
                  ModelState.AddModelError("Template_Code", Resource.The + " " + Resource.Template_Code + " " + Resource.Field + " " + Resource.Is_Duplicated);
            }
         }
         if (!string.IsNullOrEmpty(model.Template_Name))
         {
            var dupcri = new TemplateLogsheetCriteria();
            dupcri.Template_Name = model.Template_Name;
            var dupresult = tService.GetTempateLogsheet(dupcri);
            if (dupresult.Code == ReturnCode.SUCCESS)
            {
               var dup = new Template_Logsheet();
               var dups = dupresult.Object as List<Template_Logsheet>;
               if (dups != null && dups.Count() != 0)
                  ModelState.AddModelError("Template_Name", Resource.The + " " + Resource.Template_Name + " " + Resource.Field + " " + Resource.Is_Duplicated);
            }
         }

         if (ModelState.IsValid)
         {
            if (model.operation == Operation.U)
            {
               var cri = new TemplateLogsheetCriteria();
               cri.Template_ID = model.Template_ID;
               cri.include = true;
               var result = tService.GetTempateLogsheet(cri);
               if (result.Code == ReturnCode.SUCCESS)
               {
                  var tmp = new Template_Logsheet();
                  var tmps = result.Object as List<Template_Logsheet>;
                  if (tmps != null && tmps.Count() == 1)
                     tmp = tmps.FirstOrDefault();

                  var clone = new Template_Logsheet();
                  clone.Template_Name = model.Template_Name;
                  clone.Template_Code = model.Template_Code;
                  clone.Template_Description = tmp.Template_Description;
                  var cloneGroups = new List<Tmp_Log_Group>();
                  if (tmp.Tmp_Log_Group != null)
                  {
                     foreach (var group in tmp.Tmp_Log_Group)
                     {
                        var cloneGroup = new Tmp_Log_Group();
                        cloneGroup.Group_ID = group.Group_ID;
                        cloneGroup.Group_Order = group.Group_Order;

                        var cloneHeaders = new List<Tmp_Log_Header>();
                        foreach (var header in group.Tmp_Log_Header)
                        {
                           var cloneHeader = new Tmp_Log_Header();
                           cloneHeader.Header_ID = header.Header_ID;
                           cloneHeader.Header_Order = header.Header_Order;
                           cloneHeader.Sumary_Report_Display = header.Sumary_Report_Display;
                           cloneHeaders.Add(cloneHeader);
                           var cloneMaps = new List<Tmp_Log_Map>();
                           foreach (var map in header.Tmp_Log_Map)
                           {
                              var cloneMap = new Tmp_Log_Map();
                              cloneMap.Field_ID = map.Field_ID;
                              cloneMap.Header_ID = map.Header_ID;
                              cloneMap.Option_Text = map.Option_Text;
                              cloneMap.Option_Selected = map.Option_Selected;
                              cloneMap.Option_Data_Type = map.Option_Data_Type;
                              cloneMap.Option_Field_From = map.Option_Field_From;
                              cloneMap.Option_Range_From = map.Option_Range_From;
                              cloneMap.Option_Range_To = map.Option_Range_To;
                              cloneMap.Option_Dropdown_Type = map.Option_Dropdown_Type;
                              cloneMap.Lot_No = map.Lot_No;
                              cloneMaps.Add(cloneMap);
                           }
                           cloneHeader.Tmp_Log_Map = cloneMaps;
                        }
                        cloneGroup.Tmp_Log_Header = cloneHeaders;
                        var cloneFileds = new List<Tmp_Log_Field>();
                        foreach (var field in group.Tmp_Log_Field)
                        {
                           var cloneField = new Tmp_Log_Field();
                           cloneField.Field_ID = field.Field_ID;
                           cloneField.Field_Order = field.Field_Order;
                           cloneFileds.Add(cloneField);
                        }
                        cloneGroup.Tmp_Log_Field = cloneFileds;
                        cloneGroups.Add(cloneGroup);
                     }

                  }
                  clone.Tmp_Log_Group = cloneGroups;

                  model.result = tService.InsertTempateLogsheet(clone);
                  if (model.result.Code == ReturnCode.SUCCESS)
                     return RedirectToAction("TemplateLogsheet", new AppRouteValueDictionary(model));
               }
            }
         }
         var cri2 = new TemplateLogsheetCriteria();
         cri2.Template_Code = model.search_Template_Code;
         cri2.Template_Name = model.search_Template_Name;
         var tresult = tService.GetTempateLogsheet(cri2);
         if (tresult.Code == ReturnCode.SUCCESS)
            model.Tmplogsheetlist = tresult.Object as List<Template_Logsheet>;
         return View(model);
      }
      #endregion

      #region Product assign template
      [HttpGet]
      public ActionResult ProductTemplate(ProductTemplateViewModels model, ServiceResult msgresult)
      {
         var uService = new UserService();
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0006);
         if (prole == null)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
         if (prole.View == null || prole.View == false)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

         ModelState.Clear();
         if (model.operation == Operation.D)
            return ProductTemplate(model);

         model.result = msgresult; // return result from http post
         model.Modify = prole.Modify;
         model.View = prole.View;

         var tService = new TemplateService();
         var cri = new ProductTempateCriteria();

         cri.Template_ID = model.Template_ID;
         cri.Product_Code = model.Product_Code;
         var ptresult = tService.GetProductTempate(cri);
         if (ptresult.Code == ReturnCode.SUCCESS)
            model.ProductTemplateList = ptresult.Object as List<Product_Template>;

         var exService = new ExchequerService();
         var cbService = new ComboService();
         model.cTmplist = cbService.LstTemplate();
         model.cProductlist = exService.LstExchquerProduct();

         return View(model);
      }

      [HttpPost]
      public ActionResult ProductTemplate(ProductTemplateViewModels model)
      {
         var tService = new TemplateService(User.Identity.GetUserId());
         if (!string.IsNullOrEmpty(model.Product_Code))
         {
            var dupcri = new ProductTempateCriteria();
            dupcri.Product_Code = model.Product_Code;
            var dupresult = tService.GetProductTempate(dupcri);
            if (dupresult.Code == ReturnCode.SUCCESS)
            {
               var dup = new Product_Template();
               var dups = dupresult.Object as List<Product_Template>;
               if (dups != null && dups.Count() != 0)
               {
                  if (model.operation == Operation.C)
                  {
                     if (dups.Where(w => w.Record_Status != Record_Status.Delete).Select(w => w.Template_ID).Contains(model.Template_ID))
                        ModelState.AddModelError("Template_ID", Resource.The + " " + Resource.Template_Name + " " + Resource.Field + " " + Resource.Is_Duplicated);
                  }
                  else if (model.operation == Operation.U)
                  {
                     dups = dups.Where(w => w.Product_Template_ID != model.Product_Template_ID && w.Template_ID == model.Template_ID & w.Record_Status != Record_Status.Delete).ToList();
                     if (dups == null || dups.Count() > 0)
                        ModelState.AddModelError("Template_ID", Resource.The + " " + Resource.Template_Name + " " + Resource.Field + " " + Resource.Is_Duplicated);
                  }
               }
            }
         }

         if (ModelState.IsValid)
         {
            var prodtmp = new Product_Template();
            if (model.operation == Operation.U || model.operation == Operation.D)
            {
               var cri = new ProductTempateCriteria();
               cri.Product_Template_ID = model.Product_Template_ID;
               var result = tService.GetProductTempate(cri);
               if (result.Code == ReturnCode.SUCCESS)
               {
                  var prodtmps = result.Object as List<Product_Template>;
                  if (prodtmps != null && prodtmps.Count() == 1)
                     prodtmp = prodtmps.FirstOrDefault();
               }
            }

            if (model.operation != Operation.D)
            {
               prodtmp.Product_Code = model.Product_Code;
               prodtmp.Template_ID = model.Template_ID;
               prodtmp.Product_Name = model.Product_Name;
               prodtmp.From_No = model.From_No;
               prodtmp.Revision = model.Revision;
               prodtmp.Dilution_Tank_No = model.Dilution_Tank_No;
            }

            if (model.operation == Operation.C)
               model.result = tService.InsertProductTempate(prodtmp);

            else if (model.operation == Operation.U)
               model.result = tService.UpdateProductTempate(prodtmp);

            else if (model.operation == Operation.D)
            {
               prodtmp.Record_Status = Record_Status.Delete;
               model.result = tService.UpdateProductTempate(prodtmp);
               if (model.result.Code == ReturnCode.SUCCESS)
                  model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Assign_Template_To_Product };
               else
                  model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Assign_Template_To_Product };

               return RedirectToAction("ProductTemplate", new AppRouteValueDictionary(model));
            }

            if (model.result.Code == ReturnCode.SUCCESS)
               return RedirectToAction("ProductTemplate", new AppRouteValueDictionary(model));

         }

         var cbService = new ComboService();
         var exService = new ExchequerService();
         var uService = new UserService();
         var cri2 = new ProductTempateCriteria();

         var ptresult = tService.GetProductTempate(cri2);
         if (ptresult.Code == ReturnCode.SUCCESS)
            model.ProductTemplateList = ptresult.Object as List<Product_Template>;

         model.cTmplist = cbService.LstTemplate();
         model.cProductlist = exService.LstExchquerProduct();
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0006);
         if (prole != null)
            model.Modify = prole.Modify;

         return View(model);
      }

      public void ProductTemplatePrint(int? pProductTemplateID, int? pTemplateID)
      {
         if (pProductTemplateID.HasValue && pTemplateID.HasValue)
         {
            var model = new TemplateLogsheetInfoViewModels();
            model.Template_ID = pTemplateID;
            model.Product_Template_ID = pProductTemplateID;
            TemplateLogsheetPrint(model);
         }
      }
      #endregion

      #region Logsheet
      [HttpGet]
      public ActionResult Logsheet(LogsheetViewModels model, ServiceResult msgresult)
      {
         var uService = new UserService();
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0007);
         if (prole == null)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
         if (prole.View == null || prole.View == false)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

         ModelState.Clear();
         model.result = msgresult; // return result from http post
         model.Modify = prole.Modify;
         model.View = prole.View;

         var tService = new TemplateService();

         var cri = new LogsheetCriteria();
         cri.Template_ID = model.Template_ID;
         cri.Lot_No = model.Lot_No;
         cri.Product_Code = model.Product_Code;
         cri.Template_Code = model.Template_Code;
         cri.Logsheet_Status = model.Logsheet_Status;

         var ptresult = tService.GetLogsheet(cri);
         if (ptresult.Code == ReturnCode.SUCCESS)
            model.LogsheetList = ptresult.Object as List<Logsheet>;

         var cbService = new ComboService();
         var exService = new ExchequerService();
         model.cTmplist = cbService.LstTemplate(true);
         model.cTemplateCodelist = cbService.LstTemplateCode(true);
         //model.cProductlist = exService.LstExchquerProduct(true);
         model.cLogsheetStatuslist = cbService.LstLogsheetStatus(true);

         return View(model);
      }

      [HttpGet]
      public ActionResult LogsheetInfo(LogsheetViewModels pmodel, string pPrint)
      {
         var uService = new UserService();
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0007);
         if (prole == null)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
         if (prole.View == null || prole.View == false)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

         ModelState.Clear();
         var model = new LogsheetInfoViewModels();
         model.operation = pmodel.operation;
         model.Logsheet_ID = pmodel.Logsheet_ID;
         model.Modify = prole.Modify;
         model.View = prole.View;
         model.Print = pPrint;

         var tService = new TemplateService(User.Identity.GetUserId());
         var cbService = new ComboService();
         var exService = new ExchequerService();

         model.Product_Code = pmodel.Product_Code;
         model.cStatuslist = cbService.LstStatus();      
         model.cWorkOrderNolist = exService.LstExchquerWorkOrder();
         model.cDisposelist = cbService.LstDispose();
         model.Role_Name = prole.Role.Role_Name;
         model.cPackaginglist = cbService.LstLookupData(LookupType.Packaging, true);
         model.cUOMlist = cbService.LstLookupData(LookupType.UOM, true);
         model.Attachment_Files = new List<UploadAttachmentFile>();
         if (model.operation == Operation.C)
         {
            model.cProductlist = exService.LstExchquerProduct();
            if (string.IsNullOrEmpty(pmodel.Product_Code) && model.cProductlist.Count() > 0)
               pmodel.Product_Code = model.cProductlist[0].Value;

            if (!string.IsNullOrEmpty(pmodel.Product_Code))
            {
               model.cTmplist = cbService.LstTemplateByProductTemplate(false, pmodel.Product_Code);
               if (!pmodel.Template_ID.HasValue && model.cTmplist.Count() > 0)
                  pmodel.Template_ID = Convert.ToInt32(model.cTmplist[0].Value);

               var cri = new ProductTempateCriteria();
               cri.Product_Code = pmodel.Product_Code;
               cri.Template_ID = pmodel.Template_ID;
               cri.include = true;
               var result = tService.GetProductTempate(cri);
               if (result.Code == ReturnCode.SUCCESS)
               {
                  var protemps = result.Object as List<Product_Template>;
                  if (protemps != null && protemps.Count() == 1)
                  {
                     var protmp = protemps.FirstOrDefault();
                     model.Template_ID = protmp.Template_ID;
                     model.Tmp = protmp.Template_Logsheet;
                     model.Product_Code = protmp.Product_Code;
                     model.Product_Name = protmp.Product_Name;
                     model.Form_No = protmp.From_No;
                     model.Dilution_Tank_No = protmp.Dilution_Tank_No;
                     model.Revision_No = NumUtil.ParseInteger(protmp.Revision);
                  }
               }
            }
         }
         else if (model.operation == Operation.U)
         {
            var cri = new LogsheetCriteria();
            cri.Logsheet_ID = model.Logsheet_ID;
            cri.include = true;
            var result = tService.GetLogsheet(cri);
            if (result.Code == ReturnCode.SUCCESS)
            {
               var logs = result.Object as List<Logsheet>;
               if (logs != null && logs.Count() == 1)
               {
                  var log = logs.FirstOrDefault();
                  model.Logsheet_ID = log.Logsheet_ID;
                  model.Product_Code = log.Product_Code;
                  model.Product_Name = log.Product_Name;
                  model.Form_No = log.Form_No;
                  model.Lot_No = log.Lot_No;
                  model.Quantity = log.Quantity;
                  model.Expiry_Date = DateUtil.ToDisplayDate(log.Expiry_Date);
                  model.UAI = log.UAI;
                  model.RTS = log.RTS;
                  model.Rework = log.Rework;
                  model.Scrap = log.Scrap;
                  model.RMR_No = log.RMR_No;
                  model.Reason_If_Reject = log.Reason_If_Reject;
                  model.Authorized_By = log.Authorized_By;
                  model.Dispose = log.Dispose;
                  model.Status = log.Status;
                  model.Authorized_Date = DateUtil.ToDisplayDate(log.Authorized_Date);
                  model.Tmp = log.Template_Logsheet;
                  model.Work_Order_No = log.Work_Order_No;
                  model.Lotgsheet_Status = log.Lotgsheet_Status;
                  model.Dilution_Tank_No = log.Dilution_Tank_No;
                  model.Remarks = log.Remarks;
                  model.PD_Issue = log.PD_Issue;
                  model.PD_Issue_Date = DateUtil.ToDisplayDate(log.PD_Issue_Date);
                  if (model.PD_Issue.HasValue)
                     model.PD_Issue_Name = log.User_Profile.Name;
                  model.PD_Approval = log.PD_Approval;
                  model.PD_Approval_Date = DateUtil.ToDisplayDate(log.PD_Approval_Date);
                  if (model.PD_Approval.HasValue)
                     model.PD_Approval_Name = log.User_Profile1.Name;

                  model.QA_Approval = log.QA_Approval;
                  model.QA_Approval_Date = DateUtil.ToDisplayDate(log.QA_Approval_Date);

                  if (model.QA_Approval.HasValue)
                     model.QA_Approval_Name = log.User_Profile2.Name;

                  model.Display_Product_Form_Field = log.Display_Product_Form_Field;
                  model.Revision_No = log.Revision_No;
                  model.UOM = log.UOM;
                  model.Note = log.Note;
                  model.Product_Status = log.Product_Status;
                  if (log.UOM.HasValue)
                     model.UOM_Name = log.Global_Lookup_Data.Name;

                  model.Packaging = log.Packaging;
                  if (log.Packaging.HasValue)
                     model.Packaging_Name = log.Global_Lookup_Data1.Name;

                  model.cTmplist = cbService.LstTemplateByProductTemplate(false, log.Product_Code);

                  var tmpID = log.Template_ID.HasValue ? log.Template_ID.Value.ToString() : "";
                  if (!model.cTmplist.Select(s => s.Value).Contains(tmpID))
                  {
                     if (!pmodel.Template_ID.HasValue && model.cTmplist.Count() > 0)
                        pmodel.Template_ID = Convert.ToInt32(model.cTmplist[0].Value);

                     var crire = new ProductTempateCriteria();
                     crire.Product_Code = log.Product_Code;
                     crire.Template_ID = pmodel.Template_ID;
                     crire.include = true;
                     var result2 = tService.GetProductTempate(crire);
                     if (result2.Code == ReturnCode.SUCCESS)
                     {
                        var protemps = result2.Object as List<Product_Template>;
                        if (protemps != null && protemps.Count() == 1)
                        {
                           var protmp = protemps.FirstOrDefault();
                           model.Template_ID = protmp.Template_ID;
                           model.Tmp = protmp.Template_Logsheet;
                           model.Product_Code = log.Product_Code;
                        }
                     }
                  }
                  else
                  {
                     model.Template_ID = log.Template_ID;
                     var groups = new List<Logsheet_Group_Row>();
                     foreach (var grow in log.Logsheet_Group)
                     {
                        var group = new Logsheet_Group_Row();
                        group.Logsheet_Group_ID = grow.Logsheet_Group_ID;
                        group.Logsheet_ID = grow.Logsheet_ID;
                        group.Group_ID = grow.Group_ID;

                        var hdrs = new List<Logsheet_Header_Row>();
                        foreach (var hrow in grow.Logsheet_Header)
                        {
                           var hdr = new Logsheet_Header_Row();
                           hdr.Logsheet_Header_ID = hrow.Logsheet_Header_ID;
                           hdr.Logsheet_Group_ID = hrow.Logsheet_Group_ID;
                           hdr.Header_ID = hrow.Header_ID;

                           var maps = new List<Logsheet_Map_Row>();
                           foreach (var mrow in hrow.Logsheet_Map)
                           {
                              var map = new Logsheet_Map_Row();
                              map.Logsheet_Map_ID = mrow.Logsheet_Map_ID;
                              map.Logsheet_Header_ID = mrow.Logsheet_Header_ID;
                              map.Header_ID = mrow.Header_ID;
                              map.Field_ID = mrow.Field_ID;
                              map.Option_Selected = mrow.Option_Selected;
                              map.Text_Display = mrow.Text_Display;
                              maps.Add(map);
                           }
                           hdr.Logsheet_Map_Rows = maps.ToArray();
                           hdrs.Add(hdr);
                        }

                        var fields = new List<Logsheet_Field_Row>();
                        foreach (var frow in grow.Logsheet_Field)
                        {
                           var field = new Logsheet_Field_Row();
                           field.Logsheet_Field_ID = frow.Logsheet_Field_ID;
                           field.Logsheet_Group_ID = frow.Logsheet_Group_ID;
                           field.Field_ID = frow.Field_ID;
                           fields.Add(field);
                        }
                        group.Logsheet_Field_Rows = fields.ToArray();
                        group.Logsheet_Header_Rows = hdrs.ToArray();
                        groups.Add(group);
                     }
                     model.Logsheet_Group_Rows = groups.ToArray();
                  }

                  if (log.Upload_Attachment != null)
                  {
                     foreach (var file in log.Upload_Attachment)
                     {
                        if (file.Record_Status != Record_Status.Delete)
                        {
                           model.Attachment_Files.Add(new UploadAttachmentFile()
                           {
                              Attachment_File_Name = file.Attachment_File_Name,
                              Attachment_File = file.Attachment_File,
                              Attachment_ID = file.Attachment_ID
                           });
                        }
                     }
                  }
               }
            }
         }
         else if (model.operation == Operation.D)
         {
            var cri2 = new LogsheetCriteria();
            cri2.Logsheet_ID = model.Logsheet_ID;
            cri2.include = true;
            var result = tService.GetLogsheet(cri2);
            if (result.Code == ReturnCode.SUCCESS)
            {
               var log = new Logsheet();
               var logs = result.Object as List<Logsheet>;
               if (logs != null && logs.Count() == 1)
                  log = logs.FirstOrDefault();

               log.Record_Status = Record_Status.Delete;
               model.result = tService.UpdateLogsheet(log);
               if (model.result.Code == ReturnCode.SUCCESS)
                  model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Logsheet };
               else
                  model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Logsheet };
            }
            return RedirectToAction("Logsheet", new AppRouteValueDictionary(model));
         }
         //model.cTmplist = cbService.LstTemplate();
         model.cLotNumber = cbService.LstLotNumber(model.Product_Code);

         return View(model);
      }

      [HttpPost]
      public ActionResult LogsheetInfo(LogsheetInfoViewModels model, string savemode)
      {
         var currentdate = StoredProcedure.GetCurrentDate();
         var uService = new UserService();
         var tService = new TemplateService(User.Identity.GetUserId());
         var userlogin = uService.getUser(User.Identity.GetUserId());
         if (ModelState.IsValid)
         {
            var log = new Logsheet();
            if (model.operation == Operation.C)
            {
            }
            else if (model.operation == Operation.U)
            {
               var cri = new LogsheetCriteria();
               cri.Logsheet_ID = model.Logsheet_ID;
               cri.include = true;
               var result = tService.GetLogsheet(cri);
               if (result.Code == ReturnCode.SUCCESS)
               {
                  var logs = result.Object as List<Logsheet>;
                  if (logs != null && logs.Count() == 1)
                     log = logs.FirstOrDefault();
               }
            }

            log.Template_ID = model.Template_ID;
            log.Product_Code = model.Product_Code;
            log.Product_Name = model.Product_Name;
            log.Form_No = model.Form_No;
            log.Lot_No = model.Lot_No;
            log.Work_Order_No = model.Work_Order_No;
            log.Packaging = model.Packaging;
            log.UOM = model.UOM;
            log.Quantity = model.Quantity;
            log.Expiry_Date = DateUtil.ToDate(model.Expiry_Date);
            log.UAI = model.UAI;
            log.RTS = model.RTS;
            log.Rework = model.Rework;
            log.Scrap = model.Scrap;

            log.RMR_No = model.RMR_No;
            log.Reason_If_Reject = model.Reason_If_Reject;
            log.Authorized_By = model.Authorized_By;
            log.Dispose = model.Dispose;
            log.Status = model.Status;
            log.Authorized_Date = DateUtil.ToDate(model.Authorized_Date);
            log.Dilution_Tank_No = model.Dilution_Tank_No;
            log.Remarks = model.Remarks;
            log.Display_Product_Form_Field = model.Display_Product_Form_Field;
            log.Revision_No = model.Revision_No;
            log.Note = model.Note;
            log.Product_Status = model.Product_Status;
            log.PD_Approval_Date = DateUtil.ToDate(model.PD_Approval_Date);
            log.PD_Issue_Date = DateUtil.ToDate(model.PD_Issue_Date);
            log.QA_Approval_Date = DateUtil.ToDate(model.QA_Approval_Date);
            if (model.Logsheet_Group_Rows != null)
            {
               var groups = new List<Logsheet_Group>();
               foreach (var grow in model.Logsheet_Group_Rows)
               {
                  var group = new Logsheet_Group();
                  group.Logsheet_Group_ID = grow.Logsheet_Group_ID.HasValue ? grow.Logsheet_Group_ID.Value : 0;
                  group.Group_ID = grow.Group_ID;
                  group.Logsheet_ID = model.Logsheet_ID;

                  group.Update_On = log.Update_On;
                  group.Create_On = log.Create_On;
                  group.Logsheet_Header = new List<Logsheet_Header>();
                  if (grow.Logsheet_Header_Rows != null)
                  {
                     foreach (var hdrow in grow.Logsheet_Header_Rows)
                     {
                        var hdr = new Logsheet_Header();
                        hdr.Logsheet_Header_ID = hdrow.Logsheet_Header_ID.HasValue ? hdrow.Logsheet_Header_ID.Value : 0;
                        hdr.Header_ID = hdrow.Header_ID;
                        hdr.Logsheet_Group_ID = hdrow.Logsheet_Group_ID;
                        hdr.Logsheet_Map = new List<Logsheet_Map>();
                        if (hdrow.Logsheet_Map_Rows != null)
                        {
                           foreach (var mrow in hdrow.Logsheet_Map_Rows)
                           {
                              var map = new Logsheet_Map();
                              map.Logsheet_Map_ID = mrow.Logsheet_Map_ID.HasValue ? mrow.Logsheet_Map_ID.Value : 0;
                              map.Logsheet_Header_ID = mrow.Logsheet_Header_ID;
                              map.Field_ID = mrow.Field_ID;
                              map.Header_ID = mrow.Header_ID;
                              map.Option_Selected = mrow.Option_Selected;
                              map.Text_Display = mrow.Text_Display;
                              hdr.Logsheet_Map.Add(map);
                           }
                        }
                        group.Logsheet_Header.Add(hdr);
                     }
                  }
                  if (grow.Logsheet_Field_Rows != null)
                  {
                     foreach (var frow in grow.Logsheet_Field_Rows)
                     {
                        var field = new Logsheet_Field();
                        field.Logsheet_Field_ID = frow.Logsheet_Field_ID.HasValue ? frow.Logsheet_Field_ID.Value : 0;
                        field.Field_ID = frow.Field_ID;
                        group.Logsheet_Field.Add(field);
                     }
                  }
                  groups.Add(group);
               }
               log.Logsheet_Group = groups;
            }

            var gService = new GlobalLookupService();
            var ucri = new GlobalLookupCriteria();
            ucri.Lookup_Data_ID = model.UOM;
            var uresult = gService.GetGlobalLookupData(ucri);
            if (uresult.Code == ReturnCode.SUCCESS)
            {
               var uoms = uresult.Object as List<Global_Lookup_Data>;
               if (uoms != null && uoms.Count() == 1)
               {
                  var uom = uoms.FirstOrDefault();
                  if (uom != null)
                     model.UOM_Name = uom.Name;
               }
            }

            ucri.Lookup_Data_ID = model.Packaging;
            uresult = gService.GetGlobalLookupData(ucri);
            if (uresult.Code == ReturnCode.SUCCESS)
            {
               var packs = uresult.Object as List<Global_Lookup_Data>;
               if (packs != null && packs.Count() == 1)
               {
                  var pack = packs.FirstOrDefault();
                  if (pack != null)
                     model.Packaging_Name = pack.Name;
               }
            }
           

            if (model.operation == Operation.C)
            {
               using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
               {
                  var htmlToConvert = RenderPartialViewAsString("LogsheetProductStatusForm2017", model);
                  StringReader sr = new StringReader(htmlToConvert);
                  Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                  HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                  var writer = PdfWriter.GetInstance(pdfDoc, ms);
                  var pageevent = new PDFPageEvent();
                  writer.PageEvent = pageevent;
                  pdfDoc.Open();
                  htmlparser.Parse(sr);
                  pdfDoc.Close();

                  var form = new Logsheet_Product_Form();
                  form.Create_By = userlogin.Email_Address;
                  form.Create_On = currentdate;
                  form.File_Name = "LOGSPRD" + log.Lot_No + ".pdf";
                  form.Form_ID = Guid.NewGuid();
                  form.File = ms.ToArray();
                  log.Logsheet_Product_Form.Add(form);
               }

               model.result = tService.InsertLogsheet(log);
               if (model.result.Code == ReturnCode.SUCCESS)
               {
                  if (savemode == "printform")
                  {
                     var route = new AppRouteValueDictionary(model);
                     route.Add("Logsheet_ID", log.Logsheet_ID);
                     route.Add("pPrint", savemode);
                     route.Add("operation", Operation.U);
                     return RedirectToAction("LogsheetInfo", route);
                  }
                  else if (savemode == "printproductform")
                  {
                     var route = new AppRouteValueDictionary(model);
                     route.Add("Logsheet_ID", log.Logsheet_ID);
                     route.Add("pPrint", savemode);
                     route.Add("operation", Operation.U);
                     return RedirectToAction("LogsheetInfo", route);
                  }
                  else
                     return RedirectToAction("Logsheet", new AppRouteValueDictionary(model));
               }
            }
            else if (model.operation == Operation.U)
            {
               var emailstatus = String.Empty;
               var sendemail = false;
               if (savemode == "pdissue")
               {
                  if (model.Role_Name == "PD")
                  {
                     if (!log.PD_Issue.HasValue)
                        sendemail = true;

                     log.Lotgsheet_Status = Logsheet_Approval_Status.PD_Issue;
                     log.PD_Issue = userlogin.Profile_ID;
                     log.PD_Issue_Date = currentdate;

                     if (!string.IsNullOrEmpty(model.PD_Issue_Date))
                        log.PD_Issue_Date = DateUtil.ToDate(model.PD_Issue_Date);

                     emailstatus = log.Lotgsheet_Status;

                  }
               }
               else if (savemode == "pd")
               {
                  if (model.Role_Name == "PD")
                  {
                     if (!log.PD_Approval.HasValue)
                        sendemail = true;

                     log.Lotgsheet_Status = Logsheet_Approval_Status.PD_Approval;
                     log.PD_Approval = userlogin.Profile_ID;
                     log.PD_Approval_Date = currentdate;

                     if (!string.IsNullOrEmpty(model.PD_Approval_Date))
                        log.PD_Approval_Date = DateUtil.ToDate(model.PD_Approval_Date);

                     emailstatus = log.Lotgsheet_Status;
                  }
               }
               else if (savemode == "qa")
               {
                  if (model.Role_Name == "QA")
                  {
                     if (!log.QA_Approval.HasValue)
                        sendemail = true;

                     log.Lotgsheet_Status = Logsheet_Approval_Status.QA_Approval;
                     log.QA_Approval = userlogin.Profile_ID;
                     log.QA_Approval_Date = currentdate;

                     if (!string.IsNullOrEmpty(model.QA_Approval_Date))
                        log.QA_Approval_Date = DateUtil.ToDate(model.QA_Approval_Date);

                     emailstatus = log.Lotgsheet_Status;
                  }
               }
               Logsheet_Product_Form form = null;
               using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
               {
                 


                  var htmlToConvert = RenderPartialViewAsString("LogsheetProductStatusForm2017", model);
                  StringReader sr = new StringReader(htmlToConvert);
                  Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                  HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                  var writer = PdfWriter.GetInstance(pdfDoc, ms);
                  var pageevent = new PDFPageEvent();
                  writer.PageEvent = pageevent;
                  pdfDoc.Open();
                  htmlparser.Parse(sr);
                  pdfDoc.Close();

                  form = new Logsheet_Product_Form();
                  form.Create_By = userlogin.Email_Address;
                  form.Create_On = currentdate;
                  form.File_Name = "LOGSPRD" + log.Lot_No + ".pdf";
                  form.Form_ID = Guid.NewGuid();
                  form.Logsheet_ID = log.Logsheet_ID;
                  form.File = ms.ToArray();
               }
               model.result = tService.UpdateLogsheet(log, form);
               if (model.result.Code == ReturnCode.SUCCESS)
               {
                  if (savemode == "printform")
                  {
                     var route = new AppRouteValueDictionary(model);
                     route.Add("Logsheet_ID", log.Logsheet_ID);
                     route.Add("pPrint", savemode);
                     route.Add("operation", Operation.U);
                     return RedirectToAction("LogsheetInfo", route);
                  }
                  else if (savemode == "printproductform")
                  {
                     var route = new AppRouteValueDictionary(model);
                     route.Add("Logsheet_ID", log.Logsheet_ID);
                     route.Add("pPrint", savemode);
                     route.Add("operation", Operation.U);
                     return RedirectToAction("LogsheetInfo", route);
                  }
                  else if (savemode == "saveform")
                  {
                     View(model);
                  }
                  else
                  {
                     if (sendemail)
                     {
                        if (!string.IsNullOrEmpty(emailstatus))
                        {
                           var emailCri = new EmailCriteria()
                           {
                              Lot_No = log.Lot_No,
                              Status = emailstatus,
                              from = userlogin.Email_Address
                           };
                           var IsSuccess = EmailAgnos.sendClosedLogsheet(userlogin, GetRecipientsNotification(), emailCri);
                           if (IsSuccess != "Success")
                              model.result = new ServiceResult() { Code = ReturnCode.ERROR_CANT_SEND_EMAIL, Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE) + Error.GetMessage(ReturnCode.ERROR_CANT_SEND_EMAIL), Field = Resource.Logsheet };
                        }
                     }
                     return RedirectToAction("Logsheet", new AppRouteValueDictionary(model));
                  }
               }
            }
         }

         var exService = new ExchequerService();
         var cbService = new ComboService();
         model.cProductlist = exService.LstExchquerProduct();

         if (string.IsNullOrEmpty(model.Product_Code) && model.cProductlist.Count() > 0)
            model.Product_Code = model.cProductlist[0].Value;

         if (!string.IsNullOrEmpty(model.Product_Code))
         {
            model.cTmplist = cbService.LstTemplateByProductTemplate(false, model.Product_Code);
            var cri = new ProductTempateCriteria();
            cri.Product_Code = model.Product_Code;
            cri.Template_ID = model.Template_ID;
            cri.include = true;
            var result = tService.GetProductTempate(cri);
            if (result.Code == ReturnCode.SUCCESS)
            {
               var protemps = result.Object as List<Product_Template>;
               if (protemps != null && protemps.Count() == 1)
               {
                  var protmp = protemps.FirstOrDefault();
                  model.Template_ID = protmp.Template_ID;
                  model.Tmp = protmp.Template_Logsheet;
               }
            }
         }

         model.cStatuslist = cbService.LstStatus();
         model.cWorkOrderNolist = exService.LstExchquerWorkOrder();
         model.cLotNumber = cbService.LstLotNumber(model.Product_Code);
         model.cDisposelist = cbService.LstDispose();
         model.cPackaginglist = cbService.LstLookupData(LookupType.Packaging, true);
         model.cUOMlist = cbService.LstLookupData(LookupType.UOM, true);
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0007);
         if (prole != null)
            model.Modify = prole.Modify;
         return View(model);
      }

      [HttpGet]
      public ActionResult LogsheetPrint(LogsheetInfoViewModels pmodel)
      {
         var uService = new UserService();
         ModelState.Clear();
         var model = new LogsheetInfoViewModels();
         model.operation = pmodel.operation;
         model.Logsheet_ID = pmodel.Logsheet_ID;
         var tService = new TemplateService();
         var cbService = new ComboService();
         var exService = new ExchequerService();
         model.cProductlist = exService.LstExchquerProduct();

         var log = new Logsheet();
         var cri = new LogsheetCriteria();
         cri.Logsheet_ID = model.Logsheet_ID;
         cri.include = true;
         var result = tService.GetLogsheet(cri);
         if (result.Code == ReturnCode.SUCCESS)
         {
            var logs = result.Object as List<Logsheet>;
            if (logs != null && logs.Count() == 1)
            {
               log = logs.FirstOrDefault();
               model.Logsheet_ID = log.Logsheet_ID;
               model.Product_Code = log.Product_Code;
               model.Product_Name = log.Product_Name;
               model.Form_No = log.Form_No;
               model.Lot_No = log.Lot_No;
               model.Tmp = log.Template_Logsheet;
               model.Work_Order_No = log.Work_Order_No;
               model.Lotgsheet_Status = log.Lotgsheet_Status;
               model.Template_ID = log.Template_ID;
               model.Quantity = log.Quantity;
               model.Expiry_Date = DateUtil.ToDisplayDate(log.Expiry_Date);
               model.UAI = log.UAI;
               model.RTS = log.RTS;
               model.Rework = log.Rework;
               model.Scrap = log.Scrap;
               model.RMR_No = log.RMR_No;
               model.Reason_If_Reject = log.Reason_If_Reject;
               model.Authorized_By = log.Authorized_By;
               model.Dispose = model.Dispose;
               model.Status = log.Status;
               model.UOM = log.UOM;
               model.Note = log.Note;
               model.Product_Status = log.Product_Status;
               model.Remarks = log.Remarks;
               if (log.UOM.HasValue)
                  model.UOM_Name = log.Global_Lookup_Data.Name;

               model.Packaging = log.Packaging;
               if (log.Packaging.HasValue)
                  model.Packaging_Name = log.Global_Lookup_Data1.Name;

               model.Authorized_Date = DateUtil.ToDisplayDate(log.Authorized_Date);
               model.PD_Issue = log.PD_Issue;
               model.PD_Issue_Date = Resource.NA;
               model.PD_Issue_Name = Resource.NA;

               if (log.PD_Issue_Date.HasValue)
                  model.PD_Issue_Date = DateUtil.ToDisplayDate(log.PD_Issue_Date);
               if (log.User_Profile != null)
                  model.PD_Issue_Name = log.User_Profile.Name;

               model.PD_Approval = log.PD_Approval;
               model.PD_Approval_Date = Resource.NA;
               model.PD_Approval_Name = Resource.NA;
               if (log.PD_Approval_Date.HasValue)
                  model.PD_Approval_Date = DateUtil.ToDisplayDate(log.PD_Approval_Date);
               if (log.User_Profile1 != null)
                  model.PD_Approval_Name = log.User_Profile1.Name;

               model.QA_Approval = log.QA_Approval;
               model.QA_Approval_Date = Resource.NA;
               model.QA_Approval_Name = Resource.NA;
               if (log.QA_Approval_Date.HasValue)
                  model.QA_Approval_Date = DateUtil.ToDisplayDate(log.QA_Approval_Date);
               if (log.User_Profile2 != null)
                  model.QA_Approval_Name = log.User_Profile2.Name;

               var groups = new List<Logsheet_Group_Row>();
               foreach (var grow in log.Logsheet_Group)
               {
                  var group = new Logsheet_Group_Row();
                  group.Logsheet_Group_ID = grow.Logsheet_Group_ID;
                  group.Logsheet_ID = grow.Logsheet_ID;
                  group.Group_ID = grow.Group_ID;

                  var hdrs = new List<Logsheet_Header_Row>();
                  foreach (var hrow in grow.Logsheet_Header)
                  {
                     var hdr = new Logsheet_Header_Row();
                     hdr.Logsheet_Header_ID = hrow.Logsheet_Header_ID;
                     hdr.Logsheet_Group_ID = hrow.Logsheet_Group_ID;
                     hdr.Header_ID = hrow.Header_ID;

                     var maps = new List<Logsheet_Map_Row>();
                     foreach (var mrow in hrow.Logsheet_Map)
                     {
                        var map = new Logsheet_Map_Row();
                        map.Logsheet_Map_ID = mrow.Logsheet_Map_ID;
                        map.Logsheet_Header_ID = mrow.Logsheet_Header_ID;
                        map.Header_ID = mrow.Header_ID;
                        map.Field_ID = mrow.Field_ID;
                        map.Option_Selected = mrow.Option_Selected;
                        map.Text_Display = mrow.Text_Display;
                        maps.Add(map);
                     }
                     hdr.Logsheet_Map_Rows = maps.ToArray();
                     hdrs.Add(hdr);
                  }

                  var fields = new List<Logsheet_Field_Row>();
                  foreach (var frow in grow.Logsheet_Field)
                  {
                     var field = new Logsheet_Field_Row();
                     field.Logsheet_Field_ID = frow.Logsheet_Field_ID;
                     field.Logsheet_Group_ID = frow.Logsheet_Group_ID;
                     field.Field_ID = frow.Field_ID;
                     fields.Add(field);
                  }
                  group.Logsheet_Field_Rows = fields.ToArray();
                  group.Logsheet_Header_Rows = hdrs.ToArray();
                  groups.Add(group);
               }
               model.Logsheet_Group_Rows = groups.ToArray();
            }
         }
         var htmlToConvert = "";// RenderPartialViewAsString("LogsheetPrint", model);
         Response.Clear();
         Response.ClearContent();
         Response.ClearHeaders();
         Response.ContentType = "application/pdf";
         Response.Charset = Encoding.UTF8.ToString();
         Response.HeaderEncoding = Encoding.UTF8;
         Response.ContentEncoding = Encoding.UTF8;
         Response.Buffer = true;
         StringReader sr = new StringReader(htmlToConvert);
         Document pdfDoc = new Document(PageSize.A4);
         pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());


         HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
         var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
         var action = new PdfAction(PdfAction.PRINTDIALOG);
         writer.SetOpenAction(action);

         var pageevent = new PDFPageEvent();
         pageevent.Report_Code = Page_Code.P0007;
         pageevent.Approval = model.Approval_Name;
         pageevent.Lotgsheet_Status = model.Lotgsheet_Status;
         writer.PageEvent = pageevent;
         pdfDoc.Open();
         htmlparser.Parse(sr);

         var bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
         Font fontH3 = new Font(bf, 9, Font.BOLD);
         Font fontNormal = new Font(bf, 9, Font.NORMAL);
         BaseColor bgColor = iTextSharp.text.html.WebColors.GetRGBColor("#eee");
         Rectangle pageSize = pdfDoc.PageSize;

         string path = HttpContext.Server.MapPath("~/image/agnos_logo.jpeg");

         var imageFile = System.IO.File.ReadAllBytes(path);
         Image img = Image.GetInstance(imageFile);

         PdfPTable hdrtable = new PdfPTable(5);
         hdrtable.TotalWidth = pageSize.Width;
         hdrtable.SetWidthPercentage(new float[] { (float)(hdrtable.TotalWidth * 0.20), (float)(hdrtable.TotalWidth * 0.30), (float)(hdrtable.TotalWidth * 0.20), (float)(hdrtable.TotalWidth * 0.15), (float)(hdrtable.TotalWidth * 0.15) }, pageSize);

         CustomITextSharp.Cell(hdrtable, Resource.Product_Name, fontNormal, PdfPCellBorderType.TopLeft, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, log.Product_Name, fontH3, PdfPCellBorderType.TopCenter, settextdefault: false);
         if (!string.IsNullOrEmpty(log.Form_No))
         {
            CustomITextSharp.Cell(hdrtable, Resource.Form_No, fontNormal, PdfPCellBorderType.TopCenter, settextdefault: false);
            CustomITextSharp.Cell(hdrtable, log.Form_No, fontNormal, PdfPCellBorderType.TopCenter, settextdefault: false);
         }
         else
         {
            CustomITextSharp.Cell(hdrtable, "", fontNormal, PdfPCellBorderType.TopCenter, settextdefault: false);
            CustomITextSharp.Cell(hdrtable, "", fontNormal, PdfPCellBorderType.TopCenter, settextdefault: false);
         }
         CustomITextSharp.CellImage(hdrtable, img, fontNormal, PdfPCellBorderType.FullBorder, 3);

         /*************************************/
         var rno = "";
         if (log.Revision_No.HasValue)
            rno = log.Revision_No.ToString();

         CustomITextSharp.Cell(hdrtable, Resource.Lot_No, fontNormal, PdfPCellBorderType.MidLeft, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, log.Lot_No, fontNormal, PdfPCellBorderType.MidCenter, settextdefault: false);
         if (!string.IsNullOrEmpty(rno))
         {
            CustomITextSharp.Cell(hdrtable, Resource.Revision_No, fontNormal, PdfPCellBorderType.MidCenter, settextdefault: false);
            CustomITextSharp.Cell(hdrtable, rno, fontNormal, PdfPCellBorderType.MidCenter, settextdefault: false);
         }
         else
         {
            CustomITextSharp.Cell(hdrtable, "", fontNormal, PdfPCellBorderType.MidCenter, settextdefault: false);
            CustomITextSharp.Cell(hdrtable, "", fontNormal, PdfPCellBorderType.MidCenter, settextdefault: false);
         }
         /*************************************/
         CustomITextSharp.Cell(hdrtable, Resource.Dilution_Tank_No, fontNormal, PdfPCellBorderType.ButtomLeft, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, log.Dilution_Tank_No, fontNormal, PdfPCellBorderType.ButtomCenter, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, "", fontNormal, PdfPCellBorderType.ButtomCenter, settextdefault: false);
         CustomITextSharp.Cell(hdrtable, "", fontNormal, PdfPCellBorderType.ButtomCenter, settextdefault: false);
         pdfDoc.Add(hdrtable);
         pdfDoc.Add(new Paragraph("\n"));


         if (model.Tmp != null && model.Tmp.Tmp_Log_Group != null && model.Logsheet_Group_Rows != null)
         {
            foreach (var tmpG in model.Tmp.Tmp_Log_Group.OrderBy(o => o.Template_Group.Group_Order))
            {
               var group = model.Logsheet_Group_Rows.Where(w => w.Group_ID == tmpG.Group_ID).FirstOrDefault();
               if (tmpG.Template_Group != null && tmpG.Tmp_Log_Header.Count() > 0 && group != null)
               {
                  var hdrs = group.Logsheet_Header_Rows;
                  var fields = group.Logsheet_Field_Rows;
                  {
                     PdfPTable table = new PdfPTable(1);
                     table.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                     table.TotalWidth = pageSize.Width;
                     table.SetWidthPercentage(new float[] { (float)(table.TotalWidth * 1) }, pageSize);

                     CustomITextSharp.Cell(table, tmpG.Template_Group.Group_Name, fontH3, PdfPCellBorderType.TopRight);
                     pdfDoc.Add(table);
                  }

                  List<float> witdthpercens = new List<float>();
                  if (tmpG.Tmp_Log_Field != null && tmpG.Tmp_Log_Field.Count > 0 && tmpG.Tmp_Log_Header != null && tmpG.Tmp_Log_Header.Count > 0 && hdrs != null && fields != null)
                  {
                     if (tmpG.Tmp_Log_Header != null && tmpG.Tmp_Log_Header.Count > 0)
                     {
                        {

                           PdfPTable table = new PdfPTable(tmpG.Tmp_Log_Header.Count + 1);
                           table.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                           table.TotalWidth = pageSize.Width;

                           float colwidth = (float)table.TotalWidth * (1f / (tmpG.Tmp_Log_Header.Count + 1));
                           for (var i = 0; i < tmpG.Tmp_Log_Header.Count + 1; i++)
                           {
                              witdthpercens.Add(colwidth);
                           }
                           table.SetWidthPercentage(witdthpercens.ToArray(), pageSize);

                           var hcnt = 1;
                           CustomITextSharp.Cell(table, tmpG.Sub_Group_Name, fontNormal, PdfPCellBorderType.TopLeft);
                           foreach (var tmphdr in tmpG.Tmp_Log_Header.OrderBy(o => o.Header_Order))
                           {
                              if (hcnt < tmpG.Tmp_Log_Header.Count)
                                 CustomITextSharp.Cell(table, tmphdr.Template_Header.Header_Name, fontNormal, PdfPCellBorderType.TopCenter);
                              else
                                 CustomITextSharp.Cell(table, tmphdr.Template_Header.Header_Name, fontNormal, PdfPCellBorderType.TopRight);
                              hcnt++;
                           }
                           pdfDoc.Add(table);
                        }

                     }
                     if (tmpG.Tmp_Log_Field != null && tmpG.Tmp_Log_Field.Count > 0)
                     {
                        {
                           PdfPTable table = new PdfPTable(tmpG.Tmp_Log_Header.Count + 1);
                           table.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                           table.TotalWidth = pageSize.Width;
                           table.SetWidthPercentage(witdthpercens.ToArray(), pageSize);
                           foreach (var tmpfield in tmpG.Tmp_Log_Field.OrderBy(o => o.Field_Order))
                           {
                              var hcnt = 1;
                              CustomITextSharp.Cell(table, tmpfield.Template_Field.Field_Name, fontNormal, PdfPCellBorderType.MidLeft);
                              foreach (var tmphdr in tmpG.Tmp_Log_Header.OrderBy(o => o.Header_Order))
                              {
                                 var textdisplay = "-";
                                 var hdr = hdrs.Where(w => w.Header_ID == tmphdr.Header_ID).FirstOrDefault();
                                 if (hdr != null && hdr.Logsheet_Map_Rows != null)
                                 {
                                    var map = hdr.Logsheet_Map_Rows.Where(w => w.Field_ID == tmpfield.Field_ID).FirstOrDefault();
                                    if (map != null && !string.IsNullOrEmpty(map.Text_Display))
                                       textdisplay = map.Text_Display;
                                    else
                                       textdisplay = "-";
                                 }
                                 if (hcnt < tmpG.Tmp_Log_Header.Count)
                                    CustomITextSharp.Cell(table, textdisplay, fontNormal, PdfPCellBorderType.MidCenter);
                                 else
                                    CustomITextSharp.Cell(table, textdisplay, fontNormal, PdfPCellBorderType.MidRight);
                                 hcnt++;
                              }
                           }
                           pdfDoc.Add(table);
                        }


                     }
                  }
               }
            }
         }

         /* footer*/
         {
            PdfPTable table = new PdfPTable(4);
            table.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            table.TotalWidth = pageSize.Width;
            table.SetWidthPercentage(new float[] { (float)(table.TotalWidth * 0.4), (float)(table.TotalWidth * 0.2), (float)(table.TotalWidth * 0.2), (float)(table.TotalWidth * 0.2) }, pageSize);

            CustomITextSharp.Cell(table, "Notes" + Environment.NewLine + Environment.NewLine + model.Note, fontNormal, PdfPCellBorderType.ButtomLeft, 3);
            CustomITextSharp.Cell(table, model.PD_Issue_Name, fontNormal, PdfPCellBorderType.TopCenter);
            CustomITextSharp.Cell(table, model.PD_Approval_Name, fontNormal, PdfPCellBorderType.TopCenter);
            CustomITextSharp.Cell(table, model.QA_Approval_Name, fontNormal, PdfPCellBorderType.TopRight);

            CustomITextSharp.Cell(table, model.PD_Issue_Date, fontNormal, PdfPCellBorderType.MidCenter);
            CustomITextSharp.Cell(table, model.PD_Approval_Date, fontNormal, PdfPCellBorderType.MidCenter);
            CustomITextSharp.Cell(table, model.QA_Approval_Date, fontNormal, PdfPCellBorderType.MidRight);

            CustomITextSharp.Cell(table, Resource.PD_Issue, fontH3, PdfPCellBorderType.ButtomCenter);
            CustomITextSharp.Cell(table, Resource.PD_Approval, fontH3, PdfPCellBorderType.ButtomCenter);
            CustomITextSharp.Cell(table, Resource.QA_Approval, fontH3, PdfPCellBorderType.ButtomRight);
            pdfDoc.Add(table);
         }

         pdfDoc.Close();
         Response.End();
         return View(model);
      }

      [HttpGet]
      public void RepairLogsheetProductStatusForm()
      {
         var tService = new TemplateService();
         var cbService = new ComboService();
         var exService = new ExchequerService();

         var cri = new LogsheetCriteria();
         cri.include = true;
         var result2 = tService.GetLogsheet(cri);
         if (result2.Code == ReturnCode.SUCCESS)
         {
            var logs = result2.Object as List<Logsheet>;
            if (logs != null)
            {
               foreach (var log in logs)
               {
                  var form = tService.GetLogsheetStatusForm(log.Logsheet_ID);
                  if (form != null)
                     continue;

                  var model = new LogsheetProductStatusFrom();
                  model.Product_Name = log.Product_Name;
                  model.Product_Code = log.Product_Code;
                  model.Expiry_Date = DateUtil.ToDisplayDate(log.Expiry_Date);
                  model.Lot_No = log.Lot_No;
                  model.Logsheet_ID = log.Logsheet_ID;
                  model.Quantity = log.Quantity;
                  model.UAI = log.UAI;
                  model.RTS = log.RTS;
                  model.Rework = log.Rework;
                  model.Scrap = log.Scrap;
                  model.RMR_No = log.RMR_No;
                  model.Reason_If_Reject = log.Reason_If_Reject;
                  model.Authorized_By = log.Authorized_By;
                  model.Dispose = model.Dispose;
                  model.Status = log.Status;
                  model.Product_Status = log.Product_Status;
                  model.UOM = log.UOM;
                  if (log.UOM.HasValue)
                     model.UOM_Name = log.Global_Lookup_Data.Name;

                  model.Packaging = log.Packaging;
                  if (log.Packaging.HasValue)
                     model.Packaging_Name = log.Global_Lookup_Data1.Name;

                  model.Authorized_Date = DateUtil.ToDisplayDate(log.Authorized_Date);
                  model.Create_On = DateUtil.ToDisplayDate(StoredProcedure.GetCurrentDate());
                  model.Remarks = log.Remarks;

                  if (string.IsNullOrEmpty(model.Product_Name))
                     model.Product_Name = Resource.NA;
                  if (string.IsNullOrEmpty(model.Expiry_Date))
                     model.Expiry_Date = Resource.NA;
                  if (string.IsNullOrEmpty(model.Lot_No))
                     model.Lot_No = Resource.NA;
                  if (string.IsNullOrEmpty(model.UAI))
                     model.UAI = Resource.NA;
                  if (string.IsNullOrEmpty(model.RTS))
                     model.RTS = Resource.NA;
                  if (string.IsNullOrEmpty(model.Rework))
                     model.Rework = Resource.NA;
                  if (string.IsNullOrEmpty(model.Scrap))
                     model.Scrap = Resource.NA;
                  if (string.IsNullOrEmpty(model.RMR_No))
                     model.RMR_No = Resource.NA;
                  if (string.IsNullOrEmpty(model.Reason_If_Reject))
                     model.Reason_If_Reject = Resource.NA;
                  if (string.IsNullOrEmpty(model.Authorized_By))
                     model.Authorized_By = Resource.NA;
                  if (string.IsNullOrEmpty(model.Packaging_Name))
                     model.Packaging_Name = Resource.NA;
                  if (string.IsNullOrEmpty(model.Authorized_Date))
                     model.Authorized_Date = Resource.NA;

                  using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                  {
                     var htmlToConvert = RenderPartialViewAsString("LogsheetProductStatusForm", model);
                     StringReader sr = new StringReader(htmlToConvert);
                     Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                     HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                     var writer = PdfWriter.GetInstance(pdfDoc, ms);
                     var pageevent = new PDFPageEvent();
                     writer.PageEvent = pageevent;
                     pdfDoc.Open();
                     htmlparser.Parse(sr);
                     pdfDoc.Close();

                     form = new Logsheet_Product_Form();
                     form.Logsheet_ID = log.Logsheet_ID;
                     form.Create_By = log.Create_By;
                     form.Create_On = log.Create_On;
                     form.File_Name = "LOGSPRD" + log.Lot_No + ".pdf";
                     form.Form_ID = Guid.NewGuid();
                     form.File = ms.ToArray();
                     var result = tService.InsertLogsheetProductForm(form);
                  }
               }


            }
         }
      }

      [HttpGet]
      public void LogsheetProductStatusForm(Nullable<int> pLogsheetID)
      {
         var user = new UserService(User.Identity.GetUserId());
         var uService = new UserService();
         var tService = new TemplateService();
         var cbService = new ComboService();
         var exService = new ExchequerService();
         var model = new LogsheetProductStatusFrom();

         if (pLogsheetID.HasValue && pLogsheetID.Value > 0)
         {
            var form = tService.GetLogsheetStatusForm(pLogsheetID);
            if (form != null)
            {
               Response.ContentType = "application/pdf";
               Response.AddHeader("content-disposition", "inline; filename=" + form.File_Name);
               Response.BinaryWrite(form.File.ToArray());
               Response.Flush();
               Response.Close();
            }
         }
      }

      [HttpGet]
      public ActionResult LogsheetSummaryReport(LogsheetViewModels model)
      {
         var uService = new UserService();
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0007);
         if (prole == null)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
         if (prole.View == null || prole.View == false)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

         ModelState.Clear();
         model.Modify = prole.Modify;
         model.View = prole.View;

         var tService = new TemplateService();

         var cri = new LogsheetCriteria();
         cri.Template_ID = model.Template_ID;
         cri.Lot_No = model.Lot_No;
         cri.Product_Code = model.Product_Code;
         cri.Template_Code = model.Template_Code;
         var ptresult = tService.GetLogsheet(cri);
         if (ptresult.Code == ReturnCode.SUCCESS)
            model.LogsheetList = (ptresult.Object as List<Logsheet>).Where(w => w.PD_Issue.HasValue).ToList(); ;

         var cbService = new ComboService();
         var exService = new ExchequerService();
         //model.cTmplist = cbService.LstTemplate(true);

         model.cProductlist = exService.LstExchquerProduct(true);
         if (!string.IsNullOrEmpty(model.Product_Code))
            model.cTmplist = cbService.LstTemplateByProductTemplate(true, model.Product_Code);
         else
            model.cTmplist = cbService.LstTemplate(true);

         model.cTemplateCodelist = cbService.LstTemplateCode(true);

         return View(model);
      }

      [HttpGet]
      public ActionResult LogsheetSummaryPrint(LogsheetSummaryViewModels model)
      {
         var uService = new UserService();
         ModelState.Clear();

         var tService = new TemplateService();
         var cbService = new ComboService();
         var tmp = new Template_Logsheet();
         var tcri = new TemplateLogsheetCriteria();
         tcri.include = true;
         tcri.Template_ID = model.Template_ID;

         var result = tService.GetTempateLogsheet(tcri);
         if (result.Code == ReturnCode.SUCCESS)
         {
            var tmps = result.Object as List<Template_Logsheet>;
            if (tmps != null && tmps.Count() == 1)
               model.Tmp = tmps.FirstOrDefault();
         }

         var log = new Logsheet();
         var cri = new LogsheetCriteria();
         cri.include = true;
         cri.Product_Code = model.Product_Code;
         cri.Template_ID = model.Template_ID;
         cri.Lot_No = model.Lot_No;
         result = tService.GetLogsheet(cri);
         if (result.Code == ReturnCode.SUCCESS)
         {
            var logs = result.Object as List<Logsheet>;
            model.Logs = logs.Where(w => w.PD_Issue.HasValue).ToList();
         }
         var currentdate = StoredProcedure.GetCurrentDate();
         var csv = RenderPartialViewAsString("LogsheetSummaryPrint", model);
         System.Web.UI.WebControls.GridView gv = new System.Web.UI.WebControls.GridView();
         gv.DataBind();
         Response.ClearContent();
         Response.Buffer = true;
         Response.AddHeader("content-disposition", "attachment; filename= LogsheetSummary" + currentdate.ToString("ddMMyyyy") + ".xls");
         Response.ContentType = "application/ms-excel";
         Response.Charset = "";
         Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
         StringWriter sw = new StringWriter();
         sw.Write(csv);
         System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
         gv.RenderControl(htw);
         Response.Output.Write(sw.ToString());
         Response.Flush();
         Response.End();
         return View(model);
      }

      #endregion

      #region Lotnumber
      [HttpGet]
      public ActionResult GetLotNumber(string pLotNo, Nullable<int> pLotNumberID, string pProductCode)
      {
         var tService = new TemplateService();
         var cri = new LotNumberCriteria();
         cri.Lot_Number_ID = pLotNumberID;
         cri.Product_Code = pProductCode;
         cri.Lot_No = pLotNo;

         var lresult = tService.GetLotNumber(cri);
         if (lresult.Code == ReturnCode.SUCCESS)
         {
            var lotlist = lresult.Object as List<Lot_Number>;
            if (lotlist.Count() > 0)
            {
               return Json(lotlist.Select(s => new
               {
                  Lot_Number_ID = s.Lot_Number_ID,
                  Lot_No = s.Lot_No,
                  Product_Code = s.Product_Code,
                  Lot_Number_Date = DateUtil.ToDisplayDate(s.Lot_Number_Date),
                  Template_ID = s.Template_ID,
               }), JsonRequestBehavior.AllowGet);
            }
         }

         return Json(new { }, JsonRequestBehavior.AllowGet);
      }

      [HttpGet]
      public ActionResult GetLotNumberPrint()
      {
         var tService = new TemplateService();
         var model = new DailyPlanningViewModels();
         model.DailyPlannings = new List<DailyPlanning>();
         var lresult = tService.GetLotDailyPlanning();
         if (lresult.Code == ReturnCode.SUCCESS)
         {
            var lots = lresult.Object as List<Logsheet>;
            List<string> products = new List<string>();
            if (lots != null)
               products = lots.Select(s => s.Product_Code).Distinct().ToList();

            foreach (var prod in products)
            {
               var dp = new DailyPlanning();
               dp.Logsheets = new List<DailyPlanningLogsheet>();
               foreach (var lg in lots.Where(w => w.Product_Code == prod))
               {
                  dp.Product_Code = lg.Product_Code;

                  var dplg = new DailyPlanningLogsheet();
                  dplg.Lot_No = lg.Lot_No;

                  foreach (var gp in lg.Logsheet_Group)
                  {
                     foreach (var hdr in gp.Logsheet_Header)
                     {
                        foreach (var map in hdr.Logsheet_Map)
                        {
                           if (string.IsNullOrEmpty(map.Text_Display))
                              continue;
                           if (map.Text_Display == "-")
                              continue;

                           if (map.Template_Field.Field_Name == "TMAH Lot No")
                              dplg.TMAH_Lot_No = map.Text_Display;

                           if (map.Template_Field.Field_Name == "Usage Only")
                              dplg.Usage_Only = map.Text_Display;

                           if (!string.IsNullOrEmpty(dplg.TMAH_Lot_No) & !string.IsNullOrEmpty(dplg.TMAH_Lot_No))
                              break;
                        }

                        if (!string.IsNullOrEmpty(dplg.TMAH_Lot_No) & !string.IsNullOrEmpty(dplg.TMAH_Lot_No))
                           break;
                     }

                     if (!string.IsNullOrEmpty(dplg.TMAH_Lot_No) & !string.IsNullOrEmpty(dplg.TMAH_Lot_No))
                        break;
                  }


                  dp.Logsheets.Add(dplg);
               }
               model.DailyPlannings.Add(dp);
            }
         }

         var htmlToConvert = RenderPartialViewAsString("LotNumberForm", model);

         Response.ClearContent();
         Response.Buffer = true;
         Response.AddHeader("content-disposition", "attachment; filename=DailyLotNumber.xls");
         Response.ContentType = "application/ms-excel";
         Response.Charset = "";
         StringWriter sw = new StringWriter();
         sw.Write(htmlToConvert.ToString());
         System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);

         Response.Output.Write(sw.ToString());
         Response.Flush();
         Response.End();

         return View("LotNumberForm", model);
      }

      [HttpGet]
      public ActionResult LotNumber(LotNumberGenViewModels model, ServiceResult msgresult, string pProductCode)
      {
         var uService = new UserService();
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0008);
         if (prole == null)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
         if (prole.View == null || prole.View == false)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

         ModelState.Clear();

         if (model.operation == Operation.D)
            return LotNumber(model);

         var currentdate = StoredProcedure.GetCurrentDate();
         var tService = new TemplateService();
         model.result = msgresult; // return result from http post
         model.Modify = prole.Modify;
         model.View = prole.View;
         model.Default_Date = DateUtil.ToDisplayDate(currentdate);

         var cbService = new ComboService();
         var exService = new ExchequerService();
         var cri = new LotNumberCriteria();

         cri.Product_Code = model.Product_Code;
         var lresult = tService.GetLotNumber(cri);
         if (lresult.Code == ReturnCode.SUCCESS)
            model.LotNumberList = lresult.Object as List<Lot_Number>;

         model.cProductlist = exService.LstExchquerProduct();

         if (!string.IsNullOrEmpty(model.Product_Code))
            model.Default_No = GetDefaultLotNo2(model.Product_Code);
         else if (model.cProductlist.Count > 0)
            model.Default_No = GetDefaultLotNo2(model.cProductlist[0].Value);

         return View(model);
      }

      [HttpPost]
      public ActionResult LotNumber(LotNumberGenViewModels model)
      {
         var uService = new UserService();
         var userlogin = uService.getUser(User.Identity.GetUserId());
         var tService = new TemplateService(User.Identity.GetUserId());
         var currentdate = StoredProcedure.GetCurrentDate();
         if (ModelState.IsValid)
         {
            var lot = new Lot_Number();
            if (model.operation == Operation.U || model.operation == Operation.D)
            {
               var cri = new LotNumberCriteria();
               cri.Lot_Number_ID = model.Lot_Number_ID;
               var result = tService.GetLotNumber(cri);
               if (result.Code == ReturnCode.SUCCESS)
               {
                  var lots = result.Object as List<Lot_Number>;
                  if (lots != null && lots.Count() == 1)
                     lot = lots.FirstOrDefault();
               }
            }

            if (model.operation != Operation.D)
            {
               lot.Product_Code = model.Product_Code;
               lot.Template_ID = model.Template_ID;
               lot.Lot_No = model.Lot_No;
               lot.Lot_Number_Date = DateUtil.ToDate(model.Lot_Number_Date);
            }

            if (model.operation == Operation.C)
            {
               var defDate = DateUtil.ToDate(model.Lot_Number_Date).Value;
               var defYear = defDate.Year.ToString().Substring(2, 2);
               var defDay = defDate.Day.ToString("00");
               var defMonth = DateUtil.GetFullMonth(defDate.Month).Substring(0, 1);

               lot.No_Count = NumUtil.ParseInteger(lot.Lot_No.Replace(lot.Product_Code + defYear + defMonth + defDay, ""));
               model.result = tService.InsertLotNumber(lot);

               if (model.result.Code == ReturnCode.SUCCESS)
               {
                  var model2 = new LogsheetInfoViewModels();
                  var logsheet = new Logsheet();
                  logsheet.Product_Code = lot.Product_Code;
                  logsheet.Template_ID = lot.Template_ID;
                  logsheet.Lot_No = lot.Lot_No;
                  model2.Product_Code = lot.Product_Code;
                  model2.Template_ID = lot.Template_ID;
                  model2.Lot_No = lot.Lot_No;

                  var cri = new ProductTempateCriteria();
                  cri.Product_Code = lot.Product_Code;
                  cri.Template_ID = lot.Template_ID;
                  cri.include = true;
                  var result = tService.GetProductTempate(cri);
                  if (result.Code == ReturnCode.SUCCESS)
                  {
                     var protemps = result.Object as List<Product_Template>;
                     if (protemps != null && protemps.Count() == 1)
                     {
                        var protmp = protemps.FirstOrDefault();
                        logsheet.Product_Name = protmp.Product_Name;
                        logsheet.Form_No = protmp.From_No;
                        logsheet.Dilution_Tank_No = protmp.Dilution_Tank_No;
                        logsheet.Revision_No = NumUtil.ParseInteger(protmp.Revision);

                        model2.Product_Name = protmp.Product_Name;
                        model2.Form_No = protmp.From_No;
                        model2.Dilution_Tank_No = protmp.Dilution_Tank_No;
                        model2.Revision_No = NumUtil.ParseInteger(protmp.Revision);
                     }
                     using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                     {
                        var htmlToConvert = RenderPartialViewAsString("LogsheetProductStatusForm2017", model2);
                        StringReader sr = new StringReader(htmlToConvert);
                        Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        var writer = PdfWriter.GetInstance(pdfDoc, ms);
                        var pageevent = new PDFPageEvent();
                        writer.PageEvent = pageevent;
                        pdfDoc.Open();
                        htmlparser.Parse(sr);
                        pdfDoc.Close();

                        var form = new Logsheet_Product_Form();
                        form.Create_By = userlogin.Email_Address;
                        form.Create_On = currentdate;
                        form.File_Name = "LOGSPRD" + logsheet.Lot_No + ".pdf";
                        form.Form_ID = Guid.NewGuid();
                        form.File = ms.ToArray();
                        logsheet.Logsheet_Product_Form.Add(form);
                     }
                  }
                  model.result = tService.InsertLogsheet(logsheet);
                  if (model.result.Code == ReturnCode.SUCCESS)
                  {
                     if (model.tabAction == "logsheet")
                        return RedirectToAction("LogsheetInfo", new { Logsheet_ID = logsheet.Logsheet_ID, operation = Operation.U });
                  }
               }
            }
            else if (model.operation == Operation.U)
               model.result = tService.UpdateLotNumber(lot);

            else if (model.operation == Operation.D)
            {
               lot.Record_Status = Record_Status.Delete;
               model.result = tService.UpdateLotNumber(lot);
               if (model.result.Code == ReturnCode.SUCCESS)
                  model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Lot_Number_Generation };
               else
                  model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Lot_Number_Generation };

               return RedirectToAction("LotNumber", new AppRouteValueDictionary(model));
            }

            if (model.result.Code == ReturnCode.SUCCESS)
               return RedirectToAction("LotNumber", new AppRouteValueDictionary(model));

         }

         model.Default_Date = DateUtil.ToDisplayDate(currentdate);

         var cri2 = new LotNumberCriteria();
         var lresult = tService.GetLotNumber(cri2);
         if (lresult.Code == ReturnCode.SUCCESS)
            model.LotNumberList = lresult.Object as List<Lot_Number>;

         var exService = new ExchequerService();
         model.cProductlist = exService.LstExchquerProduct();

         if (!string.IsNullOrEmpty(model.Product_Code))
            model.Default_No = GetDefaultLotNo2(model.Product_Code);
         else if (model.cProductlist.Count > 0)
            model.Default_No = GetDefaultLotNo2(model.cProductlist[0].Value);

         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0008);
         if (prole != null)
            model.Modify = prole.Modify;

         return View(model);
      }

      [HttpGet]
      public ActionResult GetDefaultLotNo(string pProductCode)
      {
         return Json(new { maxlot = GetDefaultLotNo2(pProductCode) }, JsonRequestBehavior.AllowGet);
      }

      private int GetDefaultLotNo2(string pProductCode)
      {
         var currentdate = StoredProcedure.GetCurrentDate();
         var tService = new TemplateService();
         var cri = new LotNumberCriteria();
         cri.Product_Code = pProductCode;
         cri.Lot_Number_Date = currentdate;
         var result = tService.GetLotNumber(cri);
         if (result.Code == ReturnCode.SUCCESS)
         {
            var maxlot = 0;
            var todayltno = result.Object as List<Lot_Number>;
            foreach (var ln in todayltno)
            {
               if (ln.No_Count.HasValue && maxlot < ln.No_Count)
                  maxlot = ln.No_Count.Value;
            }
            maxlot++;
            return maxlot;
         }
         return 1;
      }
      #endregion

      [HttpGet]
      public String Get_Product_Name(string Product_Code)
      {

         ProductTemplateViewModels model = new ProductTemplateViewModels();
         var tService = new TemplateService();
         var cbService = new ComboService();
         var exService = new ExchequerService();
         var cProductlist = exService.LstExchquerProduct();

         if (Product_Code == null)
            return "";

         var productname = cProductlist.Where(w => w.Value == Product_Code).FirstOrDefault();

         string str = "";
         str = "<script type=\"text/javascript\"> \n\n $(function () {";
         if (productname != null)
         {
            if (productname.Value != "0")
            {
               str += "$('#Product_Name').val('" + productname.Text + "'); ";
               //str += "$('#Product_Name').get(0).setAttribute('readonly',true);";
            }
            else
            {
               //str += "$('#Product_Name').val('');";
               //str += "$('#Product_Name').attr('readonly',false);";
            }
         }
         else
         {
            str += "$('#Product_Name').val('');";
            //str += "$('#Product_Name').attr('readonly',false);";
         }

         str += "$('#Product_Name').trigger('chosen:updated');";

         str += "});\n\n</script>";

         return str;

      }

      [HttpGet]
      public String GetTemplateLogsheetByProductCode(string Product_Code, bool HasBlank)
      {
         var model = new LotNumberGenViewModels();
         var cbService = new ComboService();
         var exService = new ExchequerService();
         model.cProductlist = exService.LstExchquerProduct();
         model.Product_Code = Product_Code;
         if (string.IsNullOrEmpty(model.Product_Code) && model.cProductlist.Count() > 0)
            model.Product_Code = model.cProductlist[0].Value;

         if (!string.IsNullOrEmpty(model.Product_Code))
         {
            if (HasBlank)
            {
               model.cTmplist = cbService.LstTemplateByProductTemplate(true, model.Product_Code);
               if (!model.Template_ID.HasValue && model.cTmplist.Count() > 1)
               {
                  if (model.cTmplist[1] != null)
                     model.Template_ID = Convert.ToInt32(model.cTmplist[1].Value);
               }
            }
            else
            {
               model.cTmplist = cbService.LstTemplateByProductTemplate(false, model.Product_Code);
               if (!model.Template_ID.HasValue && model.cTmplist.Count() > 0)
                  model.Template_ID = Convert.ToInt32(model.cTmplist[0].Value);
            }
         }

         string str = "";
         str = "<script type=\"text/javascript\"> \n\n $(function () {" +
                 "$('#Product_Code').val('" + model.Product_Code + "'); ";

         str += "$('#Template_ID').empty();";
         if (model.cTmplist != null)
         {
            foreach (var itemData in model.cTmplist)
            {
               str += "$('#Template_ID').append($('<option></option>').val('" + itemData.Value + "').html('" + itemData.Text + "'));";
            }
         }
         str += "$('#Template_ID').val('" + model.Template_ID + "');";

         str += "$('#Template_ID').trigger('chosen:updated');";
         str += "});\n\n</script>";

         return str;
      }

      [HttpGet]
      public String GetTemplateLogsheetAll()
      {
         var model = new LotNumberGenViewModels();
         var cbService = new ComboService();
         var exService = new ExchequerService();
         model.cTmplist = cbService.LstTemplate(true);

         string str = "";
         str = "<script type=\"text/javascript\"> \n\n $(function () {";

         str += "$('#Template_ID').empty();";
         if (model.cTmplist != null)
         {
            foreach (var itemData in model.cTmplist)
            {
               str += "$('#Template_ID').append($('<option></option>').val('" + itemData.Value + "').html('" + itemData.Text + "'));";
            }
         }
         str += "$('#Template_ID').val('');";
         str += "$('#Template_ID').trigger('chosen:updated');";
         str += "});\n\n</script>";

         return str;
      }


      public ActionResult LogsheetSaveAttachment(Nullable<Guid> pAttID, Nullable<int> pLogID)
      {
         if (pLogID.HasValue)
         {
            HttpPostedFileBase file = Request.Files[0];

            int fileSizeInBytes = file.ContentLength;
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] data = target.ToArray();

            if (data != null && data.Length > 0)
            {
               var tService = new TemplateService(User.Identity.GetUserId());

               if (pAttID.HasValue)
               {
                  var ck = tService.GetLogsheetAttachment(pAttID);
                  if (ck == null)
                  {
                     Nullable<Guid> attID = null;
                     if (pLogID.HasValue)
                        tService.InsertLogsheetAttachment(ref attID, pLogID, data, file.FileName);
                     return Json(attID, JsonRequestBehavior.AllowGet);
                  }
                  tService.UpdateLogsheetAttachment(pAttID, data, file.FileName);
                  return Json(pAttID.Value, JsonRequestBehavior.AllowGet);
               }
               else
               {
                  Nullable<Guid> attID = null;
                  if (pLogID.HasValue)
                     tService.InsertLogsheetAttachment(ref attID, pLogID, data, file.FileName);
                  return Json(attID, JsonRequestBehavior.AllowGet);
               }

            }
         }
         return Json(null, JsonRequestBehavior.AllowGet);
      }
      [HttpGet]
      public void LogsheetDisplayAttachment(Nullable<System.Guid> pAttID)
      {
         if (pAttID.HasValue)
         {
            var tService = new TemplateService(User.Identity.GetUserId());
            var attact = tService.GetLogsheetAttachment(pAttID);
            if (attact != null)
            {
               var file = attact.Attachment_File;
               if (attact.Attachment_File_Name.Contains(".pdf"))
               {
                  Response.ClearHeaders();
                  Response.Clear();
                  Response.AddHeader("Content-Type", "application/pdf");
                  Response.AddHeader("Content-Length", file.Length.ToString());
                  Response.AddHeader("Content-Disposition", "inline; filename=\"" + attact.Attachment_File_Name + "\"");
                  Response.BinaryWrite(file);
                  Response.Flush();
                  Response.End();
               }
               else
               {
                  Response.ClearHeaders();
                  Response.Clear();
                  Response.AddHeader("Content-Type", "text/plain");
                  Response.AddHeader("content-length", file.Length.ToString());
                  Response.AddHeader("Content-Disposition", "inline; filename=\"" + attact.Attachment_File_Name + "\"");
                  Response.BinaryWrite(file);
                  Response.Flush();
                  Response.End();
               }
            }
         }
      }

      public ActionResult LogsheetDeleteAttachment(Nullable<Guid> pAttachmentID)
      {
         var tService = new TemplateService(User.Identity.GetUserId());
         var result = tService.DeleteLogsheetAttachment(pAttachmentID);
         return Json(new
         {
            code = result
         }, JsonRequestBehavior.AllowGet);
      }

      public ActionResult LogsheetAddAttachment(int pIndex, bool? pModify)
      {
         var model = new UploadAttachmentFile();
         model.Index = pIndex;
         model.Modify = pModify;
         return PartialView("_UploadAttachmentRow", model);
      }
   }
}


