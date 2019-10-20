using AgnosModel.Models;
using AppFramework.Util;
using SBSResourceAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Agnos.Common
{

   public class EmailCriteria
   {
      public string Lot_No { get; set; }
      public string Status { get; set; }
      public string from { get; set; }
   }

   public class EmailAgnos : EmailTemplete
   {
      public static string sendClosedReject(User_Profile send_to, string cc, EmailCriteria cri = null)
      {
         var IsSuccess = String.Empty;
         if (send_to != null)
         {
            var message = new StringBuilder();
            message.Append("Dear All,");
            message.Append("<br/> <br />");
            message.Append("Your <span style='font-weight:700;' >Reject Material</span> has been<span style='color:#0E9D41;font-weight:700;font-size:18px;' > " + cri.Status + "</span> with the following details:");
            message.Append("<br/> <br />");

            message.Append("<table style='border-collapse: collapse; line-height: 30px;width:100%' cellpadding='6'> ");
            message.Append("<tr style='border-bottom: 1px solid #ccc'>");
            message.Append("<td style='width:120px;'></td>");
            message.Append("<td> <span style='font-weight:700;' ></span></td>");
            message.Append("</tr>");

            if (!string.IsNullOrEmpty(cri.Lot_No))
            {
               message.Append("<tr style='border-bottom: 1px solid #ccc'>");
               message.Append("<td style='width:120px;'>" + Resource.Lot_No + " : </td>");
               message.Append("<td> <span style='font-weight:700;' >" + cri.Lot_No + "</span></td>");
               message.Append("</tr>");
            }

            //message.Append("<tr style='border-bottom: 1px solid #ccc'>");
            //message.Append("<td style='width:120px;'> Received  from : </td>");
            //message.Append("<td> <span style='font-weight:700;' >" + cc.Name + "</span></td>");
            message.Append("</tr>");
            message.Append("</table>");
            message.Append("<br/> <br />");

            IsSuccess = sendNotificationEmail(send_to.Email_Address, "Reject Materials has been " + cri.Status, message.ToString(), null, cc, cri.from);
         }
         return IsSuccess;
      }

      public static string sendClosedLogsheet(User_Profile send_to, string cc, EmailCriteria cri = null)
      {
         var IsSuccess = String.Empty;
         if (send_to != null)
         {
            var message = new StringBuilder();
            message.Append("Dear All,");
            message.Append("<br/> <br />");
            message.Append("Your <span style='font-weight:700;' >Logsheet (" + cri.Status + ")</span> has been<span style='color:#0E9D41;font-weight:700;font-size:18px;' > " + Material_Overall_Status.Closed + "</span> with the following details:");
            message.Append("<br/> <br />");

            message.Append("<table style='border-collapse: collapse; line-height: 30px;width:100%' cellpadding='6'> ");
            message.Append("<tr style='border-bottom: 1px solid #ccc'>");
            message.Append("<td style='width:120px;'></td>");
            message.Append("<td> <span style='font-weight:700;' ></span></td>");
            message.Append("</tr>");

            if (!string.IsNullOrEmpty(cri.Lot_No))
            {
               message.Append("<tr style='border-bottom: 1px solid #ccc'>");
               message.Append("<td style='width:120px;'>" + Resource.Lot_No + " : </td>");
               message.Append("<td> <span style='font-weight:700;' >" + cri.Lot_No + "</span></td>");
               message.Append("</tr>");
            }

            //message.Append("<tr style='border-bottom: 1px solid #ccc'>");
            //message.Append("<td style='width:120px;'> Received  from : </td>");
            //message.Append("<td> <span style='font-weight:700;' >" + send_to.Name + "</span></td>");

            message.Append("</tr>");
            message.Append("</table>");
            message.Append("<br/> <br />");

            IsSuccess = sendNotificationEmail(send_to.Email_Address, "Logsheet (" + cri.Status + ") has been Closed", message.ToString(), null, cc, cri.from);
         }
         return IsSuccess;
      }
   }
}