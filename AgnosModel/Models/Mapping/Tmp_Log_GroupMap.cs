using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Tmp_Log_GroupMap : EntityTypeConfiguration<Tmp_Log_Group>
    {
        public Tmp_Log_GroupMap()
        {
            // Primary Key
            this.HasKey(t => t.Tmp_Log_Group_ID);

            // Properties
            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Sub_Group_Name)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("Tmp_Log_Group");
            this.Property(t => t.Tmp_Log_Group_ID).HasColumnName("Tmp_Log_Group_ID");
            this.Property(t => t.Template_ID).HasColumnName("Template_ID");
            this.Property(t => t.Group_ID).HasColumnName("Group_ID");
            this.Property(t => t.Group_Order).HasColumnName("Group_Order");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Sub_Group_Name).HasColumnName("Sub_Group_Name");

            // Relationships
            this.HasOptional(t => t.Template_Group)
                .WithMany(t => t.Tmp_Log_Group)
                .HasForeignKey(d => d.Group_ID);
            this.HasOptional(t => t.Template_Logsheet)
                .WithMany(t => t.Tmp_Log_Group)
                .HasForeignKey(d => d.Template_ID);

        }
    }
}
