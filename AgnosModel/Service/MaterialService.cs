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

namespace AgnosModel.Service
{
    public class Product_Transaction
    {
        public string Transaction_ID { get; set; }

        public string Product_Code { get; set; }

        public string Product_Name { get; set; }

        public string Lot_No { get; set; }

        public Nullable<decimal> Total_Receiving { get; set; }

        public Nullable<DateTime> Receiving_Date { get; set; }

        public string Expiring_Date { get; set; }

        public string Unit { get; set; }

    }
    public class RawMaterialCriteria : CriteriaBase
    {
        public string Product_Name { get; set; }
        public string Product_Code { get; set; }

        public string Date_From { get; set; }
        public string Date_To { get; set; }

        public Nullable<int> Raw_Material_ID { get; set; }

        public Nullable<bool> Rejected { get; set; }

        public string Product_Search { get; set; }
    }
    public class MaterialRejectCriteria : CriteriaBase
    {
        public string Product_Name { get; set; }
        public string Product_Code { get; set; }

        public string Date_From { get; set; }
        public string Date_To { get; set; }

        public Nullable<int> Raw_Material_ID { get; set; }
        public Nullable<int> Material_Reject_ID { get; set; }

        public string RMR_No { get; set; }
        public string Product_Search { get; set; }
        public string Overall_Status { get; set; }
    }
    public class MaterialWithdrawCriteria : CriteriaBase
    {
        public string Product_Name { get; set; }
        public string Product_Code { get; set; }

        public string Date_From { get; set; }
        public string Date_To { get; set; }
        public string Product_Search { get; set; }

        public Nullable<int> Withdraw_ID { get; set; }

        public string Tank_No { get; set; }
        public string Finished_Goods { get; set; }

        public Nullable<int> Month { get; set; }

        public Nullable<int> Year { get; set; }
        public Nullable<int> Packaging_Type { get; set; }
        public string Location { get; set; }



    }
    public class ProductTransCriteria : CriteriaBase
    {
        public string Product_Name { get; set; }
        public string Product_Code { get; set; }

