using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Running_Number_ConfigMap : EntityTypeConfiguration<Running_Number_Config>
    {
        public Running_Number_ConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Running_Number_Config_ID);

            // Properties
            this.Property(t => t.Running_Number_Type)
                .HasMaxLength(50);

            this.Property(t => t.Prefix_Ref_No)
                .HasMaxLength(50);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Running_Number_Config");
            this.Property(t => t.Running_Number_Config_ID).HasColumnName("Running_Number_Config_ID");
            this.Property(t => t.Running_Number_Type).HasColumnName("Running_Number_Type");
            this.Property(t => t.Number_Of_Digit).HasColumnName("Number_Of_Digit");
            this.Property(t => t.Prefix_Ref_No).HasColumnName("Prefix_Ref_No");
            this.Property(t => t.Ref_Count).HasColumnName("Ref_Count");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Running_Year).HasColumnName("Running_Year");
            
        }
    }
}
