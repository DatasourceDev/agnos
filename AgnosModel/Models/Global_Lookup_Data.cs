using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class Global_Lookup_Data
    {
        public Global_Lookup_Data()
        {
            this.Logsheets = new List<Logsheet>();
            this.Logsheets1 = new List<Logsheet>();
            this.Material_Reject = new List<Material_Reject>();
            this.Material_Reject1 = new List<Material_Reject>();
            this.Material_Withdraw = new List<Material_Withdraw>();
            this.Material_Withdraw1 = new List<Material_Withdraw>();
            this.Raw_Material = new List<Raw_Material>();
            this.Raw_Material1 = new List<Raw_Material>();
        }

        public int Lookup_Data_ID { get; set; }
        public int Def_ID { get; set; }
        public string Data_Code { get; set; }
        public string Name { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public string Record_Status { get; set; }
        public virtual Global_Lookup_Def Global_Lookup_Def { get; set; }
        public virtual ICollection<Logsheet> Logsheets { get; set; }
        public virtual ICollection<Logsheet> Logsheets1 { get; set; }
        public virtual ICollection<Material_Reject> Material_Reject { get; set; }
        public virtual ICollection<Material_Reject> Material_Reject1 { get; set; }
        public virtual ICollection<Material_Withdraw> Material_Withdraw { get; set; }
        public virtual ICollection<Material_Withdraw> Material_Withdraw1 { get; set; }
        public virtual ICollection<Raw_Material> Raw_Material { get; set; }
        public virtual ICollection<Raw_Material> Raw_Material1 { get; set; }
    }
}
