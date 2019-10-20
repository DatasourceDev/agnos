using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace AgnosModel
{
   public class AppSetting
   {
      public static string ToolKitEnterpriseDirectory
      {
         get
         {
            try { return ConfigurationManager.AppSettings["ToolKitEnterpriseDirectory"].ToString(); }
            catch { return ""; }
         }
      }
      public static string ToolKitDataDirectory
      {
         get
         {
            try { return ConfigurationManager.AppSettings["ToolKitDataDirectory"].ToString(); }
            catch { return ""; }
         }
      }

      public static string SERVER_NAME
      {
         get
         {
            try { return ConfigurationManager.AppSettings["SERVER_NAME"].ToString(); }
            catch { return ""; }
         }
      }

      public static string SMTP_SERVER
      {
         get
         {
            try { return ConfigurationManager.AppSettings["SMTP_SERVER"].ToString(); }
            catch { return ""; }
         }
      }

      public static string SMTP_PORT
      {
         get
         {
            try { return ConfigurationManager.AppSettings["SMTP_PORT"].ToString(); }
            catch { return ""; }
         }
      }

      public static string SMTP_USERNAME
      {
         get
         {
            try { return ConfigurationManager.AppSettings["SMTP_USERNAME"].ToString(); }
            catch { return ""; }
         }
      }

      public static string SMTP_PASSWORD
      {
         get
         {
            try { return ConfigurationManager.AppSettings["SMTP_PASSWORD"].ToString(); }
            catch { return ""; }
         }
      }

      public static bool SMTP_SSL
      {
         get
         {
            try { return Convert.ToBoolean(ConfigurationManager.AppSettings["SMTP_SSL"]); }
            catch { return false; }
         }
      }

      public static bool Is_Station
      {
         get
         {
            try { return Convert.ToBoolean(ConfigurationManager.AppSettings["Is_Station"]); }
            catch { return false; }
         }
      }

      public static string Station_Code
      {
         get
         {
            try { return ConfigurationManager.AppSettings["Station_Code"].ToString(); }
            catch { return ""; }
         }
      }

      public static bool Is_Local
      {
         get
         {
            try { return Convert.ToBoolean(ConfigurationManager.AppSettings["Is_Local"]); }
            catch { return false; }
         }
      }

      public static string ComPort_Name
      {
         get
         {
            try { return ConfigurationManager.AppSettings["ComPort_Name"].ToString(); }
            catch { return ""; }
         }
      }

      public static int Drum_Type_Length
      {
         get
         {
            try { return Convert.ToInt32(ConfigurationManager.AppSettings["Drum_Type_Length"]); }
            catch { return 2; }
         }
      }

      public static Decimal App_Version
      {
         get
         {
            try { return Convert.ToDecimal(ConfigurationManager.AppSettings["App_Version"]); }
            catch { return 1; }
         }
      }

      public static string SMTP_FROM
      {
         get
         {
            try { return ConfigurationManager.AppSettings["SMTP_FROM"].ToString(); }
            catch { return ""; }
         }
      }
   }
}
