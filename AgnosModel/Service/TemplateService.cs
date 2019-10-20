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
    public class GroupCriteria : CriteriaBase
    {
        public Nullable<int> Group_ID { get; set; }
        public string Group_Name { get; set; }
        public string Duplicat_Name { get; set; }
    }
    public class HeaderCriteria : CriteriaBase
    {
        public Nullable<int> Header_ID { get; set; }
        public string Header_Name { get; set; }
        public string Duplicat_Name { get; set; }
    }
    public class FieldCriteria : CriteriaBase
    {
        public Nullable<int> Field_ID { get; set; }
        public string Field_Name { get; set; }
        public string Duplicat_Name { get; set; }
    }
    public class TemplateLogsheetCriteria : CriteriaBase
    {
        public Nullable<int> Template_Header_ID { get; set; }
        public Nullable<int> Template_Field_ID { get; set; }
        public Nullable<int> Template_Group_ID { get; set; }
        public Nullable<int> Template_ID { get; set; }
        public string Template_Code { get; set; }
        public string Template_Name { get; set; }
    }
    public class ProductTempateCriteria : CriteriaBase
    {
        public Nullable<int> Product_Template_ID { get; set; }
        public Nullable<int> Template_ID { get; set; }
        public string Product_Code { get; set; }

    }
    public class LogsheetCriteria : CriteriaBase
    {
        public Nullable<int> Logsheet_ID { get; set; }
        public Nullable<int> Template_ID { get; set; }
        public string Product_Code { get; set; }
        public string Lot_No { get; set; }
        public string Template_Code { get; set; }
        public string Logsheet_Status { get; set; }
    }
    public class LotNumberCriteria : CriteriaBase
    {
        public Nullable<int> Lot_Number_ID { get; set; }
        public string Product_Code { get; set; }
        public string Lot_No { get; set; }
        public DateTime? Lot_Number_Date { get; set; }
    }
    public class TemplateService : ServiceBase
    {

        public TemplateService()
        {
        }
        public TemplateService(User_Profile user)
        {
            userlogin = user;
        }
        public TemplateService(string aspID)
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

        #region Template Components
        public ServiceResult GetTempateGroup(GroupCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var groups = db.Template_Group.Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);
                    if (cri != null)
                    {
                        if (cri.Group_ID.HasValue)
                            groups = groups.Where(w => w.Group_ID == cri.Group_ID);

                        if (!string.IsNullOrEmpty(cri.Group_Name))
                            groups = groups.Where(w => w.Group_Name.Contains(cri.Group_Name));

                        if (!string.IsNullOrEmpty(cri.Duplicat_Name))
                            groups = groups.Where(w => w.Group_Name == cri.Duplicat_Name);

                        if (cri.Top.HasValue)
                        {
                            groups = groups.Take(cri.Top.Value);
                        }
                    }
                    result.Object = groups.OrderBy(o => o.Group_Order).ToList();
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

        public ServiceResult InsertTempateGroup(Template_Group pGroup)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pGroup.Create_By = userlogin.Email_Address;
                    pGroup.Create_On = currentdate;
                    pGroup.Update_By = userlogin.Email_Address;
                    pGroup.Update_On = currentdate;
                    pGroup.Group_Order = 1;
                    var max = db.Template_Group.Where(w => w.Record_Status != Record_Status.Delete).OrderByDescending(o => o.Group_Order).FirstOrDefault();
                    if (max != null)
                        pGroup.Group_Order = max.Group_Order + 1;

                    db.Template_Group.Add(pGroup);
                    db.SaveChanges();
                    db.Entry(pGroup).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Group
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Group,
                    Exception = ex
                };
            }
        }

        public ServiceResult UpdateTempateGroup(Template_Group pGroup)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.Template_Group.Where(w => w.Group_ID == pGroup.Group_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pGroup.Update_By = userlogin.Email_Address;
                        pGroup.Update_On = currentdate;
                        db.Entry(current).CurrentValues.SetValues(pGroup);
                        db.SaveChanges();

                        var orderNo = 1;
                        foreach (var g in db.Template_Group.Where(w => w.Record_Status != Record_Status.Delete).OrderBy(o => o.Group_Order))
                        {
                            g.Group_Order = orderNo;
                            orderNo += 1;
                        }
                        db.SaveChanges();
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Group
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Group,
                    Exception = ex
                };
            }
        }

        public ServiceResult DeleteTempateGroup(Nullable<int> pGroupID)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    if (!pGroupID.HasValue)
                        return new ServiceResult()
                        {
                            Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                            Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
                            Field = Resource.Group
                        };

                    var group = db.Template_Group.Where(w => w.Group_ID == pGroupID).FirstOrDefault();
                    if (group != null)
                    {
                        var tmpuse = group.Tmp_Log_Group.Where(w => w.Template_Logsheet.Record_Status != Record_Status.Delete).FirstOrDefault();
                        if (tmpuse != null)
                        {
                            return new ServiceResult()
                            {
                                Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                                Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
                                Field = Resource.Group
                            };
                        }
                        var loguse = group.Logsheet_Group.Where(w => w.Logsheet.Record_Status != Record_Status.Delete).FirstOrDefault();
                        if (loguse != null)
                        {
                            return new ServiceResult()
                            {
                                Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                                Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
                                Field = Resource.Group
                            };
                        }
                        group.Record_Status = Record_Status.Delete;
                        db.SaveChanges();

                        var orderNo = 1;
                        foreach (var g in db.Template_Group.Where(w => w.Record_Status != Record_Status.Delete).OrderBy(o => o.Group_Order))
                        {
                            g.Group_Order = orderNo;
                            orderNo += 1;
                        }
                        db.SaveChanges();
                    }
                    return new ServiceResult()
                    {

                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE),
                        Field = Resource.Group
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_DELETE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
                    Field = Resource.Group,
                    Exception = ex
                };

            }

        }

        public ServiceResult MoveUpTempateGroup(int? pGroupID)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    int? oldOrder = 0;
                    var move = db.Template_Group.Where(w => w.Group_ID == pGroupID).FirstOrDefault();
                    if (move == null)
                        return new ServiceResult()
                        {
                            Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                            Msg = Error.GetMessage(ReturnCode.ERROR_DATA_NOT_FOUND),
                            Field = Resource.Group
                        };
                    oldOrder = move.Group_Order;
                    if (oldOrder > 1)
                    {
                        var groups = db.Template_Group.OrderBy(o => o.Group_Order);
                        foreach (var group in groups)
                        {
                            if (group.Group_ID == pGroupID)
                            {
                                group.Group_Order -= 1;
                                group.Update_By = userlogin.Email_Address;
                                group.Update_On = currentdate;
                            }
                            else if (group.Group_Order == oldOrder - 1)
                            {
                                group.Group_Order += 1;
                                group.Update_By = userlogin.Email_Address;
                                group.Update_On = currentdate;
                            }
                        }

                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Group
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Group,
                    Exception = ex
                };
            }
        }

        public ServiceResult MoveDownTempateGroup(int? pGroupID)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    int? oldOrder = 0;
                    var move = db.Template_Group.Where(w => w.Group_ID == pGroupID).FirstOrDefault();
                    if (move == null)
                        return new ServiceResult()
                        {
                            Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                            Msg = Error.GetMessage(ReturnCode.ERROR_DATA_NOT_FOUND),
                            Field = Resource.Group
                        };

                    oldOrder = move.Group_Order;
                    var groups = db.Template_Group.OrderBy(o => o.Group_Order);

                    if (oldOrder < groups.Count())
                    {
                        foreach (var group in groups)
                        {
                            if (group.Group_ID == pGroupID)
                            {
                                group.Group_Order += 1;
                                group.Update_By = userlogin.Email_Address;
                                group.Update_On = currentdate;
                            }
                            else if (group.Group_Order == oldOrder + 1)
                            {
                                group.Group_Order -= 1;
                                group.Update_By = userlogin.Email_Address;
                                group.Update_On = currentdate;
                            }
                        }

                        db.SaveChanges();
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Group
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Group,
                    Exception = ex
                };
            }
        }

        public ServiceResult GetTempateHeader(HeaderCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {

                    var headers = db.Template_Header.Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);
                    if (cri != null)
                    {
                        if (cri.Header_ID.HasValue)
                            headers = headers.Where(w => w.Header_ID == cri.Header_ID);

                        if (!string.IsNullOrEmpty(cri.Header_Name))
                            headers = headers.Where(w => w.Header_Name.Contains(cri.Header_Name));

                        if (!string.IsNullOrEmpty(cri.Duplicat_Name))
                            headers = headers.Where(w => w.Header_Name == cri.Duplicat_Name);

                        if (cri.Top.HasValue)
                        {
                            headers = headers.Take(cri.Top.Value);
                        }
                    }
                    result.Object = headers.OrderBy(o => o.Header_Order).ToList();
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

        public ServiceResult InsertTempateHeader(Template_Header pHeader)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pHeader.Create_By = userlogin.Email_Address;
                    pHeader.Create_On = currentdate;
                    pHeader.Update_By = userlogin.Email_Address;
                    pHeader.Update_On = currentdate;
                    pHeader.Header_Order = 1;
                    var max = db.Template_Header.OrderByDescending(o => o.Header_Order).FirstOrDefault();
                    if (max != null)
                        pHeader.Header_Order = max.Header_Order + 1;

                    db.Template_Header.Add(pHeader);
                    db.SaveChanges();
                    db.Entry(pHeader).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Header
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Header,
                    Exception = ex
                };
            }
        }

        public ServiceResult UpdateTempateHeader(Template_Header pHeader)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.Template_Header.Where(w => w.Header_ID == pHeader.Header_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pHeader.Update_By = userlogin.Email_Address;
                        pHeader.Update_On = currentdate;
                        db.Entry(current).CurrentValues.SetValues(pHeader);
                        db.SaveChanges();

                        if (pHeader.Record_Status == Record_Status.Delete)
                        {
                            var orderNo = 1;
                            foreach (var g in db.Template_Header.OrderBy(o => o.Header_Order))
                            {
                                g.Header_Order = orderNo;
                                orderNo += 1;
                            }
                            db.SaveChanges();
                        }
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Header
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Header,
                    Exception = ex
                };
            }
        }

        public ServiceResult DeleteTempateHeader(Nullable<int> pHeaderID)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    if (!pHeaderID.HasValue)
                        return new ServiceResult()
                        {
                            Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                            Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
                            Field = Resource.Group
                        };

                    var hdr = db.Template_Header.Where(w => w.Header_ID == pHeaderID).FirstOrDefault();
                    if (hdr != null)
                    {
                        var tmpuse = hdr.Tmp_Log_Header.Where(w => w.Tmp_Log_Group.Template_Logsheet.Record_Status != Record_Status.Delete).FirstOrDefault();
                        if (tmpuse != null)
                        {
                            return new ServiceResult()
                            {
                                Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                                Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
                                Field = Resource.Header
                            };
                        }
                        var loguse = hdr.Logsheet_Header.Where(w => w.Logsheet_Group.Logsheet.Record_Status != Record_Status.Delete).FirstOrDefault();
                        if (loguse != null)
                        {
                            return new ServiceResult()
                            {
                                Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                                Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
                                Field = Resource.Header
                            };
                        }


                        hdr.Record_Status = Record_Status.Delete;
                        db.SaveChanges();

                        var orderNo = 1;
                        foreach (var g in db.Template_Header.OrderBy(o => o.Header_Order))
                        {
                            g.Header_Order = orderNo;
                            orderNo += 1;
                        }
                        db.SaveChanges();
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE),
                        Field = Resource.Header
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_DELETE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
                    Field = Resource.Header,
                    Exception = ex
                };
            }

        }

        public ServiceResult MoveUpTempateHeader(int? pHeaderID)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    int? oldOrder = 0;
                    var move = db.Template_Header.Where(w => w.Header_ID == pHeaderID).FirstOrDefault();
                    if (move == null)
                        return new ServiceResult()
                        {
                            Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                            Msg = Error.GetMessage(ReturnCode.ERROR_DATA_NOT_FOUND),
                            Field = Resource.Header
                        };
                    oldOrder = move.Header_Order;
                    if (oldOrder > 1)
                    {
                        var headers = db.Template_Header.OrderBy(o => o.Header_Order);
                        foreach (var hdr in headers)
                        {
                            if (hdr.Header_ID == pHeaderID)
                            {
                                hdr.Header_Order -= 1;
                                hdr.Update_By = userlogin.Email_Address;
                                hdr.Update_On = currentdate;
                            }
                            else if (hdr.Header_Order == oldOrder - 1)
                            {
                                hdr.Header_Order += 1;
                                hdr.Update_By = userlogin.Email_Address;
                                hdr.Update_On = currentdate;
                            }
                        }

                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Header
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Group,
                    Exception = ex
                };
            }
        }

        public ServiceResult MoveDownTempateHeader(int? pHeaderID)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    int? oldOrder = 0;
                    var move = db.Template_Header.Where(w => w.Header_ID == pHeaderID).FirstOrDefault();
                    if (move == null)
                        return new ServiceResult()
                        {
                            Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                            Msg = Error.GetMessage(ReturnCode.ERROR_DATA_NOT_FOUND),
                            Field = Resource.Header
                        };

                    oldOrder = move.Header_Order;
                    var headers = db.Template_Header.OrderBy(o => o.Header_Order);

                    if (oldOrder < headers.Count())
                    {
                        foreach (var header in headers)
                        {
                            if (header.Header_ID == pHeaderID)
                            {
                                header.Header_Order += 1;
                                header.Update_By = userlogin.Email_Address;
                                header.Update_On = currentdate;
                            }
                            else if (header.Header_Order == oldOrder + 1)
                            {
                                header.Header_Order -= 1;
                                header.Update_By = userlogin.Email_Address;
                                header.Update_On = currentdate;
                            }
                        }

                        db.SaveChanges();
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Header
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Header,
                    Exception = ex
                };
            }
        }

        public ServiceResult GetTempateField(FieldCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var fields = db.Template_Field.Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);
                    if (cri != null)
                    {
                        if (cri.Field_ID.HasValue)
                            fields = fields.Where(w => w.Field_ID == cri.Field_ID);

                        if (!string.IsNullOrEmpty(cri.Field_Name))
                            fields = fields.Where(w => w.Field_Name.Contains(cri.Field_Name));

                        if (!string.IsNullOrEmpty(cri.Duplicat_Name))
                            fields = fields.Where(w => w.Field_Name == cri.Duplicat_Name);

                        if (cri.Top.HasValue)
                        {
                            fields = fields.Take(cri.Top.Value);
                        }
                    }
                    result.Object = fields.OrderBy(o => o.Field_Order).ToList();
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

        public ServiceResult InsertTempateField(Template_Field pField)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pField.Create_By = userlogin.Email_Address;
                    pField.Create_On = currentdate;
                    pField.Update_By = userlogin.Email_Address;
                    pField.Update_On = currentdate;
                    pField.Field_Order = 1;
                    var max = db.Template_Field.OrderByDescending(o => o.Field_Order).FirstOrDefault();
                    if (max != null)
                        pField.Field_Order = max.Field_Order + 1;
                    db.Template_Field.Add(pField);
                    db.SaveChanges();
                    db.Entry(pField).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Field
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Field,
                    Exception = ex
                };
            }
        }

        public ServiceResult UpdateTempateField(Template_Field pField)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.Template_Field.Where(w => w.Field_ID == pField.Field_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pField.Update_By = userlogin.Email_Address;
                        pField.Update_On = currentdate;
                        db.Entry(current).CurrentValues.SetValues(pField);
                        db.SaveChanges();

                        if (pField.Record_Status == Record_Status.Delete)
                        {
                            var orderNo = 1;
                            foreach (var g in db.Template_Field.OrderBy(o => o.Field_Order))
                            {
                                g.Field_Order = orderNo;
                                orderNo += 1;
                            }
                            db.SaveChanges();
                        }
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Field
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Field,
                    Exception = ex
                };
            }
        }

        public ServiceResult DeleteTempateField(Nullable<int> pFieldID)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    if (!pFieldID.HasValue)
                        return new ServiceResult()
                        {
                            Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                            Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
                            Field = Resource.Field
                        };

                    var field = db.Template_Field.Where(w => w.Field_ID == pFieldID).FirstOrDefault();
                    if (field != null)
                    {
                        var tmpuse = field.Tmp_Log_Field.Where(w => w.Tmp_Log_Group.Template_Logsheet.Record_Status != Record_Status.Delete).FirstOrDefault();
                        if (tmpuse != null)
                        {
                            return new ServiceResult()
                            {
                                Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                                Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
                                Field = Resource.Field
                            };
                        }
                        var loguse = field.Logsheet_Field.Where(w => w.Logsheet_Group.Logsheet.Record_Status != Record_Status.Delete).FirstOrDefault();
                        if (loguse != null)
                        {
                            return new ServiceResult()
                            {
                                Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                                Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
                                Field = Resource.Field
                            };
                        }
                        field.Record_Status = Record_Status.Delete;
                        db.SaveChanges();
                        var orderNo = 1;
                        foreach (var g in db.Template_Field.OrderBy(o => o.Field_Order))
                        {
                            g.Field_Order = orderNo;
                            orderNo += 1;
                        }
                        db.SaveChanges();
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE),
                        Field = Resource.Field
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_DELETE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
                    Field = Resource.Field,
                    Exception = ex
                };
            }

        }

        public ServiceResult MoveUpTempateField(int? pFieldID)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    int? oldOrder = 0;
                    var move = db.Template_Field.Where(w => w.Field_ID == pFieldID).FirstOrDefault();
                    if (move == null)
                        return new ServiceResult()
                        {
                            Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                            Msg = Error.GetMessage(ReturnCode.ERROR_DATA_NOT_FOUND),
                            Field = Resource.Field
                        };
                    oldOrder = move.Field_Order;
                    if (oldOrder > 1)
                    {
                        var fields = db.Template_Field.OrderBy(o => o.Field_Order);
                        foreach (var field in fields)
                        {
                            if (field.Field_ID == pFieldID)
                            {
                                field.Field_Order -= 1;
                                field.Update_By = userlogin.Email_Address;
                                field.Update_On = currentdate;
                            }
                            else if (field.Field_Order == oldOrder - 1)
                            {
                                field.Field_Order += 1;
                                field.Update_By = userlogin.Email_Address;
                                field.Update_On = currentdate;
                            }
                        }

                        db.SaveChanges();

                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Field
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Group,
                    Exception = ex
                };
            }
        }

        public ServiceResult MoveDownTempateField(int? pFieldID)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    int? oldOrder = 0;
                    var move = db.Template_Field.Where(w => w.Field_ID == pFieldID).FirstOrDefault();
                    if (move == null)
                        return new ServiceResult()
                        {
                            Code = ReturnCode.ERROR_DATA_NOT_FOUND,
                            Msg = Error.GetMessage(ReturnCode.ERROR_DATA_NOT_FOUND),
                            Field = Resource.Field
                        };

                    oldOrder = move.Field_Order;
                    var fields = db.Template_Field.OrderBy(o => o.Field_Order);

                    if (oldOrder < fields.Count())
                    {
                        foreach (var field in fields)
                        {
                            if (field.Field_ID == pFieldID)
                            {
                                field.Field_Order += 1;
                                field.Update_By = userlogin.Email_Address;
                                field.Update_On = currentdate;
                            }
                            else if (field.Field_Order == oldOrder + 1)
                            {
                                field.Field_Order -= 1;
                                field.Update_By = userlogin.Email_Address;
                                field.Update_On = currentdate;
                            }
                        }

                        db.SaveChanges();
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Field
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Field,
                    Exception = ex
                };
            }
        }

        #endregion

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
        /*Logsheet*/
        public ServiceResult GetTempateLogsheet(TemplateLogsheetCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    IQueryable<Template_Logsheet> tmps;
                    if (cri != null)
                    {
                        if (cri.include)
                        {
                            tmps = db.Template_Logsheet
                                .Include(i => i.Tmp_Log_Group)
                                .Include(i => i.Tmp_Log_Group.Select(s => s.Template_Group))
                                .Include(i => i.Tmp_Log_Group.Select(s => s.Tmp_Log_Header))
                                .Include(i => i.Tmp_Log_Group.Select(s => s.Tmp_Log_Header.Select(s2 => s2.Template_Header)))
                                .Include(i => i.Tmp_Log_Group.Select(s => s.Tmp_Log_Header.Select(s2 => s2.Tmp_Log_Map)))
                                .Include(i => i.Tmp_Log_Group.Select(s => s.Tmp_Log_Field))
                                .Include(i => i.Tmp_Log_Group.Select(s => s.Tmp_Log_Field.Select(s2 => s2.Template_Field)))
                                .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);
                        }
                        else
                        {
                            tmps = db.Template_Logsheet
                            .Include(i => i.Tmp_Log_Group)
                            .Include(i => i.Tmp_Log_Group.Select(s => s.Tmp_Log_Field))
                            .Include(i => i.Tmp_Log_Group.Select(s => s.Tmp_Log_Header))
                            .Include(i => i.Tmp_Log_Group.Select(s => s.Tmp_Log_Header.Select(s2 => s2.Tmp_Log_Map)))
                            .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);
                        }

                        if (cri.Template_ID.HasValue)
                            tmps = tmps.Where(w => w.Template_ID == cri.Template_ID);

                        if (!string.IsNullOrEmpty(cri.Template_Code))
                            tmps = tmps.Where(w => w.Template_Code.Contains(cri.Template_Code));


                        if (!string.IsNullOrEmpty(cri.Template_Name))
                            tmps = tmps.Where(w => w.Template_Name.Contains(cri.Template_Name));

                        if (cri.Top.HasValue)
                        {
                            tmps = tmps.Take(cri.Top.Value);
                        }
                    }
                    else
                        tmps = db.Template_Logsheet.Where(w => 1 == 1);

                    result.Object = tmps.OrderByDescending(o => o.Template_ID).ToList();
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


        public ServiceResult InsertTempateLogsheet(Template_Logsheet pTmpLogsheet)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pTmpLogsheet.Create_By = userlogin.Email_Address;
                    pTmpLogsheet.Create_On = currentdate;
                    pTmpLogsheet.Update_By = userlogin.Email_Address;
                    pTmpLogsheet.Update_On = currentdate;
                    db.Template_Logsheet.Add(pTmpLogsheet);
                    db.SaveChanges();
                    db.Entry(pTmpLogsheet).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Template_Logsheet
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Template_Logsheet,
                    Exception = ex
                };
            }
        }
        public ServiceResult UpdateTempateLogsheet(Template_Logsheet pTmpLogsheet)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.Template_Logsheet.Where(w => w.Template_ID == pTmpLogsheet.Template_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pTmpLogsheet.Update_By = userlogin.Email_Address;
                        pTmpLogsheet.Update_On = currentdate;
                        /*Delete group*/
                        var tmpGroupIDs = pTmpLogsheet.Tmp_Log_Group.Select(s => s.Tmp_Log_Group_ID);
                        var groupRemove = new List<Tmp_Log_Group>();
                        foreach (var curGroup in current.Tmp_Log_Group)
                        {
                            if (pTmpLogsheet.Tmp_Log_Group == null)
                                groupRemove.Add(curGroup);
                            else if (pTmpLogsheet.Tmp_Log_Group.Count() == 0)
                                groupRemove.Add(curGroup);
                            else if (!tmpGroupIDs.Contains(curGroup.Tmp_Log_Group_ID))
                                groupRemove.Add(curGroup);
                        }

                        if (groupRemove.Count > 0)
                        {
                            foreach (var grow in groupRemove)
                            {
                                foreach (var hdrow in grow.Tmp_Log_Header)
                                {
                                    db.Tmp_Log_Map.RemoveRange(hdrow.Tmp_Log_Map);
                                }
                                db.Tmp_Log_Header.RemoveRange(grow.Tmp_Log_Header);
                            }
                            db.Tmp_Log_Group.RemoveRange(groupRemove);
                        }

                        if (pTmpLogsheet.Tmp_Log_Group == null)
                            pTmpLogsheet.Tmp_Log_Group = new List<Tmp_Log_Group>();

                        var logsheets = db.Logsheets.Where(w => w.Template_ID == current.Template_ID);
                        foreach (var ptmpG in pTmpLogsheet.Tmp_Log_Group)
                        {
                            var curGroup = current.Tmp_Log_Group.Where(w => w.Tmp_Log_Group_ID == ptmpG.Tmp_Log_Group_ID).FirstOrDefault();

                            if (curGroup == null | ptmpG.Tmp_Log_Group_ID == 0)
                            {
                                // add
                                ptmpG.Create_By = userlogin.Email_Address;
                                ptmpG.Create_On = currentdate;
                                ptmpG.Update_By = userlogin.Email_Address;
                                ptmpG.Update_On = currentdate;
                                db.Tmp_Log_Group.Add(ptmpG);
                            }
                            else
                            {
                                /*Delete Header*/
                                var tmpHdrIDs = ptmpG.Tmp_Log_Header.Select(s => s.Tmp_Log_Header_ID).ToList();
                                var hdrRemove = new List<Tmp_Log_Header>();
                                foreach (var currHdr in curGroup.Tmp_Log_Header)
                                {
                                    if (ptmpG.Tmp_Log_Header == null)
                                        hdrRemove.Add(currHdr);
                                    else if (ptmpG.Tmp_Log_Header.Count() == 0)
                                        hdrRemove.Add(currHdr);
                                    else if (!tmpHdrIDs.Contains(currHdr.Tmp_Log_Header_ID))
                                        hdrRemove.Add(currHdr);
                                }

                                if (hdrRemove.Count > 0)
                                {
                                    foreach (var frow in hdrRemove)
                                    {
                                        if (frow.Tmp_Log_Map.Count() > 0)
                                            db.Tmp_Log_Map.RemoveRange(frow.Tmp_Log_Map);
                                    }
                                    db.Tmp_Log_Header.RemoveRange(hdrRemove);
                                }

                                if (ptmpG.Tmp_Log_Header == null)
                                    ptmpG.Tmp_Log_Header = new List<Tmp_Log_Header>();

                                foreach (var ptmpHdr in ptmpG.Tmp_Log_Header)
                                {
                                    var curHdr = curGroup.Tmp_Log_Header.Where(w => w.Tmp_Log_Header_ID == ptmpHdr.Tmp_Log_Header_ID).FirstOrDefault();
                                    if (curHdr == null | ptmpHdr.Tmp_Log_Header_ID == 0)
                                    {
                                        db.Tmp_Log_Header.Add(ptmpHdr);
                                    }
                                    else
                                    {
                                        /*Delete Map*/
                                        var tmpMapIDs = ptmpHdr.Tmp_Log_Map.Select(s => s.Tmp_Log_Map_ID).ToList();
                                        var mapRemove = new List<Tmp_Log_Map>();
                                        foreach (var curMap in curHdr.Tmp_Log_Map)
                                        {
                                            if (ptmpHdr.Tmp_Log_Map == null)
                                                mapRemove.Add(curMap);
                                            else if (ptmpHdr.Tmp_Log_Map.Count() == 0)
                                                mapRemove.Add(curMap);
                                            else if (!tmpMapIDs.Contains(curMap.Tmp_Log_Map_ID))
                                                mapRemove.Add(curMap);
                                        }

                                        if (mapRemove.Count > 0)
                                        {
                                            db.Tmp_Log_Map.RemoveRange(mapRemove);
                                            //foreach (var curMap in mapRemove)
                                            //{
                                            //   var lgmaps = db.Logsheet_Map
                                            //     .Where(w => w.Logsheet_Header.Logsheet_Group.Logsheet.Template_ID == current.Template_ID
                                            //        & w.Logsheet_Header.Logsheet_Group.Group_ID == curGroup.Group_ID
                                            //        & w.Logsheet_Header.Header_ID == curMap.Header_ID
                                            //        & w.Field_ID == curMap.Field_ID
                                            //        & w.Header_ID == curMap.Header_ID);

                                            //   if (lgmaps.Count() > 0)
                                            //      db.Logsheet_Map.RemoveRange(lgmaps);
                                            //}
                                        }


                                        if (ptmpHdr.Tmp_Log_Map == null)
                                            ptmpHdr.Tmp_Log_Map = new List<Tmp_Log_Map>();

                                        foreach (var ptmpMap in ptmpHdr.Tmp_Log_Map)
                                        {
                                            var curMap = curHdr.Tmp_Log_Map.Where(w => w.Tmp_Log_Map_ID == ptmpMap.Tmp_Log_Map_ID).FirstOrDefault();
                                            if (curMap == null | ptmpMap.Tmp_Log_Map_ID == 0)
                                                db.Tmp_Log_Map.Add(ptmpMap);
                                            else
                                            {
                                                if (curMap.Header_ID != ptmpMap.Header_ID | curMap.Field_ID != ptmpMap.Field_ID)
                                                {
                                                    //if (mapRemove.Count > 0 && mapRemove.Where(w => w.Field_ID == curMap.Field_ID & w.Header_ID == curMap.Header_ID).FirstOrDefault() != null)
                                                    //   continue;

                                                    var lgmaps = db.Logsheet_Map.Where(w => w.Logsheet_Header.Logsheet_Group.Logsheet.Template_ID == current.Template_ID
                                                         & w.Logsheet_Header.Logsheet_Group.Group_ID == curGroup.Group_ID
                                                         & w.Logsheet_Header.Header_ID == curMap.Header_ID
                                                         & w.Field_ID == curMap.Field_ID
                                                         & w.Header_ID == curMap.Header_ID);
                                                    foreach (var lgmap in lgmaps)
                                                    {
                                                        lgmap.Header_ID = ptmpMap.Header_ID;
                                                        lgmap.Field_ID = ptmpMap.Field_ID;
                                                    }
                                                }
                                                db.Entry(curMap).CurrentValues.SetValues(ptmpMap);
                                            }


                                        }

                                        var lghdrs = db.Logsheet_Header
                                                 .Where(w => w.Logsheet_Group.Logsheet.Template_ID == current.Template_ID
                                                    & w.Logsheet_Group.Group_ID == curGroup.Group_ID
                                                    & w.Header_ID == curHdr.Header_ID);
                                        foreach (var lghdr in lghdrs)
                                        {
                                            lghdr.Header_ID = ptmpHdr.Header_ID;
                                        }
                                        db.Entry(curHdr).CurrentValues.SetValues(ptmpHdr);
                                    }
                                }

                                /*Delete Field*/
                                var tmpFieldIDs = ptmpG.Tmp_Log_Field.Select(s => s.Tmp_Log_Field_ID).ToList();
                                var fieldRemove = new List<Tmp_Log_Field>();
                                foreach (var curField in curGroup.Tmp_Log_Field)
                                {
                                    if (ptmpG.Tmp_Log_Field == null)
                                        fieldRemove.Add(curField);
                                    else if (ptmpG.Tmp_Log_Field.Count() == 0)
                                        fieldRemove.Add(curField);
                                    else if (!tmpFieldIDs.Contains(curField.Tmp_Log_Field_ID))
                                        fieldRemove.Add(curField);
                                }

                                if (fieldRemove.Count > 0)
                                    db.Tmp_Log_Field.RemoveRange(fieldRemove);

                                if (ptmpG.Tmp_Log_Field == null)
                                    ptmpG.Tmp_Log_Field = new List<Tmp_Log_Field>();

                                foreach (var ptmpField in ptmpG.Tmp_Log_Field)
                                {
                                    var curField = curGroup.Tmp_Log_Field.Where(w => w.Tmp_Log_Field_ID == ptmpField.Tmp_Log_Field_ID).FirstOrDefault();
                                    if (curField == null | ptmpField.Tmp_Log_Field_ID == 0)
                                        db.Tmp_Log_Field.Add(ptmpField);
                                    else
                                    {
                                        db.Entry(curField).CurrentValues.SetValues(ptmpField);
                                    }
                                }


                                ptmpG.Update_By = userlogin.Email_Address;
                                ptmpG.Update_On = currentdate;
                                db.Entry(curGroup).CurrentValues.SetValues(ptmpG);
                            }
                        }
                        db.Entry(current).CurrentValues.SetValues(pTmpLogsheet);
                        db.SaveChanges();
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Template_Logsheet
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Template_Logsheet,
                    Exception = ex
                };
            }
        }

        public ServiceResult DeleteTempateLogsheet(Nullable<int> pTemplateID)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.Template_Logsheet.Where(w => w.Template_ID == pTemplateID).FirstOrDefault();
                    if (current != null)
                    {
                        current.Record_Status = Record_Status.Delete;

                        var curProducts = db.Product_Template.Where(w => w.Template_ID == current.Template_ID);
                        foreach (var curProduct in curProducts)
                        {
                            curProduct.Record_Status = Record_Status.Delete;
                        }
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE),
                        Field = Resource.Template_Logsheet
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_DELETE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
                    Field = Resource.Template_Logsheet,
                    Exception = ex
                };
            }

        }

        /*Product Template*/
        public ServiceResult GetProductTempate(ProductTempateCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    IQueryable<Product_Template> protemps;
                    if (cri.include)
                    {
                        protemps = db.Product_Template
                            .Include(i => i.Template_Logsheet)
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group)
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Template_Group))
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Template_Group.Role))
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Tmp_Log_Field))
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Tmp_Log_Field.Select(s2 => s2.Template_Field)))
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Tmp_Log_Header))
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Tmp_Log_Header.Select(s2 => s2.Template_Header)))
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Tmp_Log_Header.Select(s2 => s2.Tmp_Log_Map)))
                            .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);
                    }
                    else
                        protemps = db.Product_Template.Include(i => i.Template_Logsheet).Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (!string.IsNullOrEmpty(cri.Record_Status))
                            protemps = protemps.Where(w => w.Record_Status == cri.Record_Status);

                        if (cri.Product_Template_ID.HasValue)
                            protemps = protemps.Where(w => w.Product_Template_ID == cri.Product_Template_ID);

                        if (cri.Template_ID.HasValue)
                            protemps = protemps.Where(w => w.Template_ID == cri.Template_ID);

                        if (!string.IsNullOrEmpty(cri.Product_Code))
                            protemps = protemps.Where(w => w.Product_Code.Contains(cri.Product_Code));

                        if (cri.Top.HasValue)
                        {
                            protemps = protemps.Take(cri.Top.Value);
                        }
                    }
                    result.Object = protemps.OrderBy(o => o.Product_Code).ToList();
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

        public ServiceResult InsertProductTempate(Product_Template pProdTmp)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pProdTmp.Create_By = userlogin.Email_Address;
                    pProdTmp.Create_On = currentdate;
                    pProdTmp.Update_By = userlogin.Email_Address;
                    pProdTmp.Update_On = currentdate;
                    SaveProductMaping(db, pProdTmp.Product_Code, pProdTmp.Product_Name);
                    db.Product_Template.Add(pProdTmp);
                    db.SaveChanges();
                    db.Entry(pProdTmp).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Assign_Template_To_Product
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Assign_Template_To_Product,
                    Exception = ex
                };
            }
        }

        public ServiceResult UpdateProductTempate(Product_Template pProdTmp)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var current = db.Product_Template.Where(w => w.Product_Template_ID == pProdTmp.Product_Template_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pProdTmp.Update_By = userlogin.Email_Address;
                        pProdTmp.Update_On = currentdate;
                        SaveProductMaping(db, pProdTmp.Product_Code, pProdTmp.Product_Name);
                        db.Entry(current).CurrentValues.SetValues(pProdTmp);
                        db.SaveChanges();
                    }

                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Assign_Template_To_Product
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Assign_Template_To_Product,
                    Exception = ex
                };
            }
        }

        //public ServiceResult DeleteProductTempate(Nullable<int> pProdTmpID)
        //{
        //   try
        //   {
        //      using (var db = new AgnosDBContext())
        //      {
        //         var current = db.Product_Template.Where(w => w.Product_Template_ID == pProdTmpID).FirstOrDefault();
        //         if (current != null)
        //         {
        //            db.Product_Template.Remove(current);
        //            db.SaveChanges();
        //         }
        //         return new ServiceResult()
        //         {
        //            Code = ReturnCode.SUCCESS,
        //            Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE),
        //            Field = Resource.Assign_Template_To_Product
        //         };
        //      }
        //   }
        //   catch (Exception ex)
        //   {
        //      return new ServiceResult()
        //      {
        //         Code = ReturnCode.ERROR_DELETE,
        //         Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
        //         Field = Resource.Assign_Template_To_Product,
        //         Exception = ex
        //      };
        //   }

        //}

        /*Logsheet*/

        public Logsheet_Product_Form GetLogsheetStatusForm(int? pLogID)
        {
            using (var db = new AgnosDBContext())
            {
                return db.Logsheet_Product_Form.Where(w => w.Logsheet_ID == pLogID).FirstOrDefault();
            }
        }

        public ServiceResult GetLogsheet(LogsheetCriteria cri = null)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    IQueryable<Logsheet> logs;
                    if (cri.include)
                    {
                        logs = db.Logsheets
                            .Include(w => w.User_Profile)
                            .Include(w => w.User_Profile1)
                            .Include(w => w.User_Profile2)
                            .Include(w => w.Global_Lookup_Data)
                            .Include(w => w.Global_Lookup_Data1)
                            .Include(i => i.Template_Logsheet)
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group)
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Template_Group))
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Template_Group.Role))
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Tmp_Log_Header))
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Tmp_Log_Header.Select(s2 => s2.Template_Header)))
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Tmp_Log_Field))
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Tmp_Log_Field.Select(s2 => s2.Template_Field)))
                            .Include(i => i.Template_Logsheet.Tmp_Log_Group.Select(s => s.Tmp_Log_Header.Select(s2 => s2.Tmp_Log_Map)))
                            .Include(i => i.Logsheet_Group)
                            .Include(i => i.Logsheet_Group.Select(s => s.Logsheet_Header))
                            .Include(i => i.Logsheet_Group.Select(s => s.Logsheet_Field))
                            .Include(i => i.Logsheet_Group.Select(s => s.Logsheet_Header.Select(s2 => s2.Logsheet_Map)))
                            .Include(i => i.Upload_Attachment)
                            //.Include(i => i.Upload_Attachment.Any(w=>w.Record_Status != Record_Status.Delete))
                            .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);



                    }
                    else
                        logs = db.Logsheets.Include(i => i.Template_Logsheet).Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    if (cri != null)
                    {
                        if (cri.Logsheet_ID.HasValue)
                            logs = logs.Where(w => w.Logsheet_ID == cri.Logsheet_ID);

                        if (cri.Template_ID.HasValue)
                            logs = logs.Where(w => w.Template_ID == cri.Template_ID);

                        if (!string.IsNullOrEmpty(cri.Product_Code))
                            logs = logs.Where(w => w.Product_Code.Contains(cri.Product_Code));

                        if (!string.IsNullOrEmpty(cri.Lot_No))
                            logs = logs.Where(w => w.Lot_No.Contains(cri.Lot_No));

                        if (!string.IsNullOrEmpty(cri.Template_Code))
                            logs = logs.Include(i => i.Template_Logsheet).Where(w => w.Template_Logsheet.Template_Code.Trim().Contains(cri.Template_Code));

                        if (!string.IsNullOrEmpty(cri.Logsheet_Status))
                        {
                            if (cri.Logsheet_Status == "NA")
                                logs = logs.Where(w => w.Lotgsheet_Status == null);
                            else
                                logs = logs.Where(w => w.Lotgsheet_Status.Contains(cri.Logsheet_Status));
                        }

                        if (cri.Top.HasValue)
                        {
                            logs = logs.Take(cri.Top.Value);
                        }
                    }
                    result.Object = logs.OrderByDescending(o => o.Logsheet_ID).ToList();
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
        public ServiceResult InsertLogsheetProductForm(Logsheet_Product_Form pForm)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    db.Logsheet_Product_Form.Add(pForm);
                    db.SaveChanges();
                    db.Entry(pForm).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Logsheet
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

        public ServiceResult InsertLogsheet(Logsheet pLogsheet)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pLogsheet.Create_By = userlogin.Email_Address;
                    pLogsheet.Create_On = currentdate;
                    pLogsheet.Update_By = userlogin.Email_Address;
                    pLogsheet.Update_On = currentdate;
                    if (!string.IsNullOrEmpty(pLogsheet.Lot_No) && !string.IsNullOrEmpty(pLogsheet.Product_Code))
                    {
                        var lotnumber = db.Lot_Number.Where(w => w.Lot_No == pLogsheet.Lot_No & w.Product_Code == pLogsheet.Product_Code).FirstOrDefault();
                        if (lotnumber == null)
                        {
                            var newlotnumber = new Lot_Number();
                            newlotnumber.Lot_Number_Date = pLogsheet.Create_On;
                            newlotnumber.Lot_No = pLogsheet.Lot_No;
                            newlotnumber.Product_Code = pLogsheet.Product_Code;
                            newlotnumber.Template_ID = pLogsheet.Template_ID;
                            newlotnumber.Create_By = pLogsheet.Create_By;
                            newlotnumber.Create_On = pLogsheet.Create_On;
                            newlotnumber.Update_By = pLogsheet.Update_By;
                            newlotnumber.Update_On = pLogsheet.Update_On;

                            var defDate = newlotnumber.Lot_Number_Date.Value;
                            var defYear = defDate.Year.ToString().Substring(2, 2);
                            var defDay = defDate.Day.ToString("00");
                            var defMonth = DateUtil.GetFullMonth(defDate.Month).Substring(0, 1);
                            var nocnt = NumUtil.ParseInteger(newlotnumber.Lot_No.Replace(newlotnumber.Product_Code + defYear + defMonth + defDay, ""));
                            if (nocnt > 0)
                            {
                                var dupNocnt = db.Lot_Number.Where(w => w.No_Count == nocnt && w.Product_Code == newlotnumber.Product_Code && EntityFunctions.CreateDateTime(w.Lot_Number_Date.Value.Year, w.Lot_Number_Date.Value.Month, w.Lot_Number_Date.Value.Day, 0, 0, 0) == EntityFunctions.CreateDateTime(newlotnumber.Lot_Number_Date.Value.Year, newlotnumber.Lot_Number_Date.Value.Month, newlotnumber.Lot_Number_Date.Value.Day, 0, 0, 0)).FirstOrDefault();
                                if (dupNocnt == null)
                                    newlotnumber.No_Count = nocnt;
                            }
                            db.Lot_Number.Add(newlotnumber);
                        }
                    }
                    db.Logsheets.Add(pLogsheet);
                    SaveProductMaping(db, pLogsheet.Product_Code, pLogsheet.Product_Name);
                    db.SaveChanges();
                    db.Entry(pLogsheet).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Logsheet
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Logsheet,
                    Exception = ex
                };
            }
        }

        public ServiceResult UpdateLogsheet(Logsheet pLogsheet, Logsheet_Product_Form pForm = null)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    if (!string.IsNullOrEmpty(pLogsheet.Lot_No) && !string.IsNullOrEmpty(pLogsheet.Product_Code))
                    {
                        var lotnumber = db.Lot_Number.Where(w => w.Lot_No == pLogsheet.Lot_No & w.Product_Code == pLogsheet.Product_Code).FirstOrDefault();
                        if (lotnumber == null)
                        {
                            var newlotnumber = new Lot_Number();
                            newlotnumber.Lot_Number_Date = pLogsheet.Update_On;
                            newlotnumber.Lot_No = pLogsheet.Lot_No;
                            newlotnumber.Product_Code = pLogsheet.Product_Code;
                            newlotnumber.Template_ID = pLogsheet.Template_ID;
                            newlotnumber.Create_By = pLogsheet.Update_By;
                            newlotnumber.Create_On = pLogsheet.Update_On;
                            newlotnumber.Update_By = pLogsheet.Update_By;
                            newlotnumber.Update_On = pLogsheet.Update_On;

                            var defDate = newlotnumber.Lot_Number_Date.Value;
                            var defYear = defDate.Year.ToString().Substring(2, 2);
                            var defDay = defDate.Day.ToString("00");
                            var defMonth = DateUtil.GetFullMonth(defDate.Month).Substring(0, 1);
                            var nocnt = NumUtil.ParseInteger(newlotnumber.Lot_No.Replace(newlotnumber.Product_Code + defYear + defMonth + defDay, ""));
                            if (nocnt > 0)
                            {
                                var dupNocnt = db.Lot_Number.Where(w => w.No_Count == nocnt && w.Product_Code == newlotnumber.Product_Code && EntityFunctions.CreateDateTime(w.Lot_Number_Date.Value.Year, w.Lot_Number_Date.Value.Month, w.Lot_Number_Date.Value.Day, 0, 0, 0) == EntityFunctions.CreateDateTime(newlotnumber.Lot_Number_Date.Value.Year, newlotnumber.Lot_Number_Date.Value.Month, newlotnumber.Lot_Number_Date.Value.Day, 0, 0, 0)).FirstOrDefault();
                                if (dupNocnt == null)
                                    newlotnumber.No_Count = nocnt;
                            }
                            db.Lot_Number.Add(newlotnumber);
                        }
                    }

                    var current = db.Logsheets.Where(w => w.Logsheet_ID == pLogsheet.Logsheet_ID).FirstOrDefault();
                    if (current != null)
                    {
                        if (pForm != null)
                        {
                            var form = db.Logsheet_Product_Form.Where(w => w.Logsheet_ID == current.Logsheet_ID).FirstOrDefault();
                            if (form != null)
                                db.Logsheet_Product_Form.Remove(form);

                            db.Logsheet_Product_Form.Add(pForm);
                        }

                        pLogsheet.Update_By = userlogin.Email_Address;
                        pLogsheet.Update_On = currentdate;
                        /*Delete group*/
                        var lgGroupIDs = pLogsheet.Logsheet_Group.Select(s => s.Logsheet_Group_ID);
                        var groupRemove = new List<Logsheet_Group>();
                        foreach (var curGroup in current.Logsheet_Group)
                        {
                            if (pLogsheet.Logsheet_Group == null)
                                groupRemove.Add(curGroup);
                            else if (pLogsheet.Logsheet_Group.Count() == 0)
                                groupRemove.Add(curGroup);
                            else if (!lgGroupIDs.Contains(curGroup.Logsheet_Group_ID))
                                groupRemove.Add(curGroup);
                        }

                        if (groupRemove.Count > 0)
                        {
                            foreach (var grow in groupRemove)
                            {
                                foreach (var hrow in grow.Logsheet_Header)
                                {
                                    db.Logsheet_Map.RemoveRange(hrow.Logsheet_Map);
                                }
                                db.Logsheet_Header.RemoveRange(grow.Logsheet_Header);
                                db.Logsheet_Field.RemoveRange(grow.Logsheet_Field);
                            }
                            db.Logsheet_Group.RemoveRange(groupRemove);
                        }

                        if (pLogsheet.Logsheet_Group == null)
                            pLogsheet.Logsheet_Group = new List<Logsheet_Group>();

                        foreach (var plgG in pLogsheet.Logsheet_Group)
                        {
                            var curGroup = current.Logsheet_Group.Where(w => w.Logsheet_Group_ID == plgG.Logsheet_Group_ID).FirstOrDefault();
                            if (curGroup == null || plgG.Logsheet_Group_ID == 0)
                            {
                                // add
                                plgG.Create_By = userlogin.Email_Address;
                                plgG.Create_On = currentdate;
                                plgG.Update_By = userlogin.Email_Address;
                                plgG.Update_On = currentdate;
                                db.Logsheet_Group.Add(plgG);
                            }
                            else
                            {
                                /*Delete Header*/
                                var lgHdrIDs = plgG.Logsheet_Header.Select(s => s.Logsheet_Header_ID);
                                var hdrRemove = new List<Logsheet_Header>();
                                foreach (var currHdr in curGroup.Logsheet_Header)
                                {
                                    if (plgG.Logsheet_Header == null)
                                        hdrRemove.Add(currHdr);
                                    else if (plgG.Logsheet_Header.Count() == 0)
                                        hdrRemove.Add(currHdr);
                                    else if (!lgHdrIDs.Contains(currHdr.Logsheet_Header_ID))
                                        hdrRemove.Add(currHdr);
                                }

                                if (hdrRemove.Count > 0)
                                {
                                    foreach (var hrow in hdrRemove)
                                    {
                                        db.Logsheet_Map.RemoveRange(hrow.Logsheet_Map);
                                    }
                                    db.Logsheet_Header.RemoveRange(hdrRemove);
                                }

                                if (plgG.Logsheet_Header == null)
                                    plgG.Logsheet_Header = new List<Logsheet_Header>();

                                foreach (var plgHdr in plgG.Logsheet_Header)
                                {
                                    var curHdr = curGroup.Logsheet_Header.Where(w => w.Logsheet_Header_ID == plgHdr.Logsheet_Header_ID).FirstOrDefault();
                                    if (curHdr == null || plgHdr.Logsheet_Header_ID == 0)
                                    {
                                        db.Logsheet_Header.Add(plgHdr);
                                    }
                                    else
                                    {
                                        /*Delete Map*/
                                        var lgMapIDs = plgHdr.Logsheet_Map.Select(s => s.Logsheet_Map_ID);
                                        var mapRemove = new List<Logsheet_Map>();
                                        foreach (var curMap in curHdr.Logsheet_Map)
                                        {
                                            if (plgHdr.Logsheet_Map == null)
                                                mapRemove.Add(curMap);
                                            else if (plgHdr.Logsheet_Map.Count() == 0)
                                                mapRemove.Add(curMap);
                                            else if (!lgMapIDs.Contains(curMap.Logsheet_Map_ID))
                                                mapRemove.Add(curMap);
                                        }

                                        if (mapRemove.Count > 0)
                                            db.Logsheet_Map.RemoveRange(mapRemove);

                                        if (plgHdr.Logsheet_Map == null)
                                            plgHdr.Logsheet_Map = new List<Logsheet_Map>();

                                        foreach (var plgMap in plgHdr.Logsheet_Map)
                                        {
                                            var curMap = curHdr.Logsheet_Map.Where(w => w.Logsheet_Map_ID == plgMap.Logsheet_Map_ID).FirstOrDefault();
                                            if (curMap == null || plgMap.Logsheet_Map_ID == 0)
                                                db.Logsheet_Map.Add(plgMap);
                                            else
                                                db.Entry(curMap).CurrentValues.SetValues(plgMap);
                                        }
                                        db.Entry(curHdr).CurrentValues.SetValues(plgHdr);
                                    }

                                }

                                /*Delete Field*/
                                var lgFieldIDs = plgG.Logsheet_Field.Select(s => s.Logsheet_Field_ID);
                                var fieldRemove = new List<Logsheet_Field>();
                                foreach (var currField in curGroup.Logsheet_Field)
                                {
                                    if (plgG.Logsheet_Header == null)
                                        fieldRemove.Add(currField);
                                    else if (plgG.Logsheet_Header.Count() == 0)
                                        fieldRemove.Add(currField);
                                    else if (!lgFieldIDs.Contains(currField.Logsheet_Field_ID))
                                        fieldRemove.Add(currField);
                                }

                                if (fieldRemove.Count > 0)
                                    db.Logsheet_Field.RemoveRange(fieldRemove);

                                if (plgG.Logsheet_Field == null)
                                    plgG.Logsheet_Field = new List<Logsheet_Field>();

                                foreach (var plgField in plgG.Logsheet_Field)
                                {
                                    var curField = curGroup.Logsheet_Field.Where(w => w.Logsheet_Field_ID == plgField.Logsheet_Field_ID).FirstOrDefault();
                                    if (curField == null || plgField.Logsheet_Field_ID == 0)
                                        db.Logsheet_Field.Add(plgField);
                                    else
                                        db.Entry(curField).CurrentValues.SetValues(plgField);
                                }

                                plgG.Update_By = userlogin.Email_Address;
                                plgG.Update_On = currentdate;

                                db.Entry(curGroup).CurrentValues.SetValues(plgG);
                            }
                        }
                        db.Entry(current).CurrentValues.SetValues(pLogsheet);
                        SaveProductMaping(db, pLogsheet.Product_Code, pLogsheet.Product_Name);
                        db.SaveChanges();
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Logsheet
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Logsheet,
                    Exception = ex
                };
            }
        }

        //public ServiceResult DeleteLogsheet(Nullable<int> pLogsheetID)
        //{
        //   try
        //   {
        //      using (var db = new AgnosDBContext())
        //      {
        //         var logsheet = db.Logsheets.Where(w => w.Logsheet_ID == pLogsheetID).FirstOrDefault();
        //         if (logsheet != null)
        //         {
        //            if (logsheet.Logsheet_Group != null)
        //            {
        //               foreach (var group in logsheet.Logsheet_Group)
        //               {
        //                  if (group.Logsheet_Field != null)
        //                  {
        //                     foreach (var field in group.Logsheet_Field)
        //                     {
        //                        db.Logsheet_Header.RemoveRange(field.Logsheet_Header);
        //                     }
        //                     db.Logsheet_Field.RemoveRange(group.Logsheet_Field);
        //                  }
        //               }
        //               db.Logsheet_Group.RemoveRange(logsheet.Logsheet_Group);
        //            }
        //            db.Logsheets.Remove(logsheet);
        //            db.SaveChanges();
        //         }
        //         return new ServiceResult()
        //         {
        //            Code = ReturnCode.SUCCESS,
        //            Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE),
        //            Field = Resource.Logsheet
        //         };
        //      }
        //   }
        //   catch (Exception ex)
        //   {
        //      return new ServiceResult()
        //      {
        //         Code = ReturnCode.ERROR_DELETE,
        //         Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
        //         Field = Resource.Logsheet,
        //         Exception = ex
        //      };
        //   }

        //}

        public ServiceResult GetLotNumber(LotNumberCriteria cri)
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    IQueryable<Lot_Number> lots = db.Lot_Number.Include(w => w.Template_Logsheet).Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);
                    if (cri != null)
                    {
                        if (cri.Lot_Number_ID.HasValue)
                            lots = lots.Where(w => w.Lot_Number_ID == cri.Lot_Number_ID);

                        if (!string.IsNullOrEmpty(cri.Product_Code))
                            lots = lots.Where(w => w.Product_Code.Contains(cri.Product_Code));

                        if (!string.IsNullOrEmpty(cri.Lot_No))
                            lots = lots.Where(w => w.Lot_No.Contains(cri.Lot_No));

                        if (cri.Lot_Number_Date.HasValue)
                            lots = lots.Where(w => EntityFunctions.CreateDateTime(w.Lot_Number_Date.Value.Year, w.Lot_Number_Date.Value.Month, w.Lot_Number_Date.Value.Day, 0, 0, 0) == EntityFunctions.CreateDateTime(cri.Lot_Number_Date.Value.Year, cri.Lot_Number_Date.Value.Month, cri.Lot_Number_Date.Value.Day, 0, 0, 0));


                        if (cri.Top.HasValue)
                        {
                            lots = lots.Take(cri.Top.Value);
                        }
                    }
                    result.Object = lots.OrderByDescending(o => o.Lot_Number_Date).ThenByDescending(o => o.Lot_No).ToList();
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

        public ServiceResult GetLotDailyPlanning()
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var lots = db.Logsheets
                       .Include(w => w.Logsheet_Group)
                       .Include(w => w.Logsheet_Group.Select(s1 => s1.Logsheet_Header))
                       .Include(w => w.Logsheet_Group.Select(s1 => s1.Logsheet_Header.Select(s2 => s2.Template_Header)))
                       .Include(w => w.Logsheet_Group.Select(s1 => s1.Logsheet_Header.Select(s3 => s3.Logsheet_Map)))
                       .Include(w => w.Logsheet_Group.Select(s1 => s1.Logsheet_Header.Select(s3 => s3.Logsheet_Map.Select(s4 => s4.Template_Field))))
                       .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    result.Object = lots.OrderBy(o => o.Product_Code).ThenBy(o => o.Lot_No).ToList();
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


        public ServiceResult GetLotNumberForm()
        {
            var result = new ServiceResult();
            try
            {
                using (var db = new AgnosDBContext())
                {
                    IQueryable<Lot_Number> lots = db.Lot_Number.Include(w => w.Template_Logsheet).Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

                    result.Object = lots.OrderBy(o => o.Product_Code).ThenBy(o => o.Lot_No).ToList();
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

        //public ServiceResult GetLotNumberFormDS()
        //{
        //   var result = new ServiceResult();
        //   try
        //   {
        //      using (var db = new AgnosDBContext())
        //      {
        //         IQueryable<Logsheet> lots = db.Logsheets
        //                 .Include(i => i.Logsheet_Group)
        //                 .Include(i => i.Logsheet_Group.Select(s => s.Logsheet_Header))
        //                 .Include(i => i.Logsheet_Group.Select(s => s.Logsheet_Field))
        //                 .Include(i => i.Logsheet_Group.Select(s => s.Logsheet_Header.Select(s2 => s2.Logsheet_Map)))
        //                 .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

        //         result.Object = lots.OrderBy(o => o.Product_Code).ThenBy(o => o.Lot_No).ToList();
        //         result.Code = ReturnCode.SUCCESS;
        //      }
        //   }
        //   catch (Exception ex)
        //   {
        //      result.Code = ReturnCode.ERROR_DB;
        //      result.Exception = ex;
        //   }
        //   return result;
        //}

        public ServiceResult InsertLotNumber(Lot_Number pLotNumber)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var dup = db.Lot_Number.Where(w => w.Product_Code == pLotNumber.Product_Code && w.Lot_No == pLotNumber.Lot_No && w.Lot_Number_Date == pLotNumber.Lot_Number_Date && w.Record_Status != Record_Status.Delete).FirstOrDefault();
                    if (dup != null && pLotNumber.Product_Code != "0")
                    {
                        return new ServiceResult()
                        {
                            Code = ReturnCode.ERROR_DATA_DUPLICATE,
                            Msg = Error.GetMessage(ReturnCode.ERROR_DATA_DUPLICATE),
                            Field = Resource.Product + " & " + Resource.Lot_No
                        };
                    }
                    pLotNumber.Create_By = userlogin.Email_Address;
                    pLotNumber.Create_On = currentdate;
                    pLotNumber.Update_By = userlogin.Email_Address;
                    pLotNumber.Update_On = currentdate;
                    db.Lot_Number.Add(pLotNumber);
                    db.SaveChanges();
                    db.Entry(pLotNumber).GetDatabaseValues();
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                        Field = Resource.Lot_Number_Generation
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_INSERT,
                    Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
                    Field = Resource.Lot_Number_Generation,
                    Exception = ex
                };
            }
        }

        public ServiceResult UpdateLotNumber(Lot_Number pLotNumber)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    if (pLotNumber.Record_Status != Record_Status.Delete)
                    {
                        var dup = db.Lot_Number.Where(w => w.Product_Code == pLotNumber.Product_Code & w.Lot_No == pLotNumber.Lot_No & w.Lot_Number_ID != pLotNumber.Lot_Number_ID).FirstOrDefault();
                        if (dup != null && pLotNumber.Product_Code != "0")
                        {
                            return new ServiceResult()
                            {
                                Code = ReturnCode.ERROR_DATA_DUPLICATE,
                                Msg = Error.GetMessage(ReturnCode.ERROR_DATA_DUPLICATE),
                                Field = Resource.Product + " & " + Resource.Lot_No
                            };
                        }
                    }

                    var current = db.Lot_Number.Where(w => w.Lot_Number_ID == pLotNumber.Lot_Number_ID).FirstOrDefault();
                    if (current != null)
                    {
                        pLotNumber.Update_By = userlogin.Email_Address;
                        pLotNumber.Update_On = currentdate;
                        db.Entry(current).CurrentValues.SetValues(pLotNumber);
                        db.SaveChanges();

                        if (pLotNumber.Record_Status == Record_Status.Delete)
                        {
                            var currentLog = db.Logsheets.Where(w => w.Lot_No == current.Lot_No && w.Template_ID == current.Template_ID && w.Product_Code == current.Product_Code).FirstOrDefault();
                            if (currentLog != null)
                            {
                                currentLog.Record_Status = Record_Status.Delete;
                                currentLog.Update_By = userlogin.Email_Address;
                                currentLog.Update_On = currentdate;
                                db.Entry(currentLog).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                    return new ServiceResult()
                    {
                        Code = ReturnCode.SUCCESS,
                        Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                        Field = Resource.Lot_Number_Generation
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult()
                {
                    Code = ReturnCode.ERROR_UPDATE,
                    Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
                    Field = Resource.Lot_Number_Generation,
                    Exception = ex
                };
            }
        }

        //public ServiceResult DeleteLotNumber(Nullable<int> pLotNumID)
        //{
        //   try
        //   {
        //      using (var db = new AgnosDBContext())
        //      {
        //         var current = db.Lot_Number.Where(w => w.Lot_Number_ID == pLotNumID).FirstOrDefault();
        //         if (current != null)
        //         {
        //            var currentLog = db.Logsheets.Where(w => w.Lot_No == current.Lot_No && w.Template_ID == current.Template_ID && w.Product_Code == current.Product_Code).FirstOrDefault();
        //            if (currentLog != null)
        //               db.Logsheets.Remove(currentLog);

        //            db.Lot_Number.Remove(current);
        //            db.SaveChanges();
        //         }
        //         return new ServiceResult()
        //         {
        //            Code = ReturnCode.SUCCESS,
        //            Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE),
        //            Field = Resource.Lot_Number_Generation
        //         };
        //      }
        //   }
        //   catch (Exception ex)
        //   {
        //      return new ServiceResult()
        //      {
        //         Code = ReturnCode.ERROR_DELETE,
        //         Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
        //         Field = Resource.Assign_Template_To_Product,
        //         Exception = ex
        //      };
        //   }

        //}

        public Upload_Attachment GetLogsheetAttachment(Nullable<System.Guid> pAttID)
        {
            using (var db = new AgnosDBContext())
            {
                return db.Upload_Attachment.Where(w => w.Attachment_ID == pAttID && w.Record_Status != Record_Status.Delete).FirstOrDefault();
            }

        }

        public ServiceResult InsertLogsheetAttachment(ref Nullable<Guid> pAttID, Nullable<int> pLogID, byte[] file, string filename)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    pAttID = Guid.NewGuid();
                    //Insert
                    var att = new Upload_Attachment();

                    att.Attachment_ID = pAttID.Value;
                    att.Attachment_File = file;
                    att.Logsheet_ID = pLogID;
                    att.Attachment_File_Name = filename;

                    att.Create_On = currentdate;
                    att.Create_By = userlogin.Email_Address;
                    att.Update_On = currentdate;
                    att.Update_By = userlogin.Email_Address;
                    db.Upload_Attachment.Add(att);
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

        public ServiceResult UpdateLogsheetAttachment(Nullable<Guid> pAttID, byte[] file, string filename)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var att = db.Upload_Attachment.Where(w => w.Attachment_ID == pAttID).FirstOrDefault();
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

        public int DeleteLogsheetAttachment(Nullable<System.Guid> pAttachmentID)
        {
            try
            {
                using (var db = new AgnosDBContext())
                {
                    var attach = db.Upload_Attachment.Where(w => w.Attachment_ID == pAttachmentID).FirstOrDefault();
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


    }

}
