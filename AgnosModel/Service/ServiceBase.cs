using AgnosModel.Models;
//using Enterprise01;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgnosModel.Service
{
   public class ServiceBase
   {

      private User_Profile _userlogin;

      protected User_Profile userlogin
      {
         get
         {
            if (_userlogin == null)
               _userlogin = new User_Profile();

            return _userlogin;
         }
         set
         {
            _userlogin = value;
         }
      }

      private Nullable<DateTime> _currentdate = null;

      protected DateTime currentdate
      {
         get
         {
            if (!_currentdate.HasValue)
               _currentdate = StoredProcedure.GetCurrentDate();

            return _currentdate.Value;
         }
      }

   }
}
