using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Tmp_Log_FieldMap : EntityTypeConfiguration<Tmp_Log_Field>
    {
        public Tmp_Log_FieldMap()
        {
            // Primary Key
            this.HasKey(t => t.Tmp_Log_Field_ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Tmp_Log_Field");
            this.Property(t => t.Tmp_Log_Field_ID).HasColumnName("Tmp_Log_Field_ID");
            this.Property(t => t.Tmp_Log_Group_ID).HasColumnName("Tmp_Log_Group_ID");
            this.Property(t => t.Field_ID).HasColumnName("Field_ID");
            this.Property(t => t.Field_Order).HasColumnName("Field_Order");

            // Relationships
            this.HasOptional(t => t.Template_Field)
                .WithMany(t => t.Tmp_Log_Field)
                .HasForeignKey(d => d.Field_ID);
            this.HasOptional(t => t.Tmp_Log_Group)
                .WithMany(t => t.Tmp_Log_Field)
                .HasForeignKey(d => d.Tmp_Log_Group_ID);

        }
    }
}
