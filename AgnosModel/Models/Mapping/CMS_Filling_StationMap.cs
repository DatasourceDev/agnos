using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class CMS_Filling_StationMap : EntityTypeConfiguration<CMS_Filling_Station>
    {
        public CMS_Filling_StationMap()
        {
            // Primary Key
            this.HasKey(t => t.Filling_Station_ID);

            // Properties
            this.Property(t => t.Station_Code)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("CMS_Filling_Station");
            this.Property(t => t.Filling_Station_ID).HasColumnName("Filling_Station_ID");
            this.Property(t => t.Station_Code).HasColumnName("Station_Code");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
        }
    }
}
