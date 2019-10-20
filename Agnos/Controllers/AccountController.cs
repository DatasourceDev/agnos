using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Agnos.Models;
using AgnosModel.Models;
using AgnosModel.Service;
using AppFramework.Util;
using AppFramework;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Security.Principal;
using System.Net;
using System.Security.Permissions;
using System.Globalization;
using System.Security;
using SBSResourceAPI;
using System.Configuration;

//using Novell.Directory.Ldap;

namespace Agnos.Controllers
{
   [Authorize]
   public class AccountController : ControllerBase
   {
      public AccountController()
         : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new AgnosDBContext())))
      {
      }

      public AccountController(UserManager<ApplicationUser> userManager)
      {
         UserManager = userManager;
      }

      public UserManager<ApplicationUser> UserManager { get; private set; }

      [HttpGet]
      [AllowAnonymous]
      public ActionResult Login(string returnUrl)
      {
         ViewBag.ReturnUrl = returnUrl;
         return View(new LoginViewModel());
      }


      //[HttpGet]
      //[AllowAnonymous]
      //public async Task<ActionResult> Login(string returnUrl)
      //{
      //   log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      //   string DN = "";
      //   var name = HttpContext.ApplicationInstance.Server.MachineName;
      //   logger.Debug("Machine name : " + name);
      //   //name = "ags00003";

      //   string ldapserver =  ConfigurationManager.AppSettings["ldapserver"];
      //   string ldapbasedn = ConfigurationManager.AppSettings["ldapbasedn"];
      //   string ldapuser = ConfigurationManager.AppSettings["ldapuser"];
      //   string ldappassword = ConfigurationManager.AppSettings["ldappassword"];
      //   string ldapfilter = "(&(objectclass=person)(uid={0}))";
      //   try
      //   {
      //      using (DirectoryEntry entry = new DirectoryEntry(ldapserver, ldapuser, ldappassword, AuthenticationTypes.None))
      //      {
      //         DirectorySearcher ds = new DirectorySearcher(entry);
      //         ds.Filter = string.Format(ldapfilter, name);
      //         SearchResult result = ds.FindOne();
      //         if (result != null)
      //         {
      //            var cri = new UserCriteria();
      //            cri.LDAP = name;
      //            var uService = new UserService();
      //            var users = uService.GetUser(cri);
      //            if (users != null && users.Object != null && (users.Object as List<User_Profile>).Count() == 1)
      //            {
      //               var userlogin = (users.Object as List<User_Profile>).FirstOrDefault();
      //               var u = await UserManager.FindByIdAsync(userlogin.ApplicationUser_Id);
      //               if (u != null)
      //               {
      //                  await SignInAsync(u, true);
      //                  return RedirectToAction("Material", "Material");
      //               }
      //            }
      //         }
      //      }
      //      return RedirectToAction("ErrorPage", new { Message = "The username " + name + " not found." });
      //   }
      //   catch (Exception ex)
      //   {
      //      var msg = ex.Message;
      //      return RedirectToAction("Login");
      //   }

      //}

      [HttpGet]
      public ActionResult ErrorPage(ErrorViewModel model)
      {
         return View(model);
      }

      //
      // POST: /Account/Login
      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
      {
         if (ModelState.IsValid)
         {
            var uService = new UserService();
            var user = await UserManager.FindAsync(model.Email_Address, model.Password);
            if (user != null)
            {
               var uprofile = uService.getUser(user.Id);
               if (uprofile != null)
               {
                  HttpContext.Session["User"] = uprofile;
                  uService.UpdateLastConnection(uprofile.Profile_ID);
               }
               await SignInAsync(user, model.RememberMe);
               return RedirectToLocal(returnUrl);
            }
            else
            {
               var uprofile = uService.getUserByUsername(model.Email_Address);
               if (uprofile != null)
               {
                  user = await UserManager.FindAsync(uprofile.Email_Address, model.Password);
                  if (user != null)
                  {
                     HttpContext.Session["User"] = uprofile;
                     uService.UpdateLastConnection(uprofile.Profile_ID);
                     await SignInAsync(user, model.RememberMe);
                     return RedirectToLocal(returnUrl);
                  }
               }
               model.Message = "Invalid username or password.";
               uService.UpdateLoginAttempt(model.Email_Address);
            }
         }
         return View(model);
      }

      //
      // GET: /Account/Register
      [AllowAnonymous]
      public ActionResult Register()
      {
         return View(new RegisterViewModel());
      }

      //
      // POST: /Account/Register
      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Register(RegisterViewModel model)
      {

         if (ModelState.IsValid)
         {
            var user = new User_Profile();
            user.Email_Address = model.Email_Address;
            user.PWD = EncryptUtil.Encrypt(model.Password);

            var uService = new UserService(user);
            var result = uService.InsertUser(user);
            if (result.Code == ReturnCode.SUCCESS)
            {
               UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new AgnosDBContext()));
               userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };
               IdentityResult iresult = await userManager.RemovePasswordAsync(user.ApplicationUser_Id);
               if (iresult.Succeeded)
               {
                  iresult = await userManager.AddPasswordAsync(user.ApplicationUser_Id, model.Password);
                  if (iresult.Succeeded)
                  {
                     var userauthen = await UserManager.FindAsync(model.Email_Address, model.Password);
                     if (userauthen != null)
                     {
                        await SignInAsync(userauthen, true);
                        return RedirectToAction("Material", "Material");
                     }
                  }
               }

            }

         }

         // If we got this far, something failed, redisplay form
         return View(model);
      }

      //
      // POST: /Account/Disassociate
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
      {
         ManageMessageId? message = null;
         IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
         if (result.Succeeded)
         {
            message = ManageMessageId.RemoveLoginSuccess;
         }
         else
         {
            message = ManageMessageId.Error;
         }
         return RedirectToAction("Manage", new { Message = message });
      }

      //
      // GET: /Account/Manage
      public ActionResult Manage(ManageMessageId? message)
      {
         ViewBag.StatusMessage =
             message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
             : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
             : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
             : message == ManageMessageId.Error ? "An error has occurred."
             : "";
         ViewBag.HasLocalPassword = HasPassword();
         ViewBag.ReturnUrl = Url.Action("Manage");
         return View();
      }

      public ActionResult LogOff()
      {
         AuthenticationManager.SignOut();
         return RedirectToAction("Login", "Account");
      }

      [AllowAnonymous]
      public ActionResult Logout()
      {
         AuthenticationManager.SignOut();
         HttpContext.Session.Clear();
         return RedirectToAction("Login", "Account");
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing && UserManager != null)
         {
            UserManager.Dispose();
            UserManager = null;
         }
         base.Dispose(disposing);
      }

      [HttpPost]
      [AllowAnonymous]
      public ActionResult ForgotPassword(string email)
      {
         if (!string.IsNullOrEmpty(email))
         {
            var uService = new UserService();
            var user = uService.getUserByEmail(email);
            if (user != null)
            {
               var domain = UrlUtil.GetDomains(Request.Url, "Agnos");
               int result = uService.sendResetPassword(user.Profile_ID, domain);
               if (result > 0)
               {
                  return Json(new { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_SEND_EMAIL) }, JsonRequestBehavior.AllowGet);
               }
               else
               {
                  return Json(new { Code = ReturnCode.ERROR_CANT_SEND_EMAIL, Msg = Error.GetMessage(ReturnCode.ERROR_CANT_SEND_EMAIL) }, JsonRequestBehavior.AllowGet);
               }
            }
         }
         return Json(new { Code = ReturnCode.ERROR_UNAUTHORIZED, Msg = Error.GetMessage(ReturnCode.ERROR_UNAUTHORIZED) }, JsonRequestBehavior.AllowGet);
      }

      [HttpGet]
      [AllowAnonymous]
      public ActionResult ResetPassword(String code = "", int uid = 0)
      {
         var currentdate = StoredProcedure.GetCurrentDate();
         ResetPasswordViewModel model = new ResetPasswordViewModel();
         var userService = new UserService();
         bool activate = false;
         model.IsActivationLink = false;
         if (HttpContext.Session["Activate"] != null)
         {
            activate = (bool)HttpContext.Session["Activate"];
         }
         //By Activation
         if (activate)
         {
            User_Profile user = GetUser();
            if (user != null)
            {
               model.uid = user.Profile_ID;
               model.name = user.Name;
               HttpContext.Session["ResetPassword"] = model.uid;
               HttpContext.Session["ResetPassword_NotValidateCurrent"] = true;
               model.notValidateCurrent = true;
            }
         }
         else if (code.Length == 0 && uid == 0 && isAuthenticatedUser())
         {
            User_Profile user = GetUser();
            if (user != null)
            {
               model.uid = user.Profile_ID;
               model.name = user.Name;
               HttpContext.Session["ResetPassword"] = model.uid;
               HttpContext.Session["ResetPassword_NotValidateCurrent"] = false;
               model.notValidateCurrent = false;
            }
         }
         else if (code.Length > 0 && uid == 0)
         {
            //By LINK
            Activation_Link link = userService.GetActivationLink(code);
            if (link != null)
            {
               if (link.Time_Limit > currentdate)
               {
                  User_Profile user = userService.getUser(link.Profile_ID);
                  if (user != null)
                  {
                     model.IsActivationLink = true;
                     model.uid = user.Profile_ID;
                     model.name = user.Name;
                     HttpContext.Session["ResetPassword"] = user.Profile_ID;
                     HttpContext.Session["ResetPassword_NotValidateCurrent"] = true;
                     HttpContext.Session["ResetPassword_ID"] = link.Activation_ID;
                     model.notValidateCurrent = true;
                  }
               }
               else
               {
                  //ERROR4
                  return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Error.GetMessage(ReturnCode.ERROR_RESET_PASSWORD_EXPIRE) });
               }
            }
            else
            {
               //ERROR5
               return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Error.GetMessage(ReturnCode.ERROR_RESET_PASSWORD_CODE_NOT_FOUND) });
            }
         }
         else if (uid > 0)
         {
            User_Profile user = userService.getUser(uid);
            if (user != null)
            {
               model.uid = user.Profile_ID;
               model.name = user.Name;
               HttpContext.Session["ResetPassword"] = model.uid;
               HttpContext.Session["ResetPassword_NotValidateCurrent"] = true;
               model.notValidateCurrent = true;
            }
         }
         else
         {
            return returnUnAuthorize();
         }
         return View(model);
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
      {
         var userService = new UserService();
         User_Profile user = userService.getUser(model.uid);
         model.name = user.Name;
         model.notValidateCurrent = false;
         if (HttpContext.Session["ResetPassword_NotValidateCurrent"] != null && HttpContext.Session["ResetPassword_NotValidateCurrent"] as bool? == true)
            model.notValidateCurrent = true;

         if (HttpContext.Session["ResetPassword"] != null && HttpContext.Session["ResetPassword"] as int? == model.uid)
         {
            if (!model.notValidateCurrent)
            {
               if (string.IsNullOrEmpty(model.OldPassword))
               {
                  ModelState.AddModelError("OldPassword", "The Current Password field is required.");
               }
            }

            if (model.notValidateCurrent || (!string.IsNullOrEmpty(model.OldPassword) && (user.PWD.Equals(UserService.hashSHA256(model.OldPassword)))))
            {
               if (ModelState.IsValid)
               {
                  var result = userService.ResetPassword(model.uid, model.NewPassword);
                  if (result.Code != ReturnCode.SUCCESS)
                  {
                     return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Error.GetMessage(ReturnCode.ERROR_UPDATE) });
                  }
                  else
                  {
                     UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new AgnosDBContext()));
                     userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };

                     IdentityResult iresult = await userManager.RemovePasswordAsync(user.ApplicationUser_Id);
                     if (iresult.Succeeded)
                     {
                        iresult = await userManager.AddPasswordAsync(user.ApplicationUser_Id, model.NewPassword);
                        if (iresult.Succeeded)
                        {
                           HttpContext.Session.Remove("ResetPassword_NotValidateCurrent");
                           HttpContext.Session.Remove("ResetPassword");
                           HttpContext.Session.Remove("Activate");

                           if (model.notValidateCurrent)
                           {
                              //SET LIMIT TIME
                              if (HttpContext.Session["ResetPassword_ID"] != null)
                              {
                                 userService.SetExpireActivationLinkTimeLimit((HttpContext.Session["ResetPassword_ID"] as int?).Value);
                              }
                           }
                           return View("ResetPasswordComplete");
                        }
                        else
                        {
                           //TODO
                           AddErrors(iresult);
                        }
                     }
                     else
                     {
                        //TODO
                        AddErrors(iresult);
                     }
                  }
               }
            }
            else
               ModelState.AddModelError("OldPassword", Resource.Is_Inccorect);
         }
         else
            return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Error.GetMessage(ReturnCode.ERROR_UNAUTHORIZED) });

         var v = GetErrorModelState();
         return View(model);
      }

      // GET: /Account/ResetPassword
      //[HttpGet]
      //[AllowAnonymous]
      //public ActionResult ResetPassword(String code = "", int uid = 0)
      //{
      //   var model = new ResetPasswordViewModel();
      //   return View(model);
      //}

      //[HttpPost]
      //[AllowAnonymous]
      //[ValidateAntiForgeryToken]
      //public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
      //{
      //   var uService = new UserService();
      //   var user = uService.getUser(User.Identity.GetUserId());
      //   if (string.IsNullOrEmpty(model.OldPassword))
      //      ModelState.AddModelError("OldPassword", Resource.Message_Is_Required);

      //   if(!string.IsNullOrEmpty(model.OldPassword) && user.PWD != UserService.hashSHA256(model.OldPassword))
      //      ModelState.AddModelError("OldPassword", "Old Password is incorrect.");

      //   if (ModelState.IsValid)
      //   {
      //      model.result = uService.ResetPassword(user.Profile_ID, model.NewPassword);
      //      if (model.result.Code == ReturnCode.SUCCESS)
      //      {
      //         UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new AgnosDBContext()));
      //         userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };
      //         IdentityResult iresult = await userManager.RemovePasswordAsync(user.ApplicationUser_Id);
      //         if (iresult.Succeeded)
      //         {
      //            iresult = await userManager.AddPasswordAsync(user.ApplicationUser_Id, model.NewPassword);
      //            if (iresult.Succeeded)
      //            {
      //               model.OldPassword = null;
      //               model.NewPassword = null;
      //               model.ConfirmPassword = null;
      //               return View(model);
      //            }
      //         }
      //      }
      //   }
      //   return View(model);
      //}

      #region Helpers
      // Used for XSRF protection when adding external logins
      private const string XsrfKey = "XsrfId";

      private IAuthenticationManager AuthenticationManager
      {
         get
         {
            return HttpContext.GetOwinContext().Authentication;
         }
      }

      private async Task SignInAsync(ApplicationUser user, bool isPersistent)
      {
         AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
         var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
         AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
      }

      private void AddErrors(IdentityResult result)
      {
         foreach (var error in result.Errors)
         {
            ModelState.AddModelError("", error);
         }
      }

      private bool HasPassword()
      {
         var user = UserManager.FindById(User.Identity.GetUserId());
         if (user != null)
         {
            return user.PasswordHash != null;
         }
         return false;
      }

      public enum ManageMessageId
      {
         ChangePasswordSuccess,
         SetPasswordSuccess,
         RemoveLoginSuccess,
         Error
      }

      private ActionResult RedirectToLocal(string returnUrl)
      {
         if (Url.IsLocalUrl(returnUrl))
         {
            return Redirect(returnUrl);
         }
         else
         {
            return RedirectToAction("Index", "Home");
         }
      }

      private class ChallengeResult : HttpUnauthorizedResult
      {
         public ChallengeResult(string provider, string redirectUri)
            : this(provider, redirectUri, null)
         {
         }

         public ChallengeResult(string provider, string redirectUri, string userId)
         {
            LoginProvider = provider;
            RedirectUri = redirectUri;
            UserId = userId;
         }

         public string LoginProvider { get; set; }
         public string RedirectUri { get; set; }
         public string UserId { get; set; }

         public override void ExecuteResult(ControllerContext context)
         {
            var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
            if (UserId != null)
            {
               properties.Dictionary[XsrfKey] = UserId;
            }
            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
         }
      }
      #endregion
   }
}