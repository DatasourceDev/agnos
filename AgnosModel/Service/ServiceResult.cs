using AppFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgnosModel.Service
{
    public class ServiceResult
    {
        public string Msg { get; set; }
        public ReturnCode Code { get; set; }
        public string Field { get; set; }

        public object Object { get; set; }
        public Exception Exception { get; set; }
        public int Start_Index { get; set; }
        public int Page_Size { get; set; }
    }
}
