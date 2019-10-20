using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Global_Lookup_DataMap : EntityTypeConfiguration<Global_Lookup_Data>
    {
        public Global_Lookup_DataMap()
        {
            // Primary Key
            this.HasKey(t => t.Lookup_Data_ID);

            // Properties
            this.Property(t => t.Data_Code)
                .HasMaxLength(150);

            this.Property(t => t.Name)
                .HasMaxLength(150);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Global_Lookup_Data");
            this.Property(t => t.Lookup_Data_ID).HasColumnName("Lookup_Data_ID");
            this.Property(t => t.Def_ID).HasColumnName("Def_ID");
            this.Property(t => t.Data_Code).HasColumnName("Data_Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");

            // Relationships
            this.HasRequired(t => t.Global_Lookup_Def)
                .WithMany(t => t.Global_Lookup_Data)
                .HasForeignKey(d => d.Def_ID);

        }
    }
}
