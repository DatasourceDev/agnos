using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Template_FieldMap : EntityTypeConfiguration<Template_Field>
    {
        public Template_FieldMap()
        {
            // Primary Key
            this.HasKey(t => t.Field_ID);

            // Properties
            this.Property(t => t.Field_Name)
                .HasMaxLength(500);

            this.Property(t => t.Data_Type)
                .HasMaxLength(150);

            this.Property(t => t.Field_From)
                .HasMaxLength(150);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Template_Field");
            this.Property(t => t.Field_ID).HasColumnName("Field_ID");
            this.Property(t => t.Field_Name).HasColumnName("Field_Name");
            this.Property(t => t.Field_Order).HasColumnName("Field_Order");
            this.Property(t => t.Data_Type).HasColumnName("Data_Type");
            this.Property(t => t.Field_From).HasColumnName("Field_From");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
        }
    }
}
