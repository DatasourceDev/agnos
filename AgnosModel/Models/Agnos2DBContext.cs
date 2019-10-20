using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using AgnosModel.Models.Mapping;

namespace AgnosModel.Models
{
    public partial class Agnos2DBContext : DbContext
    {
        static Agnos2DBContext()
        {
            Database.SetInitializer<Agnos2DBContext>(null);
        }

        public Agnos2DBContext()
            : base("Name=Agnos2DBContext")
        {
        }

        public DbSet<Activation_Link> Activation_Link { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<CMS_Charge> CMS_Charge { get; set; }
        public DbSet<CMS_Charging_Control> CMS_Charging_Control { get; set; }
        public DbSet<CMS_Delivery> CMS_Delivery { get; set; }
        public DbSet<CMS_Delivery_Detail> CMS_Delivery_Detail { get; set; }
        public DbSet<CMS_Drum_Control> CMS_Drum_Control { get; set; }
        public DbSet<CMS_Drum_Type> CMS_Drum_Type { get; set; }
        public DbSet<CMS_Filling_Station> CMS_Filling_Station { get; set; }
        public DbSet<CMS_Format> CMS_Format { get; set; }
        public DbSet<CMS_Product> CMS_Product { get; set; }
        public DbSet<CMS_Purge> CMS_Purge { get; set; }
        public DbSet<CMS_Skip_Purging> CMS_Skip_Purging { get; set; }
        public DbSet<Global_Lookup_Data> Global_Lookup_Data { get; set; }
        public DbSet<Global_Lookup_Def> Global_Lookup_Def { get; set; }
        public DbSet<Logsheet> Logsheets { get; set; }
        public DbSet<Logsheet_Field> Logsheet_Field { get; set; }
        public DbSet<Logsheet_Group> Logsheet_Group { get; set; }
        public DbSet<Logsheet_Header> Logsheet_Header { get; set; }
        public DbSet<Logsheet_Map> Logsheet_Map { get; set; }
        public DbSet<Logsheet_Product_Form> Logsheet_Product_Form { get; set; }
        public DbSet<Lot_Number> Lot_Number { get; set; }
        public DbSet<Material_Reject> Material_Reject { get; set; }
        public DbSet<Material_Reject_Form> Material_Reject_Form { get; set; }
        public DbSet<Material_Withdraw> Material_Withdraw { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Page_Role> Page_Role { get; set; }
        public DbSet<Product_Mapping> Product_Mapping { get; set; }
        public DbSet<Product_Template> Product_Template { get; set; }
        public DbSet<Raw_Material> Raw_Material { get; set; }
        public DbSet<Raw_Material_Attachment> Raw_Material_Attachment { get; set; }
        public DbSet<Raw_Material_Form> Raw_Material_Form { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Running_Number_Config> Running_Number_Config { get; set; }
        public DbSet<Template_Field> Template_Field { get; set; }
        public DbSet<Template_Group> Template_Group { get; set; }
        public DbSet<Template_Header> Template_Header { get; set; }
        public DbSet<Template_Logsheet> Template_Logsheet { get; set; }
        public DbSet<Tmp_Log_Field> Tmp_Log_Field { get; set; }
        public DbSet<Tmp_Log_Group> Tmp_Log_Group { get; set; }
        public DbSet<Tmp_Log_Header> Tmp_Log_Header { get; set; }
        public DbSet<Tmp_Log_Map> Tmp_Log_Map { get; set; }
        public DbSet<Upload_Attachment> Upload_Attachment { get; set; }
        public DbSet<User_Profile> User_Profile { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Activation_LinkMap());
            modelBuilder.Configurations.Add(new AspNetRoleMap());
            modelBuilder.Configurations.Add(new AspNetUserClaimMap());
            modelBuilder.Configurations.Add(new AspNetUserLoginMap());
            modelBuilder.Configurations.Add(new AspNetUserRoleMap());
            modelBuilder.Configurations.Add(new AspNetUserMap());
            modelBuilder.Configurations.Add(new CMS_ChargeMap());
            modelBuilder.Configurations.Add(new CMS_Charging_ControlMap());
            modelBuilder.Configurations.Add(new CMS_DeliveryMap());
            modelBuilder.Configurations.Add(new CMS_Delivery_DetailMap());
            modelBuilder.Configurations.Add(new CMS_Drum_ControlMap());
            modelBuilder.Configurations.Add(new CMS_Drum_TypeMap());
            modelBuilder.Configurations.Add(new CMS_Filling_StationMap());
            modelBuilder.Configurations.Add(new CMS_FormatMap());
            modelBuilder.Configurations.Add(new CMS_ProductMap());
            modelBuilder.Configurations.Add(new CMS_PurgeMap());
            modelBuilder.Configurations.Add(new CMS_Skip_PurgingMap());
            modelBuilder.Configurations.Add(new Global_Lookup_DataMap());
            modelBuilder.Configurations.Add(new Global_Lookup_DefMap());
            modelBuilder.Configurations.Add(new LogsheetMap());
            modelBuilder.Configurations.Add(new Logsheet_FieldMap());
            modelBuilder.Configurations.Add(new Logsheet_GroupMap());
            modelBuilder.Configurations.Add(new Logsheet_HeaderMap());
            modelBuilder.Configurations.Add(new Logsheet_MapMap());
            modelBuilder.Configurations.Add(new Logsheet_Product_FormMap());
            modelBuilder.Configurations.Add(new Lot_NumberMap());
            modelBuilder.Configurations.Add(new Material_RejectMap());
            modelBuilder.Configurations.Add(new Material_Reject_FormMap());
            modelBuilder.Configurations.Add(new Material_WithdrawMap());
            modelBuilder.Configurations.Add(new PageMap());
            modelBuilder.Configurations.Add(new Page_RoleMap());
            modelBuilder.Configurations.Add(new Product_MappingMap());
            modelBuilder.Configurations.Add(new Product_TemplateMap());
            modelBuilder.Configurations.Add(new Raw_MaterialMap());
            modelBuilder.Configurations.Add(new Raw_Material_AttachmentMap());
            modelBuilder.Configurations.Add(new Raw_Material_FormMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new Running_Number_ConfigMap());
            modelBuilder.Configurations.Add(new Template_FieldMap());
            modelBuilder.Configurations.Add(new Template_GroupMap());
            modelBuilder.Configurations.Add(new Template_HeaderMap());
            modelBuilder.Configurations.Add(new Template_LogsheetMap());
            modelBuilder.Configurations.Add(new Tmp_Log_FieldMap());
            modelBuilder.Configurations.Add(new Tmp_Log_GroupMap());
            modelBuilder.Configurations.Add(new Tmp_Log_HeaderMap());
            modelBuilder.Configurations.Add(new Tmp_Log_MapMap());
            modelBuilder.Configurations.Add(new Upload_AttachmentMap());
            modelBuilder.Configurations.Add(new User_ProfileMap());
        }
    }
}
