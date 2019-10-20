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
   public class UserController : ControllerBase
   {
      // GET: User
      [HttpGet]
      public ActionResult Users(UserViewModel model, ServiceResult msgresult)
      {
         var uService = new UserService(User.Identity.GetUserId());
         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0010);
         if (prole == null)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
         if (prole.View == null || prole.View == false)
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

         ModelState.Clear();
         if (model.operation == Operation.D)
         {
            var dcri = new UserCriteria();
            dcri.Profile_ID = model.Profile_ID;
            var result = uService.GetUser(dcri);
            if (result.Code == ReturnCode.SUCCESS)
            {
               var users = result.Object as List<User_Profile>;
               if (users != null && users.Count() == 1)
               {
                  var user = new User_Profile();
                  user = users.FirstOrDefault();
                  user.Record_Status = Record_Status.Delete;
                  model.result = uService.UpdateUser(user);
                  if (model.result.Code == ReturnCode.SUCCESS)
                     msgresult = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.User };
                  else
                     msgresult = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.User };
               }
            }
         }

         model.result = msgresult; // return result from http post
         model.Modify = prole.Modify;
         model.View = prole.View;

         var cri = new UserCriteria();
         var uresult = uService.GetUser(cri);
         if (uresult.Code == ReturnCode.SUCCESS)
            model.Userlist = uresult.Object as List<User_Profile>;

         var cbService = new ComboService();
         model.cRole = cbService.LstRole();

         return View(model);
      }

      [HttpPost]
      public async Task<ActionResult> Users(UserViewModel model)
      {
         var uService = new UserService(GetUser());
         if (model.operation != Operation.C)
         {
            List<string> formatTemp = new List<string>();
            formatTemp.AddRange(new string[] { "Password", "Confirm_Password" });

            foreach (var key in ModelState.Keys.ToList().Where(key => ModelState.ContainsKey(key) && (formatTemp.Contains(key))))
               ModelState[key].Errors.Clear();
         }
         var errs = GetErrorModelState();

         var guser = uService.getUserByEmail(model.Email_Address);
         if (guser != null)
         {
            if (model.operation == Operation.C)
               ModelState.AddModelError("Email_Address", Resource.The + " " + Resource.Email + " " + Resource.Is_Rrequired);
            else if (model.operation == Operation.U && model.Profile_ID != guser.Profile_ID)
               ModelState.AddModelError("Email_Address", Resource.The + " " + Resource.Email + " " + Resource.Is_Rrequired);
         }

         if (ModelState.IsValid)
         {
            var user = new User_Profile();
            if (model.operation == Operation.U)
            {
               var cri = new UserCriteria();
               cri.Profile_ID = model.Profile_ID;
               var result = uService.GetUser(cri);
               if (result.Code == ReturnCode.SUCCESS)
               {
                  var users = result.Object as List<User_Profile>;
                  if (users != null && users.Count() == 1)
                     user = users.FirstOrDefault();
               }
            }

            user.Email_Address = model.Email_Address;
            user.Name = model.Name;
            user.LDAP_Username = model.LDAP_Username;
            user.Role_ID = model.Role_ID;
            user.Email_Notification = model.Email_Notification;

            if (model.operation == Operation.C)
            {
               user.Activated = true;
               var uService2 = new UserService(user);
               user.PWD = model.Password;
               model.result = uService2.InsertUser(user);
               if (model.result.Code == ReturnCode.SUCCESS)
               {
                  UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new AgnosDBContext()));
                  userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };
                  IdentityResult iresult = await userManager.RemovePasswordAsync(user.ApplicationUser_Id);
                  if (iresult.Succeeded)
                  {
                     iresult = await userManager.AddPasswordAsync(user.ApplicationUser_Id, model.Password);
                     if (iresult.Succeeded)
                     {

                     }
                  }
               }
            }
            else if (model.operation == Operation.U)
               model.result = uService.UpdateUser(user);

            if (model.result.Code == ReturnCode.SUCCESS)
               return RedirectToAction("Users", new AppRouteValueDictionary(model));
         }

         var cri2 = new UserCriteria();
         var uresult = uService.GetUser(cri2);
         if (uresult.Code == ReturnCode.SUCCESS)
            model.Userlist = uresult.Object as List<User_Profile>;

         var cbService = new ComboService();
         model.cRole = cbService.LstRole();

         var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0010);
         if (prole != null)
            model.Modify = prole.Modify;

         return View(model);
      }
   }
}