using AgnosModel.Models;
using AppFramework.Common;
using AppFramework.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agnos.Models
{
   public class ComponentsViewModels : ModelBase
   {
      public List<Template_Group> grouplist { get; set; }
      public List<Template_Field> fieldlist { get; set; }
      public List<Template_Header> headerlist { get; set; }

      public List<ComboRow> cFieldformlist { get; set; }
      public List<ComboRow> cDatatypelist { get; set; }
      public List<ComboRow> cRolelist { get; set; }

      public Nullable<int> Group_ID { get; set; }

      [Required]
      [MaxLength(300)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Group_Name { get; set; }

      public Nullable<int> Role_ID { get; set; }
      public Nullable<int> Field_ID { get; set; }

      [Required]
      [MaxLength(300)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Field_Name { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Data_Type")]
      public string Field_Data_Type { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Field_From { get; set; }

      public Nullable<int> Header_ID { get; set; }

      [Required]
      [MaxLength(300)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Header_Name { get; set; }

      public Nullable<bool> Up { get; set; }

      public Nullable<bool> Down { get; set; }
   }

   public class TemplateLogsheetViewModels : ModelBase
   {
      public List<Template_Logsheet> Tmplogsheetlist { get; set; }

      /* for search template*/
      public Nullable<int> Template_ID { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Template_Code")]
      public string search_Template_Code { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Template_Name")]
      public string search_Template_Name { get; set; }

      [Required]
      [MaxLength(150)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Template_Code { get; set; }

      [Required]
      [MaxLength(300)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Template_Name { get; set; }

      public bool popupCloneDlg { get; set; }
   }

   public class TemplateLogsheetInfoViewModels : ModelBase
   {
      public List<ComboRow> cGrouplist { get; set; }
      public List<ComboRow> cFieldlist { get; set; }
      public List<ComboRow> cHeaderlist { get; set; }
      public List<ComboRow> cFieldformlist { get; set; }
      public List<ComboRow> cDatatypelist { get; set; }
      public List<ComboRow> cDropdowntypelist { get; set; }

      public bool Print { get; set; }
      public Nullable<int> Template_ID { get; set; }
      [Required]
      [MaxLength(150)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Template_Code { get; set; }

      [Required]
      [MaxLength(300)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Template_Name { get; set; }

      [MaxLength(500)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Template_Description { get; set; }

      public Tmp_Log_Group_Row[] Tmp_Log_Group_Rows { get; set; }

      public string RowIsNull { get; set; }
      public string savemode { get; set; }


      //-- Product Template --//
      public Nullable<int> Product_Template_ID { get; set; }
      public string Product_Name { get; set; }
      public string From_No { get; set; }
      public string Revision { get; set; }
      public string Dilution_Tank_No { get; set; }

   }


   public class Tmp_Log_Group_Row
   {
      public Nullable<int> Tmp_Log_Group_ID { get; set; }
      public Nullable<int> Group_ID { get; set; }
      public Nullable<int> Template_ID { get; set; }
      public Nullable<int> Group_Order { get; set; }

      [MaxLength(300)]
      public string Sub_Group_Name { get; set; }
      public Tmp_Log_Header_Row[] Tmp_Log_Header_Rows { get; set; }
      public Tmp_Log_Field_Row[] Tmp_Log_Field_Rows { get; set; }

      public string RowAction { get; set; }
   }


   public class Tmp_Log_Header_Row
   {
      public List<ComboRow> cHeaderlist { get; set; }
      public List<ComboRow> cFieldlist { get; set; }
      public List<ComboRow> cDropdownTypelist { get; set; }

      public int i { get; set; }
      public int h { get; set; }

      public Nullable<int> Tmp_Log_Group_ID { get; set; }
      public Nullable<int> Tmp_Log_Header_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<int> Header_ID { get; set; }
      public Nullable<int> Header_Order { get; set; }
      public bool Sumary_Report_Display { get; set; }


      public Tmp_Log_Map_Row[] Tmp_Log_Map_Rows { get; set; }

      public string RowAction { get; set; }
   }

   public class Tmp_Log_Field_Row
   {
      public List<ComboRow> cFieldlist { get; set; }
      public int i { get; set; }
      public int f { get; set; }


      public Nullable<int> Tmp_Log_Group_ID { get; set; }
      public Nullable<int> Tmp_Log_Field_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<int> Field_ID { get; set; }
      public Nullable<int> Field_Order { get; set; }

      public string RowAction { get; set; }
   }

   public class Tmp_Log_Map_Row
   {
      public int i { get; set; }
      public int h { get; set; }
      public int m { get; set; }
      public Nullable<int> Tmp_Log_Map_ID { get; set; }
      public Nullable<int> Header_ID { get; set; }
      public Nullable<int> Field_ID { get; set; }
      public Nullable<int> Tmp_Log_Header_ID { get; set; }
      public string Option_Selected { get; set; }
      public string Option_Text { get; set; }
      public string Option_Data_Type { get; set; }
      public string Option_Field_From { get; set; }
      public Nullable<decimal> Option_Range_From { get; set; }
      public Nullable<decimal> Option_Range_To { get; set; }
      public string Lot_No { get; set; }
      public string Option_Dropdown_Type { get; set; }
      public string RowAction { get; set; }
      public List<ComboRow> cDropdownTypeList { get; set; }
   }

   public class Template_Logsheet_Map_Row
   {
      public List<ComboRow> cFieldlist { get; set; }
      public int i { get; set; }
      public int j { get; set; }
      public int k { get; set; }

      public Nullable<int> Tmp_Log_Header_ID { get; set; }
      public Nullable<int> Tmp_Log_Field_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<int> Field_ID { get; set; }
      public Nullable<int> Field_Order { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Option_Selected { get; set; }

      [MaxLength(150)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Option_Text { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Option_Data_Type { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Option_Field_From { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<decimal> Option_Range_From { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<decimal> Option_Range_To { get; set; }

      [MaxLength(150)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Lot_No { get; set; }

      public string Action { get; set; }
   }
   public class ProductTemplateViewModels : ModelBase
   {
      public List<ComboRow> cTmplist { get; set; }
      public List<ComboRow> cProductlist { get; set; }
      public List<Product_Template> ProductTemplateList { get; set; }

      public Nullable<int> Product_Template_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<int> Template_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Code { get; set; }

      [Required]
      [MaxLength(300)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Name { get; set; }

      [MaxLength(150)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string From_No { get; set; }

      [MaxLength(50)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Revision_No")]
      public string Revision { get; set; }

      [MaxLength(100)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Dilution_Tank_No { get; set; }
   }

   public class LogsheetViewModels : ModelBase
   {
      public List<ComboRow> cTmplist { get; set; }
      public List<ComboRow> cProductlist { get; set; }
      public List<ComboRow> cTemplateCodelist { get; set; }
      public List<ComboRow> cLogsheetStatuslist { get; set; }


      public Nullable<int> Logsheet_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<int> Template_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Code { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Lot_No { get; set; }

      public List<Logsheet> LogsheetList { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Template_Code { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Logsheet_Status { get; set; }

   }
   public class LogsheetSummaryViewModels : ModelBase
   {
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<int> Template_ID { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Code { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Lot_No { get; set; }

      public List<Logsheet> Logs { get; set; }

      public Template_Logsheet Tmp { get; set; }
   }
   public class LogsheetInfoViewModels : ModelBase
   {

      public Nullable<int> Logsheet_ID { get; set; }

      public string Print { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<int> Template_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Code { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Name { get; set; }

      [MaxLength(150)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Form_No { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Lot_No { get; set; }

      [MaxLength(150)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Work_Order_No { get; set; }

      [MaxLength(500)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Remarks { get; set; }

      [MaxLength(50)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Logsheet_Status")]
      public string Lotgsheet_Status { get; set; }

      [MaxLength(150)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<int> Approval { get; set; }

      public string Approval_Name { get; set; }

      public Template_Logsheet Tmp { get; set; }

      public List<ComboRow> cTmplist { get; set; }
      public List<ComboRow> cProductlist { get; set; }
      public List<ComboRow> cLotNumber { get; set; }
      public List<ComboRow> cWorkOrderNolist { get; set; }
      public List<ComboRow> cDisposelist { get; set; }
      public List<ComboRow> cPackaginglist { get; set; }
      public List<ComboRow> cUOMlist { get; set; }
      public List<ComboRow> cStatuslist { get; set; }

      public Logsheet_Group_Row[] Logsheet_Group_Rows { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public int? UOM { get; set; }
      public string UOM_Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public int? Packaging { get; set; }
      public string Packaging_Name { get; set; }

      public Nullable<decimal> Quantity { get; set; }
      public string Expiry_Date { get; set; }

      [MaxLength(300)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string UAI { get; set; }
      [MaxLength(300)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string RTS { get; set; }
      [MaxLength(300)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Rework { get; set; }
      [MaxLength(300)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Scrap { get; set; }

      [MaxLength(150)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string RMR_No { get; set; }

      [MaxLength(500)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Reason_If_Reject { get; set; }

      [MaxLength(150)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Authorized_By { get; set; }

      [MaxLength(100)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Dispose { get; set; }

      [MaxLength(100)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Status { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Authorized_Date { get; set; }

      [MaxLength(100)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Dilution_Tank_No { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<int> Revision_No { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<int> PD_Issue { get; set; }
      public string PD_Issue_Date { get; set; }
      public string PD_Issue_Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<int> PD_Approval { get; set; }
      public string PD_Approval_Date { get; set; }
      public string PD_Approval_Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<int> QA_Approval { get; set; }
      public string QA_Approval_Date { get; set; }
      public string QA_Approval_Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<bool> Display_Product_Form_Field { get; set; }

      [MaxLength(500)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Note { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Status")]
      public string Product_Status { get; set; }

      public List<UploadAttachmentFile> Attachment_Files { get; set; }
   }

   public class Logsheet_Group_Row
   {
      public Nullable<int> Logsheet_Group_ID { get; set; }
      public Nullable<int> Logsheet_ID { get; set; }
      public Nullable<int> Group_ID { get; set; }

      public Logsheet_Header_Row[] Logsheet_Header_Rows { get; set; }
      public Logsheet_Field_Row[] Logsheet_Field_Rows { get; set; }
   }

   public class Logsheet_Header_Row
   {
      public Nullable<int> Logsheet_Header_ID { get; set; }
      public Nullable<int> Logsheet_Group_ID { get; set; }
      public Nullable<int> Header_ID { get; set; }
      public Logsheet_Map_Row[] Logsheet_Map_Rows { get; set; }
   }

   public class Logsheet_Field_Row
   {
      public Nullable<int> Logsheet_Field_ID { get; set; }
      public Nullable<int> Logsheet_Group_ID { get; set; }
      public Nullable<int> Field_ID { get; set; }
   }

   public class Logsheet_Map_Row
   {
      public Nullable<int> Logsheet_Map_ID { get; set; }
      public Nullable<int> Logsheet_Header_ID { get; set; }
      public Nullable<int> Field_ID { get; set; }
      public Nullable<int> Header_ID { get; set; }

      public string Option_Selected { get; set; }
      public string Text_Display { get; set; }
   }
   public class DailyPlanningViewModels : ModelBase
   {
      public List<DailyPlanning> DailyPlannings { get; set; }
   }
   public class DailyPlanning
   {
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Code { get; set; }

      public List<DailyPlanningLogsheet> Logsheets { get; set; }
   }
   public class DailyPlanningLogsheet
   {
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Lot_No { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string TMAH_Lot_No { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Usage_Only { get; set; }
   }

   public class LotNumberGenViewModels : ModelBase
   {
      public List<ComboRow> cTmplist { get; set; }
      public List<ComboRow> cProductlist { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<int> Template_ID { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Code { get; set; }

      public Nullable<int> Lot_Number_ID { get; set; }

      [Required]
      [MaxLength(150)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Lot_No { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Date")]
      public string Lot_Number_Date { get; set; }

      public List<Lot_Number> LotNumberList { get; set; }

      ///public List<Logsheet> LotLogsheetNumberList { get; set; }

      public string Default_Date { get; set; }
      public int Default_No { get; set; }
   }

   public class LogsheetProductStatusFrom : ModelBase
   {
      public Nullable<int> Logsheet_ID { get; set; }
      public string Lot_No { get; set; }
      public string Product_Name { get; set; }
      public string Product_Code { get; set; }
      public string Selected_Status { get; set; }
      public Nullable<decimal> Qty_Pass { get; set; }
      public Nullable<decimal> Qty_Pending { get; set; }
      public Nullable<decimal> Qty_Reject { get; set; }
      public string Expiring_Date { get; set; }
      public string Authorized_By { get; set; }
      public string Reject_Reason { get; set; }
      public string Remarks { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public int? UOM { get; set; }
      public string UOM_Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public int? Packaging { get; set; }
      public string Packaging_Name { get; set; }

      public Nullable<decimal> Quantity { get; set; }
      public string Expiry_Date { get; set; }
      public string UAI { get; set; }
      public string RTS { get; set; }
      public string Rework { get; set; }
      public string Scrap { get; set; }
      public string RMR_No { get; set; }
      public string Reason_If_Reject { get; set; }
      public string Dispose { get; set; }
      public string Status { get; set; }
      public string Authorized_Date { get; set; }

      public string Product_Status { get; set; }
   }

   public class UploadAttachmentFile
   {
      public Nullable<System.Guid> Attachment_ID { get; set; }
      public Nullable<int> Logsheet_ID { get; set; }
      public string Attachment_File_Name { get; set; }
      public byte[] Attachment_File { get; set; }

      public int Index { get; set; }
      public bool? Modify { get; set; }
   }
}


public class TmpLogJSName
{
   public static string GenName(int i, string fieldName)
   {
      return "Logsheet_Group_Rows[" + i + "]." + fieldName;
   }
   public static string GenFieldName(int i, int f, string fieldName)
   {
      return "Logsheet_Group_Rows[" + i + "].Logsheet_Field_Rows[" + f + "]." + fieldName;
   }

   public static string GenFieldID(int i, int f, string fieldName)
   {
      return "Logsheet_Group_Rows_" + i + "__Logsheet_Field_Rows_" + f + "__" + fieldName;
   }

   public static string GenHeaderName(int i, int f, int h, string fieldName)
   {
      return "Logsheet_Group_Rows[" + i + "].Logsheet_Field_Rows[" + f + "].Logsheet_Header_Rows[" + h + "]." + fieldName;
   }

   public static string GenHeaderID(int i, int f, int h, string fieldName)
   {
      return "Logsheet_Group_Rows_" + i + "__Logsheet_Field_Rows_" + f + "__Logsheet_Header_Rows_" + h + "__" + fieldName;
   }
}