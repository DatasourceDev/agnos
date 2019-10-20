using Agnos.Models;
using AgnosModel.Models;
using AgnosModel.Service;
using AppFramework;
using AppFramework.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SBSResourceAPI;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using AppFramework.Util;
using AgnosModel;
namespace Agnos.Controllers
{
    public class CMSReportController : ControllerBase
    {

        [HttpGet]
        public ActionResult InventoryReport(CMSReportViewModels model, ServiceResult msgresult)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0016);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            ModelState.Clear();
            model.Modify = prole.Modify;
            model.View = prole.View;
            model.result = msgresult; // return result from http post
            var cmsService = new CMSService();
            var cbService = new ComboService();
            var hasCri = new ComboCriteria() { Date_Charged = true };
            model.cSortlist = cbService.LstSortAction(false, hasCri);

            model.cProductCodelist = cbService.LstProductByCode(true);


            model.chargelist = new List<CMS_Charge>();

            if (!string.IsNullOrEmpty(model.Lot_No) || !string.IsNullOrEmpty(model.Drum_Code) || !string.IsNullOrEmpty(model.Product_Code))
            {
                var cri = new CMSCriteria();
                cri.Lot_No = model.Lot_No;
                cri.Drum_Code = model.Drum_Code;
                cri.Product_Code = model.Product_Code;
                cri.Not_Yet_Deliver = true;
                cri.Sort_By = model.Sort_By;
                cri.Drum_Code_Have_Value = true;
                var result = cmsService.GetCMSCharge(cri);
                if (result.Code == ReturnCode.SUCCESS)
                    model.chargelist = result.Object as List<CMS_Charge>;

                cri = new CMSCriteria();
                cri.Drum_Code_Have_Value = true;
                var result2 = cmsService.GetCMSCharge(cri);
                if (result2.Code == ReturnCode.SUCCESS)
                    model.chargelistAll = result2.Object as List<CMS_Charge>;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult InventoryReportPrint(CMSReportViewModels model)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0016);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            if (!string.IsNullOrEmpty(model.Lot_No) || !string.IsNullOrEmpty(model.Drum_Code) || !string.IsNullOrEmpty(model.Product_Code))
            {
                var cmsService = new CMSService();
                var cri = new CMSCriteria();
                cri.Lot_No = model.Lot_No;
                cri.Drum_Code = model.Drum_Code;
                cri.Product_Code = model.Product_Code;
                cri.Not_Yet_Deliver = true;
                cri.Sort_By = model.Sort_By;
                cri.Drum_Code_Have_Value = true;
                var result = cmsService.GetCMSCharge(cri);
                if (result.Code == ReturnCode.SUCCESS)
                    model.chargelist = result.Object as List<CMS_Charge>;

                cri = new CMSCriteria();
                cri.Drum_Code_Have_Value = true;
                var result2 = cmsService.GetCMSCharge(cri);
                if (result2.Code == ReturnCode.SUCCESS)
                    model.chargelistAll = result2.Object as List<CMS_Charge>;

            }
            var htmlToConvert = RenderPartialViewAsString("InventoryReportPrint", model);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.Charset = Encoding.UTF8.ToString();
            Response.HeaderEncoding = Encoding.UTF8;
            Response.ContentEncoding = Encoding.UTF8;
            Response.Buffer = true;
            StringReader sr = new StringReader(htmlToConvert);
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 40);
            pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            var action = new PdfAction(PdfAction.PRINTDIALOG);
            writer.SetOpenAction(action);

            var pageevent = new PDFPageEvent();
            writer.PageEvent = pageevent;
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.End();
            return View(model);
        }


        [HttpGet]
        public ActionResult DrumHistoryReport(CMSReportViewModels model, ServiceResult msgresult)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0016);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            ModelState.Clear();
            model.Modify = prole.Modify;
            model.View = prole.View;
            model.result = msgresult; // return result from http post
            var cmsService = new CMSService();
            var cbService = new ComboService();
            var hasCri = new ComboCriteria() { Date_Delivered = true, Delivery_Order_No = true };
            model.cSortlist = cbService.LstSortAction(false, hasCri);

            model.cProductCodelist = cbService.LstProductByCode(true);

            model.chargelist = new List<CMS_Charge>();

            if (!string.IsNullOrEmpty(model.Date_From) || !string.IsNullOrEmpty(model.Date_To) || !string.IsNullOrEmpty(model.Product_Code))
            {
                var cri = new CMSCriteria();
                cri.Product_Code = model.Product_Code;
                cri.Date_From = model.Date_From;
                cri.Date_To = model.Date_To;
                cri.Sort_By = model.Sort_By;
                cri.Drum_Code_Have_Value = true;
                var result = cmsService.GetCMSCharge(cri);
                if (result.Code == ReturnCode.SUCCESS)
                    model.chargelist = result.Object as List<CMS_Charge>;

                cri = new CMSCriteria();
                cri.Drum_Code_Have_Value = true;
                var result2 = cmsService.GetCMSCharge(cri);
                if (result2.Code == ReturnCode.SUCCESS)
                   model.chargelistAll = result2.Object as List<CMS_Charge>;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DrumHistoryReportPrint(CMSReportViewModels model)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0016);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            if (!string.IsNullOrEmpty(model.Date_From) || !string.IsNullOrEmpty(model.Date_To) || !string.IsNullOrEmpty(model.Product_Code))
            {
                var cmsService = new CMSService();
                var cri = new CMSCriteria();
                cri.Product_Code = model.Product_Code;
                cri.Date_From = model.Date_From;
                cri.Date_To = model.Date_To;
                cri.Sort_By = model.Sort_By;
                cri.Drum_Code_Have_Value = true;
                var result = cmsService.GetCMSCharge(cri);
                if (result.Code == ReturnCode.SUCCESS)
                    model.chargelist = result.Object as List<CMS_Charge>;
            }
            var htmlToConvert = RenderPartialViewAsString("DrumHistoryReportPrint", model);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.Charset = Encoding.UTF8.ToString();
            Response.HeaderEncoding = Encoding.UTF8;
            Response.ContentEncoding = Encoding.UTF8;
            Response.Buffer = true;
            StringReader sr = new StringReader(htmlToConvert);
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 40);
            pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            var action = new PdfAction(PdfAction.PRINTDIALOG);
            writer.SetOpenAction(action);

            var pageevent = new PDFPageEvent();
            writer.PageEvent = pageevent;
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.End();
            return View(model);
        }

        [HttpGet]
        public ActionResult PurgeHistoryReport(CMSReportViewModels model, ServiceResult msgresult)
        {
           var uService = new UserService();
           var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0016);
           if (prole == null)
              return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
           if (prole.View == null || prole.View == false)
              return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

           ModelState.Clear();
           model.Modify = prole.Modify;
           model.View = prole.View;
           model.result = msgresult; // return result from http post
           var cmsService = new CMSService();
           var cbService = new ComboService();
           var hasCri = new ComboCriteria() { Date_Delivered = true, Delivery_Order_No = true };

           model.purgelist = new List<CMS_Purge>();

           if (!string.IsNullOrEmpty(model.Date_From) || !string.IsNullOrEmpty(model.Date_To) || !string.IsNullOrEmpty(model.Drum_Code))
           {
              var cri = new CMSCriteria();
              cri.Date_From = model.Date_From;
              cri.Date_To = model.Date_To;
              cri.Drum_Code = model.Drum_Code;
              cri.Drum_Code_Have_Value = true;
              var result = cmsService.GetCMSPurge(cri);
              if (result.Code == ReturnCode.SUCCESS)
                 model.purgelist = result.Object as List<CMS_Purge>;

              cri = new CMSCriteria();
              cri.Drum_Code_Have_Value = true;
              var result2 = cmsService.GetCMSPurge(cri);
              if (result2.Code == ReturnCode.SUCCESS)
                 model.purgelistAll = result2.Object as List<CMS_Purge>;
           }
           return View(model);
        }


        [HttpGet]
        public ActionResult LotHistoryReport(CMSReportViewModels model, ServiceResult msgresult)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0016);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            ModelState.Clear();
            model.Modify = prole.Modify;
            model.View = prole.View;
            model.result = msgresult; // return result from http post
            var cmsService = new CMSService();
            var cbService = new ComboService();
            var hasCri = new ComboCriteria() { Delivery_Order_No = true, Date_Delivered = true, Product_Code = true };
            model.cSortlist = cbService.LstSortAction(false, hasCri);
            model.chargelist = new List<CMS_Charge>();

            if (!string.IsNullOrEmpty(model.Lot_No) || !string.IsNullOrEmpty(model.Date_From) || !string.IsNullOrEmpty(model.Date_To))
            {
                var cri = new CMSCriteria();
                cri.Lot_No = model.Lot_No;
                cri.Date_From = model.Date_From;
                cri.Date_To = model.Date_To;
                cri.Sort_By = model.Sort_By;
                cri.Drum_Code_Have_Value = true;
                var result = cmsService.GetCMSCharge(cri);
                if (result.Code == ReturnCode.SUCCESS)
                    model.chargelist = result.Object as List<CMS_Charge>;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult LotHistoryReportPrint(CMSReportViewModels model)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0016);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });


            if (!string.IsNullOrEmpty(model.Lot_No) || !string.IsNullOrEmpty(model.Date_From) || !string.IsNullOrEmpty(model.Date_To))
            {
                var cmsService = new CMSService();
                var cri = new CMSCriteria();
                cri.Lot_No = model.Lot_No;
                cri.Date_From = model.Date_From;
                cri.Date_To = model.Date_To;
                cri.Sort_By = model.Sort_By;
                cri.Drum_Code_Have_Value = true;
                var result = cmsService.GetCMSCharge(cri);
                if (result.Code == ReturnCode.SUCCESS)
                    model.chargelist = result.Object as List<CMS_Charge>;
            }

            var htmlToConvert = RenderPartialViewAsString("LotHistoryReportPrint", model);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.Charset = Encoding.UTF8.ToString();
            Response.HeaderEncoding = Encoding.UTF8;
            Response.ContentEncoding = Encoding.UTF8;
            Response.Buffer = true;
            StringReader sr = new StringReader(htmlToConvert);
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 40);
            pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            var action = new PdfAction(PdfAction.PRINTDIALOG);
            writer.SetOpenAction(action);

            var pageevent = new PDFPageEvent();
            writer.PageEvent = pageevent;
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.End();
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteChargeInfo(CMSReportViewModels model)
        {
            var cmsService = new CMSService(User.Identity.GetUserId());
            model.result = new ServiceResult();
            var charge = new CMS_Charge();
            if (model.operation == Operation.D)
            {
                var cri = new CMSCriteria();
                cri.Charge_ID = model.Charge_ID;
                cri.include = true;
                var result = cmsService.GetCMSCharge(cri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var charges = result.Object as List<CMS_Charge>;
                    if (charges != null && charges.Count() == 1)
                        charge = charges.FirstOrDefault();
                }

                if (charge != null)
                {
                    charge.Record_Status = Record_Status.Delete;
                    model.result = cmsService.UpdateCMSCharge(charge);
                    if (model.result.Code == ReturnCode.SUCCESS)
                        model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.CMS_Charge };
                    else
                        model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.CMS_Charge };


                    var result_route = new RouteValueDictionary(model.result);
                    result_route.Add("Lot_No", model.Lot_No);
                    result_route.Add("Drum_Code", model.Drum_Code);
                    result_route.Add("Product_Code", model.Product_Code);
                    return RedirectToAction("InventoryReport", result_route);
                    //return RedirectToAction("InventoryReport", new AppRouteValueDictionary(model));
                }
            }
            return RedirectToAction("InventoryReport", new AppRouteValueDictionary(model));
        }
    }
}