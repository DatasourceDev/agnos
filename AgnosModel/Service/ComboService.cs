using AgnosModel.Models;
using AppFramework;
using AppFramework.Util;
using SBSResourceAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AppFramework.Control;
using System.Data.Entity.SqlServer;
using System.Collections;
//using Enterprise01;

namespace AgnosModel.Service
{


    public class ComboCriteria
    {
        public bool Delivery_Order_No { get; set; }
        public bool Product_Code { get; set; }
        public bool Date_Charged { get; set; }
        public bool Date_Delivered { get; set; }


    }
    public class ComboService
    {
        public List<ComboRow> LstDrumType()
        {
            using (var db = new AgnosDBContext())
            {
                return db.CMS_Drum_Type
                    .Where(w => w.Drum_Type != null && w.Record_Status != Record_Status.Delete)
                    .OrderBy(o => o.Drum_Type)
                    .Select(s => new ComboRow
                    {
                        Value = SqlFunctions.StringConvert((double)s.Drum_Type_ID).Trim(),
                        Text = s.Drum_Type
                    }).ToList();
            }
        }

        public List<ComboRow> LstFillingStationType(bool includeAll = false)
        {
            using (var db = new AgnosDBContext())
            {
                if (!includeAll)
                {
                    return db.CMS_Filling_Station
                       .Where(w => w.Station_Code != null && w.Record_Status != Record_Status.Delete)
                       .OrderBy(o => SqlFunctions.IsNumeric(o.Station_Code))
                       .Select(s => new ComboRow
                       {
                           Value = SqlFunctions.StringConvert((double)s.Filling_Station_ID).Trim(),
                           Text = s.Station_Code
                       }).ToList();

                }
                else
                {
                    return db.CMS_Filling_Station
                         .Where(w => w.Station_Code != null)
                         .OrderBy(o => SqlFunctions.IsNumeric(o.Station_Code))
                         .Select(s => new ComboRow
                         {
                             Value = SqlFunctions.StringConvert((double)s.Filling_Station_ID).Trim(),
                             Text = s.Station_Code
                         }).ToList();
                }
            }
        }

        public List<ComboRow> LstAction(bool hasBlank = false)
        {
            var clist = new List<ComboRow>();
            if (hasBlank)
            {
                clist.Add(new ComboRow { Value = "", Text = "-" });
            }
            clist.Add(new ComboRow { Value = Charging_Control_Action.Warning, Text = Charging_Control_Action.Warning });
            clist.Add(new ComboRow { Value = Charging_Control_Action.Skip, Text = Charging_Control_Action.Skip });
            clist.Add(new ComboRow { Value = Charging_Control_Action.Discard, Text = Charging_Control_Action.Discard });
            return clist;
        }

