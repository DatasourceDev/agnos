using System;
using System.Collections.Generic;

namespace AgnosModel.Models
{
    public partial class User_Profile
    {
        public User_Profile()
        {
            this.Activation_Link = new List<Activation_Link>();
            this.CMS_Charge = new List<CMS_Charge>();
            this.Logsheets = new List<Logsheet>();
            this.Logsheets1 = new List<Logsheet>();
            this.Logsheets2 = new List<Logsheet>();
            this.Material_Reject = new List<Material_Reject>();
            this.Material_Reject1 = new List<Material_Reject>();
            this.Material_Reject2 = new List<Material_Reject>();
            this.Material_Reject3 = new List<Material_Reject>();
            this.Material_Reject4 = new List<Material_Reject>();
            this.Material_Withdraw = new List<Material_Withdraw>();
            this.Raw_Material = new List<Raw_Material>();
            this.Raw_Material1 = new List<Raw_Material>();
        }

        public int Profile_ID { get; set; }
        public string Email_Address { get; set; }
        public string PWD { get; set; }
        public Nullable<int> Login_Attempt { get; set; }
        public Nullable<bool> Activated { get; set; }
        public string ApplicationUser_Id { get; set; }
        public string Name { get; set; }
        public string Record_Status { get; set; }
        public Nullable<System.DateTime> Latest_Connection { get; set; }
        public string Phone { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_On { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_On { get; set; }
        public Nullable<int> Role_ID { get; set; }
        public string LDAP_Username { get; set; }
        public Nullable<bool> Email_Notification { get; set; }
        public virtual ICollection<Activation_Link> Activation_Link { get; set; }
        public virtual ICollection<CMS_Charge> CMS_Charge { get; set; }
        public virtual ICollection<Logsheet> Logsheets { get; set; }
        public virtual ICollection<Logsheet> Logsheets1 { get; set; }
        public virtual ICollection<Logsheet> Logsheets2 { get; set; }
        public virtual ICollection<Material_Reject> Material_Reject { get; set; }
        public virtual ICollection<Material_Reject> Material_Reject1 { get; set; }
        public virtual ICollection<Material_Reject> Material_Reject2 { get; set; }
        public virtual ICollection<Material_Reject> Material_Reject3 { get; set; }
        public virtual ICollection<Material_Reject> Material_Reject4 { get; set; }
        public virtual ICollection<Material_Withdraw> Material_Withdraw { get; set; }
        public virtual ICollection<Raw_Material> Raw_Material { get; set; }
        public virtual ICollection<Raw_Material> Raw_Material1 { get; set; }
        public virtual Role Role { get; set; }
    }
}
