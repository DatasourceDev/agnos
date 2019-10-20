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
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;

namespace AgnosModel.Service
{
    public class CMSCriteria : CriteriaBase
    {
        public Nullable<int> Purge_ID { get; set; }
        public Nullable<int> Charge_ID { get; set; }
        public Nullable<int> Product_ID { get; set; }
        public Nullable<int> Filling_Station_ID { get; set; }

        public string Drum_Code { get; set; }
        public string Lot_No { get; set; }
        public string Date_From { get; set; }
        public string Date_To { get; set; }
        public string Product_Code { get; set; }

        /*  Verify data */
        public string Lot_No_Equals { get; set; }
        public string Drum_Code_Equals { get; set; }
        public string Station_Code_Equals { get; set; }
        public bool Not_Yet_Deliver { get; set; }

        public bool Drum_Code_Have_Value { get; set; }
    }

    public class CMSService : ServiceBase
    {
        public CMSService()
        {
        }
        public CMSService(User_Profile user)
        {
            userlogin = user;
        }
        public CMSService(string aspID)
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

        #region CMS Setting
        //CMS_Format
        public ServiceResult GetFormat(FormatCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var raws = db.CMS_Format
                        .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Format_ID.HasValue && cri.Format_ID.Value > 0)
                            raws = raws.Where(w => w.Format_ID == cri.Format_ID);

                        if (!string.IsNullOrEmpty(cri.Format_Code))
                            raws = raws.Where(w => w.Format_Code.Contains(cri.Format_Code));

                        if (cri.Lot_No_Length.HasValue && cri.Lot_No_Length.Value > 0)
                            raws = raws.Where(w => w.Lot_No_Length == cri.Lot_No_Length);

                        if (cri.Product_Code_Length.HasValue && cri.Product_Code_Length.Value > 0)
                            raws = raws.Where(w => w.Product_Code_Length == cri.Product_Code_Length);
                    }

                    result.Object = raws.OrderBy(o => o.Format_Code).ToList();
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
        public ServiceResult InsertFormat(CMS_Format pFormat)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pFormat.Create_By = userlogin.Email_Address;
                    pFormat.Create_On = currentdate;
                    pFormat.Update_By = userlogin.Email_Address;
                    pFormat.Update_On = currentdate;

