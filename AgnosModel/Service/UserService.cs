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
using System.Security.Cryptography;

namespace AgnosModel.Service
{
   public class UserCriteria : CriteriaBase
   {
      public Nullable<int> Profile_ID { get; set; }
      public Nullable<int> Role_ID { get; set; }
      public string Email_Address { get; set; }
      public string LDAP { get; set; }

      public bool Email_Notification { get; set; }

   }
   public class UserService : ServiceBase
   {
      public UserService()
      {

      }

      public UserService(User_Profile pUserlogin)
      {
         userlogin = pUserlogin;
      }

      public UserService(string aspID)
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

      public static string hashSHA256(string password)
      {
         SHA256Managed crypt = new SHA256Managed();
         string hash = String.Empty;
         byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
         foreach (byte bit in crypto)
         {
            hash += bit.ToString("x2");
         }
         return hash;
      }

      public ServiceResult ResetPassword(int uID, String PWD)
      {
         using (var db = new AgnosDBContext())
         {
            try
            {
               var user = getUser(uID);
               user.PWD = hashSHA256(PWD);

               db.Entry(user).State = EntityState.Modified;
               db.SaveChanges();

               return new ServiceResult()
               {
                  Code = ReturnCode.SUCCESS,
                  Msg = Success.GetMessage(ReturnCode.SUCCESS),
                  Field = Resource.Reset_Password
               };
            }
            catch (Exception ex)
            {
               return new ServiceResult()
               {
                  Code = ReturnCode.ERROR_SAVE,
                  Msg = Error.GetMessage(ReturnCode.ERROR_SAVE),
                  Field = Resource.Reset_Password,
                  Exception = ex
               };
            }
         }
      }

      public Page_Role ValidatePageRole(string aspID, string pPageCode)
      {
         using (var db = new AgnosDBContext())
         {
            var user = db.User_Profile.Include(i => i.Role).Where(w => w.ApplicationUser_Id == aspID).FirstOrDefault();
            if (user != null)
               return user.Role.Page_Role.Where(w => w.Page.Page_Code == pPageCode).FirstOrDefault();
         }
         return null;
      }

      public List<string> ValidatePageRole(string aspID)
      {
         using (var db = new AgnosDBContext())
         {
            var user = db.User_Profile.Where(w => w.ApplicationUser_Id == aspID).FirstOrDefault();
            if (user != null)
               return user.Role.Page_Role.Where(w => w.View == true && w.Record_Status != Record_Status.Delete).Select(s => s.Page.Page_Code).ToList();
            //return user.Role.Page_Role.Select(s => s.Page.Page_Code).ToList();
         }
         return null;
      }

      public string DefaultPageRole(string aspID)
      {
         using (var db = new AgnosDBContext())
         {
            var user = db.User_Profile.Where(w => w.ApplicationUser_Id == aspID).FirstOrDefault();
            if (user != null)
               return user.Role.Page_Role.Where(w => w.View == true).OrderBy(o => o.Page.Page_Code).Select(s => s.Page.Page_Code).FirstOrDefault();
         }
         return null;
      }

      public ServiceResult InsertUser(User_Profile pProfile)
      {
         try
         {
            var currentdate = StoredProcedure.GetCurrentDate();
            using (var db = new AgnosDBContext())
            {
               if (db.Users.Where(w => w.UserName.Equals(pProfile.Email_Address.ToLower())).FirstOrDefault() != null)
                  return new ServiceResult { Code = ReturnCode.ERROR_DATA_DUPLICATE, Msg = "duplicate " + "Email", Field = "Employee" };

               var guid = Guid.NewGuid().ToString();
               while (db.Users.Where(w => w.Id == guid).FirstOrDefault() != null)
               {
                  guid = Guid.NewGuid().ToString();
               }

               db.Users.Add(new ApplicationUser() { Id = guid, UserName = pProfile.Email_Address.ToLower() });

               pProfile.PWD = hashSHA256(pProfile.PWD);
               pProfile.ApplicationUser_Id = guid;
               pProfile.Create_By = userlogin.Email_Address;
               pProfile.Create_On = currentdate;
               pProfile.Update_By = userlogin.Email_Address;
               pProfile.Update_On = currentdate;

               db.User_Profile.Add(pProfile);
               db.SaveChanges();
               return new ServiceResult()
               {
                  Code = ReturnCode.SUCCESS,
                  Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT),
                  Field = Resource.User
               };

            }

         }
         catch (Exception ex)
         {
            return new ServiceResult()
            {
               Code = ReturnCode.ERROR_INSERT,
               Msg = Error.GetMessage(ReturnCode.ERROR_INSERT),
               Field = Resource.User,
               Exception = ex
            };
         }
      }

