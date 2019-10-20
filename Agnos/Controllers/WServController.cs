using Agnos.Models;
using AgnosModel.Models;
using AgnosModel.Service;
using AppFramework;
using AppFramework.Util;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using SBSResourceAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Agnos.Controllers
{
   public class WServController : ControllerBase
   {

      public WServController()
         : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new AgnosDBContext())))
      {
      }

      public WServController(UserManager<ApplicationUser> userManager)
      {
         UserManager = userManager;
      }

      public UserManager<ApplicationUser> UserManager { get; private set; }

      [AllowAnonymous]
      public JsonResult SyncProfile()
      {
         List<UserWViewModel> SyncDatas = new List<UserWViewModel>();
         var uService = new UserService();
         var result = uService.getUsers();
         if (result.Code == ReturnCode.SUCCESS)
         {
            var profiles = result.Object as List<User_Profile>;
            if (profiles != null && profiles.Count > 0)
            {
               foreach (var row in profiles)
               {
                  var SyncData = new UserWViewModel();
                  SyncData.Profile_ID = row.Profile_ID;
                  SyncData.Name = row.Name;
                  SyncData.Email_Address = row.Email_Address;
                  SyncData.Password = row.PWD;
                  SyncData.Record_Status = row.Record_Status;
                  SyncDatas.Add(SyncData);
               }
            }
         }
         return Json(SyncDatas, JsonRequestBehavior.AllowGet);
      }

      #region  Sync Down Delivery
      [AllowAnonymous]
      public JsonResult SyncDownDeliveryByRecord(string DeliveryID)
      {
         var SyncData = new CMSDeliveryWModels();
         var cmsService = new MobileService();
         var cri = new MobileCri();
         cri.Delivery_ID = NumUtil.ParseInteger(DeliveryID);
         cri.Sync_Not_Completed = true;
         cri.Is_Active = true;
         var result = cmsService.GetCMSDelivery(cri);
         if (result.Code == ReturnCode.SUCCESS)
         {
            var DOlist = result.Object as List<CMS_Delivery>;
            if (DOlist != null && DOlist.Count > 0)
            {
               var d = DOlist.FirstOrDefault();
               if (d != null)
               {
                  SyncData.Delivery_ID = d.Delivery_ID;
                  SyncData.Delivery_Order_No = d.Delivery_Order_No;
                  SyncData.Update_On = DateUtil.ToDisplayDateTime(d.Update_On);
                  SyncData.Record_Status = d.Record_Status;
                  List<CMSDeliveryDetailWModels> details = new List<CMSDeliveryDetailWModels>();
                  foreach (var row2 in d.CMS_Delivery_Detail as List<CMS_Delivery_Detail>)
                  {
                     var detail = new CMSDeliveryDetailWModels();
                     detail.CMS_Delivery_Detail_ID = row2.CMS_Delivery_Detail_ID;
                     if (!string.IsNullOrEmpty(row2.Product_Code))
                     {
                        detail.Product_ID = 1; //remove after improve code in mobile
                        detail.Product_Code = row2.Product_Code.Trim();
                        detail.Product_Name = "";
                     }
                     detail.No_Of_Containers = row2.No_Of_Containers.HasValue ? row2.No_Of_Containers.Value : 1;
                     details.Add(detail);
                  }
                  SyncData.DeliveryDetail = details;
               }

            }
         }
         return Json(SyncData, JsonRequestBehavior.AllowGet);
      }

      [AllowAnonymous]
      public JsonResult SyncDeliverys()
      {
         List<CMSDeliveryWModels> SyncDatas = new List<CMSDeliveryWModels>();
         var cmsService = new MobileService();
         var cri = new MobileCri();

         cri.Sync_Not_Completed = true;
         cri.Is_Active = true;
         var result = cmsService.GetCMSDelivery(cri);
         if (result.Code == ReturnCode.SUCCESS)
         {
            var DOlist = result.Object as List<CMS_Delivery>;
            if (DOlist != null && DOlist.Count > 0)
            {
               foreach (var row in DOlist)
               {
                  var data = new CMSDeliveryWModels();
                  data.Delivery_ID = row.Delivery_ID;
                  data.Delivery_Order_No = row.Delivery_Order_No;
                  data.Update_On = DateUtil.ToDisplayDateTime(row.Update_On);
                  data.Record_Status = row.Record_Status;
                  List<CMSDeliveryDetailWModels> details = new List<CMSDeliveryDetailWModels>();
                  foreach (var row2 in row.CMS_Delivery_Detail as List<CMS_Delivery_Detail>)
                  {
                     var detail = new CMSDeliveryDetailWModels();
                     detail.CMS_Delivery_Detail_ID = row2.CMS_Delivery_Detail_ID;
                     if (!string.IsNullOrEmpty(row2.Product_Code))
                     {
                        detail.Product_ID = 1; //remove after improve code in mobile
                        detail.Product_Code = row2.Product_Code;
                        detail.Product_Name = "";
                     }
                     detail.No_Of_Containers = row2.No_Of_Containers.HasValue ? row2.No_Of_Containers.Value : 1;
                     details.Add(detail);
                  }
                  data.DeliveryDetail = details;
                  SyncDatas.Add(data);
               }
            }
         }
         return Json(SyncDatas, JsonRequestBehavior.AllowGet);
      }

      [AllowAnonymous]
      public JsonResult GetDeliveryDetails(string DeliveryID)
      {
         List<CMSDeliveryDetailWModels> SyncDatas = new List<CMSDeliveryDetailWModels>();
         var cmsService = new CMSService();
         int Delivery_ID = NumUtil.ParseInteger(DeliveryID);
         var cri = new DeliveryCriteria();
         cri.Delivery_ID = Delivery_ID;
         var result = cmsService.GetCMSDelivery(cri);
         if (result.Code == ReturnCode.SUCCESS)
         {
            var deliverys = result.Object as List<CMS_Delivery>;
            if (deliverys != null && deliverys.Count() == 1)
            {
               var delivery = deliverys.FirstOrDefault();
               foreach (var row in delivery.CMS_Delivery_Detail)
               {
                  var lrow = new CMSDeliveryDetailWModels();
                  lrow.CMS_Delivery_Detail_ID = row.CMS_Delivery_Detail_ID;
                  if (!string.IsNullOrEmpty(row.Product_Code))
                  {
                     lrow.Product_ID = 1; //remove after improve code in mobile
                     lrow.Product_Code = row.Product_Code;
                     lrow.Product_Name = "";
                  }
                  SyncDatas.Add(lrow);
               }
            }
         }
         return Json(SyncDatas, JsonRequestBehavior.AllowGet);
      }
      #endregion

      #region  Sync Up Delivery
      [AllowAnonymous]
      public JsonResult SyncUpDeliverys(List<CMSDeliveryWModels> SyncUpData)
      {
         List<CMS_Delivery> Deliverys = new List<CMS_Delivery>();
         var currentdate = StoredProcedure.GetCurrentDate();
         var _result = new wResult();
         var cmsService = new MobileService();
         Boolean IsPass = false;
         if (SyncUpData != null)
         {
            foreach (var row in SyncUpData)
            {
               IsPass = false;
               CMS_Delivery _Delivery = new CMS_Delivery();
               var cri = new MobileCri();
               cri.Delivery_ID = row.Delivery_ID;
               var result = cmsService.GetCMSDelivery(cri);
               if (result.Code == ReturnCode.SUCCESS)
               {
                  var deliverys = result.Object as List<CMS_Delivery>;
                  if (deliverys != null && deliverys.Count() == 1)
                  {
                     IsPass = true;
                     _Delivery = deliverys.FirstOrDefault();
                     _Delivery.Delivery_ID = row.Delivery_ID;
                     _Delivery.Delivery_Order_No = row.Delivery_Order_No;
                     _Delivery.Completed = true;
                     _Delivery.Update_By = row.Update_By;
                     _Delivery.Update_On = currentdate;

                     List<CMS_Delivery_Detail> _Details = new List<CMS_Delivery_Detail>();
                     foreach (var row2 in row.DeliveryDetail)
                     {
                        CMS_Delivery_Detail _Detail = new CMS_Delivery_Detail();
                        _Detail.CMS_Delivery_Detail_ID = row2.CMS_Delivery_Detail_ID;
                        _Detail.Delivery_ID = row2.Delivery_ID;
                        _Detail.Drum_Code = row2.Drum_Code.Trim();
                        _Detail.Update_By = row2.Update_By;
                        _Detail.Update_On = currentdate;
                        _Detail.Date_Delivered = DateUtil.ToDate(row2.Date_Delivered);
                        _Details.Add(_Detail);
                     }
                     _Delivery.CMS_Delivery_Detail = _Details;
                  }
                  Deliverys.Add(_Delivery);
               }
               if (!IsPass)
               {
                  _result.status = "1";
                  _result.message = "Error! can not found Delivery";
                  return Json(new { Data = _result }, JsonRequestBehavior.AllowGet);
               }
            }
            var result2 = cmsService.UpdateCMSDelivery(Deliverys);
            if (result2.Code == ReturnCode.SUCCESS)
            {
               _result.status = "200";
               _result.message = "Update successfully";
            }
            else
            {
               _result.status = "1";
               _result.message = "Error! can not update data";
            }
         }
         else
         {
            _result.status = "1";
            _result.message = "Error! can not update data";
         }
         return Json(new { Data = _result }, JsonRequestBehavior.AllowGet);
      }

      public JsonResult SyncUpDeliverysByRecord(CMSDeliveryWModels SyncUpData)
      {
         var currentdate = StoredProcedure.GetCurrentDate();
         var cmsService = new MobileService();
         Boolean IsPass = false;
         var _result = new wResult();
         if (SyncUpData != null)
         {
            IsPass = false;
            CMS_Delivery _Delivery = new CMS_Delivery();
            var cri = new MobileCri();
            cri.Delivery_ID = SyncUpData.Delivery_ID;
            var result = cmsService.GetCMSDelivery(cri);
            if (result.Code == ReturnCode.SUCCESS)
            {
               var deliverys = result.Object as List<CMS_Delivery>;
               if (deliverys != null && deliverys.Count() == 1)
               {
                  IsPass = true;
                  _Delivery = deliverys.FirstOrDefault();
                  _Delivery.Delivery_ID = SyncUpData.Delivery_ID;
                  _Delivery.Delivery_Order_No = SyncUpData.Delivery_Order_No;
                  _Delivery.Completed = true;
                  _Delivery.Update_By = SyncUpData.Update_By;
                  _Delivery.Update_On = currentdate;

                  List<CMS_Delivery_Detail> _Details = new List<CMS_Delivery_Detail>();
                  if (SyncUpData.DeliveryDetail != null)
                  {
                     foreach (var row2 in SyncUpData.DeliveryDetail)
                     {
                        CMS_Delivery_Detail _Detail = new CMS_Delivery_Detail();
                        _Detail.CMS_Delivery_Detail_ID = row2.CMS_Delivery_Detail_ID;
                        _Detail.Delivery_ID = row2.Delivery_ID;
                        _Detail.Drum_Code = row2.Drum_Code.Trim();
                        _Detail.Update_By = SyncUpData.Update_By;
                        _Detail.Update_On = currentdate;
                        _Detail.Date_Delivered = DateUtil.ToDate(row2.Date_Delivered);
                        _Details.Add(_Detail);
                     }
                     _Delivery.CMS_Delivery_Detail = _Details;
                  }
               }

               if (!IsPass)
               {
                  _result.status = "1";
                  _result.message = "Error! can not found Delivery";
                  return Json(new { Data = _result }, JsonRequestBehavior.AllowGet);
               }
               var result2 = cmsService.UpdateCMSDelivery(_Delivery);
               if (result2.Code == ReturnCode.SUCCESS)
               {
                  _result.status = "200";
                  _result.message = "Update successfully";
               }
               else
               {
                  _result.status = "1";
                  _result.message = "Error! can not update data";
               }
            }
         }
         else
         {
            _result.status = "1";
            _result.message = "Error! can not update data";
         }
         return Json(new { Data = _result }, JsonRequestBehavior.AllowGet);
      }


      [AllowAnonymous]
      public JsonResult SyncCheckDeliverysByIDs(List<CMSDeliveryWModels> SyncUpData)
      {
         List<CMSDeliveryWModels> SyncDownDatas = new List<CMSDeliveryWModels>();
         var cmsService = new MobileService();
         if (SyncUpData != null)
         {
            foreach (var row in SyncUpData)
            {
               var cri = new MobileCri();
               cri.Delivery_ID = row.Delivery_ID;
               cri.Completed = true;
               var result = cmsService.GetCMSDelivery(cri);
               if (result.Code == ReturnCode.SUCCESS)
               {
                  var deliverys = result.Object as List<CMS_Delivery>;
                  if (deliverys != null && deliverys.Count > 0)
                  {
                     CMS_Delivery get_delivery = new CMS_Delivery();
                     get_delivery = deliverys.FirstOrDefault();
                     if (get_delivery != null)
                     {
                        CMSDeliveryWModels _delivery = new CMSDeliveryWModels();
                        _delivery.Local_Delivery_ID = row.Local_Delivery_ID;
                        _delivery.Delivery_ID = get_delivery.Delivery_ID;
                        _delivery.Delivery_Order_No = get_delivery.Delivery_Order_No;
                        _delivery.Record_Status = get_delivery.Record_Status;

                        //Boolean in Sqlite (0,Fail) , (1,true)  
                        if (get_delivery.Completed.HasValue && get_delivery.Completed.Value)
                           _delivery.Completed = 1;
                        else
                           _delivery.Completed = 0;
                        SyncDownDatas.Add(_delivery);
                     }
                  }
               }
            }
         }
         return Json(SyncDownDatas, JsonRequestBehavior.AllowGet);
      }


      [AllowAnonymous]
      public JsonResult SyncDataToSQLite(List<CMSDeliveryWModels> SyncUpData)
      {
         List<CMSDeliveryWModels> SyncDownDatas = new List<CMSDeliveryWModels>();
         var cmsService = new MobileService();
         if (SyncUpData != null)
         {
            foreach (var row in SyncUpData)
            {
               var cri = new MobileCri();
               cri.Delivery_ID = row.Delivery_ID;
               cri.Sync_Current_Data = true;
               var result = cmsService.GetCMSDelivery(cri);
               if (result.Code == ReturnCode.SUCCESS)
               {
                  var deliverys = result.Object as List<CMS_Delivery>;
                  if (deliverys != null && deliverys.Count > 0)
                  {
                     CMS_Delivery get_delivery = new CMS_Delivery();
                     get_delivery = deliverys.FirstOrDefault();
                     if (get_delivery != null)
                     {
                        CMSDeliveryWModels _delivery = new CMSDeliveryWModels();
                        _delivery.Local_Delivery_ID = row.Local_Delivery_ID;
                        _delivery.Delivery_ID = get_delivery.Delivery_ID;
                        _delivery.Delivery_Order_No = get_delivery.Delivery_Order_No;
                        _delivery.Record_Status = get_delivery.Record_Status;

                        //Boolean in Sqlite (0,Fail) , (1,true)  
                        if (get_delivery.Completed.HasValue && get_delivery.Completed.Value)
                           _delivery.Completed = 1;
                        else
                           _delivery.Completed = 0;

                        SyncDownDatas.Add(_delivery);
                     }
                  }
               }
            }
         }

         return Json(SyncDownDatas, JsonRequestBehavior.AllowGet);
      }
      #endregion

      public class wResult
      {
         public string status { get; set; }
         public string message { get; set; }
      }

      public void TestSyncUp(string pDeliveryID = "", string pDrumCode = "", string pDeliveryDetailID = "")
      {
         var currentdate = StoredProcedure.GetCurrentDate();
         var cmsService = new MobileService();
         CMS_Delivery _Delivery = new CMS_Delivery();
         var cri = new MobileCri();
         cri.Delivery_ID = NumUtil.ParseInteger(pDeliveryID);
         var result = cmsService.GetCMSDelivery(cri);
         if (result.Code == ReturnCode.SUCCESS)
         {
            var deliverys = result.Object as List<CMS_Delivery>;
            if (deliverys != null && deliverys.Count() == 1)
            {
               _Delivery = deliverys.FirstOrDefault();
               _Delivery.Delivery_ID = _Delivery.Delivery_ID;
               _Delivery.Delivery_Order_No = _Delivery.Delivery_Order_No;
               _Delivery.Completed = true;
               _Delivery.Update_By = "Test";
               _Delivery.Update_On = currentdate;
               List<CMS_Delivery_Detail> _Details = new List<CMS_Delivery_Detail>();
               CMS_Delivery_Detail _Detail = new CMS_Delivery_Detail();
               _Detail.CMS_Delivery_Detail_ID = NumUtil.ParseInteger(pDeliveryDetailID); ;
               _Detail.Delivery_ID = _Delivery.Delivery_ID;
               _Detail.Drum_Code = pDrumCode;
               _Detail.Update_By = "Test";
               _Detail.Update_On = currentdate;
               _Detail.Date_Delivered = currentdate;
               _Details.Add(_Detail);
               _Delivery.CMS_Delivery_Detail = _Details;
            }
         }

         var result2 = cmsService.UpdateCMSDelivery(_Delivery);
      }

   }
}