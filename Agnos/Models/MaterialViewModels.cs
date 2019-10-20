using System;
using System.ComponentModel.DataAnnotations;
using SBSResourceAPI;
using AppFramework.Common;
using System.Resources;
using AgnosModel.Models;
using System.Collections.Generic;
using AppFramework.Control;
using AgnosModel.Service;

namespace Agnos.Models
{
   public class MaterialViewModels : ModelBase
   {
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Code { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Date_From { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Date_To { get; set; }

      private List<Raw_Material> _rawMaterialList;
      public List<Raw_Material> RawMaterialList
      {
         get
         {
            if (_rawMaterialList == null)
               _rawMaterialList = new List<Raw_Material>();

            return _rawMaterialList;
         }
         set
         {
            _rawMaterialList = value;
         }
      }

      public List<MaterialFileAttachList> Attachment_FilesList { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Product_Code")]
      public string Product_Code_Print { get; set; }
      public List<ComboRow> cProductlist { get; set; }

   }

   public class MaterialChecklistViewModels : ModelBase
   {
      public List<MaterialViewModels> lists { get; set; }

      public List<ComboRow> cProductlist { get; set; }
   }

   public class MaterialInfoViewModels : ModelBase
   {
      public string User_Login_Name { get; set; }
      public int? User_Login_ID { get; set; }
      public string Current_Date { get; set; }

      public Nullable<int> Raw_Material_ID { get; set; }
      public string Transaction_ID { get; set; }

      public string Search_Product { get; set; }

      public List<ComboRow> cProductlist { get; set; }
      public List<ComboRow> cPackaginglist { get; set; }
      public List<ComboRow> cUOMlist { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Report_Date { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Code { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Name { get; set; }

      private List<Product_Transaction> _productTrancList;
      public List<Product_Transaction> ProductTrancList
      {
         get
         {
            if (_productTrancList == null)
               _productTrancList = new List<Product_Transaction>();

            return _productTrancList;
         }
         set
         {
            _productTrancList = value;
         }
      }

      /*Packing condition*/
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public bool Dented { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public bool Hole { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Lot_No { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<decimal> Total_Receiving { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public int? UOM { get; set; }
      public string UOM_Name { get; set; }
      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Receiving_Date { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string COA { get; set; }


      /*Attachments*/
      //public List<RawMaterialAttachment> Raw_Attachment { get; set; }

      public List<MaterialFileAttach> Attachment_Files { get; set; }

      /*Qty*/
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Pass")]
      public bool Status_Pass { get; set; }
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Pending")]
      public bool Status_Pending { get; set; }
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Reject")]
      public bool Status_Reject { get; set; }
      public string Selected_Status { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Qty")]
      public Nullable<decimal> Qty_Pass { get; set; }
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Qty")]
      public Nullable<decimal> Qty_Pending { get; set; }
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Qty")]
      public Nullable<decimal> Qty_Reject { get; set; }

      public Nullable<int> UOM_Pass { get; set; }
      public Nullable<int> UOM_Pending { get; set; }
      public Nullable<int> UOM_Reject { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Pass")]
      public bool Print_Pass { get; set; }
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Pending")]
      public bool Print_Pending { get; set; }
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Reject")]
      public bool Print_Reject { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Qty")]
      public Nullable<decimal> Print_Qty_Pass { get; set; }
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Qty")]
      public Nullable<decimal> Print_Qty_Pending { get; set; }
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Qty")]
      public Nullable<decimal> Print_Qty_Reject { get; set; }


      /*Rejected Detail*/
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Reject_Reason { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Remarks")]
      public string Reject_Remarks { get; set; }

      public string savemode { get; set; }
      public bool showPrintDlg { get; set; }


      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Expiry_Date")]
      public string Expiring_Date { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public int? Packaging { get; set; }
      public string Packaging_Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Remarks")]
      public string Remarks_Pass { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Remarks")]
      public string Remarks_Pending { get; set; }


      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Analysis")]
      public string Analysis_Type { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public int? Authorized_By { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Authorized_Date { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Authorized_By")]
      public string Authorized_By_Name { get; set; }
   }

   public class MaterialRejectViewModels : ModelBase
   {
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Code { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Date_From { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Date_To { get; set; }


      private List<Material_Reject> _materialRejList;
      public List<Material_Reject> MaterialRejList
      {
         get
         {
            if (_materialRejList == null)
               _materialRejList = new List<Material_Reject>();

            return _materialRejList;
         }
         set
         {
            _materialRejList = value;
         }
      }

      public List<ComboRow> cProductlist { get; set; }
   }

   public class MaterialRejectInfoViewModels : ModelBase
   {
      public Nullable<int> Material_Reject_ID { get; set; }
      public Nullable<int> Raw_Material_ID { get; set; }

      public string Search_Product { get; set; }

      public List<ComboRow> cProductlist { get; set; }
      public List<ComboRow> cPackaginglist { get; set; }
      public List<ComboRow> cUOMlist { get; set; }
      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Code { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Name { get; set; }


      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Project_Name { get; set; }


      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Lot_No { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public int? UOM { get; set; }
      public string UOM_Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public int? Packaging { get; set; }
      public string Packaging_Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<decimal> Quantity { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Reject_From { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Reject_Customer_Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Reject_Supplier_Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Reject_Inhouse_Location { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Defect_Description { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string QA_Staff { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string D8_No { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<bool> Disposition_RTS { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<bool> Disposition_Rework { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<bool> Disposition_Scrap { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<bool> Disposition_UAI { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Others")]
      public Nullable<bool> Disposition_Others { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Description")]
      public string Disposition_Others_Description { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Instructions { get; set; }

      /*Signed by MRT*/
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<bool> PD { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<bool> QA { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<bool> Logistics { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<bool> Sales { get; set; }



      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Disposition_Action_By { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Disposition_Date { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Re_Inspection_On_Rework { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Reject_Status { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Carried_Out_By { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Carried_Out_Date { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Review_Date { get; set; }

      public List<ComboRow> cRejectStatuslist { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Reject status")]
      public string Overall_Status { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Remarks { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Approval_By_Management")]
      public string GM_Approval_Date { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string PD_Date { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string QA_Date { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Logistics_Date { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Sales_Date { get; set; }

      public Nullable<int> GM_Approval { get; set; }
      public Nullable<int> PD_Approval { get; set; }
      public Nullable<int> QA_Approval { get; set; }
      public Nullable<int> Logistics_Approval { get; set; }
      public Nullable<int> Sales_Approval { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Approval_By_Management")]
      public string GM_Approval_Name { get; set; }
      public string PD_Approval_Name { get; set; }
      public string QA_Approval_Name { get; set; }
      public string Logistics_Approval_Name { get; set; }
      public string Sales_Approval_Name { get; set; }


      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string RMR_No { get; set; }

      private List<Product_Transaction> _productTrancList;
      public List<Product_Transaction> ProductTrancList
      {
         get
         {
            if (_productTrancList == null)
               _productTrancList = new List<Product_Transaction>();

            return _productTrancList;
         }
         set
         {
            _productTrancList = value;
         }
      }

   }

   public class MaterialWithdrawViewModels : ModelBase
   {
      public List<ComboRow> cProductlist { get; set; }
      public List<ComboRow> cTanklist { get; set; }
      public List<ComboRow> cMonthlist { get; set; }
      public List<ComboRow> cUOMlist { get; set; }
      public List<ComboRow> cPackaginglist { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Code { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Name { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Date_From { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Date_To { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Product_Code")]
      public string Product_Code_Print { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Product_Name")]
      public string Finished_Goods { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Tank_No { get; set; }

      public Nullable<int> Packaging_Type { get; set; }
      public string Location { get; set; }
      public Nullable<int> Month { get; set; }
      public Nullable<int> Year { get; set; }
      public string Month_Name { get; set; }
      public string Packaging_Type_Name { get; set; }

      private List<Material_Withdraw> _withdrawMaterialList;
      public List<Material_Withdraw> WithdrawMaterialList
      {
         get
         {
            if (_withdrawMaterialList == null)
               _withdrawMaterialList = new List<Material_Withdraw>();

            return _withdrawMaterialList;
         }
         set
         {
            _withdrawMaterialList = value;
         }
      }


   }

   public class MaterialWithdrawFormViewModels : ModelBase
   {
      public List<MaterialWithdrawViewModels> lists { get; set; }
   }

   public class MaterialWithdrawInfoViewModels : ModelBase
   {
      public Nullable<int> Withdraw_ID { get; set; }
      public string Transaction_ID { get; set; }

      public string Search_Product { get; set; }

      public List<ComboRow> cProductlist { get; set; }
      public List<ComboRow> cPackaginglist { get; set; }
      public List<ComboRow> cUOMlist { get; set; }
      public List<ComboRow> cTrnsaTypelist { get; set; }
      public List<Material_Withdraw> MaterialWithdrawlist { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Code { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Product_Name { get; set; }

      public string Transaction_Type { get; set; }

      private List<Product_Transaction> _productTrancList;
      public List<Product_Transaction> ProductTrancList
      {
         get
         {
            if (_productTrancList == null)
               _productTrancList = new List<Product_Transaction>();

            return _productTrancList;
         }
         set
         {
            _productTrancList = value;
         }
      }



      //[Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Lot_No { get; set; }

      //[Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<decimal> Total_Receiving { get; set; }

      //[Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Receiving_Date { get; set; }

      public string Unit { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "UOM")]
      public int? UOM { get; set; }
      public string UOM_Name { get; set; }


      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Date")]
      public string Withdraw_Date { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string From_Time { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string To_Time { get; set; }

      [Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<decimal> Qty_Withdraw { get; set; }

      //[Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Finished_Goods { get; set; }

      //[Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Finished_Goods_Lot_No { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string PLC { get; set; }

      //[Required]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Transferring_Date { get; set; }

      [MaxLength(500)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Remarks { get; set; }


      [MaxLength(50)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Withdraw_From { get; set; }

      [MaxLength(50)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Withdraw_To { get; set; }

      public List<ComboRow> cTanklist { get; set; }

      [MaxLength(150)]
      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public string Location { get; set; }

      [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
      public Nullable<decimal> Qty_Balance { get; set; }
   }

   public class ProductTransactionViewModels
   {


      public string Transaction_ID { get; set; }

      private List<Product_Transaction> _productTrancList;
      public List<Product_Transaction> ProductTrancList
      {
         get
         {
            if (_productTrancList == null)
               _productTrancList = new List<Product_Transaction>();

            return _productTrancList;
         }
         set
         {
            _productTrancList = value;
         }
      }
   }




   public class MaterialFileAttach
   {
      public Nullable<System.Guid> Attachment_ID { get; set; }
      public int Index { get; set; }
      public string Attachment_Field { get; set; }
      public string File_Name { get; set; }
      public byte[] File { get; set; }
      public bool? Modify { get; set; }
   }

   public class MaterialFileAttachList
   {
      public string Attach_Type { get; set; }
      public Nullable<int> Raw_Material_ID { get; set; }
      public List<Raw_Material_Attachment> Attachmentlist { get; set; }
      public int Index { get; set; }
   }
}
