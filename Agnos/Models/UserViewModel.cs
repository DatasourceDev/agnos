using AgnosModel.Models;
using AppFramework.Common;
using AppFramework.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agnos.Models
{
   public class UserViewModel : ModelBase
   {
      public List<User_Profile> Userlist { get; set; }
      public List<ComboRow> cRole { get; set; }

      public int Profile_ID { get; set; }

      [Required]
      [EmailAddress]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Email")]
      public string Email_Address { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Role_Name")]
      public Nullable<int> Role_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string LDAP_Username { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string Password { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "Confirm password")]
      [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
      public string Confirm_Password { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<bool> Email_Notification { get; set; }

   }
}