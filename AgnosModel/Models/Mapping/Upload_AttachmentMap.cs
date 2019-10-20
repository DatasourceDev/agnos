using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Upload_AttachmentMap : EntityTypeConfiguration<Upload_Attachment>
    {
        public Upload_AttachmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Attachment_ID);

            // Properties
            this.Property(t => t.Attachment_File_Name)
                .HasMaxLength(300);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Upload_Attachment");
            this.Property(t => t.Attachment_ID).HasColumnName("Attachment_ID");
            this.Property(t => t.Logsheet_ID).HasColumnName("Logsheet_ID");
            this.Property(t => t.Attachment_File_Name).HasColumnName("Attachment_File_Name");
            this.Property(t => t.Attachment_File).HasColumnName("Attachment_File");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");

            // Relationships
            this.HasOptional(t => t.Logsheet)
                .WithMany(t => t.Upload_Attachment)
                .HasForeignKey(d => d.Logsheet_ID);

        }
    }
}
