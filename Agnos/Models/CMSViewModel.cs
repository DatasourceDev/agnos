using System;
using System.ComponentModel.DataAnnotations;

using SBSResourceAPI;
using AgnosModel.Models;
using System.Collections.Generic;
using AppFramework.Control;
using AppFramework.Common;


namespace Agnos.Models
{
    public class CMSSetupViewModel : ModelBase
    {
        public List<ComboRow> cProduct_Codelist { get; set; }
        public List<ComboRow> cDrum_Typelist { get; set; }
        public List<ComboRow> cActionlist { get; set; }
        public List<ComboRow> cStationlist { get; set; }
        public List<ComboRow> cProduct_ByCodelist { get; set; }


        #region CMS Format
        public List<CMS_Format> CMS_Formatlist { get; set; }


        public int Format_ID { get; set; }

        [Required]
        [MaxLength(150)]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Format_Code")]
        public string Format_Code_Format { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Lot_No_Length")]
        public Nullable<int> Lot_No_Length_Format { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Product_Code_Length")]
        public Nullable<int> Product_Code_Length_Format { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Record_Status")]
        [MaxLength(50)]
        public string Record_Status_Format { get; set; }
        #endregion


        #region Drum Type
        public List<CMS_Drum_Type> CMS_Drum_Typelist { get; set; }

        public int Drum_Type_ID { get; set; }

        [Required]
        [MaxLength(150)]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Drum_Types")]
        public string Drum_Type_Drum { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Record_Status")]
        public string Record_Status_Drum { get; set; }
        #endregion

        #region CMS Skip Purging
        public List<CMS_Skip_Purging> CMS_Skip_Purginglist { get; set; }

        public int Skip_Purging_ID { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Product_Code")]
        public string Product_Code_Skip { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Drum_Types")]
        public Nullable<int> Drum_Type_ID_Skip { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Record_Status")]
        public string Record_Status_Skip { get; set; }
        #endregion

        #region CMS Charging Control
        public List<CMS_Charging_Control> CMS_Charging_Controllist { get; set; }

        public int Charging_Control_ID { get; set; }


        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Drum_Code")]
        [MaxLength(150)]
        public string Drum_Code_Charging { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Max_Of_Charges")]
        public Nullable<int> Max_Of_Change_Charging { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Action")]
        [MaxLength(150)]
        public string Action_Charging { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Record_Status")]
        public string Record_Status_Charging { get; set; }
        #endregion

        #region CMS Charging Station
        public List<CMS_Filling_Station> CMS_Filling_Stationlist { get; set; }

        public int Filling_Station_ID { get; set; }

        [Required]
        [MaxLength(150)]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Station_Code")]
        public string Station_Code_Filling { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Record_Status")]
        public string Record_Status_Filling { get; set; }
        #endregion

        #region CMS Product
        public List<CMS_Product> CMS_Productlist { get; set; }

        public Nullable<int> CMS_Product_ID { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Station_Code")]
        public Nullable<int> Filling_Station_ID_Product { get; set; }

        [Required]
        [MaxLength(150)]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Product_Code { get; set; }

        [MaxLength(150)]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Product_Name { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Record_Status")]
        public string Record_Status_Product { get; set; }
        #endregion

        #region CMS Drum Control
        public List<CMS_Drum_Control> CMS_Drum_Controllist { get; set; }

        public int Drum_Control_ID { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Product_Code")]
        public string Product_Code_Control { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Drum_Types")]
        public Nullable<int> Drum_Type_ID_Control { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Running_Number")]
        public Nullable<int> Running_Number_Control { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Action")]
        [MaxLength(150)]
        public string Action_Control { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Record_Status")]
        public string Record_Status_Control { get; set; }


        #endregion

    }

    public class ProductDetailListViewModel
    {
        public int Product_Detail_ID { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Product_Code { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Product_Name { get; set; }

    }

    public class CMSPurgeViewModels : ModelBase
    {
        public Nullable<int> Purge_ID { get; set; }

        public List<CMS_Purge> purgelist { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Drum_Code { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Date_From { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Date_To { get; set; }

    }

    public class CMSPurgeInfoViewModels : ModelBase
    {
       public List<ComboRow> cStationlist { get; set; }

        public Nullable<int> Purge_ID { get; set; }

        [Required]
        [MaxLength(150)]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Drum_Code { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Filling Station")]
        public Nullable<int> Filling_Station_ID { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Filling_Station")]
        public Nullable<int> Filling_Station_ID_Dispaly { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public Nullable<decimal> Initial_Weight { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public Nullable<decimal> Final_Weight { get; set; }

        public bool Completed { get; set; }
        public bool Is_Clone { get; set; }

    }

    public class CMSChargeViewModels : ModelBase
    {
        public Nullable<int> Charge_ID { get; set; }

        public List<CMS_Charge> chargelist { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Drum_Code { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Lot_No { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Filling_Stations")]
        public Nullable<int> Filling_Station_ID { get; set; }

        public List<ComboRow> cStationlist { get; set; }
        public string Product_Code_Dispaly { get; set; }
        public int Quantity_Scanned { get; set; }
        public Nullable<decimal> No_Of_Charging { get; set; }
        public int Format_ID { get; set; }
        public string Product_Code { get; set; }
        public bool Is_Clone { get; set; }
        public string Page_Action { get; set; }

    }

    public class CMSChargeInfoViewModels : ModelBase
    {
        public List<ComboRow> cStationlist { get; set; }
        public List<ComboRow> cProductlist { get; set; }
        public Nullable<int> Charge_ID { get; set; }

        [Required]
        [MaxLength(150)]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Drum_Code { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public Nullable<decimal> Initial_Weight { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public Nullable<decimal> Final_Weight { get; set; }

        [Required]
        [MaxLength(150)]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Lot_No { get; set; }

        //[MaxLength(150)]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public int Quantity_Scanned { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Filling_Station")]
        public Nullable<int> Filling_Station_ID_Dispaly { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Filling Station")]
        public Nullable<int> Filling_Station_ID { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public Nullable<int> No_Of_Charging { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public int Max_No_Of_Charging { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Product_Code")]
        public string Product_Code_Dispaly { get; set; }

        public string Charging_Cl_Action { get; set; }
        public bool? IsOverLoad { get; set; }
        public string Delivery_Status { get; set; }
        public int Format_ID { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Product_Code")]
        public string Product_Code { get; set; }
        public bool Is_Clone { get; set; }
        public string Save_Mode { get; set; }
        public string Page_Action { get; set; }

    }


    public class CMSDeliveryViewModels : ModelBase
    {
        public List<CMS_Delivery> Deliverylist { get; set; }
        public int Delivery_ID { get; set; }

        [MaxLength(150)]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Delivery_Order_No { get; set; }
    }

    public class CMSDeliveryInfoViewModels : ModelBase
    {
        public int Delivery_ID { get; set; }

        [Required]
        [MaxLength(150)]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Delivery_Order_No { get; set; }

        public CMSDeliveryDetail[] Product_Rows { get; set; }
        public List<ComboRow> cProduct_Codelist { get; set; }
        public Nullable<bool> Completed { get; set; }
        public string Save_Mode { get; set; }
    }

    public class CMSDeliveryDetail : ModelBase
    {
        public Nullable<int> CMS_Delivery_Detail_ID { get; set; }

        [Required]
        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Product_Code { get; set; }
        //public Nullable<int> Product_ID { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public int No_Of_Containers { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string[] Drum_Code { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Date_Delivered { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Record_Status { get; set; }

        public string Row_Type { get; set; }
        public int Index { get; set; }


        public List<ComboRow> cProduct_Codelist { get; set; }
        public List<ComboRow> cDrum_Codelist { get; set; }

    }

    public class CMSReportViewModels : ModelBase
    {
        public List<ComboRow> cSortlist { get; set; }
        public List<ComboRow> cProductCodelist { get; set; }


        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Drum_Code { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Lot_No { get; set; }

        public Nullable<int> Purge_ID { get; set; }
        public List<CMS_Purge> purgelist { get; set; }

        public Nullable<int> Charge_ID { get; set; }
        public List<CMS_Charge> chargelist { get; set; }


        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Filling_Stations")]
        public Nullable<int> Filling_Station_ID { get; set; }

        public List<ComboRow> cStationlist { get; set; }


        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Date_From { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Date_To { get; set; }


        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource))]
        public string Product_Code { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Sort By")]
        public string Sort_By { get; set; }

        [LocalizedDisplayName(typeof(SBSResourceAPI.Resource), "Product Code")]
        public Nullable<int> Product_ID { get; set; }

        public List<CMS_Charge> chargelistAll { get; set; }
        public List<CMS_Purge> purgelistAll { get; set; }

    }


}