        public string Date_From { get; set; }
        public string Date_To { get; set; }
        public string Product_Search { get; set; }
    }
    public class MaterialService : ServiceBase
    {
        public MaterialService()
        {
        }
        public MaterialService(User_Profile user)
        {
            userlogin = user;
        }
        public MaterialService(string aspID)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    userlogin = db.User_Profile.Where(w => w.ApplicationUser_Id == aspID).FirstOrDefault();
                }
            }
            catch
            {

            }
        }

        public Product_Mapping GetProductMapping(string pCode)
        {
            using (var db = new AgnosDBContext())
            {
                return db.Product_Mapping.Where(w => w.Product_Code == pCode).FirstOrDefault();
            }
        }


        public Raw_Material_Form GetRawMaterialForm(int? pRawID, string pSelStatus)
        {
            using (var db = new AgnosDBContext())
            {
                return db.Raw_Material_Form.Where(w => w.Raw_Material_ID == pRawID && w.Status == pSelStatus).FirstOrDefault();
            }
        }
        public ServiceResult GetRawMaterial(RawMaterialCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var raws = db.Raw_Material
                       .Include(w => w.Raw_Material_Form)
                       .Include(w => w.User_Profile)
                       .Include(w => w.Global_Lookup_Data)
                       .Include(w => w.Global_Lookup_Data1)
                        .Include(w => w.Material_Reject)
                        .Include(w => w.Raw_Material_Attachment)
                        .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Raw_Material_ID.HasValue)
                            raws = raws.Where(w => w.Raw_Material_ID == cri.Raw_Material_ID);

                        if (!string.IsNullOrEmpty(cri.Product_Code))
                            raws = raws.Where(w => w.Product_Code.Contains(cri.Product_Code));

                        if (!string.IsNullOrEmpty(cri.Product_Name))
                            raws = raws.Where(w => w.Product_Name.Contains(cri.Product_Name));

                        if (!string.IsNullOrEmpty(cri.Product_Search))
                            raws = raws.Where(w => w.Product_Code.Contains(cri.Product_Search) | w.Product_Name.Contains(cri.Product_Search));


                        if (!string.IsNullOrEmpty(cri.Date_From))
                        {
                            var d = DateUtil.ToDate(cri.Date_From);
                            if (d.HasValue)
                                raws = raws.Where(w => DbFunctions.CreateDateTime(w.Receiving_Date.Value.Year, w.Receiving_Date.Value.Month, w.Receiving_Date.Value.Day, 0, 0, 0) >= d);
                        }

                        if (!string.IsNullOrEmpty(cri.Date_To))
                        {
                            var d = DateUtil.ToDate(cri.Date_To);
                            if (d.HasValue)
                                raws = raws.Where(w => DbFunctions.CreateDateTime(w.Receiving_Date.Value.Year, w.Receiving_Date.Value.Month, w.Receiving_Date.Value.Day, 0, 0, 0) <= d);
                        }

                        if (cri.Rejected.HasValue && cri.Rejected.Value)
                        {
                            raws = raws.Where(w => w.Qty_Reject > 0 && w.Status_Reject == true);
                        }
                        if (cri.Top.HasValue)
                        {
                            raws = raws.Take(cri.Top.Value);
                        }
                        else
                        {
                            //raws = raws.Skip(cri.Start_Index).Take(cri.Page_Size);
                            //result.Start_Index = cri.Start_Index;
                            //result.Page_Size = cri.Page_Size;
                        }
                    }


                    result.Object = raws.OrderByDescending(o => o.Raw_Material_ID).ToList();
                    result.Code = ReturnCode.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                result.Code = ReturnCode.ERROR_DB;
                result.Exception = ex;
            }
            return result;
        }

        public void SaveProductMaping(AgnosDBContext db, string code, string name)
        {
            try
            {
                var prodmap = db.Product_Mapping.Where(w => w.Product_Code == code).FirstOrDefault();
                if (prodmap == null)
                {
                    prodmap = new Product_Mapping();
                    prodmap.Product_Code = code;
                    prodmap.Product_Name = name;
                    db.Product_Mapping.Add(prodmap);
                }
                prodmap.Product_Name = name;
            }
            catch
            {
                throw;
            }
        }
        public ServiceResult InsertRawMaterialForm(Raw_Material_Form pForm)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    db.Raw_Material_Form.Add(pForm);
                    db.SaveChanges();
                    db.Entry(pForm).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Income_Raw_Material
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Income_Raw_Material,
                    Exception = ex
                };
            }
        }
        public ServiceResult InsertRawMaterial(Raw_Material pRaw)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    SaveProductMaping(db, pRaw.Product_Code, pRaw.Product_Name);
                    pRaw.Create_By = userlogin.Email_Address;
                    pRaw.Create_On = currentdate;
                    pRaw.Update_By = userlogin.Email_Address;
                    pRaw.Update_On = currentdate;
                    pRaw.Record_By = userlogin.Profile_ID;
                    db.Raw_Material.Add(pRaw);
                    db.SaveChanges();
                    db.Entry(pRaw).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Income_Raw_Material
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Income_Raw_Material,
                    Exception = ex
                };
            }
        }

        public ServiceResult UpdateRawMaterial(Raw_Material pRaw, Raw_Material_Form pPendingform = null, Raw_Material_Form pRejform = null, Raw_Material_Form pPassform = null)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.Raw_Material.Include(w => w.Raw_Material_Attachment).Where(w => w.Raw_Material_ID == pRaw.Raw_Material_ID).FirstOrDefault();
                    if (current != null)
                    {
                        if (pPendingform != null)
                        {
                            var form = db.Raw_Material_Form.Where(w => w.Raw_Material_ID == current.Raw_Material_ID && w.Status == Material_Status.Pending).FirstOrDefault();
                            if (form != null)
                                db.Raw_Material_Form.Remove(form);
                            db.Raw_Material_Form.Add(pPendingform);
                        }
                        if (pRejform != null)
                        {
                            var form = db.Raw_Material_Form.Where(w => w.Raw_Material_ID == current.Raw_Material_ID && w.Status == Material_Status.Reject).FirstOrDefault();
                            if (form != null)
                                db.Raw_Material_Form.Remove(form);
                            db.Raw_Material_Form.Add(pRejform);
                        }
                        if (pPassform != null)
                        {
                            var form = db.Raw_Material_Form.Where(w => w.Raw_Material_ID == current.Raw_Material_ID && w.Status == Material_Status.Passed).FirstOrDefault();
                            if (form != null)
                                db.Raw_Material_Form.Remove(form);
                            db.Raw_Material_Form.Add(pPassform);
                        }
                        SaveProductMaping(db, pRaw.Product_Code, pRaw.Product_Name);
                        pRaw.Update_By = userlogin.Email_Address;
                        pRaw.Update_On = currentdate;
                        pRaw.Record_By = userlogin.Profile_ID;
                        db.Entry(current).CurrentValues.SetValues(pRaw);
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Income_Raw_Material
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Income_Raw_Material,
                    Exception = ex
                };
            }
        }

        public ServiceResult GetMaterialReject(MaterialRejectCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var raws = db.Material_Reject
                       .Include(w => w.Global_Lookup_Data)
                       .Include(w => w.Global_Lookup_Data1)
                       .Include(w => w.User_Profile)
                       .Include(w => w.User_Profile1)
                       .Include(w => w.User_Profile2)
                       .Include(w => w.User_Profile3)
                       .Include(w => w.User_Profile4)
                        .Include(i => i.Raw_Material)
                        .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {

                        if (!string.IsNullOrEmpty(cri.Overall_Status))
                            raws = raws.Where(w => w.Overall_Status == cri.Overall_Status);

                        if (cri.Material_Reject_ID.HasValue)
                            raws = raws.Where(w => w.Material_Reject_ID == cri.Material_Reject_ID);

                        if (cri.Raw_Material_ID.HasValue)
                            raws = raws.Where(w => w.Raw_Material_ID == cri.Raw_Material_ID);

                        if (!string.IsNullOrEmpty(cri.Product_Code))
                            raws = raws.Where(w => w.Product_Code.Contains(cri.Product_Code));

                        if (!string.IsNullOrEmpty(cri.Product_Name))
                            raws = raws.Where(w => w.Product_Name.Contains(cri.Product_Name));

                        if (!string.IsNullOrEmpty(cri.Product_Search))
                            raws = raws.Where(w => w.Product_Code.Contains(cri.Product_Search) | w.Product_Name.Contains(cri.Product_Search));

                        if (!string.IsNullOrEmpty(cri.Date_From))
                        {
                            var date = DateUtil.ToDate(cri.Date_From);
                            if (date.HasValue)
                                raws = raws.Where(w => w.Raw_Material.Receiving_Date >= date);
                        }
                        if (!string.IsNullOrEmpty(cri.Date_To))
                        {
                            var date = DateUtil.ToDate(cri.Date_To);
                            if (date.HasValue)
                                raws = raws.Where(w => w.Raw_Material.Receiving_Date <= date);
                        }

                        if (cri.Top.HasValue)
                        {
                            raws = raws.Take(cri.Top.Value);
                        }
                        else
                        {
                            //raws = raws.Skip(cri.Start_Index).Take(cri.Page_Size);
                            //result.Start_Index = cri.Start_Index;
                            //result.Page_Size = cri.Page_Size;
                        }

                        if (!string.IsNullOrEmpty(cri.RMR_No))
                            raws = raws.Where(w => w.RMR_No.Contains(cri.RMR_No));

                    }

                    result.Object = raws.OrderByDescending(o => o.Material_Reject_ID).ToList();
                    result.Code = ReturnCode.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                result.Code = ReturnCode.ERROR_DB;
                result.Exception = ex;
            }
            return result;
        }

        public Material_Reject_Form GetMaterialRejectForm(int? pRfID)
        {
            using (var db = new AgnosDBContext())
            {
                return db.Material_Reject_Form.Where(w => w.Material_Reject_ID == pRfID).FirstOrDefault();
            }
        }

        public ServiceResult SaveMaterialRejectForm(Material_Reject_Form pForm)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var form = db.Material_Reject_Form.Where(w => w.Material_Reject_ID == pForm.Material_Reject_ID).FirstOrDefault();
                    if (form != null)
                        db.Material_Reject_Form.Remove(form);

                    db.Material_Reject_Form.Add(pForm);
                    db.SaveChanges();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Material_Reject
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Attachment_File,
                    Exception = ex
                };
            }
        }
        public ServiceResult InsertMaterialRejectForm(Material_Reject_Form pForm)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    db.Material_Reject_Form.Add(pForm);
                    db.SaveChanges();
                    db.Entry(pForm).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Material_Reject
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Material_Reject,
                    Exception = ex
                };
            }
        }

        public ServiceResult InsertMaterialReject(Material_Reject pRej)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {

                    //------------- Start Running Number-----------------//
                    var runningNumber = db.Running_Number_Config.Where(w => w.Running_Number_Type == Running_Number_Type.RMR).FirstOrDefault();
                    if (runningNumber == null)
                    {
                        runningNumber = new Running_Number_Config()
                        {
                            Ref_Count = 1,
                            Number_Of_Digit = 2,
                            Running_Number_Type = Running_Number_Type.RMR,
                            Create_On = currentdate,
                            Create_By = userlogin.Email_Address,
                            Running_Year = currentdate.Year
                        };
                    }
                    else
                    {
                        if (!(runningNumber.Running_Year.HasValue && runningNumber.Running_Year == currentdate.Year))
                        {
                            // reset Ref_Count
                            runningNumber.Ref_Count = 1;
                            runningNumber.Running_Year = currentdate.Year;
                            runningNumber.Update_On = currentdate;
                            runningNumber.Update_By = userlogin.Email_Address;
                        }
                    }
                    string RMR_No = runningNumber.Ref_Count.ToString().PadLeft(runningNumber.Number_Of_Digit.Value, '0') + "/" + currentdate.Year.ToString().Substring(2, 2);
                    var rcri = new MaterialRejectCriteria();
                    rcri.RMR_No = RMR_No;
                    bool validNo = false;
                    while (!validNo)
                    {
                        var rej = new Material_Reject();
                        var result = GetMaterialReject(rcri);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var rejs = result.Object as List<Material_Reject>;
                            if (rejs.Count() == 0)
                                validNo = true;
                            else
                            {
                                runningNumber.Update_On = currentdate;
                                runningNumber.Update_By = userlogin.Email_Address;
                                runningNumber.Ref_Count++;
                                RMR_No = runningNumber.Ref_Count.ToString().PadLeft(runningNumber.Number_Of_Digit.Value, '0') + "/" + currentdate.Year.ToString().Substring(2, 2);
                                rcri.RMR_No = RMR_No;
                            }
                        }
                    }
                    runningNumber.Ref_Count++;
                    pRej.RMR_No = RMR_No;
                    if (runningNumber.Running_Number_Config_ID == 0)
                        db.Running_Number_Config.Add(runningNumber);
                    //------------- End Running Number-----------------//

                    pRej.Create_On = currentdate;
                    pRej.Create_By = userlogin.Email_Address;
                    pRej.Update_On = currentdate;
                    pRej.Update_By = userlogin.Email_Address;
                    SaveProductMaping(db, pRej.Product_Code, pRej.Product_Name);
                    db.Material_Reject.Add(pRej);
                    db.SaveChanges();
                    db.Entry(pRej).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Material_Reject
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Material_Reject,
                    Exception = ex
                };
            }
        }

        public ServiceResult UpdateMaterialReject(Material_Reject pRej, Material_Reject_Form pForm = null)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.Material_Reject.Where(w => w.Material_Reject_ID == pRej.Material_Reject_ID).FirstOrDefault();
                    if (current != null)
                    {
                        if (string.IsNullOrEmpty(pRej.RMR_No))
                        {
                            //------------- Start Running Number-----------------//
                            var runningNumber = db.Running_Number_Config.Where(w => w.Running_Number_Type == Running_Number_Type.RMR).FirstOrDefault();
                            if (runningNumber == null)
                            {
                                runningNumber = new Running_Number_Config()
                                {
                                    Ref_Count = 1,
                                    Number_Of_Digit = 2,
                                    Running_Number_Type = Running_Number_Type.RMR,
                                    Create_On = currentdate,
                                    Create_By = userlogin.Email_Address,
                                    Running_Year = currentdate.Year
                                };
                            }
                            else
                            {
                                if (!(runningNumber.Running_Year.HasValue && runningNumber.Running_Year == currentdate.Year))
                                {
                                    // reset Ref_Count
                                    runningNumber.Ref_Count = 1;
                                    runningNumber.Running_Year = currentdate.Year;
                                    runningNumber.Update_On = currentdate;
                                    runningNumber.Update_By = userlogin.Email_Address;
                                }
                            }
                            string RMR_No = runningNumber.Ref_Count.ToString().PadLeft(runningNumber.Number_Of_Digit.Value, '0') + "/" + currentdate.Year.ToString().Substring(2, 2);
                            var rcri = new MaterialRejectCriteria();
                            rcri.RMR_No = RMR_No;
                            bool validNo = false;
                            while (!validNo)
                            {
                                var rej = new Material_Reject();
                                var result = GetMaterialReject(rcri);
                                if (result.Code == ReturnCode.SUCCESS)
                                {
                                    var rejs = result.Object as List<Material_Reject>;
                                    if (rejs.Count() == 0)
                                        validNo = true;
                                    else
                                    {
                                        if (runningNumber.Ref_Count == 0)
                                            runningNumber.Ref_Count = 1;

                                        runningNumber.Update_On = currentdate;
                                        runningNumber.Update_By = userlogin.Email_Address;
                                        runningNumber.Ref_Count++;
                                        RMR_No = runningNumber.Ref_Count.ToString().PadLeft(runningNumber.Number_Of_Digit.Value, '0') + "/" + currentdate.Year.ToString().Substring(2, 2);
                                    }
                                }
                            }
                            runningNumber.Ref_Count++;
                            pRej.RMR_No = RMR_No;
                            if (runningNumber.Running_Number_Config_ID == 0)
                                db.Running_Number_Config.Add(runningNumber);
                            //------------- End Running Number-----------------//
                        }

                        if (pForm != null)
                        {
                            var form = db.Material_Reject_Form.Where(w => w.Material_Reject_ID == current.Material_Reject_ID).FirstOrDefault();
                            if (form != null)
                                db.Material_Reject_Form.Remove(form);

                            db.Material_Reject_Form.Add(pForm);
                        }

                        SaveProductMaping(db, pRej.Product_Code, pRej.Product_Name);
                        pRej.Update_On = currentdate;
                        pRej.Update_By = userlogin.Email_Address;
                        db.Entry(current).CurrentValues.SetValues(pRej);
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Material_Reject
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Material_Reject,
                    Exception = ex
                };
            }
        }

        public ServiceResult GetMaterialWithdraw(MaterialWithdrawCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var raws = db.Material_Withdraw
                       .Include(w => w.Global_Lookup_Data)
                       .Include(w => w.Global_Lookup_Data1)
                       .Include(w => w.User_Profile)
                       .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Withdraw_ID.HasValue)
                            raws = raws.Where(w => w.Withdraw_ID == cri.Withdraw_ID);

                        if (!string.IsNullOrEmpty(cri.Product_Code))
                            raws = raws.Where(w => w.Product_Code.Contains(cri.Product_Code));

                        if (!string.IsNullOrEmpty(cri.Product_Name))
                            raws = raws.Where(w => w.Product_Name.Contains(cri.Product_Name));

                        if (!string.IsNullOrEmpty(cri.Product_Search))
                            raws = raws.Where(w => w.Product_Code.Contains(cri.Product_Search) | w.Product_Name.Contains(cri.Product_Search));

                        if (!string.IsNullOrEmpty(cri.Tank_No))
                        {
                            //raws = raws.Where(w => w.Transaction_Type == Withdraw_Transaction_Type.Transfer);
                            if (cri.Tank_No == "NA")
                                raws = raws.Where(w => w.Withdraw_From == null | w.Withdraw_To == null);
                            else
                                raws = raws.Where(w => w.Withdraw_From.Contains(cri.Tank_No) | w.Withdraw_To.Contains(cri.Tank_No));
                        }

                        if (!string.IsNullOrEmpty(cri.Finished_Goods))
                            raws = raws.Where(w => w.Finished_Goods.Contains(cri.Finished_Goods));

                        if (!string.IsNullOrEmpty(cri.Location))
                            raws = raws.Where(w => w.Location.Contains(cri.Location));

                        if (cri.Packaging_Type.HasValue && cri.Packaging_Type.Value > 0)
                            raws = raws.Where(w => w.UOM == cri.Packaging_Type);

                        if (cri.Month.HasValue && cri.Month.Value > 0)
                            raws = raws.Where(w => w.Withdraw_Date.Value.Month == cri.Month);

                        if (cri.Year.HasValue && cri.Year.Value > 0)
                            raws = raws.Where(w => w.Withdraw_Date.Value.Year == cri.Year);

                        if (!string.IsNullOrEmpty(cri.Date_From))
                        {
                            var date = DateUtil.ToDate(cri.Date_From);
                            if (date.HasValue)
                                raws = raws.Where(w => w.Withdraw_Date >= date);
                        }
                        if (!string.IsNullOrEmpty(cri.Date_To))
                        {
                            var date = DateUtil.ToDate(cri.Date_To);
                            if (date.HasValue)
                                raws = raws.Where(w => w.Withdraw_Date <= date);
                        }

                        if (cri.Top.HasValue)
                        {
                            raws = raws.Take(cri.Top.Value);
                        }
                        else
                        {
                            //raws = raws.Skip(cri.Start_Index).Take(cri.Page_Size);
                            //result.Start_Index = cri.Start_Index;
                            //result.Page_Size = cri.Page_Size;
                        }
                    }
                    result.Object = raws.OrderByDescending(o => o.Withdraw_Date).ThenByDescending(o => o.From_Time).ToList();
                    result.Code = ReturnCode.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                result.Code = ReturnCode.ERROR_DB;
                result.Exception = ex;
            }
            return result;
        }

        public ServiceResult InsertMaterialWithdraw(Material_Withdraw pWith)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pWith.Create_On = currentdate;
                    pWith.Create_By = userlogin.Email_Address;
                    pWith.Update_On = currentdate;
                    pWith.Update_By = userlogin.Email_Address;
                    pWith.PLC = userlogin.Profile_ID;

                    db.Material_Withdraw.Add(pWith);
                    SaveProductMaping(db, pWith.Product_Code, pWith.Product_Name);
                    db.SaveChanges();
                    db.Entry(pWith).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Material_Withdraw
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Material_Withdraw,
                    Exception = ex
                };
            }
        }

        public ServiceResult UpdateMaterialWithdraw(Material_Withdraw pWith)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.Material_Withdraw.Where(w => w.Withdraw_ID == pWith.Withdraw_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pWith.Update_On = currentdate;
                        pWith.Update_By = userlogin.Email_Address;
                        SaveProductMaping(db, pWith.Product_Code, pWith.Product_Name);
                        db.Entry(current).CurrentValues.SetValues(pWith);
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Material_Withdraw
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Material_Withdraw,
                    Exception = ex
                };
            }
        }

        public Raw_Material_Attachment GetRawMaterialAttachment(Nullable<System.Guid> pAttID)
        {
            using (var db = new AgnosDBContext())
            {
                return db.Raw_Material_Attachment.Where(w => w.Attachment_ID == pAttID && w.Record_Status != Record_Status.Delete).FirstOrDefault();
            }

        }

        public ServiceResult InsertRawMaterialAttachment(ref Nullable<Guid> pAttID, Nullable<int> pRawID, byte[] file, string filename, string field)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pAttID = Guid.NewGuid();
                    //Insert
                    var att = new Raw_Material_Attachment();

                    att.Attachment_ID = pAttID.Value;
                    att.Attachment_File = file;
                    att.Raw_Material_ID = pRawID;
                    att.Attachment_File_Name = filename;
                    att.Attachment_Field = field;

                    att.Create_On = currentdate;
                    att.Create_By = userlogin.Email_Address;
                    att.Update_On = currentdate;
                    att.Update_By = userlogin.Email_Address;
                    db.Raw_Material_Attachment.Add(att);
                    db.SaveChanges();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = "File"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = "File",
                    Exception = ex
                };
            }
        }

        public ServiceResult UpdateRawMaterialAttachment(Nullable<Guid> pAttID, byte[] file, string filename)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var att = db.Raw_Material_Attachment.Where(w => w.Attachment_ID == pAttID).FirstOrDefault();
                    if (att != null)
                    {
                        //UPDATE
                        att.Attachment_File = file;
                        att.Attachment_File_Name = filename;
                        att.Update_On = currentdate;
                        att.Update_By = userlogin.Email_Address;
                        db.Entry(att).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = "File"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = "File",
                    Exception = ex
                };
            }
        }

        public int DeleteAttachment(Nullable<System.Guid> pAttachmentID)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var attach = db.Raw_Material_Attachment.Where(w => w.Attachment_ID == pAttachmentID).FirstOrDefault();
                    if (attach == null)
                    {
                        return -500;
                    }
                    else
                    {
                        attach.Record_Status = Record_Status.Delete;
                        attach.Update_On = currentdate;
                        attach.Update_By = userlogin.Email_Address;
                        db.Entry(attach).State = EntityState.Modified;
                        //db.Raw_Material_Attachment.Remove(attach);
                        db.SaveChanges();
                        return 1;
                    }
                }
            }
            catch
            {
                //Log
                return -500;
            }
        }

        //public ServiceResult DeleteMaterialMaterialReject(Nullable<int> pMaterialRejectID)
        //{
        //   try
        //   {
        //      using (var db = new AgnosDBContext())
        //      {
        //         var MaterialReject = db.Material_Reject.Where(w => w.Material_Reject_ID == pMaterialRejectID).FirstOrDefault();
        //         if (MaterialReject != null && MaterialReject.Overall_Status != Material_Overall_Status.Closed)
        //         {
        //            db.Material_Reject.Remove(MaterialReject);
        //            db.SaveChanges();
        //         }


        //         return new ServiceResult()
        //         {
        //            Code = ReturnCode.SUCCESS,
        //            Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE),
        //            Field = Resource.Material_Reject
        //         };
        //      }
        //   }
        //   catch (Exception ex)
        //   {
        //      return new ServiceResult()
        //      {
        //         Code = ReturnCode.ERROR_DELETE,
        //         Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
        //         Field = Resource.Material_Reject,
        //         Exception = ex
        //      };
        //   }

        //}

        public List<Raw_Material_Attachment> LstRawMaterialAttachment(Nullable<int> pRMID, string PType)
        {
            using (var db = new AgnosDBContext())
            {
                return db.Raw_Material_Attachment
                   .Where(w => w.Raw_Material_ID == pRMID && w.Attachment_Field == PType && w.Record_Status != Record_Status.Delete).ToList();
            }

        }


    }
}
