using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class CMS_PurgeMap : EntityTypeConfiguration<CMS_Purge>
    {
        public CMS_PurgeMap()
        {
            // Primary Key
            this.HasKey(t => t.Purge_ID);

            // Properties
            this.Property(t => t.Drum_Code)
                .HasMaxLength(150);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Delivery_Status)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("CMS_Purge");
            this.Property(t => t.Purge_ID).HasColumnName("Purge_ID");
            this.Property(t => t.Drum_Code).HasColumnName("Drum_Code");
            this.Property(t => t.Initial_Weight).HasColumnName("Initial_Weight");
            this.Property(t => t.Final_Weight).HasColumnName("Final_Weight");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Delivery_ID).HasColumnName("Delivery_ID");
            this.Property(t => t.Delivery_Status).HasColumnName("Delivery_Status");
        }
    }
}
