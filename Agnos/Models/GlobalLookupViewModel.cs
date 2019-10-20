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
   public class GlobalLookupViewModel : ModelBase
   {
      public List<ComboRow> cGlobalDefList { get; set; }
      public List<Global_Lookup_Data> GlobalDataList { get; set; }

      public int? Lookup_Data_ID { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Type")]
      public int? search_Def_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Type")]
      public int? Def_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Code")]
      public string Data_Code { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Text")]
      public string Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Status")]
      public string Record_Status { get; set; }

   }


}