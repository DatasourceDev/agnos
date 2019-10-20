using Agnos.Models;
using AgnosModel.Models;
using AgnosModel.Service;
using AppFramework;
using AppFramework.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SBSResourceAPI;

namespace Agnos.Controllers
{

   public class RoleController : ControllerBase
   {
      [HttpGet]
      public ActionResult RoleSetup(RoleSetupViewModel model, ServiceResult msgresult)
      {
         var uService = new UserService();
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0000);
         if (prole == null)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
         if (prole.View == null || prole.View == false)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

         ModelState.Clear();
         if (model.operation == Operation.D)
            return RoleSetup(model);

         model.result = msgresult;
         model.Modify = prole.Modify;
         model.View = prole.View;

         var cbService = new ComboService();
         var rService = new RoleService();
         model.cPageList = cbService.LstPage();
         model.cRoleList = cbService.LstRole(true);

         var rcri = new RoleCriteria();
         rcri.Role_ID = model.sRole_ID;
         var result = rService.GetPageRole(rcri);
         if (result.Code == ReturnCode.SUCCESS)
            model.PageRoleList = result.Object as List<Page_Role>;

         return View(model);
      }

      [HttpPost]
      public ActionResult RoleSetup(RoleSetupViewModel model)
      {
         var rService = new RoleService();
         if (model.tabAction == "pagerole")
         {

            if (model.Role_ID.HasValue && model.Page_ID.HasValue)
            {
               var cri = new RoleCriteria();
               cri.Role_ID = model.Role_ID;
               cri.Page_ID = model.Page_ID;
               var result = rService.GetPageRole(cri);
               if (result.Code == ReturnCode.SUCCESS)
               {
                  var dup = new Page_Role();
                  var dups = result.Object as List<Page_Role>;
                  if (dups != null && dups.Count() != 0)
                     if (model.operation == Operation.C)
                        ModelState.AddModelError("Page_ID", Resource.The + " " + Resource.Page + " " + Resource.Field + " " + Resource.Is_Duplicated);
                     else if (model.operation == Operation.U)
                     {
                        dup = dups.FirstOrDefault();
                        if (dup.Page_Role_ID != model.Page_Role_ID)
                           ModelState.AddModelError("Page_ID", Resource.The + " " + Resource.Page + " " + Resource.Field + " " + Resource.Is_Duplicated);
                     }
               }
            }

            if (ModelState.IsValid)
            {
               var prole = new Page_Role();
               if (model.operation == Operation.U || model.operation == Operation.D)
               {
                  var cri = new RoleCriteria();
                  cri.Page_Role_ID = model.Page_Role_ID;
                  var result = rService.GetPageRole(cri);
                  if (result.Code == ReturnCode.SUCCESS)
                  {
                     var proles = result.Object as List<Page_Role>;
                     if (proles != null && proles.Count() == 1)
                        prole = proles.FirstOrDefault();
                  }
               }

               if (model.operation != Operation.D)
               {
                  prole.Role_ID = model.Role_ID;
                  prole.Page_ID = model.Page_ID;
                  prole.Modify = (model.Modify.HasValue && model.Modify.Value ? true : false);
                  prole.View = (model.View.HasValue && model.View.Value ? true : false);
                  prole.Close = (model.Close.HasValue && model.Close.Value ? true : false);

                  if (prole.Modify.HasValue && prole.Modify.Value)
                     prole.View = true;

                  if (prole.Page_ID == 9 && prole.Role_ID == 5)
                  {
                     prole.View = true;
                     prole.Modify = true;
                  }
               }

               if (model.operation == Operation.C)
                  model.result = rService.InsertPageRole(prole);

               else if (model.operation == Operation.U)
                  model.result = rService.UpdatePageRole(prole);

               else if (model.operation == Operation.D)
               {
                  prole.Record_Status = Record_Status.Delete;
                  model.result = rService.UpdatePageRole(prole);
                  if (model.result.Code == ReturnCode.SUCCESS)
                     model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Page_Role };
                  else
                     model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Page_Role };

                  return RedirectToAction("RoleSetup", new AppRouteValueDictionary(model));
               }

               if (model.result.Code == ReturnCode.SUCCESS)
                  return RedirectToAction("RoleSetup", new AppRouteValueDictionary(model));
            }
         }

         var cbService = new ComboService();
         model.cPageList = cbService.LstPage();
         model.cRoleList = cbService.LstRole(true);

         var rcri = new RoleCriteria();
         rcri.Role_ID = model.Role_ID;
         var result2 = rService.GetPageRole(rcri);
         if (result2.Code == ReturnCode.SUCCESS)
            model.PageRoleList = result2.Object as List<Page_Role>;
         return View(model);
      }
   }
}