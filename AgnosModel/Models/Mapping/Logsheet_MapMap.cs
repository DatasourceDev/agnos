using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Logsheet_MapMap : EntityTypeConfiguration<Logsheet_Map>
    {
        public Logsheet_MapMap()
        {
            // Primary Key
            this.HasKey(t => t.Logsheet_Map_ID);

            // Properties
            this.Property(t => t.Option_Selected)
                .HasMaxLength(50);

            this.Property(t => t.Text_Display)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Logsheet_Map");
            this.Property(t => t.Logsheet_Map_ID).HasColumnName("Logsheet_Map_ID");
            this.Property(t => t.Logsheet_Header_ID).HasColumnName("Logsheet_Header_ID");
            this.Property(t => t.Field_ID).HasColumnName("Field_ID");
            this.Property(t => t.Header_ID).HasColumnName("Header_ID");
            this.Property(t => t.Option_Selected).HasColumnName("Option_Selected");
            this.Property(t => t.Text_Display).HasColumnName("Text_Display");

            // Relationships
            this.HasOptional(t => t.Logsheet_Header)
                .WithMany(t => t.Logsheet_Map)
                .HasForeignKey(d => d.Logsheet_Header_ID);
            this.HasOptional(t => t.Template_Header)
                .WithMany(t => t.Logsheet_Map)
                .HasForeignKey(d => d.Header_ID);
            this.HasOptional(t => t.Template_Field)
                .WithMany(t => t.Logsheet_Map)
                .HasForeignKey(d => d.Field_ID);

        }
    }
}
