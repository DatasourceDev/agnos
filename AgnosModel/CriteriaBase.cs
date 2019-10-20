using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgnosModel
{
    public class CriteriaBase
    {
        public int Start_Index { get; set; }
        public int Page_Size { get; set; }
        public Nullable<int> Top { get; set; }
        public string Sort_By { get; set; }
        public bool hasBlank { get; set; }
        public string Record_Status { get; set; }
        public Nullable<DateTime> Update_On { get; set; }
        public string Text_Search { get; set; }
        public bool include { get; set; }


        private bool _queryChild = true;
        public bool QueryChild
        {
            get
            {
                return _queryChild;
            }
            set
            {
                _queryChild = value;
            }
        }

 
    }
}