      public ServiceResult GetUser(UserCriteria cri = null)
      {
         var result = new ServiceResult();
         try
         {
            using (var db = new AgnosDBContext())
            {
               IQueryable<User_Profile> users;
               users = db.User_Profile
                   .Include(i => i.Role)
              .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);

               if (cri != null)
               {
                  if (cri.Profile_ID.HasValue && cri.Profile_ID.Value > 0)
                     users = users.Where(w => w.Profile_ID == cri.Profile_ID);

                  if (!string.IsNullOrEmpty(cri.LDAP))
                     users = users.Where(w => w.LDAP_Username == cri.LDAP);

                  if (!string.IsNullOrEmpty(cri.Email_Address))
                     users = users.Where(w => w.Email_Address == cri.Email_Address);

                  if(cri.Email_Notification)
                     users = users.Where(w => w.Email_Notification == true);
               }
               result.Object = users.OrderBy(o => o.Email_Address).ToList();
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

      public ServiceResult UpdateUser(User_Profile pProfile)
      {
         try
         {
            using (var db = new AgnosDBContext())
            {
               var users = db.User_Profile.Where(w => w.Profile_ID != pProfile.Profile_ID && w.Email_Address.Equals(pProfile.Email_Address.ToLower())).FirstOrDefault();
               if (users != null)
               {
                  return new ServiceResult()
                  {
                     Code = ReturnCode.ERROR_DATA_DUPLICATE,
                     Msg = Error.GetMessage(ReturnCode.ERROR_DATA_DUPLICATE),
                     Field = Resource.User
                  };
               }

               var current = db.User_Profile.Where(w => w.Profile_ID == pProfile.Profile_ID).FirstOrDefault();
               if (current != null)
               {
                  pProfile.Update_By = userlogin.Email_Address;
                  pProfile.Update_On = currentdate;
                  if (current.Email_Address.ToLower() != pProfile.Email_Address.ToLower())
                  {
                     if (db.Users.Where(w => w.UserName.ToLower() == pProfile.Email_Address.ToLower()).FirstOrDefault() != null)
                        return new ServiceResult { Code = ReturnCode.ERROR_DATA_DUPLICATE, Msg = Error.GetMessage(ReturnCode.ERROR_DATA_DUPLICATE), Field = Resource.User };
                  }

                  var currasp = db.Users.Where(w => w.Id == current.ApplicationUser_Id).FirstOrDefault();
                  if (currasp != null)
                  {
                     if (currasp.UserName != pProfile.Email_Address)
                        currasp.UserName = pProfile.Email_Address;

                     if (pProfile.Record_Status == Record_Status.Delete)
                     {
                        currasp.UserName = "_" + currasp.UserName + "_";
                        pProfile.Activated = false;
                     }
                  }

                  db.Entry(current).CurrentValues.SetValues(pProfile);
                  db.SaveChanges();
               }

               return new ServiceResult()
               {
                  Code = ReturnCode.SUCCESS,
                  Msg = Success.GetMessage(ReturnCode.SUCCESS_UPDATE),
                  Field = Resource.User
               };
            }
         }
         catch (Exception ex)
         {
            return new ServiceResult()
            {
               Code = ReturnCode.ERROR_UPDATE,
               Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE),
               Field = Resource.User,
               Exception = ex
            };
         }
      }

      //public ServiceResult DeleteUser(Nullable<int> pProfileID)
      //{
      //    try
      //    {
      //        using (var db = new AgnosDBContext())
      //        {
      //            var current = db.User_Profile.Where(w => w.Profile_ID == pProfileID).FirstOrDefault();
      //            if (current != null)
      //            {
      //                db.User_Profile.Remove(current);
      //                db.SaveChanges();
      //            }
      //            return new ServiceResult()
      //            {
      //                Code = ReturnCode.SUCCESS,
      //                Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE),
      //                Field = Resource.User
      //            };
      //        }
      //    }
      //    catch (Exception ex)
      //    {
      //        return new ServiceResult()
      //        {
      //            Code = ReturnCode.ERROR_DELETE,
      //            Msg = Error.GetMessage(ReturnCode.ERROR_DELETE),
      //            Field = Resource.User,
      //            Exception = ex
      //        };
      //    }

      //}
      public User_Profile getUserByUsername(String pUName)
      {
         using (var db = new AgnosDBContext())
         {
            User_Profile user = db.User_Profile
                .Where(i => i.LDAP_Username.Equals(pUName))
                .FirstOrDefault();

            return user;
         }
      }

      public User_Profile getUserByEmail(String pEmail)
      {
         using (var db = new AgnosDBContext())
         {
            User_Profile user = db.User_Profile
                .Where(i => i.Email_Address.Equals(pEmail))
                .FirstOrDefault();

            return user;
         }
      }

      public User_Profile getUser(String AspNetUser_ID)
      {
         using (var db = new AgnosDBContext())
         {
            User_Profile user = db.User_Profile
                .Where(i => i.ApplicationUser_Id.Equals(AspNetUser_ID))
                .FirstOrDefault();

            return user;
         }
      }

      public User_Profile getUser(int? pProfileID)
      {
         using (var db = new AgnosDBContext())
         {
            User_Profile user = db.User_Profile.Include(i => i.Role) 
                .Where(i => i.Profile_ID == pProfileID)
                .FirstOrDefault();

            return user;
         }
      }


      public static int LINK_TIME_LIMIT = 120;
      private readonly Random _rng = new Random();
      public const string _chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmopqrstuvwxyz";
      public int sendResetPassword(int Profile_ID, string domain)
      {
         DateTime currentdate = StoredProcedure.GetCurrentDate();
         using (var db = new AgnosDBContext())
         {
            try
            {
               User_Profile user = getUser(Profile_ID);
               //GENERATE ACTIVATION CODE
               String code;
               do
               {
                  code = "R" + randomString(40);
               } while (!validateActivationCode(code));

               Activation_Link activation_link = new Activation_Link()
               {
                  Activation_Code = code,
                  //SET Time_Limit to activate within LINK_TIME_LIMIT hour
                  Time_Limit = currentdate.AddHours(LINK_TIME_LIMIT),
                  Profile_ID = user.Profile_ID
               };

               db.Activation_Link.Add(activation_link);
               db.SaveChanges();
               try
               {
                  //SEND EMAIL
                  //4		System	Send reset password link to user 	
                  EmailTemplete.sendResetPasswordEmail(user.Email_Address, code, user.Name, domain);
               }
               catch
               {
                  return 0;
               }
            }
            catch
            {
               return 0;
            }
         }

         return 1;
      }

      public Boolean validateActivationCode(String code)
      {
         using (var db = new AgnosDBContext())
         {
            Activation_Link u = (from a in db.Activation_Link where a.Activation_Code.Equals(code) select a).FirstOrDefault();
            if (u != null)
               return false;
            else
               return true;
         }
      }

      public string randomString(int size)
      {

         char[] buffer = new char[size];

         for (int i = 0; i < size; i++)
         {
            buffer[i] = _chars[_rng.Next(_chars.Length)];
         }
         return new string(buffer);
      }

      public Activation_Link GetActivationLink(string activationCode)
      {
         using (var db = new AgnosDBContext())
         {
            Activation_Link link = db.Activation_Link
                .Where(w => w.Activation_Code.Equals(activationCode))
                .FirstOrDefault();

            return link;
         }
      }

      public void SetExpireActivationLinkTimeLimit(int link_id)
      {
         DateTime currentdate = StoredProcedure.GetCurrentDate();
         using (var db = new AgnosDBContext())
         {
            Activation_Link link = db.Activation_Link
                .Where(w => w.Activation_ID == link_id)
                .FirstOrDefault();

            link.Time_Limit = currentdate;
            db.Entry(link).State = EntityState.Modified;
            db.SaveChanges();

         }
      }

      public Boolean UpdateLoginAttempt(String email)
      {
         try
         {
            using (var db = new AgnosDBContext())
            {
               User_Profile user = db.User_Profile.Where(i => i.Email_Address.Equals(email.ToLower())).FirstOrDefault();
               if (user != null)
               {
                  user.Login_Attempt = user.Login_Attempt.HasValue ? user.Login_Attempt.Value : 0 + 1;
                  db.Entry(user).State = EntityState.Modified;
                  db.SaveChanges();
               }

               return true;
            }
         }
         catch
         {
            //Log
            return false;
         }
      }

      public Boolean UpdateLastConnection(int Profile_ID)
      {
         try
         {
            DateTime currentdate = StoredProcedure.GetCurrentDate();
            using (var db = new AgnosDBContext())
            {
               User_Profile user = getUser(Profile_ID);
               user.Latest_Connection = currentdate;
               user.Update_By = userlogin.Email_Address;
               db.Entry(user).State = EntityState.Modified;
               db.SaveChanges();

               return true;
            }
         }
         catch
         {
            //Log
            return false;
         }
      }

      public ServiceResult getUsers()
      {
         var result = new ServiceResult();
         try
         {
            using (var db = new AgnosDBContext())
            {
               IQueryable<User_Profile> purges = db.User_Profile
                  .Where(w => 1 == 1 && w.Record_Status != Record_Status.Delete);
               result.Object = purges.OrderBy(o => o.Profile_ID).ToList();
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



   }
}