        public List<ComboRow> LstTemplate(bool hasBlank = false)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });

                clist.AddRange(db.Template_Logsheet
                    .Where(w => w.Template_Name != null && w.Record_Status != Record_Status.Delete)
                    .OrderBy(o => o.Template_Code)
                    .Select(s => new ComboRow
                    {
                        Value = SqlFunctions.StringConvert((double)s.Template_ID).Trim(),
                        Text = s.Template_Code.Trim() + " - " + s.Template_Name.Trim()

                    }));

                return clist;
            }
        }

        public List<ComboRow> LstTemplateCode(bool hasBlank = false)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });

                clist.AddRange(db.Template_Logsheet
                    .Where(w => w.Template_Code != null && w.Record_Status != Record_Status.Delete)
                    .OrderBy(o => o.Template_Code)
                    .Select(s => new ComboRow
                    {
                        Value = s.Template_Code.Trim(),
                        Text = s.Template_Code
                    }));

                return clist;
            }
        }

        public List<ComboRow> LstTemplateByProductTemplate(bool hasBlank = false, string pProductCode = null)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });

                clist.AddRange(db.Product_Template
                   .Include(i => i.Template_Logsheet)
                   .Where(w => w.Product_Code.Contains(pProductCode) && w.Record_Status != Record_Status.Delete)
                   .OrderBy(o => o.Template_Logsheet.Template_Name)
                   .Select(s => new ComboRow
                   {
                       Value = SqlFunctions.StringConvert((double)s.Template_ID).Trim(),
                       Text = s.Template_Logsheet.Template_Code.Trim() + "  -  " + s.Template_Logsheet.Template_Name.Trim()
                   }));

                return clist;
            }
        }

        public List<ComboRow> LstLotNumber(string pProductCode = null, int? pTmpID = null, bool hasBlank = false)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });

                var lots = db.Lot_Number.Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                if (!string.IsNullOrEmpty(pProductCode))
                    lots = lots.Where(w => w.Product_Code == pProductCode);

                if (pTmpID.HasValue)
                    lots = lots.Where(w => w.Template_ID == pTmpID);

                clist.AddRange(lots
                    .Select(s => new ComboRow
                    {
                        Value = SqlFunctions.StringConvert((double)s.Lot_Number_ID).Trim(),
                        Text = s.Lot_No
                    }));

                return clist;
            }
        }

        public List<ComboRow> LstStatus(bool hasBlank = false)
        {
            var clist = new List<ComboRow>();
            if (hasBlank)
            {
                clist.Add(new ComboRow { Value = "", Text = "-" });
            }
            clist.Add(new ComboRow { Value = Material_Status.Passed, Text = Material_Status.Passed });
            clist.Add(new ComboRow { Value = Material_Status.Pending, Text = Material_Status.Pending });
            clist.Add(new ComboRow { Value = Material_Status.Reject, Text = Material_Status.Reject });
            return clist;
        }

        public List<ComboRow> LstRejectStatus(bool hasBlank = false)
        {
            var clist = new List<ComboRow>();
            if (hasBlank)
            {
                clist.Add(new ComboRow { Value = "", Text = "-" });
            }
            clist.Add(new ComboRow { Value = Material_Overall_Status.Open, Text = Material_Overall_Status.Open });
            clist.Add(new ComboRow { Value = Material_Overall_Status.Closed, Text = Material_Overall_Status.Closed });

            return clist;
        }

        public List<ComboRow> LstPage()
        {
            using (var db = new AgnosDBContext())
            {
                return db.Pages
                    .OrderBy(o => o.Page_Code)
                    .Select(s => new ComboRow
                    {
                        Value = SqlFunctions.StringConvert((double)s.Page_ID).Trim(),
                        Text = s.Page_Name
                    }).ToList();
            }
        }

        public List<ComboRow> LstRole(bool hasBlank = false)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });

                clist.AddRange(db.Roles
                    .OrderBy(o => o.Role_ID)
                    .Select(s => new ComboRow
                    {
                        Value = SqlFunctions.StringConvert((double)s.Role_ID).Trim(),
                        Text = s.Role_Name
                    }));

                return clist;
            }
        }

        public List<ComboRow> LstFieldDataType(bool hasBlank = false)
        {
            var clist = new List<ComboRow>();
            if (hasBlank)
            {
                clist.Add(new ComboRow { Value = "", Text = "-" });
            }
            clist.Add(new ComboRow { Value = "Exchequer", Text = "Exchequer" });
            clist.Add(new ComboRow { Value = "System", Text = "System" });
            clist.Add(new ComboRow { Value = "N.A", Text = "N.A" });
            return clist;
        }

        public List<ComboRow> LstDropdownListType(bool hasBlank = false)
        {
            var clist = new List<ComboRow>();
            if (hasBlank)
            {
                clist.Add(new ComboRow { Value = "", Text = "-" });
            }
            clist.Add(new ComboRow { Value = Dropdown_List_Type.Pass_Fail_NA, Text = "Pass / Fail / NA" });
            clist.Add(new ComboRow { Value = Dropdown_List_Type.Resample_RJ_NA, Text = "Resample / Reject / NA" });
            clist.Add(new ComboRow { Value = Dropdown_List_Type.TMAH_Storage_Tank_No, Text = "TMAH Storage Tank No." });
            return clist;
        }

        public List<ComboRow> LstFieldFrom(bool hasBlank = false)
        {
            var clist = new List<ComboRow>();
            if (hasBlank)
            {
                clist.Add(new ComboRow { Value = "", Text = "-" });
            }
            clist.Add(new ComboRow { Value = "ABC", Text = "ABC" });
            clist.Add(new ComboRow { Value = "Lot No.", Text = "Lot No." });
            clist.Add(new ComboRow { Value = "User Name", Text = "User Name" });
            clist.Add(new ComboRow { Value = "N.A", Text = "N.A" });
            return clist;
        }

        public List<ComboRow> LstTmpGroup(bool hasBlank = false)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });

                clist.AddRange(db.Template_Group
                   .Where(w => w.Record_Status != Record_Status.Delete)
                    .OrderBy(o => o.Group_Order)
                    .Select(s => new ComboRow
                    {
                        Value = SqlFunctions.StringConvert((double)s.Group_ID).Trim(),
                        Text = s.Group_Name
                    }));

                return clist;
            }
        }

        public List<ComboRow> LstTmpHeader(bool hasBlank = false)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });

                clist.AddRange(db.Template_Header
                   .Where(w => w.Record_Status != Record_Status.Delete)
                    .OrderBy(o => o.Header_Order)
                    .Select(s => new ComboRow
                    {
                        Value = SqlFunctions.StringConvert((double)s.Header_ID).Trim(),
                        Text = s.Header_Name
                    }));

                return clist;
            }
        }

        public List<ComboRow> LstTmpField(bool hasBlank = false)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });

                clist.AddRange(db.Template_Field
                    .Where(w => w.Record_Status != Record_Status.Delete)
                    .OrderBy(o => o.Field_Order)
                    .Select(s => new ComboRow
                    {
                        Value = SqlFunctions.StringConvert((double)s.Field_ID).Trim(),
                        Text = s.Field_Name
                    }));

                return clist;
            }
        }

        public List<ComboRow> LstDispose(bool hasBlank = false)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();
                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });
                clist.Add(new ComboRow() { Value = "N/A", Text = "N/A", Desc = "N/A" });
                clist.Add(new ComboRow() { Value = "Internally", Text = "Dispose Internally", Desc = "Dispose Internally" });
                clist.Add(new ComboRow() { Value = "Externally", Text = "Dispose Externally", Desc = "Dispose Dispose" });
                return clist;
            }
        }

        public List<ComboRow> LstLookupDef()
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                clist.AddRange(db.Global_Lookup_Def
                    .OrderBy(o => o.Name)
                    .Select(s => new ComboRow
                    {
                        Value = SqlFunctions.StringConvert((double)s.Def_ID).Trim(),
                        Text = s.Name
                    }));

                return clist;
            }
        }

        public List<ComboRow> LstLookupData(string pDefCode, bool hasBlank = false)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });

                clist.AddRange(db.Global_Lookup_Data
                    .Where(w => w.Global_Lookup_Def.Def_Code == pDefCode)
                    .OrderBy(o => o.Name)
                    .Select(s => new ComboRow
                    {
                        Value = SqlFunctions.StringConvert((double)s.Lookup_Data_ID).Trim(),
                        Text = s.Name,
                    }));

                return clist;
            }
        }

        public List<ComboRow> LstWithdrawtransType(bool hasBlank = false)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();
                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });
                clist.Add(new ComboRow() { Value = Withdraw_Transaction_Type.Withdrawal, Text = Withdraw_Transaction_Type.Withdrawal, });
                clist.Add(new ComboRow() { Value = Withdraw_Transaction_Type.Transfer, Text = Withdraw_Transaction_Type.Transfer });
                clist.Add(new ComboRow() { Value = Withdraw_Transaction_Type.Incoming, Text = Withdraw_Transaction_Type.Incoming });
                return clist;
            }
        }

        public List<ComboRow> LstProduct(bool hasBlank = false)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });

                clist.AddRange(db.CMS_Product
                    .Where(w => w.Record_Status != Record_Status.Delete)
                    .OrderBy(o => o.Product_Code)
                    .Select(s => new ComboRow
                    {
                        Value = SqlFunctions.StringConvert((double)s.CMS_Product_ID).Trim(),
                        Text = s.Product_Code
                    }));

                return clist;
            }
        }

        public List<ComboRow> LstTMAHStorageTankNo(bool hasBlank = false, bool IncludeNA = false)
        {
            var clist = new List<ComboRow>();
            if (hasBlank)
                clist.Add(new ComboRow() { Value = "", Text = "-" });

            if (IncludeNA)
                clist.Add(new ComboRow() { Value = "NA", Text = "N/A" });

            for (var j = 1; j <= 10; j++)
            {
                clist.Add(new ComboRow() { Value = "ST-2" + j.ToString("000"), Text = "ST-2" + j.ToString("000") });
            }
            return clist;
        }

        public List<ComboRow> LstMonth(bool hasBlank = false)
        {
            var clist = new List<ComboRow>();
            if (hasBlank)
                clist.Add(new ComboRow { Value = "", Text = "-" });
            clist.Add(new ComboRow() { Value = "1", Text = "January" });
            clist.Add(new ComboRow() { Value = "2", Text = "February" });
            clist.Add(new ComboRow() { Value = "3", Text = "March" });
            clist.Add(new ComboRow() { Value = "4", Text = "April", });
            clist.Add(new ComboRow() { Value = "5", Text = "May" });
            clist.Add(new ComboRow() { Value = "6", Text = "June" });
            clist.Add(new ComboRow() { Value = "7", Text = "July" });
            clist.Add(new ComboRow() { Value = "8", Text = "August" });
            clist.Add(new ComboRow() { Value = "9", Text = "September" });
            clist.Add(new ComboRow() { Value = "10", Text = "October" });
            clist.Add(new ComboRow() { Value = "11", Text = "November" });
            clist.Add(new ComboRow() { Value = "12", Text = "December" });
            return clist;
        }

        public List<ComboRow> LstLogsheetStatus(bool hasBlank = false)
        {

            var clist = new List<ComboRow>();
            if (hasBlank)
                clist.Add(new ComboRow { Value = "", Text = "-" });

            clist.Add(new ComboRow() { Value = Logsheet_Approval_Status.PD_Issue, Text = Logsheet_Approval_Status.PD_Issue });
            clist.Add(new ComboRow() { Value = Logsheet_Approval_Status.PD_Approval, Text = Logsheet_Approval_Status.PD_Approval });
            clist.Add(new ComboRow() { Value = Logsheet_Approval_Status.QA_Approval, Text = Logsheet_Approval_Status.QA_Approval });
            clist.Add(new ComboRow() { Value = "NA", Text = "N/A" });

            return clist;
        }


        public List<ComboRow> LstSortAction(bool hasBlank = false, ComboCriteria cri = null)
        {
            var clist = new List<ComboRow>();
            if (hasBlank)
                clist.Add(new ComboRow { Value = "", Text = "-" });

            clist.Add(new ComboRow { Value = Sort_Type.Drum_Code, Text = Resource.Drum_Code });
            clist.Add(new ComboRow { Value = Sort_Type.Lot_No, Text = Resource.Lot_No });
            if (cri != null)
            {
                if (cri.Product_Code)
                    clist.Add(new ComboRow { Value = Sort_Type.Product_Code, Text = Resource.Product_Code });
                if (cri.Date_Charged)
                    clist.Add(new ComboRow { Value = Sort_Type.Charge_Date, Text = Resource.Charge_Date });
                if (cri.Delivery_Order_No)
                    clist.Add(new ComboRow { Value = Sort_Type.Delivery_Order_No, Text = Resource.Delivery_Order_No });
                if (cri.Date_Delivered)
                    clist.Add(new ComboRow { Value = Sort_Type.Date_Delivered, Text = Resource.Date_Delivered });
            }
            return clist;
        }

        public List<ComboRow> LstProductByCode(bool hasBlank = false, bool includeAll = false)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });

                if (!includeAll)
                {
                    clist.AddRange(db.CMS_Product
                        .Where(w => w.Record_Status != Record_Status.Delete).Select(m => m.Product_Code).Distinct()
                        .Select(s => new ComboRow
                        {
                            Value = s.Trim(),
                            Text = s.Trim()
                        }));
                }
                else
                {
                    clist.AddRange(db.CMS_Product.Select(m => m.Product_Code).Distinct()
                        .Select(s => new ComboRow
                        {
                            Value = s.Trim(),
                            Text = s.Trim()
                        }));
                }

                return clist;
            }
        }


        public List<ComboRow> LstStationByProductCode(string pProductCode)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                var StationIDs = db.CMS_Product.Where(w => w.Product_Code == pProductCode).Select(m => m.Filling_Station_ID).Distinct();

                return db.CMS_Filling_Station
                       .Where(w => w.Station_Code != null && StationIDs.Contains(w.Filling_Station_ID))
                       .OrderBy(o => SqlFunctions.IsNumeric(o.Station_Code))
                       .Select(s => new ComboRow
                       {
                           Value = SqlFunctions.StringConvert((double)s.Filling_Station_ID).Trim(),
                           Text = s.Station_Code
                       }).ToList();

                return clist;
            }
        }

        public List<ComboRow> LstDrumCode(string pProductCode, bool hasBlank = false, bool IsNotCompletedOnly = false)
        {
            using (var db = new AgnosDBContext())
            {
                var clist = new List<ComboRow>();

                if (hasBlank)
                    clist.Add(new ComboRow { Value = "", Text = "-" });

                if (!IsNotCompletedOnly)
                {
                    clist.AddRange(db.CMS_Charge
                        .Where(w => w.Product_Code == pProductCode && w.Record_Status != Record_Status.Delete)
                        .OrderBy(o => o.Drum_Code)
                        .Select(s => new ComboRow
                        {
                            Value = SqlFunctions.StringConvert((double)s.Charge_ID).Trim(),
                            Text = s.Drum_Code,
                        }));
                }
                else
                {
                    clist.AddRange(db.CMS_Charge
                       .Where(w => w.Product_Code == pProductCode && w.Delivery_Status != Delivery_Status.Completed && w.Record_Status != Record_Status.Delete)
                       .OrderBy(o => o.Drum_Code)
                       .Select(s => new ComboRow
                       {
                           Value = SqlFunctions.StringConvert((double)s.Charge_ID).Trim(),
                           Text = s.Drum_Code,
                       }));
                }

                return clist;
            }
        }
    }
}
