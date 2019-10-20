using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Raw_Material_AttachmentMap : EntityTypeConfiguration<Raw_Material_Attachment>
    {
        public Raw_Material_AttachmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Attachment_ID);

            // Properties
            this.Property(t => t.Attachment_Field)
                .HasMaxLength(50);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Attachment_File_Name)
                .HasMaxLength(300);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Raw_Material_Attachment");
            this.Property(t => t.Attachment_ID).HasColumnName("Attachment_ID");
            this.Property(t => t.Raw_Material_ID).HasColumnName("Raw_Material_ID");
            this.Property(t => t.Attachment_File).HasColumnName("Attachment_File");
            this.Property(t => t.Attachment_Field).HasColumnName("Attachment_Field");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Attachment_File_Name).HasColumnName("Attachment_File_Name");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");

            // Relationships
            this.HasOptional(t => t.Raw_Material)
                .WithMany(t => t.Raw_Material_Attachment)
                .HasForeignKey(d => d.Raw_Material_ID);

        }
    }
}
