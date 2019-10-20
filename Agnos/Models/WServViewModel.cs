using AgnosModel.Models;
using AppFramework.Common;
using AppFramework.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agnos.Models
{

   public class WServViewModel : ModelBase
   {

   }
   public class UserWViewModel
   {
      public int Profile_ID { get; set; }
      public string Email_Address { get; set; }
      public string Name { get; set; }
      public string Password { get; set; }
      public string Record_Status { get; set; }
   }

   public class CMSDeliveryWModels
   {
      public int Delivery_ID { get; set; }
      public string Delivery_Order_No { get; set; }
      public string Update_On { get; set; }
      public string Update_By { get; set; }
      public string Record_Status { get; set; }
      public List<CMSDeliveryDetailWModels> DeliveryDetail { get; set; }

      public int Local_Delivery_ID { get; set; }
      public int Completed { get; set; }

   }

   public class CMSDeliveryDetailWModels : ModelBase
   {
      public int Local_Delivery_Detail_ID { get; set; }

      public int CMS_Delivery_Detail_ID { get; set; }
      public int Delivery_ID { get; set; }
      public int Product_ID { get; set; }
      public string Product_Code { get; set; }
      public string Product_Name { get; set; }
      public string Drum_Code { get; set; }
      public string Date_Delivered { get; set; }
      public string Update_By { get; set; }

      public int No_Of_Containers { get; set; }
   }
}