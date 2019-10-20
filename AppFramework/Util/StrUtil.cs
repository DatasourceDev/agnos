using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppFramework.Util
{
   public class StrUtil
   {
      public static bool IsValidEmail(string email)
      {
         try
         {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
         }
         catch
         {
            return false;
         }
      }

      public static string SubstringLeft(string str, int length)
      {

         try
         {
            str = (str ?? string.Empty);
            return str.Substring(0, Math.Min(length, str.Length));
         }
         catch
         {
            return "";
         }


      }

      public static string SubstringRight(string str, int length)
      {


         try
         {
            str = (str ?? string.Empty);
            return (str.Length >= length)
                ? str.Substring(str.Length - length, length)
                : str;
         }
         catch
         {
            return "";
         }


      }
   }


}