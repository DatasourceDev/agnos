using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework
{
   public enum ReturnCode
   {
      SUCCESS = 1,
      SUCCESS_INSERT,
      SUCCESS_UPDATE,
      SUCCESS_DELETE,
      SUCCESS_APPROVE,
      SUCCESS_REJECT,
      SUCCESS_CANCEL,
      SUCCESS_CONFIRM,
      SUCCESS_SEND_EMAIL,


      ERROR_UNAUTHORIZED = 401,
      ERROR_DB,
      ERROR_DATA_NOT_FOUND,
      ERROR_CANT_SEND_EMAIL,
      ERROR_INSERT,
      ERROR_UPDATE,
      ERROR_DELETE,
      ERROR_SAVE,
      ERROR_DATA_DUPLICATE,
      ERROR_INVALID_INPUT,
      ERROR_PAYPAL,
      ERROR_DATA_IN_USE,
      ERROR_RESET_PASSWORD_EXPIRE,
      ERROR_RESET_PASSWORD_CODE_NOT_FOUND,

   }


   public class Success
   {
      public static string GetMessage(ReturnCode code, string field = null)
      {
         var msg = "";
         if (!string.IsNullOrEmpty(field))
            msg = field + " ";

         switch (code)
         {
            case ReturnCode.SUCCESS:
               return msg + "has been submitted successfully.";
            case ReturnCode.SUCCESS_INSERT:
               return msg + "has been created successfully.";
            case ReturnCode.SUCCESS_UPDATE:
               return msg + "has been edited successfully.";
            case ReturnCode.SUCCESS_DELETE:
               return msg + "has been deleted successfully.";
            case ReturnCode.SUCCESS_APPROVE:
               return msg + "has been approved successfully.";
            case ReturnCode.SUCCESS_REJECT:
               return msg + "has been rejected successfully.";
            case ReturnCode.SUCCESS_CANCEL:
               return msg + "has been canceled successfully.";
            case ReturnCode.SUCCESS_SEND_EMAIL:
               return "The Email has been sent successfully.";
            default:
               return "";
         }
      }
   }


   public class Error
   {
      public static string GetMessage(ReturnCode code, string field = null)
      {
         var msg = "";
         if (!string.IsNullOrEmpty(field))
            msg = field + " ";

         switch (code)
         {
            case ReturnCode.ERROR_UNAUTHORIZED:
               return msg + "unauthorized.";
            case ReturnCode.ERROR_DB:
               return "The database error.";
            case ReturnCode.ERROR_DATA_NOT_FOUND:
               return msg + "has not found.";
            case ReturnCode.ERROR_CANT_SEND_EMAIL:
               return "The email cannot be sent.";
            case ReturnCode.ERROR_INSERT:
               return msg + "cannot be inserted.";
            case ReturnCode.ERROR_UPDATE:
               return msg + "cannot be updated";
            case ReturnCode.ERROR_DELETE:
               return msg + "cannot delete because data is in use.";
            case ReturnCode.ERROR_SAVE:
               return msg + "cannot be saved";
            case ReturnCode.ERROR_DATA_DUPLICATE:
               return msg + "duplicated";
            case ReturnCode.ERROR_INVALID_INPUT:
               return msg + "is invalid";
            case ReturnCode.ERROR_PAYPAL:
               return msg + "The paypal incomplete payments.";
            case ReturnCode.ERROR_DATA_IN_USE:
               return msg + "cannot delete because data is in use.";
            case ReturnCode.ERROR_RESET_PASSWORD_EXPIRE:
               return msg + "Your reset password code is not valid!.";
            case ReturnCode.ERROR_RESET_PASSWORD_CODE_NOT_FOUND:
               return msg + "Your reset password code is not valid!";
            default:
               return "";
         }


      }
   }
}
