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
    public class GlobalLookupCriteria : CriteriaBase
    {
        public int? Def_ID { get; set; }
        public int? Lookup_Data_ID { get; set; }
    }

    public class GlobalLookupService : ServiceBase
    {
        public ServiceResult GetGlobalLookupData(GlobalLookupCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var rows = db.Global_Lookup_Data
                        .Include(w => w.Global_Lookup_Def)
                        .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Def_ID.HasValue)
                            rows = rows.Where(w => w.Def_ID == cri.Def_ID);
                        if (cri.Lookup_Data_ID.HasValue)
                            rows = rows.Where(w => w.Lookup_Data_ID == cri.Lookup_Data_ID);
                    }
                    result.Object = rows.OrderBy(o => o.Lookup_Data_ID).ToList();
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
        public ServiceResult InsertGlobalLookupData(Global_Lookup_Data pData)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {

                    db.Global_Lookup_Data.Add(pData);
                    db.SaveChanges();
                    db.Entry(pData).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Global_Lookup
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Global_Lookup,
                    Exception = ex
                };
            }
        }
        public ServiceResult UpdateGlobalLookupData(Global_Lookup_Data pData)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.Global_Lookup_Data.Where(w => w.Lookup_Data_ID == pData.Lookup_Data_ID).FirstOrDefault();
                    if (current != null)
                    {
                        db.Entry(current).CurrentValues.SetValues(pData);
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Global_Lookup
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Global_Lookup,
                    Exception = ex
                };
            }
        }

    }
}
