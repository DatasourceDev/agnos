using Agnos.Models;
using AgnosModel;
using AgnosModel.Models;
using AgnosModel.Service;
using AppFramework;
using AppFramework.Util;
//using Enterprise01;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SBSResourceAPI;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text;
using System.Web.UI.WebControls;
using Agnos.Common;
using System.Configuration;
//using cmsComPort;

namespace Agnos.Controllers
{
    public class MaterialController : ControllerBase
    {
        //Toolkit _toolkit;

        [HttpGet]
        public ActionResult Material(MaterialViewModels model, ServiceResult msgresult)
        {
            var exService = new ExchequerService();
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0001);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            model.result = msgresult;
            model.Modify = prole.Modify;
            model.View = prole.View;

            var mService = new MaterialService();
            var rcri = new RawMaterialCriteria();
            rcri.Product_Code = model.Product_Code;
            rcri.Date_From = model.Date_From;
            rcri.Date_To = model.Date_To;

            var result = mService.GetRawMaterial(rcri);
            if (result.Code == ReturnCode.SUCCESS)
                model.RawMaterialList = result.Object as List<Raw_Material>;


            model.cProductlist = exService.LstExchquerProduct();

            return View(model);
        }

        [HttpGet]
        public ActionResult PrintPendingLabel()
        {
            return View();
        }
        [HttpGet]
        public ActionResult PrintPassLabel()
        {
            return View();
        }
        [HttpGet]
        public ActionResult PrintRejectLabel()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MaterialInfo(Nullable<int> pRawID, string pCode, bool pShowPrintDlg = false, Operation operation = Operation.None)
        {
            var uService = new UserService();
            var userlogin = uService.getUser(User.Identity.GetUserId());

            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0001);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            var exService = new ExchequerService();
            var mService = new MaterialService();
            var cbService = new ComboService();
            var currentdate = StoredProcedure.GetCurrentDate();
            MaterialInfoViewModels model = new MaterialInfoViewModels();
            model.operation = operation;
            model.showPrintDlg = pShowPrintDlg;
            model.Modify = prole.Modify;
            model.View = prole.View;
            model.cProductlist = exService.LstExchquerProduct(true);
            model.cPackaginglist = cbService.LstLookupData(LookupType.Packaging, true);
            model.cUOMlist = cbService.LstLookupData(LookupType.UOM, true);

            if (!string.IsNullOrEmpty(pCode)) // Search product from Exchequer
                model.Product_Code = pCode;

            model.User_Login_ID = userlogin.Profile_ID;
            model.User_Login_Name = userlogin.Name;
            model.Current_Date = DateUtil.ToDisplayDate(currentdate);

            if (operation == Operation.C)
            {

            }
            else if (operation == Operation.U)
            {
                var rcri = new RawMaterialCriteria();
                rcri.Raw_Material_ID = pRawID;
                var result = mService.GetRawMaterial(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var raws = result.Object as List<Raw_Material>;
                    if (raws != null && raws.Count() == 1)
                    {
                        var raw = raws.FirstOrDefault();
                        model.Raw_Material_ID = raw.Raw_Material_ID;

                        model.Product_Code = raw.Product_Code;
                        model.Product_Name = raw.Product_Name;

                        model.Dented = raw.Dented.HasValue ? raw.Dented.Value : false;
                        model.Hole = raw.Hole.HasValue ? raw.Hole.Value : false;

                        model.Qty_Pass = raw.Qty_Pass;
                        model.Qty_Pending = raw.Qty_Pending;
                        model.Qty_Reject = raw.Qty_Reject;
                        model.Status_Pass = raw.Status_Pass.HasValue ? raw.Status_Pass.Value : false;
                        model.Status_Pending = raw.Status_Pending.HasValue ? raw.Status_Pending.Value : false;
                        model.Status_Reject = raw.Status_Reject.HasValue ? raw.Status_Reject.Value : false;

                        model.Print_Qty_Pass = model.Qty_Pass;
                        model.Print_Qty_Pending = model.Qty_Pending;
                        model.Print_Qty_Reject = model.Qty_Reject;
                        model.Print_Pass = model.Status_Pass;
                        model.Print_Pending = model.Status_Pending;
                        model.Print_Reject = model.Status_Reject;

                        model.Total_Receiving = raw.Total_Receiving;
                        model.Receiving_Date = DateUtil.ToDisplayDate(raw.Receiving_Date);
                        model.Report_Date = DateUtil.ToDisplayDate(raw.Report_Date);
                        model.Lot_No = raw.Lot_No;
                        model.COA = raw.COA;
                        model.Expiring_Date = DateUtil.ToDisplayDate(raw.Expiring_Date);
                        model.Reject_Reason = raw.Reject_Reason;
                        model.Reject_Remarks = raw.Reject_Remarks;

                        model.Remarks_Pass = raw.Remarks_Pass;
                        model.Remarks_Pending = raw.Remarks_Pending;
                        model.Authorized_By = raw.Authorized_By;
                        model.Authorized_Date = DateUtil.ToDisplayDate(raw.Authorized_Date);

                        if (raw.Authorized_By.HasValue && raw.Authorized_By.Value > 0)
                        {
                            var User_Auth = uService.getUser(raw.Authorized_By);
                            if (User_Auth != null && User_Auth.Name != null)
                            {
                                model.Authorized_By_Name = User_Auth.Name;
                            }
                        }

                        model.UOM = raw.UOM;
                        if (raw.UOM.HasValue)
                            model.UOM_Name = raw.Global_Lookup_Data.Name;

                        model.Packaging = raw.Packaging;
                        if (raw.Packaging.HasValue)
                            model.Packaging_Name = raw.Global_Lookup_Data1.Name;

                        model.Analysis_Type = raw.Analysis_Type;

                        model.Attachment_Files = new List<MaterialFileAttach>();
                        if (raw.Raw_Material_Attachment != null)
                        {
                            foreach (var file in raw.Raw_Material_Attachment)
                            {
                                if (file.Record_Status != Record_Status.Delete)
                                {
                                    model.Attachment_Files.Add(new MaterialFileAttach()
                                    {
                                        Attachment_Field = file.Attachment_Field,
                                        File_Name = file.Attachment_File_Name,
                                        File = file.Attachment_File,
                                        Attachment_ID = file.Attachment_ID
                                    });
                                }
                            }

                        }
                    }
                }

            }
            if (!string.IsNullOrEmpty(model.Product_Code))
            {
                var rcri = new ProductTransCriteria();
                rcri.Product_Code = model.Product_Code;
                var result = exService.GetExchquerProductTrans(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                    model.ProductTrancList = result.Object as List<Product_Transaction>;

                if (string.IsNullOrEmpty(model.Product_Name))
                    model.Product_Name = model.ProductTrancList.Select(s => s.Product_Name).FirstOrDefault();

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult MaterialInfo(MaterialInfoViewModels model)
        {
            var uService = new UserService();
            var userlogin = uService.getUser(User.Identity.GetUserId());
            var currentdate = StoredProcedure.GetCurrentDate();
            var cbService = new ComboService();
            var files = Request.Files;

            var cktotal = false;
            decimal total = 0;
            if (model.Status_Pass || model.Status_Pending || model.Status_Reject)
            {
                cktotal = true;
            }

            if (cktotal)
            {
                total = total + (model.Qty_Pass.HasValue ? model.Qty_Pass.Value : 0);
                total = total + (model.Qty_Pending.HasValue ? model.Qty_Pending.Value : 0);
                total = total + (model.Qty_Reject.HasValue ? model.Qty_Reject.Value : 0);

                if (model.Total_Receiving != total)
                {
                    ModelState.AddModelError("Total_Qty", "Total Qty should be equal Total Receiving.");
                }

            }
            var mService = new MaterialService(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                var raw = new Raw_Material();
                if (model.operation == Operation.U)
                {
                    var rcri = new RawMaterialCriteria();
                    rcri.Raw_Material_ID = model.Raw_Material_ID;
                    var result = mService.GetRawMaterial(rcri);
                    if (result.Code == ReturnCode.SUCCESS)
                    {
                        var raws = result.Object as List<Raw_Material>;
                        if (raws != null && raws.Count() == 1)
                            raw = raws.FirstOrDefault();
                    }
                }

                raw.Product_Code = model.Product_Code;
                raw.Product_Name = model.Product_Name;

                raw.Dented = model.Dented;
                raw.Hole = model.Hole;

                raw.Qty_Pass = model.Qty_Pass;
                raw.Qty_Pending = model.Qty_Pending;
                raw.Qty_Reject = model.Qty_Reject;
                raw.Status_Pass = model.Status_Pass;
                raw.Status_Pending = model.Status_Pending;
                raw.Status_Reject = model.Status_Reject;
                raw.Report_Date = DateUtil.ToDate(model.Report_Date);
                raw.Total_Receiving = model.Total_Receiving;
                raw.UOM = model.UOM;
                raw.Receiving_Date = DateUtil.ToDate(model.Receiving_Date);
                raw.Lot_No = model.Lot_No;
                raw.COA = model.COA;

                raw.Reject_Reason = model.Reject_Reason;
                raw.Reject_Remarks = model.Reject_Remarks;

                raw.Expiring_Date = DateUtil.ToDate(model.Expiring_Date);

                raw.Remarks_Pass = model.Remarks_Pass;
                raw.Remarks_Pending = model.Remarks_Pending;
                raw.Packaging = model.Packaging;

                raw.Authorized_By = model.Authorized_By;
                raw.Authorized_Date = DateUtil.ToDate(model.Authorized_Date);
                raw.Analysis_Type = model.Analysis_Type;

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
                    var Raw_Attachment = new List<Raw_Material_Attachment>();
                    if (files != null)
                    {
                        for (var i = 0; i < Request.Files.Count; i++)
                        {
                            var file = Request.Files[i];
                            var target = new MemoryStream();
                            file.InputStream.CopyTo(target);
                            var data = target.ToArray();
                            var key = Request.Files.Keys[i];

                            var filetype = "";
                            if (key.Contains(Attachment_Field_Name.COA))
                                filetype = Attachment_Field_Name.COA;
                            else if (key.Contains(Attachment_Field_Name.Invoice))
                                filetype = Attachment_Field_Name.Invoice;
                            else if (key.Contains(Attachment_Field_Name.Packing))
                                filetype = Attachment_Field_Name.Packing;
                            if (data != null && data.Length > 0)
                            {
                                Raw_Attachment.Add(new Raw_Material_Attachment()
                                {
                                    Attachment_ID = Guid.NewGuid(),
                                    Raw_Material_ID = raw.Raw_Material_ID,
                                    Attachment_File = data,
                                    Attachment_Field = filetype,
                                    Attachment_File_Name = file.FileName,
                                });
                            }

                        }
                        raw.Raw_Material_Attachment = Raw_Attachment;
                    }

                    Raw_Material_Form pendingform = null;
                    Raw_Material_Form rejform = null;
                    Raw_Material_Form passform = null;

                    if (raw.Status_Pending == true)
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            model.Selected_Status = Material_Status.Pending;
                            var htmlToConvert = RenderPartialViewAsString("MaterialForm2017", model);
                            StringReader sr = new StringReader(htmlToConvert);
                            Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            var writer = PdfWriter.GetInstance(pdfDoc, ms);
                            var pageevent = new PDFPageEvent();
                            writer.PageEvent = pageevent;
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            pendingform = new Raw_Material_Form();
                            pendingform.Create_By = userlogin.Email_Address;
                            pendingform.Create_On = currentdate;
                            pendingform.File_Name = "RAWPEND" + raw.Raw_Material_ID + ".pdf";
                            pendingform.File = ms.ToArray();
                            pendingform.Status = Material_Status.Pending;
                            pendingform.Form_ID = Guid.NewGuid();
                            raw.Raw_Material_Form.Add(pendingform);
                        }
                    }
                    if (raw.Status_Reject == true)
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            model.Selected_Status = Material_Status.Reject;
                            var htmlToConvert = RenderPartialViewAsString("MaterialForm2017", model);
                            StringReader sr = new StringReader(htmlToConvert);
                            Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            var writer = PdfWriter.GetInstance(pdfDoc, ms);
                            var pageevent = new PDFPageEvent();
                            writer.PageEvent = pageevent;
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            rejform = new Raw_Material_Form();
                            rejform.Create_By = userlogin.Email_Address;
                            rejform.Create_On = currentdate;
                            rejform.File_Name = "RAWREJ" + raw.Raw_Material_ID + ".pdf";
                            rejform.File = ms.ToArray();
                            rejform.Status = Material_Status.Reject;
                            rejform.Form_ID = Guid.NewGuid();
                            raw.Raw_Material_Form.Add(rejform);
                        }
                    }

