using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Util
{
   public class UrlUtil
   {
      public static string GetDomain(Uri Url)
      {
         var urlstr = "";
         if (Url.Host.ToLower() == "localhost")
         {
            urlstr = Url.Scheme + "://" + Url.Authority + "/";
         }
         else
         {
            urlstr = Url.Scheme + "://" + Url.Host + "/";
         }
         return urlstr;
      }

      public static string GetDomains(Uri Url, string modulename = "")
      {
         var urlstr = "";
         if (Url.Host.ToLower() == "localhost")
         {
            urlstr = Url.Scheme + "://" + Url.Authority + "/";
         }
         else
         {
            urlstr = Url.Scheme + "://" + Url.Host + "/" + modulename + "/";
         }

         return urlstr;
      }
   }
}
