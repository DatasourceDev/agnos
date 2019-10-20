using SBSResourceAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum Operation
{
   A,
   C,
   U,
   D,
   None
}

public class LookupType
{
   public const string UOM = "UOM";
   public const string Packaging = "Packaging";
}

public class Logsheet_Control_Type
{
   public const string Text = "Text";
   public const string Field = "Field";
   public const string Range = "Range";
   public const string Date = "Date";
   public const string DropdownList = "Drop-down list";
   public const string Leave_As_Textbox = "Leave as Textbox";
}

public class AgnosConst
{


}

public class Reject_From_Type
{
   public const string Customer = "Customer";
   public const string Supplier = "Supplier";
   public const string Inhouse = "Inhouse";
}

public class Attachment_Field_Name
{
   public const string COA = "COA";
   public const string Invoice = "Invoice";
   public const string Packing = "Packing";
}

public class Material_Status
{
   public const string Pending = "Pending";
   public const string Passed = "Passed";
   public const string Reject = "Reject";
}

public class Reject_Status
{
   public const string Pass = "Pass";
   public const string Fail = "Fail";
   public const string NA = "N/A";
   public const string Resample = "Resample";
   public const string Reject = "Reject";
}

public class Material_Overall_Status
{
   public const string Closed = "Closed";
   public const string Open = "Open";
}

public class Logsheet_Approval_Status
{
   public const string PD_Issue = "PD Issue";
   public const string QA_Approval = "QA Approval";
   public const string PD_Approval = "PD Approval";
}

public class Role_Name
{
   public const string Logistics = "Logistics";
   public const string Sales = "Sales";
   public const string Admin = "Admin";
   public const string PD = "PD";
   public const string QA = "QA";
   public const string GM = "GM";
   public const string PD_HOD = "PD-HOD";
   public const string QA_HOD = "QA-HOD";
}

public class Page_Action
{
   public string Action { get; set; }
   public string Controller { get; set; }
}

public class Page_Code
{
   public const string P0000 = "P0000";//Role Setup
   public const string P0001 = "P0001";//Incoming Raw Material
   public const string P0002 = "P0002";//Reject Material
   public const string P0003 = "P0003";//Withdraw Raw Material
   public const string P0004 = "P0004";//Template Components
   public const string P0005 = "P0005";//Logsheet Template
   public const string P0006 = "P0006";//Assign Template to Product
   public const string P0007 = "P0007";//Logsheet 
   public const string P0008 = "P0008";//Gen Lot Number
   public const string P0009 = "P0009";//CMS Setup
   public const string P0010 = "P0010";//User
   public const string P0011 = "P0011";//Incoming_Raw_Material_Checklist
   public const string P0012 = "P0012";//CMS Purge
   public const string P0013 = "P0013";//CMS Charge
   public const string P0014 = "P0014";//Global Lookup
   public const string P0015 = "P0015";//CMS Delivery
   public const string P0016 = "P0016";//CMS Report

   public static Page_Action GetPageAction(string pCode)
   {
      if (pCode == P0000)
         return new Page_Action() { Controller = "Role", Action = "RoleSetup" };
      else if (pCode == P0001)
         return new Page_Action() { Controller = "Material", Action = "Material" };
      else if (pCode == P0002)
         return new Page_Action() { Controller = "Material", Action = "MaterialReject" };
      else if (pCode == P0003)
         return new Page_Action() { Controller = "Material", Action = "MaterialWithdraw" };
      else if (pCode == P0004)
         return new Page_Action() { Controller = "Template", Action = "Components" };
      else if (pCode == P0005)
         return new Page_Action() { Controller = "Template", Action = "TemplateLogsheet" };
      else if (pCode == P0006)
         return new Page_Action() { Controller = "Template", Action = "ProductTemplate" };
      else if (pCode == P0007)
         return new Page_Action() { Controller = "Template", Action = "Logsheet" };
      else if (pCode == P0008)
         return new Page_Action() { Controller = "Template", Action = "GetLotNumber" };
      else if (pCode == P0009)
         return new Page_Action() { Controller = "CMS", Action = "CMSSetup" };
      else if (pCode == P0010)
         return new Page_Action() { Controller = "User", Action = "Users" };
      else if (pCode == P0011)
         return new Page_Action() { Controller = "Material", Action = "MaterialChecklist" };
      else if (pCode == P0012)
         return new Page_Action() { Controller = "CMS", Action = "CMSPurge" };
      else if (pCode == P0013)
         return new Page_Action() { Controller = "CMS", Action = "CMSCharge" };
      else if (pCode == P0015)
         return new Page_Action() { Controller = "CMS", Action = "CMSDelivery" };
      else if (pCode == P0016)
         return new Page_Action() { Controller = "CMSReport", Action = "InventoryReport" };
      return null;
   }
}

public class Running_Number_Type
{
   public const string RMR = "RMR";
}

public class Record_Status
{
   public const string Delete = "Delete";
   public const string Active = "Active";
   public const string InActive = "InActive";
}

public class RAction
{
   public const string Add = "Add";
   public const string Edit = "Edit";
   public const string Delete = "Delete";
}

public class Logsheet_Dispose
{
   public const string Internally = "Internally";
   public const string Externally = "Externally";
}

public class Withdraw_Transaction_Type
{
   public const string Withdrawal = "Withdrawal";
   public const string Transfer = "Transfer";
   public const string Incoming = "Incoming";
}

public class Dropdown_List_Type
{
   public const string Pass_Fail_NA = "Pass_Fail_NA";
   public const string Resample_RJ_NA = "Resample_RJ_NA";
   public const string TMAH_Storage_Tank_No = "TMAH_Storage_Tank_No";
}

public class Analysist_Type
{
   public const string NG = "NG";
   public const string NA = "NA";
   public const string OK = "OK";
}

public class Charging_Control_Action
{
   public const string Warning = "Warning";
   public const string Skip = "Skip";
   public const string Discard = "Discard";
}

public class Fixed_Field_Name
{
   //User Lot Number Report
   public const string TMAH_Lot_No = "TMAH Lot No";
   public const string Usage_Only = "Usage Only";
}

public class Fixed_Action
{
   public const string Update_Remark_Only = "Update Remark Only";
}

public class Delivery_Status
{
   public const string Completed = "Completed";
}

public class Sort_Type
{
   public const string Lot_No = "Lot_No";
   public const string Drum_Code = "Drum_Code";
   public const string Product_Code = "Product_Code";
   public const string Delivery_Order_No = "Delivery_Order_No";
   public const string Charge_Date = "Charge_Date";
   public const string Date_Delivered = "Date_Delivered";


   public static String GetSortType(string pSort)
   {
      if (pSort == Sort_Type.Lot_No)
         return Resource.Lot_No;
      else if (pSort == Sort_Type.Drum_Code)
         return Resource.Drum_Code;
      else if (pSort == Sort_Type.Product_Code)
         return Resource.Product_Code;
      else if (pSort == Sort_Type.Delivery_Order_No)
         return Resource.Delivery_Order_No;
      else if (pSort == Sort_Type.Charge_Date)
         return Resource.Charge_Date;
      else if (pSort == Sort_Type.Date_Delivered)
         return Resource.Date_Delivered;
      else
         return string.Empty;
   }
}




