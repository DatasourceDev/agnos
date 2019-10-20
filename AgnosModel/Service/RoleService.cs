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
    public class RoleCriteria : CriteriaBase
    {
        public int? Role_ID { get; set; }
        public int? Page_ID { get; set; }
        public int? Page_Role_ID { get; set; }
    }

    public class RoleService : ServiceBase
    {
        public ServiceResult GetPage(RoleCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var rows = db.Pages
                        .Where(w => 1 == 1);

                    if (cri != null)
                    {
                        if (cri.Page_ID.HasValue)
                            rows = rows.Where(w => w.Page_ID == cri.Page_ID);
                    }
                    result.Object = rows.OrderByDescending(o => o.Page_Name).ToList();
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

        public ServiceResult GetRole(RoleCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var rows = db.Roles
                        .Where(w => 1 == 1);

                    if (cri != null)
                    {
                        if (cri.Role_ID.HasValue)
                            rows = rows.Where(w => w.Role_ID == cri.Role_ID);
                    }
                    result.Object = rows.OrderByDescending(o => o.Role_Name).ToList();
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

        public ServiceResult GetPageRole(RoleCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var rows = db.Page_Role
                       .Include(i => i.Role)
                       .Include(i => i.Page)
                        .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Page_ID.HasValue)
                            rows = rows.Where(w => w.Page_ID == cri.Page_ID);

                        if (cri.Role_ID.HasValue)
                            rows = rows.Where(w => w.Role_ID == cri.Role_ID);

                        if (cri.Page_Role_ID.HasValue)
                            rows = rows.Where(w => w.Page_Role_ID == cri.Page_Role_ID);
                    }
                    result.Object = rows.OrderBy(o => o.Role_ID).ThenBy(o => o.Page.Page_Code).ToList();
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

        public ServiceResult InsertPageRole(Page_Role pPR)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {

                    db.Page_Role.Add(pPR);
                    db.SaveChanges();
                    db.Entry(pPR).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Page_Role
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Page_Role,
                    Exception = ex
                };
            }
        }

        public ServiceResult UpdatePageRole(Page_Role pPR)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.Page_Role.Where(w => w.Page_Role_ID == pPR.Page_Role_ID).FirstOrDefault();
                    if (current != null)
                    {
                        db.Entry(current).CurrentValues.SetValues(pPR);
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Page_Role
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Page_Role,
                    Exception = ex
                };
            }
        }

        //public ServiceResult DeletePageRole(Nullable<int> pPRID)
        //{
        //   try
        //   {
        //      using (var db = new AgnosDBContext())
        //      {
        //         var current = db.Page_Role.Where(w => w.Page_Role_ID == pPRID).FirstOrDefault();
        //         if (current != null)
        //         {
        //            db.Page_Role.Remove(current);
        //            db.SaveChanges();
        //         }
        //         return new ServiceResult()
        //         {
        //            Code = ReturnCode.SUCCESS,
        //            Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE),
        //            Field = Resource.Page_Role
        //         };
        //      }
        //   }
        //   catch (Exception ex)
        //   {
        //      return new ServiceResult()
        //      {
        //         Code = ReturnCode.ERROR_DELETE,
        //         Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
        //         Field = Resource.Page_Role,
        //         Exception = ex
        //      };
        //   }

        //}
    }
}
