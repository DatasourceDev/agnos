using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Logsheet_FieldMap : EntityTypeConfiguration<Logsheet_Field>
    {
        public Logsheet_FieldMap()
        {
            // Primary Key
            this.HasKey(t => t.Logsheet_Field_ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Logsheet_Field");
            this.Property(t => t.Logsheet_Field_ID).HasColumnName("Logsheet_Field_ID");
            this.Property(t => t.Logsheet_Group_ID).HasColumnName("Logsheet_Group_ID");
            this.Property(t => t.Field_ID).HasColumnName("Field_ID");

            // Relationships
            this.HasOptional(t => t.Logsheet_Group)
                .WithMany(t => t.Logsheet_Field)
                .HasForeignKey(d => d.Logsheet_Group_ID);
            this.HasOptional(t => t.Template_Field)
                .WithMany(t => t.Logsheet_Field)
                .HasForeignKey(d => d.Field_ID);

        }
    }
}
