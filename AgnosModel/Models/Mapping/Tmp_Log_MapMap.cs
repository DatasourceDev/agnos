using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Tmp_Log_MapMap : EntityTypeConfiguration<Tmp_Log_Map>
    {
        public Tmp_Log_MapMap()
        {
            // Primary Key
            this.HasKey(t => t.Tmp_Log_Map_ID);

            // Properties
            this.Property(t => t.Option_Selected)
                .HasMaxLength(50);

            this.Property(t => t.Option_Text)
                .HasMaxLength(300);

            this.Property(t => t.Option_Data_Type)
                .HasMaxLength(150);

            this.Property(t => t.Option_Field_From)
                .HasMaxLength(150);

            this.Property(t => t.Option_Range_From)
                .HasMaxLength(50);

            this.Property(t => t.Option_Range_To)
                .HasMaxLength(50);

            this.Property(t => t.Lot_No)
                .HasMaxLength(150);

            this.Property(t => t.Option_Dropdown_Type)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Tmp_Log_Map");
            this.Property(t => t.Tmp_Log_Map_ID).HasColumnName("Tmp_Log_Map_ID");
            this.Property(t => t.Tmp_Log_Header_ID).HasColumnName("Tmp_Log_Header_ID");
            this.Property(t => t.Field_ID).HasColumnName("Field_ID");
            this.Property(t => t.Header_ID).HasColumnName("Header_ID");
            this.Property(t => t.Option_Selected).HasColumnName("Option_Selected");
            this.Property(t => t.Option_Text).HasColumnName("Option_Text");
            this.Property(t => t.Option_Data_Type).HasColumnName("Option_Data_Type");
            this.Property(t => t.Option_Field_From).HasColumnName("Option_Field_From");
            this.Property(t => t.Option_Range_From).HasColumnName("Option_Range_From");
            this.Property(t => t.Option_Range_To).HasColumnName("Option_Range_To");
            this.Property(t => t.Lot_No).HasColumnName("Lot_No");
            this.Property(t => t.Option_Dropdown_Type).HasColumnName("Option_Dropdown_Type");
            this.Property(t => t.Map_Order).HasColumnName("Map_Order");

            // Relationships
            this.HasOptional(t => t.Tmp_Log_Header)
                .WithMany(t => t.Tmp_Log_Map)
                .HasForeignKey(d => d.Tmp_Log_Header_ID);

        }
    }
}