                    db.CMS_Format.Add(pFormat);
                    db.SaveChanges();
                    db.Entry(pFormat).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Formatting
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Formatting,
                    Exception = ex
                };
            }
        }
        public ServiceResult UpdateFormat(CMS_Format pFormat)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.CMS_Format.Where(w => w.Format_ID == pFormat.Format_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pFormat.Update_By = userlogin.Email_Address;
                        pFormat.Update_On = currentdate;
                        db.Entry(current).CurrentValues.SetValues(pFormat);
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Formatting
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Formatting,
                    Exception = ex
                };
            }
        }


        //CMS_Drum_Type
        public ServiceResult GetDrumType(DrumCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var raws = db.CMS_Drum_Type
                          .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Drum_Type_ID.HasValue && cri.Drum_Type_ID.Value > 0)
                            raws = raws.Where(w => w.Drum_Type_ID == cri.Drum_Type_ID);

                        if (!string.IsNullOrEmpty(cri.Drum_Type))
                            raws = raws.Where(w => w.Drum_Type.Contains(cri.Drum_Type));

                        if (!string.IsNullOrEmpty(cri.Drum_Type_Equals))
                            raws = raws.Where(w => w.Drum_Type.Equals(cri.Drum_Type_Equals));

                        if (!string.IsNullOrEmpty(cri.Record_Status))
                            raws = raws.Where(w => w.Record_Status.Contains(cri.Record_Status));
                    }

                    result.Object = raws.OrderBy(o => o.Drum_Type).ToList();
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
        public ServiceResult InsertDrumType(CMS_Drum_Type pDrumType)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pDrumType.Create_By = userlogin.Email_Address;
                    pDrumType.Create_On = currentdate;
                    pDrumType.Update_By = userlogin.Email_Address;
                    pDrumType.Update_On = currentdate;

                    db.CMS_Drum_Type.Add(pDrumType);
                    db.SaveChanges();
                    db.Entry(pDrumType).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Drum_Types
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Drum_Types,
                    Exception = ex
                };
            }
        }
        public ServiceResult UpdateDrumType(CMS_Drum_Type pDrumType)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.CMS_Drum_Type.Where(w => w.Drum_Type_ID == pDrumType.Drum_Type_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pDrumType.Update_By = userlogin.Email_Address;
                        pDrumType.Update_On = currentdate;
                        db.Entry(current).CurrentValues.SetValues(pDrumType);
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Drum_Types
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Drum_Types,
                    Exception = ex
                };
            }
        }


        //CMS_Skip_Purging
        public ServiceResult GetSkipPurg(SkipPurgCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var raws = db.CMS_Skip_Purging
                        .Include(s => s.CMS_Drum_Type)
                        //.Include(s => s.CMS_Product)
                        .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Skip_Purging_ID.HasValue && cri.Skip_Purging_ID.Value > 0)
                            raws = raws.Where(w => w.Skip_Purging_ID == cri.Skip_Purging_ID);

                        if (!string.IsNullOrEmpty(cri.Product_Code))
                            raws = raws.Where(w => w.Product_Code == cri.Product_Code);

                        if (cri.Drum_Type_ID.HasValue && cri.Drum_Type_ID.Value > 0)
                            raws = raws.Where(w => w.Drum_Type_ID == cri.Drum_Type_ID);

                        if (!string.IsNullOrEmpty(cri.Drum_Type))
                            raws = raws.Where(w => w.CMS_Drum_Type.Drum_Type == cri.Drum_Type);

                        if (!string.IsNullOrEmpty(cri.Record_Status))
                            raws = raws.Where(w => w.Record_Status.Contains(cri.Record_Status));


                        if (!string.IsNullOrEmpty(cri.Product_Code_Equals))
                            raws = raws.Where(w => w.Product_Code.Equals(cri.Product_Code_Equals));
                    }

                    result.Object = raws.OrderBy(o => o.Product_Code).ToList();
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
        public ServiceResult InsertSkipPurg(CMS_Skip_Purging pSkipPurg)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pSkipPurg.Create_By = userlogin.Email_Address;
                    pSkipPurg.Create_On = currentdate;
                    pSkipPurg.Update_By = userlogin.Email_Address;
                    pSkipPurg.Update_On = currentdate;
                    db.CMS_Skip_Purging.Add(pSkipPurg);
                    db.SaveChanges();
                    db.Entry(pSkipPurg).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Skip_Purging
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Skip_Purging,
                    Exception = ex
                };
            }
        }
        public ServiceResult UpdateSkipPurg(CMS_Skip_Purging pSkipPurg)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.CMS_Skip_Purging.Where(w => w.Skip_Purging_ID == pSkipPurg.Skip_Purging_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pSkipPurg.Update_By = userlogin.Email_Address;
                        pSkipPurg.Update_On = currentdate;
                        db.Entry(current).CurrentValues.SetValues(pSkipPurg);
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Skip_Purging
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Skip_Purging,
                    Exception = ex
                };
            }
        }

        public ServiceResult GetChargingControl(ChargingControlCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var raws = db.CMS_Charging_Control
                           .Include(s => s.CMS_Drum_Type)
                           .Include(s => s.CMS_Product)
                           .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Charging_Control_ID.HasValue && cri.Charging_Control_ID.Value > 0)
                            raws = raws.Where(w => w.Charging_Control_ID == cri.Charging_Control_ID);

                        if (!string.IsNullOrEmpty(cri.Action))
                            raws = raws.Where(w => w.Action.Contains(cri.Action));

                        if (!string.IsNullOrEmpty(cri.Record_Status))
                            raws = raws.Where(w => w.Record_Status.Contains(cri.Record_Status));

                        if (!string.IsNullOrEmpty(cri.Drum_Code))
                            raws = raws.Where(w => w.Drum_Code.Equals(cri.Drum_Code));
                    }

                    result.Object = raws.OrderBy(o => o.Charging_Control_ID).ToList();
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
        public ServiceResult InsertChargingControl(CMS_Charging_Control pChargingControl)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pChargingControl.Create_By = userlogin.Email_Address;
                    pChargingControl.Create_On = currentdate;
                    pChargingControl.Update_By = userlogin.Email_Address;
                    pChargingControl.Update_On = currentdate;

                    db.CMS_Charging_Control.Add(pChargingControl);
                    db.SaveChanges();
                    db.Entry(pChargingControl).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Charging_Control
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Charging_Control,
                    Exception = ex
                };
            }
        }
        public ServiceResult UpdateChargingControl(CMS_Charging_Control pChargingControl)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.CMS_Charging_Control.Where(w => w.Charging_Control_ID == pChargingControl.Charging_Control_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pChargingControl.Update_By = userlogin.Email_Address;
                        pChargingControl.Update_On = currentdate;
                        db.Entry(current).CurrentValues.SetValues(pChargingControl);
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Charging_Control
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Charging_Control,
                    Exception = ex
                };
            }
        }


        //CMS_Filling_Station
        public ServiceResult GetFillingStation(FillingStationCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var raws = db.CMS_Filling_Station
                          .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Filling_Station_ID.HasValue && cri.Filling_Station_ID.Value > 0)
                            raws = raws.Where(w => w.Filling_Station_ID == cri.Filling_Station_ID);

                        if (!string.IsNullOrEmpty(cri.Station_Code))
                            raws = raws.Where(w => w.Station_Code.Contains(cri.Station_Code));

                        if (!string.IsNullOrEmpty(cri.Record_Status))
                            raws = raws.Where(w => w.Record_Status.Contains(cri.Record_Status));

                        if (!string.IsNullOrEmpty(cri.Station_Code_Equals))
                            raws = raws.Where(w => w.Station_Code.Equals(cri.Station_Code_Equals));

                    }

                    result.Object = raws.OrderBy(o => SqlFunctions.IsNumeric(o.Station_Code)).ToList();
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
        public ServiceResult InsertFillingStation(CMS_Filling_Station pFillingStation)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pFillingStation.Create_By = userlogin.Email_Address;
                    pFillingStation.Create_On = currentdate;
                    pFillingStation.Update_By = userlogin.Email_Address;
                    pFillingStation.Update_On = currentdate;

                    db.CMS_Filling_Station.Add(pFillingStation);
                    db.SaveChanges();
                    db.Entry(pFillingStation).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Filling_Stations
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Filling_Stations,
                    Exception = ex
                };
            }
        }
        public ServiceResult UpdateFillingStation(CMS_Filling_Station pFillingStation)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.CMS_Filling_Station.Where(w => w.Filling_Station_ID == pFillingStation.Filling_Station_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pFillingStation.Update_By = userlogin.Email_Address;
                        pFillingStation.Update_On = currentdate;
                        db.Entry(current).CurrentValues.SetValues(pFillingStation);
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Filling_Stations
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Filling_Stations,
                    Exception = ex
                };
            }
        }


        //CMS_Product
        public ServiceResult GetProduct(ProductCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var raws = db.CMS_Product
                           .Include(s => s.CMS_Filling_Station)
                          .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.CMS_Product_ID.HasValue && cri.CMS_Product_ID.Value > 0)
                            raws = raws.Where(w => w.CMS_Product_ID == cri.CMS_Product_ID);

                        if (!string.IsNullOrEmpty(cri.Product_Code))
                            raws = raws.Where(w => w.Product_Code.Contains(cri.Product_Code));

                        if (!string.IsNullOrEmpty(cri.Record_Status))
                            raws = raws.Where(w => w.Record_Status.Contains(cri.Record_Status));

                        if (cri.Filling_Station_ID.HasValue && cri.Filling_Station_ID.Value > 0)
                            raws = raws.Where(w => w.Filling_Station_ID == cri.Filling_Station_ID);

                        if (!string.IsNullOrEmpty(cri.Station_Code_Equals))
                            raws = raws.Where(w => w.CMS_Filling_Station.Station_Code.Equals(cri.Station_Code_Equals));

                        if (!string.IsNullOrEmpty(cri.Product_Code_Equals))
                            raws = raws.Where(w => w.Product_Code.Equals(cri.Product_Code_Equals));
                    }

                    result.Object = raws.OrderBy(o => SqlFunctions.IsNumeric(o.CMS_Filling_Station.Station_Code)).ToList();
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
        public ServiceResult InsertProduct(CMS_Product pProduct)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pProduct.Create_By = userlogin.Email_Address;
                    pProduct.Create_On = currentdate;
                    pProduct.Update_By = userlogin.Email_Address;
                    pProduct.Update_On = currentdate;

                    db.CMS_Product.Add(pProduct);
                    db.SaveChanges();
                    db.Entry(pProduct).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Product_List
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Product_List,
                    Exception = ex
                };
            }
        }
        public ServiceResult UpdateProduct(CMS_Product pProduct)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.CMS_Product.Where(w => w.CMS_Product_ID == pProduct.CMS_Product_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pProduct.Update_By = userlogin.Email_Address;
                        pProduct.Update_On = currentdate;
                        db.Entry(current).CurrentValues.SetValues(pProduct);
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Product_List
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Product_List,
                    Exception = ex
                };
            }
        }


        public ServiceResult GetDrumControl(DrumControlCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var raws = db.CMS_Drum_Control
                           .Include(s => s.CMS_Drum_Type)
                        //.Include(s => s.CMS_Product)
                           .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Drum_Control_ID.HasValue && cri.Drum_Control_ID.Value > 0)
                            raws = raws.Where(w => w.Drum_Control_ID == cri.Drum_Control_ID);

                        if (cri.Drum_Type_ID.HasValue && cri.Drum_Type_ID.Value > 0)
                            raws = raws.Where(w => w.Drum_Type_ID == cri.Drum_Type_ID);

                        if (!string.IsNullOrEmpty(cri.Record_Status))
                            raws = raws.Where(w => w.Record_Status.Contains(cri.Record_Status));

                        if (cri.Running_Number.HasValue && cri.Running_Number.Value > 0)
                            raws = raws.Where(w => w.Running_Number == cri.Running_Number);



                        if (!string.IsNullOrEmpty(cri.Drum_Type_Equals))
                            raws = raws.Where(w => w.CMS_Drum_Type.Drum_Type.Equals(cri.Drum_Type_Equals));

                        if (!string.IsNullOrEmpty(cri.Product_Code_Equals))
                            raws = raws.Where(w => w.Product_Code.Equals(cri.Product_Code_Equals));

                    }

                    result.Object = raws.OrderBy(o => o.Product_Code).ThenBy(o => o.CMS_Drum_Type.Drum_Type).ToList();
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
        public ServiceResult InsertDrumControl(CMS_Drum_Control pDrumControl)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pDrumControl.Create_By = userlogin.Email_Address;
                    pDrumControl.Create_On = currentdate;
                    pDrumControl.Update_By = userlogin.Email_Address;
                    pDrumControl.Update_On = currentdate;

                    db.CMS_Drum_Control.Add(pDrumControl);
                    db.SaveChanges();
                    db.Entry(pDrumControl).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Drum_Control
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Drum_Control,
                    Exception = ex
                };
            }
        }
        public ServiceResult UpdateDrumControl(CMS_Drum_Control pDrumControl)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.CMS_Drum_Control.Where(w => w.Drum_Control_ID == pDrumControl.Drum_Control_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pDrumControl.Update_By = userlogin.Email_Address;
                        pDrumControl.Update_On = currentdate;
                        db.Entry(current).CurrentValues.SetValues(pDrumControl);
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Drum_Control
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Drum_Control,
                    Exception = ex
                };
            }
        }

        #endregion

        #region CMS Purge
        //public ServiceResult DeleteProduct(Nullable<int> pProduct)
        //{
        //   try
        //   {
        //      using (var db = new AgnosDBContext())
        //      {
        //         var product = db.CMS_Product.Where(w => w.CMS_Product_ID == pProduct).FirstOrDefault();
        //         if (product != null)
        //         {
        //            db.CMS_Product.Remove(product);
        //            db.SaveChanges();
        //         }
        //         return new ServiceResult()
        //         {
        //            Code = ReturnCode.SUCCESS,
        //            Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE),
        //            Field = Resource.Product_List
        //         };
        //      }
        //   }
        //   catch (Exception ex)
        //   {
        //      return new ServiceResult()
        //      {
        //         Code = ReturnCode.ERROR_DELETE,
        //         Msg = Success.GetMessage(ReturnCode.ERROR_DELETE),
        //         Field = Resource.Product_List,
        //         Exception = ex
        //      };
        //   }

        //}

        //CMS_Purge
        public ServiceResult GetCMSPurge(CMSCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                   IQueryable<CMS_Purge> purges = db.CMS_Purge.Include(i => i.CMS_Filling_Station)
                       .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Purge_ID.HasValue && cri.Purge_ID.Value > 0)
                            purges = purges.Where(w => w.Purge_ID == cri.Purge_ID);

                        if (!string.IsNullOrEmpty(cri.Drum_Code))
                            purges = purges.Where(w => w.Drum_Code.Contains(cri.Drum_Code));

                        if (!string.IsNullOrEmpty(cri.Date_From))
                        {
                            var d = DateUtil.ToDate(cri.Date_From);
                            purges = purges.Where(w => w.Create_On >= d);
                        }

                        if (!string.IsNullOrEmpty(cri.Date_To))
                        {
                            var d = DateUtil.ToDate(cri.Date_To);
                            purges = purges.Where(w => w.Create_On <= d);
                        }

                        if (cri.Not_Yet_Deliver)
                            purges = purges.Where(w => w.Delivery_Status != Delivery_Status.Completed);

                        if (cri.Top.HasValue)
                        {
                            purges = purges.Take(cri.Top.Value);
                        }
                    }

                    result.Object = purges.OrderBy(o => o.Drum_Code).ToList();
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
        public ServiceResult InsertCMSPurge(CMS_Purge pPurge)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pPurge.Create_By = userlogin.Email_Address;
                    pPurge.Create_On = currentdate;
                    pPurge.Update_By = userlogin.Email_Address;
                    pPurge.Update_On = currentdate;

                    db.CMS_Purge.Add(pPurge);
                    db.SaveChanges();
                    db.Entry(pPurge).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.CMS_Purge
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.CMS_Purge,
                    Exception = ex
                };
            }
        }
        public ServiceResult UpdateCMSPurge(CMS_Purge pPurge)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.CMS_Purge.Where(w => w.Purge_ID == pPurge.Purge_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pPurge.Update_By = userlogin.Email_Address;
                        pPurge.Update_On = currentdate;
                        db.Entry(current).CurrentValues.SetValues(pPurge);
                        db.SaveChanges();
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.CMS_Purge
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.CMS_Purge,
                    Exception = ex
                };
            }
        }
        #endregion

        #region CMS Charge

        //CMS_Charge
        public ServiceResult GetCMSCharge(CMSCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    IQueryable<CMS_Charge> charges = db.CMS_Charge
                       .Include(i => i.CMS_Filling_Station)
                       .Include(i => i.User_Profile)
                       .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Charge_ID.HasValue && cri.Charge_ID.Value > 0)
                            charges = charges.Where(w => w.Charge_ID == cri.Charge_ID);

                        if (cri.Filling_Station_ID.HasValue && cri.Filling_Station_ID.Value > 0)
                            charges = charges.Where(w => w.Filling_Station_ID == cri.Filling_Station_ID);

                        if (cri.Product_ID.HasValue && cri.Product_ID.Value > 0)
                            charges = charges.Where(w => w.Product_ID == cri.Product_ID);

                        if (!string.IsNullOrEmpty(cri.Drum_Code))
                            charges = charges.Where(w => w.Drum_Code.Contains(cri.Drum_Code.Trim()));

                        if (!string.IsNullOrEmpty(cri.Lot_No))
                            charges = charges.Where(w => w.Lot_No.Contains(cri.Lot_No.Trim()));

                        if (!string.IsNullOrEmpty(cri.Product_Code))
                            charges = charges.Where(w => w.Product_Code.Contains(cri.Product_Code));

                        if (!string.IsNullOrEmpty(cri.Drum_Code_Equals))
                            charges = charges.Where(w => w.Drum_Code.Equals(cri.Drum_Code_Equals));

                        if (!string.IsNullOrEmpty(cri.Lot_No_Equals))
                            charges = charges.Where(w => w.Lot_No.Equals(cri.Lot_No_Equals));

                        if (!string.IsNullOrEmpty(cri.Station_Code_Equals))
                            charges = charges.Where(w => w.CMS_Filling_Station.Station_Code.Equals(cri.Station_Code_Equals));

                        if (cri.Not_Yet_Deliver)
                            charges = charges.Where(w => w.Delivery_Status != Delivery_Status.Completed);

                        if (cri.Drum_Code_Have_Value)
                            charges = charges.Where(w => !string.IsNullOrEmpty(w.Drum_Code));

                        if (!string.IsNullOrEmpty(cri.Date_From))
                        {
                            var d = DateUtil.ToDate(cri.Date_From);
                            if (d.HasValue)
                                charges = charges.Where(w => DbFunctions.CreateDateTime(w.Create_On.Value.Year, w.Create_On.Value.Month, w.Create_On.Value.Day, 0, 0, 0) >= d);
                        }

                        if (!string.IsNullOrEmpty(cri.Date_To))
                        {
                            var d = DateUtil.ToDate(cri.Date_To);
                            if (d.HasValue)
                                charges = charges.Where(w => DbFunctions.CreateDateTime(w.Create_On.Value.Year, w.Create_On.Value.Month, w.Create_On.Value.Day, 0, 0, 0) <= d);
                        }

                        if (cri.Top.HasValue)
                        {
                            charges = charges.Take(cri.Top.Value);
                        }

                        if (!string.IsNullOrEmpty(cri.Sort_By))
                        {
                            if (cri.Sort_By == Sort_Type.Lot_No)
                                charges = charges.OrderBy(o => o.Lot_No);
                            else if (cri.Sort_By == Sort_Type.Drum_Code)
                                charges = charges.OrderBy(o => o.Drum_Code);
                            else if (cri.Sort_By == Sort_Type.Product_Code)
                                charges = charges.OrderBy(o => o.CMS_Product.Product_Code);
                            else if (cri.Sort_By == Sort_Type.Delivery_Order_No)
                                charges = charges.OrderBy(o => o.Delivery_Order_No);
                            else if (cri.Sort_By == Sort_Type.Charge_Date)
                                charges = charges.OrderBy(o => o.Update_On.Value).OrderByDescending(o => o.Update_On.Value.Day);
                            else if (cri.Sort_By == Sort_Type.Date_Delivered)
                                charges = charges.OrderBy(r => r.Date_Delivered).OrderByDescending(o => o.Date_Delivered.Value.Day);
                            else
                                charges = charges.OrderBy(o => o.Charge_ID);
                        }
                        else
                            charges.OrderBy(o => o.Lot_No);
                    }
                    result.Object = charges.ToList();
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
        public ServiceResult InsertCMSCharge(CMS_Charge pCharge)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pCharge.Profile_ID = userlogin.Profile_ID;
                    pCharge.Create_By2 = userlogin.Email_Address;
                    pCharge.Create_By = userlogin.Email_Address;
                    pCharge.Create_On = currentdate;
                    pCharge.Update_By = userlogin.Email_Address;
                    pCharge.Update_On = currentdate;

                    db.CMS_Charge.Add(pCharge);
                    db.SaveChanges();
                    db.Entry(pCharge).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.CMS_Charge
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.CMS_Charge,
                    Exception = ex
                };
            }
        }
        public ServiceResult UpdateCMSCharge(CMS_Charge pCharge)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.CMS_Charge.Where(w => w.Charge_ID == pCharge.Charge_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pCharge.Profile_ID = userlogin.Profile_ID;
                        pCharge.Update_By = userlogin.Email_Address;
                        pCharge.Update_On = currentdate;
                        db.Entry(current).CurrentValues.SetValues(pCharge);
                        db.SaveChanges();
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.CMS_Charge
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.CMS_Charge,
                    Exception = ex
                };
            }
        }
        public int DeleteSaveNewCMSChargeNotUse(string pStationCode)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    if (!string.IsNullOrEmpty(pStationCode))
                    {
                        var chargeNotUse = db.CMS_Charge.Where(w => string.IsNullOrEmpty(w.Drum_Code) && w.CMS_Filling_Station.Station_Code.Equals(pStationCode));
                        if (chargeNotUse != null)
                        {
                            db.CMS_Charge.RemoveRange(chargeNotUse);
                            db.SaveChanges();
                        }
                    }
                    return 1;
                }
            }
            catch
            {
                return -500;
            }
        }
        #endregion

        #region CMS Delivery
        public ServiceResult GetCMSDelivery(DeliveryCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    IQueryable<CMS_Delivery> deliverys = db.CMS_Delivery
                       .Include(i => i.CMS_Delivery_Detail)
                       .Include(i => i.CMS_Delivery_Detail.Select(s => s.CMS_Product))
                       .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Delivery_ID.HasValue && cri.Delivery_ID.Value > 0)
                            deliverys = deliverys.Where(w => w.Delivery_ID == cri.Delivery_ID);

                        if (!string.IsNullOrEmpty(cri.Delivery_Order_No))
                            deliverys = deliverys.Where(w => w.Delivery_Order_No.Contains(cri.Delivery_Order_No));

                        if (!string.IsNullOrEmpty(cri.Delivery_Order_No_Equals))
                            deliverys = deliverys.Where(w => w.Delivery_Order_No.Equals(cri.Delivery_Order_No_Equals));

                        if (cri.Not_Completed)
                            deliverys = deliverys.Where(w => w.Completed != true);

                        if (cri.Top.HasValue)
                        {
                            deliverys = deliverys.Take(cri.Top.Value);
                        }
                    }

                    result.Object = deliverys.OrderByDescending(o => o.Create_On).ToList();
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
        public ServiceResult InsertCMSDelivery(CMS_Delivery pDelivery)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pDelivery.Create_By = userlogin.Email_Address;
                    pDelivery.Create_On = currentdate;
                    pDelivery.Update_By = userlogin.Email_Address;
                    pDelivery.Update_On = currentdate;

                    db.CMS_Delivery.Add(pDelivery);
                    db.SaveChanges();
                    db.Entry(pDelivery).GetDatabaseValues();

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.CMS_Delivery,
                        Object = pDelivery.Delivery_ID
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.CMS_Delivery,
                    Exception = ex
                };
            }
        }
        public ServiceResult UpdateCMSDelivery(CMS_Delivery pDelivery)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.CMS_Delivery.Where(w => w.Delivery_ID == pDelivery.Delivery_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pDelivery.Update_By = userlogin.Email_Address;
                        pDelivery.Update_On = currentdate;
                        if (pDelivery.Record_Status != Record_Status.Delete)
                        {
                            if (pDelivery.CMS_Delivery_Detail == null)
                                db.CMS_Delivery_Detail.RemoveRange(current.CMS_Delivery_Detail);
                            else
                            {
                                var currentID = current.CMS_Delivery_Detail.Select(s => s.CMS_Delivery_Detail_ID).ToArray();
                                foreach (var detailID in currentID)
                                {
                                    if (!pDelivery.CMS_Delivery_Detail.Select(s => s.CMS_Delivery_Detail_ID).Contains(detailID))
                                    {
                                        var currentDetail = db.CMS_Delivery_Detail.Where(w => w.CMS_Delivery_Detail_ID == detailID).FirstOrDefault();

                                        /* start clear map cms charge flow */
                                        if (current.Completed.HasValue && current.Completed.Value)
                                        {

                                            if (!string.IsNullOrEmpty(currentDetail.Drum_Code))
                                            {
                                                string[] drumcodelst = currentDetail.Drum_Code.Split(',');
                                                foreach (string drumcode in drumcodelst)
                                                {
                                                    if (!string.IsNullOrEmpty(drumcode))
                                                    {
                                                        var charge = db.CMS_Charge.Where(w => w.Drum_Code == drumcode.Trim() && w.Delivery_Status == Delivery_Status.Completed && currentDetail.Delivery_ID == w.Delivery_ID).FirstOrDefault();
                                                        if (charge != null)
                                                        {
                                                            charge.Delivery_ID = null;
                                                            charge.Delivery_Order_No = null;
                                                            charge.Date_Delivered = null;
                                                            charge.Delivery_Status = null;
                                                        }

                                                        var purge = db.CMS_Purge.Where(w => w.Drum_Code == drumcode.Trim() && w.Delivery_Status == Delivery_Status.Completed && currentDetail.Delivery_ID == w.Delivery_ID).FirstOrDefault();
                                                        if (purge != null)
                                                        {
                                                            purge.Delivery_ID = null;
                                                            purge.Delivery_Status = null;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        /* end  clear map cms charge flow */

                                        db.CMS_Delivery_Detail.Remove(currentDetail);
                                    }
                                }
                                foreach (var detailID in pDelivery.CMS_Delivery_Detail)
                                {
                                    detailID.Update_By = userlogin.Email_Address;
                                    detailID.Update_On = currentdate;
                                    if (detailID.CMS_Delivery_Detail_ID == 0)
                                    {
                                        detailID.Create_By = userlogin.Email_Address;
                                        detailID.Create_On = currentdate;
                                        db.CMS_Delivery_Detail.Add(detailID);
                                    }
                                    else
                                    {
                                        var currentDetail = db.CMS_Delivery_Detail.Where(w => w.CMS_Delivery_Detail_ID == detailID.CMS_Delivery_Detail_ID).FirstOrDefault();
                                        db.Entry(currentDetail).CurrentValues.SetValues(detailID);
                                    }
                                }
                            }
                        }
                        db.Entry(current).CurrentValues.SetValues(pDelivery);
                        db.SaveChanges();
                        
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.CMS_Delivery,
                        Object = current.Delivery_ID
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.CMS_Delivery,
                    Exception = ex
                };
            }
        }

        #endregion

    }

    public class FormatCriteria : CriteriaBase
    {
        public Nullable<int> Format_ID { get; set; }
        public string Format_Code { get; set; }
        public Nullable<int> Lot_No_Length { get; set; }
        public Nullable<int> Product_Code_Length { get; set; }
    }

    public class DrumCriteria : CriteriaBase
    {
        public Nullable<int> Drum_Type_ID { get; set; }
        public string Drum_Type { get; set; }
        public string Drum_Type_Equals { get; set; }
    }

    public class SkipPurgCriteria : CriteriaBase
    {
        public Nullable<int> Skip_Purging_ID { get; set; }
        public Nullable<int> Drum_Type_ID { get; set; }
        public string Product_Code { get; set; }
        public string Drum_Type { get; set; }
        public string Product_Code_Equals { get; set; }

    }

    public class ChargingControlCriteria : CriteriaBase
    {
        public Nullable<int> Charging_Control_ID { get; set; }
        public string Product_Code { get; set; }
        public string Drum_Type { get; set; }
        public string Action { get; set; }
        public string Drum_Code { get; set; }
    }

    public class FillingStationCriteria : CriteriaBase
    {
        public Nullable<int> Filling_Station_ID { get; set; }
        public string Station_Code { get; set; }
        public string Station_Code_Equals { get; set; }
    }

    public class ProductCriteria : CriteriaBase
    {
        public Nullable<int> CMS_Product_ID { get; set; }
        public Nullable<int> Filling_Station_ID { get; set; }
        public string Product_Code { get; set; }
        public string Product_Code_Equals { get; set; }
        public string Station_Code_Equals { get; set; }

    }

    public class DeliveryCriteria : CriteriaBase
    {
        public Nullable<int> Delivery_ID { get; set; }
        public string Delivery_Order_No { get; set; }
        public bool Not_Completed { get; set; }
        public string Delivery_Order_No_Equals { get; set; }
    }

    public class DrumControlCriteria : CriteriaBase
    {
        public Nullable<int> Drum_Control_ID { get; set; }
        public Nullable<int> Drum_Type_ID { get; set; }
        public Nullable<int> Running_Number { get; set; }
        public string Product_Code_Equals { get; set; }
        public string Drum_Type_Equals { get; set; }
    }
}
