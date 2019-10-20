using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace AppFramework.Util
{
   public class FileAttach
   {
      public string File_Name { get; set; }
      public MemoryStream File { get; set; }
   }
   public class EmailTemplete
   {
      public static string sendNotificationEmail(String to, String header, String message, List<FileAttach> files = null, String cc = "", string form = "")
      {
         try
         {
            bool Is_Local = false;
            bool.TryParse(ConfigurationManager.AppSettings["Is_Local"].ToString(), out Is_Local);
            if (Is_Local)
            {
               if (!Directory.Exists(@"c:\agnos\agnos_email_send_out"))
                  Directory.CreateDirectory(@"c:\agnos\agnos_email_send_out");

               var path = @"c:\agnos\agnos_email_send_out\" + header.Replace(":", "_") + ".htm";
               using (FileStream fs = new FileStream(path, FileMode.Create))
               {
                  using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                  {
                     w.WriteLine(message);
                  }
               }
            }

            //to = "ijane2462@gmail.com";
            var SMTP_SERVER = ConfigurationManager.AppSettings["SMTP_SERVER"].ToString();
            var SMTP_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"].ToString());
            var SMTP_USERNAME = ConfigurationManager.AppSettings["SMTP_USERNAME"].ToString();
            var SMTP_PASSWORD = ConfigurationManager.AppSettings["SMTP_PASSWORD"].ToString();
            var SMTP_FROM = ConfigurationManager.AppSettings["SMTP_FROM"].ToString();
            if (form != "")
            {
                //SMTP_FROM = ConfigurationManager.AppSettings["SMTP_FROM"].ToString();
                SMTP_FROM = form;
            }

            bool STMP_SSL = false;
            bool.TryParse(ConfigurationManager.AppSettings["SMTP_SSL"].ToString(), out STMP_SSL);

            SmtpClient smtpClient = new SmtpClient(SMTP_SERVER, SMTP_PORT);

            System.Net.NetworkCredential cred = new System.Net.NetworkCredential();
            cred.UserName = SMTP_USERNAME;
            cred.Password = SMTP_PASSWORD;

            smtpClient.UseDefaultCredentials = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = STMP_SSL;

            message += "<br/> <br />" +
                "<br/> <br />" +
                "This is an auto-generated email. Please do not reply.";

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(SMTP_FROM);
            mail.To.Add(to);
            mail.Subject = header;
            mail.Body = message;

            if (!string.IsNullOrEmpty(cc))
            {
               string[] CCId = cc.Split(',');
               foreach (string CCEmail in CCId)
                  mail.CC.Add(new MailAddress(CCEmail));
            }

            if (files != null && files.Count > 0)
            {
               foreach (var file in files)
               {
                  Attachment data = new Attachment(file.File, file.File_Name);
                  mail.Attachments.Add(data);
               }
            }

            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            smtpClient.Credentials = cred;
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            smtpClient.Send(mail);
            return "Success";
         }
         catch (Exception ex)
         {
            System.Diagnostics.Debug.WriteLine(ex);
            return ex.Message;
         }
      }

      public static void sendResetPasswordEmail(String email, String code, String Name, string domain)
      {
         String ResetPassword_link = domain + "Account/ResetPassword?code=" + code;

         String message = "<p>Dear " + Name + ", " +
             "<br/> <br />" +
             "Reset Password Link: <a href='" + ResetPassword_link + "'>" + ResetPassword_link + "</a>" +
             "<br/> <br />" +
            //"If you have any enquiries, please contact us at 6696 5457 or email us at enquiry@bluecube.com.sg" +
             "<br/> <br />" +
             "Regards," +
             "<br/> " +
             "Agnos Team";

         sendNotificationEmail(email, "Agnos Reset Password of Account", message);
      }
   }
}