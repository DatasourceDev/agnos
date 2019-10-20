using AgnosModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AgnosModel.Service
{
    public class StoredProcedure
    {
        public static DateTime GetCurrentDate()
        {
            using (var db = new AgnosDBContext())
            {
                var d = db.Database.SqlQuery<DateTime>("select distinct getdate() from sys.databases").ToList();
                if (d.Count > 0)
                {
                    return d[0];
                }
            }
            return new DateTime();
        }
    }
}