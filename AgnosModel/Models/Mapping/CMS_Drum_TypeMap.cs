using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class CMS_Drum_TypeMap : EntityTypeConfiguration<CMS_Drum_Type>
    {
        public CMS_Drum_TypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Drum_Type_ID);

            // Properties
            this.Property(t => t.Drum_Type)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("CMS_Drum_Type");
            this.Property(t => t.Drum_Type_ID).HasColumnName("Drum_Type_ID");
            this.Property(t => t.Drum_Type).HasColumnName("Drum_Type");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
        }
    }
}
