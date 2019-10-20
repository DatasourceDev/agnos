using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgnosModel.Models;
using AgnosModel.Service;
using System.Web.Routing;
using AgnosCMS.Models;
using System.IO;
using System.Data.Entity.Core.Objects;
using AppFramework;

namespace AgnosCMS.Controllers
{
   public class ControllerBase : Controller
   {
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