                    if (raw.Status_Pass == true)
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            model.Selected_Status = Material_Status.Passed;
                            var htmlToConvert = RenderPartialViewAsString("MaterialForm2017", model);
                            StringReader sr = new StringReader(htmlToConvert);
                            Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            var writer = PdfWriter.GetInstance(pdfDoc, ms);
                            var pageevent = new PDFPageEvent();
                            writer.PageEvent = pageevent;
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            passform = new Raw_Material_Form();
                            passform.Create_By = userlogin.Email_Address;
                            passform.Create_On = currentdate;
                            passform.File_Name = "RAWPASS" + raw.Raw_Material_ID + ".pdf";
                            passform.File = ms.ToArray();
                            passform.Status = Material_Status.Passed;
                            passform.Form_ID = Guid.NewGuid();
                            raw.Raw_Material_Form.Add(passform);
                        }
                    }


                    model.result = mService.InsertRawMaterial(raw);
                    if (model.result.Code == ReturnCode.SUCCESS)
                    {
                        if (model.savemode == "save")
                            return RedirectToAction("Material", new RouteValueDictionary(model.result));
                        else
                            return RedirectToAction("MaterialInfo", new { pRawID = raw.Raw_Material_ID, pShowPrintDlg = true, operation = Operation.U });

                    }
                }
                else if (model.operation == Operation.U)
                {
                    Raw_Material_Form pendingform = null;
                    Raw_Material_Form rejform = null;
                    Raw_Material_Form passform = null;


                    if (raw.Status_Pending == true)
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            model.Selected_Status = Material_Status.Pending;
                            var htmlToConvert = RenderPartialViewAsString("MaterialForm2017", model);
                            StringReader sr = new StringReader(htmlToConvert);
                            Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            var writer = PdfWriter.GetInstance(pdfDoc, ms);
                            var pageevent = new PDFPageEvent();
                            writer.PageEvent = pageevent;
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            pendingform = new Raw_Material_Form();
                            pendingform.Create_By = userlogin.Email_Address;
                            pendingform.Create_On = currentdate;
                            pendingform.File_Name = "RAWPEND" + raw.Raw_Material_ID + ".pdf";
                            pendingform.File = ms.ToArray();
                            pendingform.Status = Material_Status.Pending;
                            pendingform.Raw_Material_ID = raw.Raw_Material_ID;
                            pendingform.Form_ID = Guid.NewGuid();
                        }
                    }
                    if (raw.Status_Reject == true)
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            model.Selected_Status = Material_Status.Reject;
                            var htmlToConvert = RenderPartialViewAsString("MaterialForm2017", model);
                            StringReader sr = new StringReader(htmlToConvert);
                            Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            var writer = PdfWriter.GetInstance(pdfDoc, ms);
                            var pageevent = new PDFPageEvent();
                            writer.PageEvent = pageevent;
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            rejform = new Raw_Material_Form();
                            rejform.Create_By = userlogin.Email_Address;
                            rejform.Create_On = currentdate;
                            rejform.File_Name = "RAWREJ" + raw.Raw_Material_ID + ".pdf";
                            rejform.File = ms.ToArray();
                            rejform.Status = Material_Status.Reject;
                            rejform.Raw_Material_ID = raw.Raw_Material_ID;
                            rejform.Form_ID = Guid.NewGuid();
                        }
                    }

                    if (raw.Status_Pass == true)
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            model.Selected_Status = Material_Status.Passed;
                            var htmlToConvert = RenderPartialViewAsString("MaterialForm2017", model);
                            StringReader sr = new StringReader(htmlToConvert);
                            Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            var writer = PdfWriter.GetInstance(pdfDoc, ms);
                            var pageevent = new PDFPageEvent();
                            writer.PageEvent = pageevent;
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            passform = new Raw_Material_Form();
                            passform.Create_By = userlogin.Email_Address;
                            passform.Create_On = currentdate;
                            passform.File_Name = "RAWPASS" + raw.Raw_Material_ID + ".pdf";
                            passform.File = ms.ToArray();
                            passform.Status = Material_Status.Passed;
                            passform.Raw_Material_ID = raw.Raw_Material_ID;
                            passform.Form_ID = Guid.NewGuid();
                        }
                    }

                    model.result = mService.UpdateRawMaterial(raw, pendingform, rejform, passform);
                    if (model.result.Code == ReturnCode.SUCCESS)
                    {
                        if (model.savemode == "save")
                            return RedirectToAction("Material", new RouteValueDictionary(model.result));
                        else
                            return RedirectToAction("MaterialInfo", new { pRawID = raw.Raw_Material_ID, pShowPrintDlg = true, operation = Operation.U });
                    }
                }
            }

            model.Attachment_Files = new List<MaterialFileAttach>();
            if (model.operation == Operation.U)
            {
                var raw = new Raw_Material();
                var rcri = new RawMaterialCriteria();
                rcri.Raw_Material_ID = model.Raw_Material_ID;
                var result = mService.GetRawMaterial(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var raws = result.Object as List<Raw_Material>;
                    if (raws != null && raws.Count() == 1)
                        raw = raws.FirstOrDefault();
                }
                if (raw.Raw_Material_Attachment != null)
                {
                    foreach (var file in raw.Raw_Material_Attachment)
                    {
                        model.Attachment_Files.Add(new MaterialFileAttach()
                        {
                            Attachment_Field = file.Attachment_Field,
                            File_Name = file.Attachment_File_Name,
                            File = file.Attachment_File,
                            Attachment_ID = file.Attachment_ID
                        });
                    }

                }
            }
            var exService = new ExchequerService();
            if (!string.IsNullOrEmpty(model.Product_Code))
            {
                var rcri = new ProductTransCriteria();
                rcri.Product_Code = model.Product_Code;
                var result = exService.GetExchquerProductTrans(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                    model.ProductTrancList = result.Object as List<Product_Transaction>;

                if (string.IsNullOrEmpty(model.Product_Name))
                    model.Product_Name = model.ProductTrancList.Select(s => s.Product_Name).FirstOrDefault();
            }

            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0001);
            if (prole != null)
                model.Modify = prole.Modify;

            model.cProductlist = exService.LstExchquerProduct(true);
            model.cPackaginglist = cbService.LstLookupData(LookupType.Packaging, true);
            model.cUOMlist = cbService.LstLookupData(LookupType.UOM, true);
            return View(model);
        }

        public ActionResult MaterialAuthorized(int? pRawID, int? pPID)
        {
            var uService = new UserService();
            var userlogin = uService.getUser(User.Identity.GetUserId());
            if (userlogin != null)
            {
                var mService = new MaterialService(User.Identity.GetUserId());
                var rcri = new RawMaterialCriteria();
                rcri.Raw_Material_ID = pRawID;
                var result = mService.GetRawMaterial(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var raws = result.Object as List<Raw_Material>;
                    if (raws != null && raws.Count() == 1)
                    {
                        var raw = raws.FirstOrDefault();
                        if (raw != null)
                        {
                            var currentdate = StoredProcedure.GetCurrentDate();
                            raw.Authorized_Date = currentdate;
                            raw.Authorized_By = pPID;
                            raw.Update_On = currentdate;
                            raw.Update_By = userlogin.Email_Address;
                            mService.UpdateRawMaterial(raw);
                        }
                    }
                }
            }

            return Json(new
            {
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void MaterialDisplayFile(Nullable<System.Guid> pAttID)
        {
            if (pAttID.HasValue)
            {
                var mService = new MaterialService();
                var attact = mService.GetRawMaterialAttachment(pAttID);
                if (attact != null)
                {
                    var file = attact.Attachment_File;
                    if (attact.Attachment_File_Name.Contains(".pdf") || attact.Attachment_File_Name.Contains(".PDF"))
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

        public ActionResult MaterialAddFile(int pIndex, string pAttType, bool? pModify)
        {
            var model = new MaterialFileAttach();

            model.Index = pIndex;
            model.Attachment_Field = pAttType;
            model.Modify = pModify;
            return PartialView("UploadRow", model);
        }

        public ActionResult MaterialSaveFile(Nullable<Guid> pAttID, Nullable<int> pRawID, string pField)
        {
            if (pRawID.HasValue)
            {
                HttpPostedFileBase file = Request.Files[0];

                int fileSizeInBytes = file.ContentLength;
                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] data = target.ToArray();

                if (data != null && data.Length > 0)
                {
                    var mService = new MaterialService(User.Identity.GetUserId());

                    if (pAttID.HasValue)
                    {
                        var ck = mService.GetRawMaterialAttachment(pAttID);
                        if (ck == null)
                        {
                            Nullable<Guid> attID = null;
                            if (pRawID.HasValue)
                                mService.InsertRawMaterialAttachment(ref attID, pRawID, data, file.FileName, pField);
                            return Json(attID, JsonRequestBehavior.AllowGet);
                        }
                        mService.UpdateRawMaterialAttachment(pAttID, data, file.FileName);
                        return Json(pAttID.Value, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Nullable<Guid> attID = null;
                        if (pRawID.HasValue)
                            mService.InsertRawMaterialAttachment(ref attID, pRawID, data, file.FileName, pField);
                        return Json(attID, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void RepairRawMaterialForm()
        {
            var user = new UserService(User.Identity.GetUserId());
            var mService = new MaterialService();
            var cri = new RawMaterialCriteria();
            var result2 = mService.GetRawMaterial(cri);
            if (result2.Code == ReturnCode.SUCCESS)
            {
                var raws = result2.Object as List<Raw_Material>;
                if (raws != null)
                {
                    foreach (var raw in raws)
                    {
                        var model = new MaterialInfoViewModels();
                        model.Raw_Material_ID = raw.Raw_Material_ID;
                        model.Product_Code = raw.Product_Code;
                        model.Product_Name = raw.Product_Name;
                        model.Dented = raw.Dented.HasValue ? raw.Dented.Value : false;
                        model.Hole = raw.Hole.HasValue ? raw.Hole.Value : false;
                        model.Qty_Pass = raw.Qty_Pass;
                        model.Qty_Pending = raw.Qty_Pending;
                        model.Qty_Reject = raw.Qty_Reject;
                        model.Status_Pass = raw.Status_Pass.HasValue ? raw.Status_Pass.Value : false;
                        model.Status_Pending = raw.Status_Pending.HasValue ? raw.Status_Pending.Value : false;
                        model.Status_Reject = raw.Status_Reject.HasValue ? raw.Status_Reject.Value : false;
                        model.Print_Qty_Pass = model.Qty_Pass;
                        model.Print_Qty_Pending = model.Qty_Pending;
                        model.Print_Qty_Reject = model.Qty_Reject;
                        model.Print_Pass = model.Status_Pass;
                        model.Print_Pending = model.Status_Pending;
                        model.Print_Reject = model.Status_Reject;
                        model.Total_Receiving = raw.Total_Receiving;
                        model.Report_Date = DateUtil.ToDisplayDate(raw.Report_Date);
                        model.Receiving_Date = DateUtil.ToDisplayDate(raw.Receiving_Date);
                        model.Lot_No = raw.Lot_No;
                        model.COA = raw.COA;
                        model.Create_On = DateUtil.ToDisplayDate(StoredProcedure.GetCurrentDate());
                        model.Expiring_Date = DateUtil.ToDisplayDate(raw.Expiring_Date);

                        model.Remarks_Pass = raw.Remarks_Pass;
                        model.Remarks_Pending = raw.Remarks_Pending;

                        if (string.IsNullOrEmpty(model.Remarks_Pass))
                            model.Remarks_Pass = Resource.NA;
                        if (string.IsNullOrEmpty(model.Remarks_Pending))
                            model.Remarks_Pending = Resource.NA;

                        model.UOM = raw.UOM;
                        if (raw.UOM.HasValue)
                            model.UOM_Name = raw.Global_Lookup_Data.Name;

                        model.Packaging = raw.Packaging;
                        if (raw.Packaging.HasValue)
                            model.Packaging_Name = raw.Global_Lookup_Data1.Name;

                        var username = user.getUser(raw.Authorized_By);
                        if (username != null && username.Name != null)
                            model.Authorized_By_Name = username.Name;


                        if (raw.Status_Pending == true)
                        {
                            model.Reject_Remarks = raw.Remarks_Pending;
                            model.Selected_Status = Material_Status.Pending;
                            var pendingform = mService.GetRawMaterialForm(raw.Raw_Material_ID, Material_Status.Pending);
                            if (pendingform == null)
                            {
                                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                                {
                                    var htmlToConvert = RenderPartialViewAsString("MaterialForm", model);
                                    StringReader sr = new StringReader(htmlToConvert);
                                    Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                                    var writer = PdfWriter.GetInstance(pdfDoc, ms);
                                    var pageevent = new PDFPageEvent();
                                    writer.PageEvent = pageevent;
                                    pdfDoc.Open();
                                    htmlparser.Parse(sr);
                                    pdfDoc.Close();
                                    pendingform = new Raw_Material_Form();
                                    pendingform.Raw_Material_ID = raw.Raw_Material_ID;
                                    pendingform.Create_By = raw.Create_By;
                                    pendingform.Create_On = raw.Create_On;
                                    pendingform.File_Name = "RAWPEND" + raw.Raw_Material_ID + ".pdf";
                                    pendingform.File = ms.ToArray();
                                    pendingform.Status = Material_Status.Pending;
                                    pendingform.Form_ID = Guid.NewGuid();
                                    var result = mService.InsertRawMaterialForm(pendingform);
                                }
                            }

                        }
                        if (raw.Status_Reject == true)
                        {
                            model.Reject_Remarks = raw.Reject_Remarks;
                            model.Reject_Reason = raw.Reject_Reason;
                            model.Selected_Status = Material_Status.Reject;
                            var rejform = mService.GetRawMaterialForm(raw.Raw_Material_ID, Material_Status.Reject);
                            if (rejform == null)
                            {
                                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                                {
                                    var htmlToConvert = RenderPartialViewAsString("MaterialForm", model);
                                    StringReader sr = new StringReader(htmlToConvert);
                                    Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                                    var writer = PdfWriter.GetInstance(pdfDoc, ms);
                                    var pageevent = new PDFPageEvent();
                                    writer.PageEvent = pageevent;
                                    pdfDoc.Open();
                                    htmlparser.Parse(sr);
                                    PdfPTable table = new PdfPTable(1);
                                    table.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    table.TotalWidth = 300;
                                    PdfPCell Cell = new PdfPCell(table.DefaultCell);
                                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                    Font fontH1 = new Font(bf, 10, Font.NORMAL);
                                    Font fontAp = new Font(bf, 9, Font.BOLD);
                                    Cell = new PdfPCell(new Phrase("**Delete where applicable"));
                                    Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    Cell.PaddingBottom = 4f;
                                    Cell.BorderWidth = 0;
                                    table.AddCell(Cell);
                                    PdfContentByte cb = writer.DirectContent;
                                    table.WriteSelectedRows(0, -1, 30, (pdfDoc.BottomMargin + 80), cb);
                                    pdfDoc.Close();

                                    rejform = new Raw_Material_Form();
                                    rejform.Raw_Material_ID = raw.Raw_Material_ID;
                                    rejform.Create_By = raw.Create_By;
                                    rejform.Create_On = raw.Create_On;
                                    rejform.File_Name = "RAWREJ" + raw.Raw_Material_ID + ".pdf";
                                    rejform.File = ms.ToArray();
                                    rejform.Status = Material_Status.Reject;
                                    rejform.Form_ID = Guid.NewGuid();
                                    var result = mService.InsertRawMaterialForm(rejform);
                                }
                            }
                        }

                        if (raw.Status_Pass == true)
                        {
                            model.Reject_Remarks = raw.Remarks_Pass;
                            model.Selected_Status = Material_Status.Passed;
                            var passform = mService.GetRawMaterialForm(raw.Raw_Material_ID, Material_Status.Passed);
                            if (passform == null)
                            {
                                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                                {
                                    var htmlToConvert = RenderPartialViewAsString("MaterialForm", model);
                                    StringReader sr = new StringReader(htmlToConvert);
                                    Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                                    var writer = PdfWriter.GetInstance(pdfDoc, ms);
                                    var pageevent = new PDFPageEvent();
                                    writer.PageEvent = pageevent;
                                    pdfDoc.Open();
                                    htmlparser.Parse(sr);
                                    pdfDoc.Close();
                                    passform = new Raw_Material_Form();
                                    passform.Raw_Material_ID = raw.Raw_Material_ID;
                                    passform.Create_By = raw.Create_By;
                                    passform.Create_On = raw.Create_On;
                                    passform.File_Name = "RAWPASS" + raw.Raw_Material_ID + ".pdf";
                                    passform.File = ms.ToArray();
                                    passform.Status = Material_Status.Passed;
                                    passform.Form_ID = Guid.NewGuid();
                                    var result = mService.InsertRawMaterialForm(passform);
                                }
                            }

                        }
                    }
                }
            }
        }

        [HttpGet]
        public void MaterialForm(int? pRawID = null, string pSelStatus = "")
        {
            var user = new UserService(User.Identity.GetUserId());
            var mService = new MaterialService();


            var form = mService.GetRawMaterialForm(pRawID, pSelStatus);
            if (form != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "inline; filename=" + form.File_Name);
                Response.BinaryWrite(form.File.ToArray());
                Response.Flush();
                Response.Close();
            }



            ////if (pSelStatus == Material_Status.Reject)
            ////{
            //PdfPTable table = new PdfPTable(1);
            //table.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            //table.TotalWidth = 300;
            //PdfPCell Cell = new PdfPCell(table.DefaultCell);
            //BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            //Font fontH1 = new Font(bf, 10, Font.NORMAL);
            //Font fontAp = new Font(bf, 9, Font.BOLD);
            //Cell = new PdfPCell(new Phrase("**Delete where applicable"));
            //Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            //Cell.PaddingBottom = 4f;
            //Cell.BorderWidth = 0;
            //table.AddCell(Cell);
            //PdfContentByte cb = writer.DirectContent;
            //table.WriteSelectedRows(0, -1, 30, (pdfDoc.BottomMargin + 80), cb);
            ////}
            //pdfDoc.Close();
        }

        [HttpGet]
        public ActionResult MaterialReject(MaterialRejectViewModels model, ServiceResult msgresult)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0002);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            model.result = msgresult;
            model.Modify = prole.Modify;
            model.View = prole.View;

            var mService = new MaterialService();
            var cri = new MaterialRejectCriteria();
            cri.Product_Search = model.Product_Code;
            cri.Date_From = model.Date_From;
            cri.Date_To = model.Date_To;

            var result = mService.GetMaterialReject(cri);
            if (result.Code == ReturnCode.SUCCESS)
                model.MaterialRejList = result.Object as List<Material_Reject>;


            var exService = new ExchequerService();
            model.cProductlist = exService.LstExchquerProduct(true);

            return View(model);
        }

        [HttpGet]
        public ActionResult MaterialRejectInfo(Nullable<int> pRawID, Nullable<int> pRejID, string pCode)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0002);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            var model = new MaterialRejectInfoViewModels();

            var mService = new MaterialService();
            var cService = new ComboService();
            Raw_Material raw = null;

            model.operation = Operation.C;
            model.Modify = prole.Modify;
            model.View = prole.View;
            model.Close = (prole.Close.HasValue ? prole.Close.Value : false);
            model.Role_Name = prole.Role.Role_Name;

            var exService = new ExchequerService();
            model.cProductlist = exService.LstExchquerProduct(true);
            var cbService = new ComboService();
            model.cPackaginglist = cbService.LstLookupData(LookupType.Packaging, true);
            model.cUOMlist = cbService.LstLookupData(LookupType.UOM, true);
            if (!string.IsNullOrEmpty(pCode)) // Search product from Exchequer
                model.Product_Code = pCode;

            if (pRawID.HasValue)
            {
                var rcri = new RawMaterialCriteria();
                rcri.Raw_Material_ID = pRawID;
                var result = mService.GetRawMaterial(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var raws = result.Object as List<Raw_Material>;
                    if (raws != null && raws.Count() == 1)
                    {
                        raw = raws.FirstOrDefault();
                        model.Raw_Material_ID = raw.Raw_Material_ID;
                        model.Product_Code = raw.Product_Code;
                        model.Product_Name = raw.Product_Name;
                        model.Quantity = raw.Qty_Reject;
                        model.Lot_No = raw.Lot_No;
                        //model.Remarks = raw.Reject_Reason;
                        model.UOM = raw.UOM;
                        model.Packaging = raw.Packaging;
                    }
                }
            }

            if (pRejID.HasValue)
            {
                model.operation = Operation.U;
                var cri2 = new MaterialRejectCriteria();
                cri2.Material_Reject_ID = pRejID;
                var result2 = mService.GetMaterialReject(cri2);
                if (result2.Code == ReturnCode.SUCCESS)
                {
                    var rejs = result2.Object as List<Material_Reject>;
                    if (rejs != null && rejs.Count() == 1)
                    {
                        var rej = rejs.FirstOrDefault();
                        model.Raw_Material_ID = rej.Raw_Material_ID;
                        model.Product_Code = rej.Product_Code;
                        model.Product_Name = rej.Product_Name;
                        model.Quantity = rej.Quantity;
                        model.Lot_No = rej.Lot_No;
                        model.Material_Reject_ID = rej.Material_Reject_ID;
                        model.RMR_No = rej.RMR_No;
                        model.UOM = rej.UOM;
                        if (rej.UOM.HasValue)
                            model.UOM_Name = rej.Global_Lookup_Data.Name;
                        model.Packaging = rej.Packaging;
                        if (rej.Packaging.HasValue)
                            model.Packaging_Name = rej.Global_Lookup_Data1.Name;
                        model.Reject_From = rej.Reject_From != null ? rej.Reject_From : Reject_From_Type.Customer;

                        if (model.Reject_From == Reject_From_Type.Customer)
                            model.Reject_Customer_Name = rej.Reject_Customer_Name;
                        else if (model.Reject_From == Reject_From_Type.Supplier)
                            model.Reject_Supplier_Name = rej.Reject_Supplier_Name;
                        else if (model.Reject_From == Reject_From_Type.Inhouse)
                            model.Reject_Inhouse_Location = rej.Reject_Inhouse_Location;

                        if (!string.IsNullOrEmpty(rej.Remarks))
                            model.Remarks = rej.Remarks;

                        model.QA_Staff = rej.QA_Staff;
                        model.D8_No = rej.D8_No;
                        model.Disposition_RTS = rej.Disposition_RTS;
                        model.Disposition_Rework = rej.Disposition_Rework;
                        model.Disposition_Scrap = rej.Disposition_Scrap;
                        model.Disposition_UAI = rej.Disposition_UAI;
                        model.Disposition_Others = rej.Disposition_Others;

                        if (rej.Disposition_Others.HasValue && rej.Disposition_Others.Value)
                            model.Disposition_Others_Description = rej.Disposition_Others_Description;

                        model.Instructions = rej.Instructions;
                        model.PD = rej.PD.HasValue ? rej.PD.Value : false;
                        model.QA = rej.QA.HasValue ? rej.QA.Value : false;
                        model.Logistics = rej.Logistics.HasValue ? rej.Logistics.Value : false;
                        model.Sales = rej.Sales.HasValue ? rej.Sales.Value : false;
                        model.Disposition_Action_By = rej.Disposition_Action_By;
                        model.Disposition_Date = DateUtil.ToDisplayDate(rej.Disposition_Date);
                        model.Re_Inspection_On_Rework = rej.Re_Inspection_On_Rework;
                        model.Reject_Status = rej.Reject_Status != null ? rej.Reject_Status : "Pass";
                        model.Carried_Out_By = rej.Carried_Out_By;
                        model.Carried_Out_Date = DateUtil.ToDisplayDate(rej.Carried_Out_Date);
                        model.Review_Date = DateUtil.ToDisplayDate(rej.Review_Date);
                        model.operation = Operation.U;
                        model.Overall_Status = rej.Overall_Status;
                        model.Create_By = rej.Create_By;
                        model.Create_On = DateUtil.ToDisplayDate(rej.Create_On);
                        model.Update_By = rej.Update_By;
                        model.Update_On = DateUtil.ToDisplayDate(rej.Update_On);
                        model.Defect_Description = rej.Defect_Description;
                        model.Project_Name = "";

                        if (model.PD.Value)
                        {
                            model.PD_Approval = rej.PD_Approval;
                            model.PD_Date = DateUtil.ToDisplayDate(rej.PD_Date);
                            if (rej.User_Profile != null)
                                model.PD_Approval_Name = rej.User_Profile.Name;
                        }
                        if (model.QA.Value)
                        {
                            model.QA_Date = DateUtil.ToDisplayDate(rej.QA_Date);
                            model.QA_Approval = rej.QA_Approval;
                            if (rej.User_Profile1 != null)
                                model.QA_Approval_Name = rej.User_Profile1.Name;
                        }
                        if (model.Logistics.Value)
                        {
                            model.Logistics_Date = DateUtil.ToDisplayDate(rej.Logistics_Date);
                            model.Logistics_Approval = rej.Logistics_Approval;
                            if (rej.User_Profile2 != null)
                                model.Logistics_Approval_Name = rej.User_Profile2.Name;
                        }
                        if (model.Sales.Value)
                        {
                            model.Sales_Date = DateUtil.ToDisplayDate(rej.Sales_Date);
                            model.Sales_Approval = rej.Sales_Approval;

                            if (rej.User_Profile3 != null)
                                model.Sales_Approval_Name = rej.User_Profile3.Name;
                        }

                        model.GM_Approval_Date = DateUtil.ToDisplayDate(rej.GM_Approval_Date);
                        model.GM_Approval = rej.GM_Approval;
                        if (rej.User_Profile4 != null)
                            model.GM_Approval_Name = rej.User_Profile4.Name;
                    }

                    model.cRejectStatuslist = cService.LstRejectStatus(true);
                }
            }
            else
            {
                model.operation = Operation.C;
                model.Reject_Status = "Pass";
                model.Reject_From = Reject_From_Type.Customer;
            }

            //Search product from Exchequer
            if (!string.IsNullOrEmpty(model.Product_Code))
            {
                var rcri = new ProductTransCriteria();
                rcri.Product_Code = model.Product_Code;
                var result = exService.GetExchquerProductTrans(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                    model.ProductTrancList = result.Object as List<Product_Transaction>;

                if (string.IsNullOrEmpty(model.Product_Name))
                    model.Product_Name = model.ProductTrancList.Select(s => s.Product_Name).FirstOrDefault();
            }

            return View(model);
        }

        [HttpGet]
        public JsonResult TestSendEmail()
        {
            var uService = new UserService();
            var userlogin = uService.getUser(User.Identity.GetUserId());
            if (userlogin == null)
            {
                return Json(new
                {
                    Message = "userlogin has not found!"
                }, JsonRequestBehavior.AllowGet);
            }
            userlogin.Email_Address = ConfigurationManager.AppSettings["TEST_EMAIL"].ToString();
            var emailCri = new EmailCriteria()
            {
                Lot_No = "Lot-Test-000x",
                Status = Material_Overall_Status.Closed
            };
            var msg = EmailAgnos.sendClosedReject(userlogin, GetRecipientsNotification(), emailCri);
            return Json(new
            {
                Message = msg
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MaterialRejectInfo(MaterialRejectInfoViewModels model, string pStatus = "")
        {
            var uService = new UserService();
            if (ModelState.IsValid)
            {
                var currentdate = StoredProcedure.GetCurrentDate();
                var userlogin = uService.getUser(User.Identity.GetUserId());

                var mService = new MaterialService(User.Identity.GetUserId());
                var rej = new Material_Reject();
                if (model.operation == Operation.U)
                {
                    var rcri = new MaterialRejectCriteria();
                    rcri.Raw_Material_ID = model.Raw_Material_ID;
                    rcri.Material_Reject_ID = model.Material_Reject_ID;
                    var result = mService.GetMaterialReject(rcri);
                    if (result.Code == ReturnCode.SUCCESS)
                    {
                        var rejs = result.Object as List<Material_Reject>;
                        if (rejs != null && rejs.Count() == 1)
                            rej = rejs.FirstOrDefault();
                    }
                }

                if (rej.Overall_Status == Material_Overall_Status.Closed && pStatus == Fixed_Action.Update_Remark_Only)
                    rej.Remarks = model.Remarks;
                else
                {
                    rej.Raw_Material_ID = model.Raw_Material_ID;
                    rej.Packaging = model.Packaging;
                    rej.Reject_From = model.Reject_From;
                    rej.Reject_Customer_Name = model.Reject_Customer_Name;
                    rej.Reject_Supplier_Name = model.Reject_Supplier_Name;
                    rej.Reject_Inhouse_Location = model.Reject_Inhouse_Location;
                    rej.QA_Staff = model.QA_Staff;
                    rej.D8_No = model.D8_No;
                    rej.Disposition_RTS = model.Disposition_RTS;
                    rej.Disposition_Rework = model.Disposition_Rework;
                    rej.Disposition_Scrap = model.Disposition_Scrap;
                    rej.Disposition_UAI = model.Disposition_UAI;
                    rej.Disposition_Others = model.Disposition_Others;
                    rej.Disposition_Others_Description = model.Disposition_Others_Description;
                    rej.Instructions = model.Instructions;
                    rej.PD = model.PD;
                    rej.QA = model.QA;
                    rej.Logistics = model.Logistics;
                    rej.Sales = model.Sales;
                    rej.PD_Date = DateUtil.ToDate(model.PD_Date);
                    rej.QA_Date = DateUtil.ToDate(model.QA_Date);
                    rej.Logistics_Date = DateUtil.ToDate(model.Logistics_Date);
                    rej.Sales_Date = DateUtil.ToDate(model.Sales_Date);

                    rej.Disposition_Action_By = model.Disposition_Action_By;
                    rej.Disposition_Date = DateUtil.ToDate(model.Disposition_Date);
                    rej.Re_Inspection_On_Rework = model.Re_Inspection_On_Rework;
                    rej.Reject_Status = model.Reject_Status;
                    rej.Carried_Out_By = model.Carried_Out_By;
                    rej.Carried_Out_Date = DateUtil.ToDate(model.Carried_Out_Date);
                    rej.Review_Date = DateUtil.ToDate(model.Review_Date);
                    rej.Overall_Status = pStatus;
                    rej.Remarks = model.Remarks;
                    rej.Product_Code = model.Product_Code;
                    rej.Product_Name = model.Product_Name;
                    rej.Lot_No = model.Lot_No;
                    rej.Quantity = model.Quantity;
                    rej.Defect_Description = model.Defect_Description;
                    rej.Project_Name = ""; //model.Project_Name;
                    rej.UOM = model.UOM;

                    if (model.PD.HasValue && model.PD.Value && model.Role_Name == Role_Name.PD)
                        rej.PD_Approval = userlogin.Profile_ID;
                    else if (model.QA.HasValue && model.QA.Value && model.Role_Name == Role_Name.QA)
                        rej.QA_Approval = userlogin.Profile_ID;
                    else if (model.Logistics.HasValue && model.Logistics.Value && model.Role_Name == Role_Name.Logistics)
                        rej.Logistics_Approval = userlogin.Profile_ID;
                    else if (model.Sales.HasValue && model.Sales.Value && model.Role_Name == Role_Name.Sales)
                        rej.Sales_Approval = userlogin.Profile_ID;
                    else if (pStatus == Material_Overall_Status.Closed && model.Role_Name == Role_Name.GM)
                        rej.GM_Approval = userlogin.Profile_ID;

                    rej.GM_Approval_Date = DateUtil.ToDate(model.GM_Approval_Date);
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
                    model.result = mService.InsertMaterialReject(rej);
                    if (model.result.Code == ReturnCode.SUCCESS)
                    {
                        model.RMR_No = rej.RMR_No;
                        model.Create_On = DateUtil.ToDisplayDate(rej.Create_On);
                        model.Create_By = rej.Create_By;
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            var htmlToConvert = RenderPartialViewAsString("MaterialRejectForm2017", model);
                            StringReader sr = new StringReader(htmlToConvert);
                            Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            var writer = PdfWriter.GetInstance(pdfDoc, ms);
                            var pageevent = new PDFPageEvent();
                            writer.PageEvent = pageevent;
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();

                            var form = new Material_Reject_Form();
                            form.Create_By = userlogin.Email_Address;
                            form.Create_On = currentdate;
                            form.File_Name = "MREJ" + rej.Material_Reject_ID + ".pdf";
                            form.Form_ID = Guid.NewGuid();
                            form.File = ms.ToArray();
                            rej.Material_Reject_Form.Add(form);
                        }
                    }

                    if (model.result.Code == ReturnCode.SUCCESS)
                    {

                        var emailCri = new EmailCriteria()
                        {
                            Lot_No = rej.Lot_No,
                            Status = rej.Overall_Status,
                            from = userlogin.Email_Address
                        };
                        var IsSuccess = EmailAgnos.sendClosedReject(userlogin, GetRecipientsNotification(), emailCri);
                        if (IsSuccess != "Success")
                            model.result = new ServiceResult() { Code = ReturnCode.ERROR_CANT_SEND_EMAIL, Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE) + Error.GetMessage(ReturnCode.ERROR_CANT_SEND_EMAIL), Field = Resource.Material_Reject };

                    }

                    return RedirectToAction("MaterialReject", new RouteValueDictionary(model.result));
                }
                else if (model.operation == Operation.U)
                {
                    Material_Reject_Form form = null;
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        model.Create_On = DateUtil.ToDisplayDate(rej.Create_On);
                        model.Create_By = rej.Create_By;
                        model.Update_On = DateUtil.ToDisplayDate(rej.Update_On);
                        model.Update_By = rej.Update_By;
                        var htmlToConvert = RenderPartialViewAsString("MaterialRejectForm2017", model);
                        StringReader sr = new StringReader(htmlToConvert);
                        Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        var writer = PdfWriter.GetInstance(pdfDoc, ms);
                        var pageevent = new PDFPageEvent();
                        writer.PageEvent = pageevent;
                        pdfDoc.Open();
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        form = new Material_Reject_Form();
                        form.Create_By = userlogin.Email_Address;
                        form.Create_On = currentdate;
                        form.File_Name = "MREJ" + rej.Material_Reject_ID + ".pdf";
                        form.Form_ID = Guid.NewGuid();
                        form.Material_Reject_ID = rej.Material_Reject_ID;
                        form.File = ms.ToArray();
                    }

                    model.result = mService.UpdateMaterialReject(rej, form);
                    if (model.result.Code == ReturnCode.SUCCESS)
                    {
                        if (rej.Overall_Status == Material_Overall_Status.Closed && pStatus != Fixed_Action.Update_Remark_Only)
                        {
                            //sent to GM (status closed submit by GM role)
                            var emailCri = new EmailCriteria()
                            {
                                Lot_No = rej.Lot_No,
                                Status = rej.Overall_Status,
                                from = userlogin.Email_Address
                            };
                            var IsSuccess = EmailAgnos.sendClosedReject(userlogin, GetRecipientsNotification(), emailCri);
                            if (IsSuccess != "Success")
                                model.result = new ServiceResult() { Code = ReturnCode.ERROR_CANT_SEND_EMAIL, Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE) + Error.GetMessage(ReturnCode.ERROR_CANT_SEND_EMAIL), Field = Resource.Material_Reject };
                        }


                        return RedirectToAction("MaterialReject", new RouteValueDictionary(model.result));
                    }
                }
            }
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0002);
            if (prole != null)
                model.Modify = prole.Modify;

            var exService = new ExchequerService();
            model.cProductlist = exService.LstExchquerProduct(true);
            var cbService = new ComboService();
            model.cPackaginglist = cbService.LstLookupData(LookupType.Packaging, true);
            model.cUOMlist = cbService.LstLookupData(LookupType.UOM, true);
            //Search product from Exchequer
            if (!string.IsNullOrEmpty(model.Product_Code))
            {
                var rcri = new ProductTransCriteria();
                rcri.Product_Code = model.Product_Code;
                var result = exService.GetExchquerProductTrans(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                    model.ProductTrancList = result.Object as List<Product_Transaction>;

                if (string.IsNullOrEmpty(model.Product_Name))
                    model.Product_Name = model.ProductTrancList.Select(s => s.Product_Name).FirstOrDefault();
            }

            return View(model);
        }

        [HttpGet]
        public void RepairMaterialRejectForm()
        {
            var uService = new UserService(User.Identity.GetUserId());
            var mService = new MaterialService();
            var rcri = new MaterialRejectCriteria();
            rcri.Overall_Status = Material_Overall_Status.Closed;
            var result2 = mService.GetMaterialReject(rcri);
            if (result2.Code == ReturnCode.SUCCESS)
            {
                var rejs = result2.Object as List<Material_Reject>;
                if (rejs != null)
                {
                    foreach (var rej in rejs)
                    {
                        var form = mService.GetMaterialRejectForm(rej.Material_Reject_ID);
                        if (form != null)
                            continue;
                        var model = new MaterialRejectInfoViewModels();
                        model.Product_Code = rej.Product_Code;
                        model.Product_Name = rej.Product_Name;
                        model.Lot_No = rej.Lot_No;
                        model.Quantity = rej.Quantity;
                        model.Material_Reject_ID = rej.Material_Reject_ID;
                        model.Raw_Material_ID = rej.Raw_Material_ID;

                        model.UOM = rej.UOM;
                        if (rej.UOM.HasValue)
                            model.UOM_Name = rej.Global_Lookup_Data.Name;

                        model.Packaging = rej.Packaging;
                        if (rej.Packaging.HasValue)
                            model.Packaging_Name = rej.Global_Lookup_Data1.Name;

                        model.Reject_From = rej.Reject_From != null ? rej.Reject_From : Reject_From_Type.Customer;

                        if (model.Reject_From == Reject_From_Type.Customer)
                            model.Reject_Customer_Name = rej.Reject_Customer_Name;
                        else if (model.Reject_From == Reject_From_Type.Supplier)
                            model.Reject_Supplier_Name = rej.Reject_Supplier_Name;
                        else if (model.Reject_From == Reject_From_Type.Inhouse)
                            model.Reject_Inhouse_Location = rej.Reject_Inhouse_Location;

                        model.QA_Staff = rej.QA_Staff;
                        model.D8_No = rej.D8_No;
                        model.Disposition_RTS = rej.Disposition_RTS;
                        model.Disposition_Rework = rej.Disposition_Rework;
                        model.Disposition_Scrap = rej.Disposition_Scrap;
                        model.Disposition_UAI = rej.Disposition_UAI;
                        model.Disposition_Others = rej.Disposition_Others;

                        if (rej.Disposition_Others.HasValue && rej.Disposition_Others.Value)
                            model.Disposition_Others_Description = rej.Disposition_Others_Description;

                        model.Instructions = rej.Instructions;
                        model.PD = rej.PD.HasValue ? rej.PD.Value : false;
                        model.QA = rej.QA.HasValue ? rej.QA.Value : false;
                        model.Logistics = rej.Logistics.HasValue ? rej.Logistics.Value : false;
                        model.Sales = rej.Sales.HasValue ? rej.Sales.Value : false;
                        model.Disposition_Action_By = rej.Disposition_Action_By;
                        model.Disposition_Date = DateUtil.ToDisplayDate(rej.Disposition_Date);
                        model.Re_Inspection_On_Rework = rej.Re_Inspection_On_Rework;
                        model.Reject_Status = rej.Reject_Status != null ? rej.Reject_Status : "Pass";
                        model.Carried_Out_By = rej.Carried_Out_By;

                        model.Carried_Out_Date = Resource.NA;
                        if (rej.Carried_Out_Date.HasValue)
                            model.Carried_Out_Date = DateUtil.ToDisplayDate(rej.Carried_Out_Date);
                        model.Review_Date = DateUtil.ToDisplayDate(rej.Review_Date);
                        model.operation = Operation.U;
                        model.Overall_Status = rej.Overall_Status;
                        model.Remarks = rej.Remarks;
                        model.Create_By = rej.Create_By;
                        model.Create_On = DateUtil.ToDisplayDate(rej.Create_On);
                        model.Update_By = rej.Update_By;
                        model.Update_On = DateUtil.ToDisplayDate(rej.Update_On);
                        model.Defect_Description = rej.Defect_Description;
                        model.Project_Name = "";// rej.Project_Name;
                        model.RMR_No = rej.RMR_No;
                        model.GM_Approval = rej.GM_Approval;

                        if (model.PD.Value)
                        {
                            model.PD_Approval = rej.PD_Approval;
                            model.PD_Date = DateUtil.ToDisplayDate(rej.PD_Date);
                            if (rej.User_Profile != null)
                                model.PD_Approval_Name = rej.User_Profile.Name;
                        }
                        if (model.QA.Value)
                        {
                            model.QA_Date = DateUtil.ToDisplayDate(rej.QA_Date);
                            model.QA_Approval = rej.QA_Approval;
                            if (rej.User_Profile1 != null)
                                model.QA_Approval_Name = rej.User_Profile1.Name;
                        }
                        if (model.Logistics.Value)
                        {
                            model.Logistics_Date = DateUtil.ToDisplayDate(rej.Logistics_Date);
                            model.Logistics_Approval = rej.Logistics_Approval;
                            if (rej.User_Profile2 != null)
                                model.Logistics_Approval_Name = rej.User_Profile2.Name;
                        }
                        if (model.Sales.Value)
                        {
                            model.Sales_Date = DateUtil.ToDisplayDate(rej.Sales_Date);
                            model.Sales_Approval = rej.Sales_Approval;

                            if (rej.User_Profile3 != null)
                                model.Sales_Approval_Name = rej.User_Profile3.Name;
                        }

                        if (model.GM_Approval.HasValue)
                        {
                            model.GM_Approval_Date = DateUtil.ToDisplayDate(rej.GM_Approval_Date);
                            model.GM_Approval = rej.GM_Approval;
                            if (rej.User_Profile4 != null)
                                model.GM_Approval_Name = rej.User_Profile4.Name;
                        }

                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            var htmlToConvert = RenderPartialViewAsString("MaterialRejectForm", model);
                            StringReader sr = new StringReader(htmlToConvert);
                            Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            var writer = PdfWriter.GetInstance(pdfDoc, ms);
                            var pageevent = new PDFPageEvent();
                            writer.PageEvent = pageevent;
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();

                            form = new Material_Reject_Form();
                            form.Material_Reject_ID = rej.Material_Reject_ID;
                            form.Create_By = rej.Create_By;
                            form.Create_On = rej.Create_On;
                            form.File_Name = "MREJ" + rej.Material_Reject_ID + ".pdf";
                            form.Form_ID = Guid.NewGuid();
                            form.File = ms.ToArray();
                            var result = mService.InsertMaterialRejectForm(form);
                        }
                    }


                }
            }
        }

        [HttpGet]
        public void MaterialRejectForm(int? pRawID = null, Nullable<int> pRejID = null, string pSelStatus = "")
        {
            var mService = new MaterialService();
            var form = mService.GetMaterialRejectForm(pRejID);
            if (form != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "inline; filename=" + form.File_Name);
                Response.BinaryWrite(form.File.ToArray());
                Response.Flush();
                Response.Close();
            }
        }

        [HttpGet]
        public ActionResult MaterialWithdraw(MaterialWithdrawViewModels model, ServiceResult msgresult)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0003);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            model.result = msgresult;
            model.Modify = prole.Modify;
            model.View = prole.View;

            var exService = new ExchequerService();
            var mService = new MaterialService();
            var cbService = new ComboService();
            var rcri = new MaterialWithdrawCriteria();
            rcri.Product_Search = model.Product_Code;
            //rcri.Product_Name = model.Product_Name;
            rcri.Tank_No = model.Tank_No;
            rcri.Month = model.Month;
            rcri.Year = model.Year;
            //rcri.Location = model.Location;
            //rcri.Packaging_Type = model.Packaging_Type;

            var result = mService.GetMaterialWithdraw(rcri);
            if (result.Code == ReturnCode.SUCCESS)
                model.WithdrawMaterialList = result.Object as List<Material_Withdraw>;

            model.cProductlist = exService.LstExchquerProduct();
            model.cTanklist = cbService.LstTMAHStorageTankNo(true, true);
            model.cMonthlist = cbService.LstMonth(true);
            model.cUOMlist = cbService.LstLookupData(LookupType.UOM, true);
            model.cPackaginglist = cbService.LstLookupData(LookupType.Packaging, true);

            return View(model);
        }

        [HttpGet]
        public ActionResult MaterialWithdrawInfo(Nullable<int> pWID, string pCode, Operation operation = Operation.None)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0003);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });


            var mService = new MaterialService();
            var model = new MaterialWithdrawInfoViewModels();
            var exService = new ExchequerService();
            model.operation = operation;
            model.Modify = prole.Modify;

            model.cProductlist = exService.LstExchquerProduct(true);
            var cbService = new ComboService();
            model.cPackaginglist = cbService.LstLookupData(LookupType.Packaging, true);
            model.cUOMlist = cbService.LstLookupData(LookupType.UOM, true);
            model.cTrnsaTypelist = cbService.LstWithdrawtransType();
            model.cTanklist = cbService.LstTMAHStorageTankNo(true);

            if (!string.IsNullOrEmpty(pCode)) // Search product from Exchequer
                model.Product_Code = pCode;

            if (operation == Operation.C)
            {

            }
            else if (operation == Operation.U)
            {

                var rcri = new MaterialWithdrawCriteria();
                rcri.Withdraw_ID = pWID;
                var result = mService.GetMaterialWithdraw(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var withs = result.Object as List<Material_Withdraw>;
                    if (withs != null && withs.Count() == 1)
                    {
                        var with = withs.FirstOrDefault();
                        model.Withdraw_ID = with.Withdraw_ID;

                        model.Product_Code = with.Product_Code;
                        model.Product_Name = with.Product_Name;
                        model.Remarks = with.Remarks;
                        model.Lot_No = with.Lot_No;
                        model.Receiving_Date = DateUtil.ToDisplayDate(with.Receiving_Date);
                        model.Total_Receiving = with.Total_Receiving;
                        model.Withdraw_Date = DateUtil.ToDisplayDate(with.Withdraw_Date);
                        model.From_Time = DateUtil.ToDisplayTime(with.From_Time);
                        model.To_Time = DateUtil.ToDisplayTime(with.To_Time);
                        model.Qty_Withdraw = with.Qty_Withdraw;

                        model.Receiving_Date = DateUtil.ToDisplayDate(with.Receiving_Date);
                        model.Lot_No = with.Lot_No;
                        model.Unit = with.Unit;
                        model.Transaction_Type = with.Transaction_Type;
                        model.Transferring_Date = DateUtil.ToDisplayDate(with.Transferring_Date);
                        model.Finished_Goods = with.Finished_Goods;
                        model.Finished_Goods_Lot_No = with.Finished_Goods_Lot_No;
                        model.Location = with.Location;
                        model.Qty_Balance = with.Qty_Balance;

                        model.UOM = with.UOM;
                        if (with.UOM.HasValue)
                            model.UOM_Name = with.Global_Lookup_Data.Name;

                        //if (with.Transaction_Type == Withdraw_Transaction_Type.Transfer)
                        //{
                        model.Withdraw_From = with.Withdraw_From;
                        model.Withdraw_To = with.Withdraw_To;
                        //}

                    }
                }
            }
            else if (operation == Operation.D)
            {
                model.Withdraw_ID = pWID;
                model.operation = Operation.D;
                return MaterialWithdrawInfo(model);
            }


            //// Search product from Exchequer
            if (!string.IsNullOrEmpty(model.Product_Code))
            {

                var rcri = new ProductTransCriteria();
                rcri.Product_Code = model.Product_Code;
                var result = exService.GetExchquerProductTrans(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                    model.ProductTrancList = result.Object as List<Product_Transaction>;

                if (string.IsNullOrEmpty(model.Product_Name))
                    model.Product_Name = model.ProductTrancList.Select(s => s.Product_Name).FirstOrDefault();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult MaterialWithdrawInfo(MaterialWithdrawInfoViewModels model)
        {
            var mService = new MaterialService(User.Identity.GetUserId());
            if ((!string.IsNullOrEmpty(model.From_Time)) && (!string.IsNullOrEmpty(model.To_Time)))
            {
                if (DateUtil.ToTime(model.From_Time) > DateUtil.ToTime(model.To_Time))
                    ModelState.AddModelError("From_Time", "The From Time field cannot be over To Time field.");
            }

            if ((model.Qty_Withdraw > 0) && (model.Total_Receiving > 0))
            {
                if (model.Total_Receiving < model.Qty_Withdraw)
                    ModelState.AddModelError("Qty_Withdraw", "Withdrawal quantity cannot be more than received quantity!");
            }

            if (model.Qty_Withdraw == 0)
                ModelState.AddModelError("Qty_Withdraw", "The Qty Withdraw field have be more than zero.");

            if (ModelState.IsValid)
            {
                var with = new Material_Withdraw();
                if (model.operation == Operation.U || model.operation == Operation.D)
                {
                    var rcri = new MaterialWithdrawCriteria();
                    rcri.Withdraw_ID = model.Withdraw_ID;
                    var result = mService.GetMaterialWithdraw(rcri);
                    if (result.Code == ReturnCode.SUCCESS)
                    {
                        var withs = result.Object as List<Material_Withdraw>;
                        if (withs != null && withs.Count() == 1)
                        {
                            with = withs.FirstOrDefault();
                        }
                    }
                }

                if (model.operation != Operation.D)
                {
                    with.Product_Code = model.Product_Code;
                    with.Product_Name = model.Product_Name;
                    with.Remarks = model.Remarks;
                    with.Withdraw_Date = DateUtil.ToDate(model.Withdraw_Date);
                    with.From_Time = DateUtil.ToTime(model.From_Time);
                    with.To_Time = DateUtil.ToTime(model.To_Time);
                    with.Qty_Withdraw = model.Qty_Withdraw;
                    with.Total_Receiving = model.Total_Receiving;
                    with.Receiving_Date = DateUtil.ToDate(model.Receiving_Date);
                    with.Lot_No = model.Lot_No;
                    with.Unit = model.Unit;
                    with.UOM = model.UOM;
                    with.Transaction_Type = model.Transaction_Type;
                    with.Transferring_Date = DateUtil.ToDate(model.Transferring_Date);
                    with.Finished_Goods = model.Finished_Goods;
                    with.Finished_Goods_Lot_No = model.Finished_Goods_Lot_No;
                    with.Location = model.Location;
                    with.Qty_Balance = model.Qty_Balance;

                    //if (model.Transaction_Type == Withdraw_Transaction_Type.Transfer)
                    //{
                    with.Withdraw_From = model.Withdraw_From;
                    with.Withdraw_To = model.Withdraw_To;
                    //}
                }

                if (model.operation == Operation.C)
                {
                    model.result = mService.InsertMaterialWithdraw(with);
                    if (model.result.Code == ReturnCode.SUCCESS)
                    {
                        return RedirectToAction("MaterialWithdraw", new RouteValueDictionary(model.result));
                    }
                }
                else if (model.operation == Operation.U)
                {
                    model.result = mService.UpdateMaterialWithdraw(with);
                    if (model.result.Code == ReturnCode.SUCCESS)
                    {
                        return RedirectToAction("MaterialWithdraw", new RouteValueDictionary(model.result));
                    }
                }
                else if (model.operation == Operation.D)
                {
                    with.Record_Status = Record_Status.Delete;
                    model.result = mService.UpdateMaterialWithdraw(with);
                    if (model.result.Code == ReturnCode.SUCCESS)
                        model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Material_Withdraw };
                    else
                        model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Material_Withdraw };

                    return RedirectToAction("MaterialWithdraw", new AppRouteValueDictionary(model));
                }
            }

            var exService = new ExchequerService();
            if (!string.IsNullOrEmpty(model.Product_Code))
            {
                var rcri = new ProductTransCriteria();
                rcri.Product_Code = model.Product_Code;
                var result = exService.GetExchquerProductTrans(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                    model.ProductTrancList = result.Object as List<Product_Transaction>;

                if (string.IsNullOrEmpty(model.Product_Name))
                    model.Product_Name = model.ProductTrancList.Select(s => s.Product_Name).FirstOrDefault();
            }
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0003);
            if (prole != null)
                model.Modify = prole.Modify;

            model.cProductlist = exService.LstExchquerProduct(true);
            var cbService = new ComboService();
            model.cPackaginglist = cbService.LstLookupData(LookupType.Packaging, true);
            model.cUOMlist = cbService.LstLookupData(LookupType.UOM, true);
            model.cTrnsaTypelist = cbService.LstWithdrawtransType();
            model.cTanklist = cbService.LstTMAHStorageTankNo(true);
            return View(model);
        }

        public ActionResult MaterialDeleteFile(Nullable<Guid> pAttachmentID)
        {
            var mService = new MaterialService(User.Identity.GetUserId());
            var result = mService.DeleteAttachment(pAttachmentID);
            return Json(new
            {
                code = result
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MaterialMaterialRejectDelete(MaterialRejectInfoViewModels pmodel)
        {
            var model = new MaterialRejectInfoViewModels();
            var mService = new MaterialService();
            if (pmodel.operation == Operation.D)
            {
                var rcri = new MaterialRejectCriteria();
                rcri.Material_Reject_ID = pmodel.Material_Reject_ID;
                var result = mService.GetMaterialReject(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var rejs = result.Object as List<Material_Reject>;
                    if (rejs != null && rejs.Count() == 1)
                    {
                        var rej = new Material_Reject();
                        rej = rejs.FirstOrDefault();
                        if (rej.Overall_Status != Material_Overall_Status.Closed)
                        {
                            rej.Record_Status = Record_Status.Delete;
                            model.result = mService.UpdateMaterialReject(rej);
                            if (model.result.Code == ReturnCode.SUCCESS)
                                model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Material_Reject };
                            else
                                model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Material_Reject };

                            return RedirectToAction("MaterialReject", new RouteValueDictionary(model.result));
                        }
                    }
                }
            }

            return RedirectToAction("MaterialReject", new AppRouteValueDictionary(model));
        }

        [HttpGet]
        public ActionResult WithdrawRawMaterialPrint(string pProductCode, int? pMonth, int? pYear, string pProduct_Name, string pTank_No, int? pPackaging_Type, string pLocation)
        {

            var model = new MaterialWithdrawFormViewModels();
            var mService = new MaterialService();
            var exService = new ExchequerService();
            var user = new UserService();

            var wlist = new List<Material_Withdraw>();
            var rcri = new MaterialWithdrawCriteria();
            if (!string.IsNullOrEmpty(pProductCode))
            {
                rcri.Product_Code = pProductCode;
                rcri.Month = pMonth;
                rcri.Year = pYear;
                //rcri.Product_Name = pProduct_Name;
                rcri.Tank_No = pTank_No;
                //rcri.Packaging_Type = pPackaging_Type;
                //rcri.Location = pLocation;

                var result = mService.GetMaterialWithdraw(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    wlist = result.Object as List<Material_Withdraw>;
                }
            }
            model.lists = new List<MaterialWithdrawViewModels>();
            if (wlist.Count > 0)
            {
                var pCodes = wlist.Select(s => s.Product_Code).Distinct();
                foreach (var pcode in pCodes)
                {
                    var list = new MaterialWithdrawViewModels();
                    var map = mService.GetProductMapping(pcode);
                    list.Product_Code = pcode;
                    //if (map != null)
                    //   list.Product_Name = map.Product_Name;

                    list.Product_Name = pProduct_Name;

                    list.Tank_No = pTank_No;
                    //list.Finished_Goods = pProduct_Name;
                    list.Year = pYear;
                    if (pMonth.HasValue)
                        list.Month_Name = DateUtil.GetFullMonth(pMonth);
                    list.Location = pLocation;
                    if (pPackaging_Type.HasValue)
                    {
                        var sLookup = new GlobalLookupService();
                        var result = sLookup.GetGlobalLookupData(new GlobalLookupCriteria() { Lookup_Data_ID = pPackaging_Type });
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var uons = result.Object as List<Global_Lookup_Data>;
                            if (uons != null && uons.Count() == 1)
                            {
                                var uon = uons.FirstOrDefault();
                                if (uon != null)
                                    list.Packaging_Type_Name = uon.Name;
                            }
                        }
                    }
                    list.WithdrawMaterialList = wlist.Where(w => w.Product_Code == pcode).ToList();
                    model.lists.Add(list);
                }
            }

            var htmlToConvert = RenderPartialViewAsString("MaterialWithdrawForm", model);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.Charset = Encoding.UTF8.ToString();
            Response.HeaderEncoding = Encoding.UTF8;
            Response.ContentEncoding = Encoding.UTF8;
            Response.Buffer = true;
            StringReader sr = new StringReader(htmlToConvert);
            Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            var pageevent = new PDFPageEvent();
            writer.PageEvent = pageevent;
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.End();
            return View("MaterialWithdrawForm", model);
        }

        [HttpGet]
        public ActionResult MaterialChecklist(MaterialViewModels model, ServiceResult msgresult)
        {

            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0011);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            model.result = msgresult;
            model.Modify = prole.Modify;
            model.View = prole.View;

            var mService = new MaterialService();
            var rcri = new RawMaterialCriteria();
            rcri.Product_Search = model.Product_Code;
            rcri.Date_From = model.Date_From;
            rcri.Date_To = model.Date_To;

            var result = mService.GetRawMaterial(rcri);
            if (result.Code == ReturnCode.SUCCESS)
                model.RawMaterialList = result.Object as List<Raw_Material>;

            var exService = new ExchequerService();
            model.cProductlist = exService.LstExchquerProduct(true);
            return View(model);
        }

        [HttpGet]
        public ActionResult MaterialChecklistForm(string pProductCode, string pDateFrom, string pDateTo)
        {
            var model = new MaterialChecklistViewModels();
            var mService = new MaterialService();
            var rcri = new RawMaterialCriteria();
            rcri.Product_Search = pProductCode;
            rcri.Date_From = pDateFrom;
            rcri.Date_To = pDateTo;

            var rlist = new List<Raw_Material>();
            if (!string.IsNullOrEmpty(pProductCode))
            {
                rcri.Product_Code = pProductCode;
                rcri.Date_From = pDateFrom;
                rcri.Date_To = pDateTo;
                var result = mService.GetRawMaterial(rcri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    rlist = result.Object as List<Raw_Material>;
                }
            }
            model.lists = new List<MaterialViewModels>();
            if (rlist.Count > 0)
            {
                var pCodes = rlist.Select(s => s.Product_Code).Distinct();
                foreach (var pcode in pCodes)
                {
                    var list = new MaterialViewModels();
                    var map = mService.GetProductMapping(pcode);
                    list.Product_Code = pcode;
                    if (map != null)
                        list.Product_Name = map.Product_Name;

                    list.RawMaterialList = rlist.Where(w => w.Product_Code == pcode).ToList();
                    model.lists.Add(list);
                }
            }


            var htmlToConvert = RenderPartialViewAsString("MaterialChecklistForm", model);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.Charset = Encoding.UTF8.ToString();
            Response.HeaderEncoding = Encoding.UTF8;
            Response.ContentEncoding = Encoding.UTF8;
            Response.Buffer = true;
            StringReader sr = new StringReader(htmlToConvert);
            Document pdfDoc = new Document(PageSize.A4, 40.2f, 21.6f, 18f, 13.7f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            var pageevent = new PDFPageEvent();
            writer.PageEvent = pageevent;
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.End();
            return View("MaterialChecklistForm", model);
        }
        [HttpGet]
        public void TestPrint()
        {
            var model = new MaterialWithdrawViewModels();
            var htmlToConvert = RenderPartialViewAsString("TestPrint", model);
            Response.ContentType = "application/pdf";
            StringReader sr = new StringReader(htmlToConvert);
            Document pdfDoc = new Document(new Rectangle(300f, 800f), 0f, 0f, 0f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            var action = new PdfAction(PdfAction.PRINTDIALOG);
            writer.SetOpenAction(action);
            var pageevent = new PDFPageEvent();
            writer.PageEvent = pageevent;
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();

        }

        public ActionResult MaterialViewAttachmentFile(int index, Nullable<int> pAttID, string pAttType)
        {
            var model = new MaterialFileAttachList();
            var mService = new MaterialService();
            if (pAttID.HasValue && !string.IsNullOrEmpty(pAttType))
            {
                var attact = mService.LstRawMaterialAttachment(pAttID, pAttType);
                model.Attachmentlist = new List<Raw_Material_Attachment>();
                model.Attachmentlist = attact;
                model.Index = index;
            }
            return PartialView("AttachmentDlg", model);
        }
    }
}