using AgnosModel.Service;
using AppFramework.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Agnos.Models
{
   public class ModelBase
   {
      public string tabAction { get; set; }
      public ServiceResult result { get; set; }

      public Operation operation { get; set; }

      public ModelBase()
      {
         if (result == null)
            result = new ServiceResult();
      }
      public bool? Modify { get; set; }
      public bool? View { get; set; }
      public bool? Close { get; set; }
      public string Role_Name { get; set; }

      public string Create_By2 { get; set; }
      public string Create_By { get; set; }
      public string Create_On { get; set; }
      public string Update_By { get; set; }
      public string Update_On { get; set; }

      private Nullable<DateTime> _currentdate = null;
      public DateTime currentdate
      {
         get
         {
            if (!_currentdate.HasValue)
               _currentdate = StoredProcedure.GetCurrentDate();

            return _currentdate.Value;
         }
      }
   }

   public class LoginViewModel
   {
      [Required]
      [Display(Name = "Email Address")]
      public string Email_Address { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string Password { get; set; }

      [Display(Name = "Remember me?")]
      public bool RememberMe { get; set; }

      [Display(Name = "Message")]
      public string Message { get; set; }
   }

   public class RegisterViewModel
   {
      [Required]
      [Display(Name = "Email Address")]
      public string Email_Address { get; set; }

      //[Required]
      //[Display(Name = "Name")]
      //public string Name { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string Password { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "Confirm password")]
      [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }
   }

   public class ErrorViewModel
   {
      [Display(Name = "Message")]
      public string Message { get; set; }
   }

   public class ResetPasswordViewModel : ModelBase
   {
      public int uid { get; set; }
      public bool notValidateCurrent { get; set; }
      public String name { get; set; }

      //[Required]
      [DataType(DataType.Password)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      [MaxLength(24)]
      public string OldPassword { get; set; }

      [Required]
      [StringLength(24, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string NewPassword { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      [MaxLength(24)]
      [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }


      public bool IsActivationLink { get; set; }

   }
}
