using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Template_HeaderMap : EntityTypeConfiguration<Template_Header>
    {
        public Template_HeaderMap()
        {
            // Primary Key
            this.HasKey(t => t.Header_ID);

            // Properties
            this.Property(t => t.Header_Name)
                .HasMaxLength(500);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Template_Header");
            this.Property(t => t.Header_ID).HasColumnName("Header_ID");
            this.Property(t => t.Header_Name).HasColumnName("Header_Name");
            this.Property(t => t.Header_Order).HasColumnName("Header_Order");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
        }
    }
}
