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
namespace AgnosModel.Service
{
    public class MobileCri : CriteriaBase
    {
        public Nullable<int> Delivery_ID { get; set; }
        public Nullable<bool> Sync_Not_Completed { get; set; }
        public Nullable<bool> Completed { get; set; }
        public Nullable<bool> Sync_Current_Data { get; set; }
        public Nullable<bool> Is_Active { get; set; }
    }
    public class MobileService : ServiceBase
    {
        public MobileService()
        {
        }
        public MobileService(User_Profile user)
        {
            userlogin = user;
        }
        public MobileService(string aspID)
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
        public ServiceResult GetCMSDelivery(MobileCri cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    IQueryable<CMS_Delivery> deliverys = db.CMS_Delivery
                       .Include(i => i.CMS_Delivery_Detail)
                       .Include(i => i.CMS_Delivery_Detail.Select(s => s.CMS_Product))
                       .Where(w => 1 == 1);

                    if (cri != null)
                    {
                        if (cri.Delivery_ID.HasValue && cri.Delivery_ID.Value > 0)
                            deliverys = deliverys.Where(w => w.Delivery_ID == cri.Delivery_ID);

                        if (cri.Sync_Not_Completed.HasValue && cri.Sync_Not_Completed.Value)
                            deliverys = deliverys.Where(w => w.Completed == !cri.Sync_Not_Completed || w.Completed == null);

                        if (cri.Completed.HasValue && cri.Completed.Value)
                            deliverys = deliverys.Where(w => w.Completed == cri.Completed.Value);

                        if (cri.Sync_Current_Data.HasValue && cri.Sync_Current_Data.Value)
                            deliverys = deliverys.Where(w => w.Completed == true || w.Record_Status == Record_Status.Delete);

                        if (cri.Is_Active.HasValue && cri.Is_Active.Value)
                            deliverys = deliverys.Where(w => w.Record_Status == Record_Status.Active);
                    }

                    result.Object = deliverys.OrderBy(o => o.Delivery_Order_No).ToList();
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
        public ServiceResult UpdateCMSDelivery(List<CMS_Delivery> Deliverys)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var chargeIDs = new List<int>();
                    var purgeIDs = new List<int>();
                    foreach (var row in Deliverys)
                    {
                        var current = db.CMS_Delivery.Where(w => w.Delivery_ID == row.Delivery_ID).FirstOrDefault();
                        if (current != null)
                        {
                            foreach (var wrow in row.CMS_Delivery_Detail)
                            {
                                var current2 = db.CMS_Delivery_Detail.Where(w => w.CMS_Delivery_Detail_ID == wrow.CMS_Delivery_Detail_ID).FirstOrDefault();
                                if (current2 != null)
                                {
                                    if (!string.IsNullOrEmpty(wrow.Drum_Code))
                                    {
                                        string[] drumcodelst = wrow.Drum_Code.Split(',');
                                        foreach (string drumcode in drumcodelst)
                                        {
                                            if (!string.IsNullOrEmpty(drumcode))
                                            {
                                                var charge = db.CMS_Charge.Where(w => w.Drum_Code == drumcode.Trim() && w.Delivery_Status != Delivery_Status.Completed).FirstOrDefault();
                                                if (charge != null)
                                                {
                                                    if (!chargeIDs.Contains(charge.Charge_ID))
                                                    {
                                                        charge.Delivery_ID = current.Delivery_ID;
                                                        charge.Delivery_Order_No = current.Delivery_Order_No;
                                                        charge.Date_Delivered = wrow.Date_Delivered;
                                                        charge.Delivery_Status = Delivery_Status.Completed;
                                                        chargeIDs.Add(charge.Charge_ID);
                                                    }
                                                }

                                                var purge = db.CMS_Purge.Where(w => w.Drum_Code == drumcode.Trim() && w.Delivery_Status != Delivery_Status.Completed).FirstOrDefault();
                                                if (purge != null)
                                                {
                                                    if (!purgeIDs.Contains(purge.Purge_ID))
                                                    {
                                                        purge.Delivery_ID = current.Delivery_ID;
                                                        purge.Delivery_Status = Delivery_Status.Completed;
                                                        purgeIDs.Add(purge.Purge_ID);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    current2.Drum_Code = wrow.Drum_Code;
                                    current2.Update_By = wrow.Update_By;
                                    current2.Update_On = wrow.Update_On;
                                    current2.Date_Delivered = wrow.Date_Delivered;
                                }
                            }
                            db.Entry(current).CurrentValues.SetValues(row);
                        }
                    }
                    db.SaveChanges();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.CMS_Delivery
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
        public ServiceResult UpdateCMSDelivery(CMS_Delivery Delivery)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.CMS_Delivery.Where(w => w.Delivery_ID == Delivery.Delivery_ID).FirstOrDefault();
                    if (current != null)
                    {
                        var chargeIDs = new List<int>();
                        var purgeIDs = new List<int>();
                        foreach (var wrow in Delivery.CMS_Delivery_Detail)
                        {
                            var current2 = db.CMS_Delivery_Detail.Where(w => w.CMS_Delivery_Detail_ID == wrow.CMS_Delivery_Detail_ID).FirstOrDefault();
                            if (current2 != null)
                            {
                                if (!string.IsNullOrEmpty(wrow.Drum_Code))
                                {
                                    string[] drumcodelst = wrow.Drum_Code.Split(',');
                                    foreach (string drumcode in drumcodelst)
                                    {
                                        if (!string.IsNullOrEmpty(drumcode))
                                        {
                                            var charge = db.CMS_Charge.Where(w => w.Drum_Code == drumcode.Trim() && w.Delivery_Status != Delivery_Status.Completed).FirstOrDefault();
                                            if (charge != null)
                                            {
                                                if (!chargeIDs.Contains(charge.Charge_ID))
                                                {
                                                    charge.Delivery_ID = current.Delivery_ID;
                                                    charge.Delivery_Order_No = current.Delivery_Order_No;
                                                    charge.Date_Delivered = wrow.Date_Delivered;
                                                    charge.Delivery_Status = Delivery_Status.Completed;
                                                    chargeIDs.Add(charge.Charge_ID);
                                                }
                                            }

                                            var purge = db.CMS_Purge.Where(w => w.Drum_Code == drumcode.Trim() && w.Delivery_Status != Delivery_Status.Completed).FirstOrDefault();
                                            if (purge != null)
                                            {
                                                if (!purgeIDs.Contains(purge.Purge_ID))
                                                {
                                                    purge.Delivery_ID = current.Delivery_ID;
                                                    purge.Delivery_Status = Delivery_Status.Completed;
                                                    purgeIDs.Add(purge.Purge_ID);
                                                }
                                            }
                                        }
                                    }
                                }
                                current2.Drum_Code = wrow.Drum_Code;
                                current2.Update_By = wrow.Update_By;
                                current2.Update_On = wrow.Update_On;
                                current2.Date_Delivered = wrow.Date_Delivered;
                            }
                        }
                        db.Entry(current).CurrentValues.SetValues(Delivery);
                    }

                    db.SaveChanges();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.CMS_Delivery
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
    }
}
