using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Template_LogsheetMap : EntityTypeConfiguration<Template_Logsheet>
    {
        public Template_LogsheetMap()
        {
            // Primary Key
            this.HasKey(t => t.Template_ID);

            // Properties
            this.Property(t => t.Template_Code)
                .HasMaxLength(150);

            this.Property(t => t.Template_Name)
                .HasMaxLength(300);

            this.Property(t => t.Template_Description)
                .HasMaxLength(500);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Option_Dropdown_Type)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Template_Logsheet");
            this.Property(t => t.Template_ID).HasColumnName("Template_ID");
            this.Property(t => t.Template_Code).HasColumnName("Template_Code");
            this.Property(t => t.Template_Name).HasColumnName("Template_Name");
            this.Property(t => t.Template_Description).HasColumnName("Template_Description");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Option_Dropdown_Type).HasColumnName("Option_Dropdown_Type");
        }
    }
}
