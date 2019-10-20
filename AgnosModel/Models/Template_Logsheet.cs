using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Template_Logsheet
    {
        public Template_Logsheet()
        {
            this.Logsheets = new List<Logsheet>();
            this.Lot_Number = new List<Lot_Number>();
            this.Product_Template = new List<Product_Template>();
            this.Tmp_Log_Group = new List<Tmp_Log_Group>();
        }

        public int Template_ID { get; set; }
        public string Template_Code { get; set; }
        public string Template_Name { get; set; }
        public string Template_Description { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Record_Status { get; set; }
        public string Option_Dropdown_Type { get; set; }
        public virtual ICollection<Logsheet> Logsheets { get; set; }
        public virtual ICollection<Lot_Number> Lot_Number { get; set; }
        public virtual ICollection<Product_Template> Product_Template { get; set; }
        public virtual ICollection<Tmp_Log_Group> Tmp_Log_Group { get; set; }
    }
}
