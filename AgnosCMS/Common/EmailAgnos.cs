using AgnosModel.Models;
using AppFramework.Util;
using SBSResourceAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AgnosCMS.Common
{
   public class EmailAgnos : EmailTemplete
   {
      //public static bool sendClosedReject(User_Profile sendto)
      //{
      //   var message = new StringBuilder();
      //   message.Append("Dear <span >" + sendto.Name + "</span>,");
      //   message.Append("<br/> <br />");
      //   message.Append("Your <span style='font-weight:700;' >Reject Material</span> has been<span style='color:#0E9D41;font-weight:700;font-size:18px;' > " + Material_Overall_Status.Closed + "</span>.");
      //   message.Append("<br/> <br />");
      //   return sendNotificationEmail(sendto.Email_Address, "Reject Materials has been Closed", message.ToString());
      //   //return sendNotificationEmail("ijane2462@gmail.com", "Reject Material has been Closed", message.ToString());
      //}
   }
}