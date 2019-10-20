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
using System.Threading.Tasks;
using System.Net;
using Microsoft.Owin.Security;

namespace Agnos.Controllers
{
    public class GlobalLookupController : Controller
    {
        // GET: GlobalLookup
         [HttpGet]
       public ActionResult GlobalLookup(GlobalLookupViewModel model, ServiceResult msgresult)
        {
           var uService = new UserService(User.Identity.GetUserId());
           var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0014);
           if (prole == null)
              return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
           if (prole.View == null || prole.View == false)
              return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

           ModelState.Clear();
           if (model.operation == Operation.D)
              return GlobalLookup(model);

           model.result = msgresult;
           model.Modify = prole.Modify;
           model.View = prole.View;

           var cService = new ComboService();
           model.cGlobalDefList =cService.LstLookupDef();
           if (model.cGlobalDefList.Count > 0 && !model.search_Def_ID.HasValue)
              model.search_Def_ID = NumUtil.ParseInteger(model.cGlobalDefList[0].Value);

          if(  model.search_Def_ID.HasValue){
               var gService = new GlobalLookupService();
               var cri = new GlobalLookupCriteria();
               cri.Def_ID = model.search_Def_ID;
               var result = gService.GetGlobalLookupData(cri);
               if (result.Code == ReturnCode.SUCCESS)
                  model.GlobalDataList = result.Object as List<Global_Lookup_Data>;
          }
           return View(model);
        }

       [HttpPost]
       public ActionResult GlobalLookup(GlobalLookupViewModel model)
       {
          var gService = new GlobalLookupService();
          if (ModelState.IsValid)
          {
             var gdata = new Global_Lookup_Data();
             if (model.operation == Operation.U || model.operation == Operation.D)
             {
                var cri = new GlobalLookupCriteria();
                cri.Lookup_Data_ID = model.Lookup_Data_ID;
                var result = gService.GetGlobalLookupData(cri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                   var gdatas = result.Object as List<Global_Lookup_Data>;
                   if (gdatas != null && gdatas.Count() == 1)
                      gdata = gdatas.FirstOrDefault();
                }
             }

             if (model.operation != Operation.D)
             {
                gdata.Def_ID = model.Def_ID.Value ;
                gdata.Data_Code = model.Data_Code;
                gdata.Name = model.Name;
             }

             if (model.operation == Operation.C)
             {
                gdata.Record_Status = Record_Status.Active;
                model.result = gService.InsertGlobalLookupData(gdata);
             }
             else if (model.operation == Operation.U)
                model.result = gService.UpdateGlobalLookupData(gdata);
             else if (model.operation == Operation.D)
             {
                gdata.Record_Status = Record_Status.Delete;
                model.result = gService.UpdateGlobalLookupData(gdata);
                if (model.result.Code == ReturnCode.SUCCESS)
                   model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Global_Lookup };
                else
                   model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Global_Lookup };

                var route = new RouteValueDictionary(model.result);
                route.Add("search_Def_ID", gdata.Def_ID);
                return RedirectToAction("GlobalLookup", route);
             }

             if (model.result.Code == ReturnCode.SUCCESS)
             {
                var route = new RouteValueDictionary(model.result);
                route.Add("search_Def_ID", model.Def_ID );
                return RedirectToAction("GlobalLookup", route);
             }
               
          }

          var cService = new ComboService();
          model.cGlobalDefList = cService.LstLookupDef();
          if (model.cGlobalDefList.Count > 0 && !model.search_Def_ID.HasValue)
             model.search_Def_ID = NumUtil.ParseInteger(model.cGlobalDefList[0].Value);

          if (model.search_Def_ID.HasValue)
          {
             var cri = new GlobalLookupCriteria();
             var result = gService.GetGlobalLookupData(cri);
             if (result.Code == ReturnCode.SUCCESS)
                model.GlobalDataList = result.Object as List<Global_Lookup_Data>;
          }
          return View(model);
       }
    }
}