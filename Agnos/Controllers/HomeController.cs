using AgnosModel.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Agnos.Controllers
{
   public class HomeController : ControllerBase
   {
      public ActionResult Index()
      {
         if (User.Identity.IsAuthenticated)
         {
            var uService = new UserService();
            var pAction = Page_Code.GetPageAction(uService.DefaultPageRole(User.Identity.GetUserId()));
            if (pAction != null)
               return RedirectToAction(pAction.Action, pAction.Controller);
         }

         //return View();
         return RedirectToAction("Login", "Account");
      }
   }
}