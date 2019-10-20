using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgnosModel.Models;
using AgnosModel.Service;
using System.Web.Routing;
using Agnos.Models;
using System.IO;
using AppFramework;

namespace Agnos.Controllers
{
   public class ControllerBase : Controller
   {
      public ActionResult ReloadProductList()
      {
         var cbService = new ComboService();
         var exService = new ExchequerService();
         var list =   exService.LstExchquerProduct(true);
         return Json(list, JsonRequestBehavior.AllowGet);
      }

      public static byte[] ToByteArray(Stream stream)
      {
         if (stream is MemoryStream)
         {
            return ((MemoryStream)stream).ToArray();
         }
         else
         {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
               int read;
               while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
               {
                  ms.Write(buffer, 0, read);
               }
               return ms.ToArray();
            }
         }
      }

      protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
      {
         string lang = null;
         HttpCookie langCookie = Request.Cookies["culture"];
         if (langCookie != null)
         {
            lang = langCookie.Value;
         }
         else
         {
            var userLanguage = Request.UserLanguages;
            var userLang = userLanguage != null ? userLanguage[0] : "";
            if (userLang != "")
            {
               lang = userLang;
            }
            else
            {
               lang = SBSResourceAPI.SBSResourceAPI.SiteLanguages.GetDefaultLanguage();
            }
         }

         new SBSResourceAPI.SBSResourceAPI.SiteLanguages().SetLanguage(lang);

         return base.BeginExecuteCore(callback, state);
      }

      public string[] GetErrorModelState()
      {
         return this.ViewData.ModelState.SelectMany(m => m.Value.Errors, (m, error) => (m.Key + " : " + error.ErrorMessage)).ToArray();
      }

      public string RenderPartialViewAsString(string viewName, object model)
      {
         StringWriter stringWriter = new StringWriter();

         ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
         ViewContext viewContext = new ViewContext(
                 ControllerContext,
                 viewResult.View,
                 new ViewDataDictionary(model),
                 new TempDataDictionary(),
                 stringWriter
                 );

         viewResult.View.Render(viewContext, stringWriter);
         return stringWriter.ToString();
      }

      public User_Profile GetUser()
      {
         var userSession = HttpContext.Session["User"] as User_Profile;
         if (User.Identity.IsAuthenticated)
         {
            if (userSession == null)
            {
               var userService = new UserService();
               User_Profile profile = userService.getUserByEmail(User.Identity.Name);
               if (profile != null)
               {
                  HttpContext.Session["User"] = profile;
                  userSession = HttpContext.Session["User"] as User_Profile;
               }
            }
         }
         return userSession;
      }

      public Boolean isAuthenticatedUser()
      {
         if (GetUser() != null)
            return true;
         else
            return false;
      }

      public ActionResult returnUnAuthorize()
      {
         return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Error.GetMessage(ReturnCode.ERROR_RESET_PASSWORD_CODE_NOT_FOUND) });
      }

      public string GetRecipientsNotification()
      {
         var receivers = string.Empty;
         var uService = new UserService();
         var cri = new UserCriteria();
         cri.Email_Notification = true;
         var uresult = uService.GetUser(cri);
         if (uresult.Code == ReturnCode.SUCCESS)
         {
            var users = uresult.Object as List<User_Profile>;
            if (users != null && users.Count > 0)
               receivers = (string.Join(",", users.Select(x => x.Email_Address.ToString()).ToArray()));
         }
         return receivers;
      }
   }

   public class AppRouteValueDictionary : RouteValueDictionary
   {
      public AppRouteValueDictionary(object obj)
      {
         var model = obj as ModelBase;
         this.Add("tabAction", model.tabAction);
         if (model.result != null)
         {
            this.Add("Code", model.result.Code);
            this.Add("Msg", model.result.Msg);
            this.Add("Field", model.result.Field);
            //this.Add("operation", model.operation);
         }
      }
   }
}