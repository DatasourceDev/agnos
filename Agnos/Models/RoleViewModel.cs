using AgnosModel.Models;
using AppFramework.Common;
using AppFramework.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Agnos.Models
{


   public class RoleSetupViewModel : ModelBase
   {
      public List<Page_Role> PageRoleList { get; set; }

      public Nullable<int> Page_Role_ID { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Role")]
      public Nullable<int> sRole_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Page")]
      public Nullable<int> Page_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Role")]
      public Nullable<int> Role_ID { get; set; }

      public Nullable<bool> View { get; set; }
      public Nullable<bool> Modify { get; set; }

      public Nullable<bool> Close { get; set; }

      public List<ComboRow> cPageList { get; set; }
      public List<ComboRow> cRoleList { get; set; }
   }
}