using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Tmp_Log_HeaderMap : EntityTypeConfiguration<Tmp_Log_Header>
    {
        public Tmp_Log_HeaderMap()
        {
            // Primary Key
            this.HasKey(t => t.Tmp_Log_Header_ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Tmp_Log_Header");
            this.Property(t => t.Tmp_Log_Header_ID).HasColumnName("Tmp_Log_Header_ID");
            this.Property(t => t.Tmp_Log_Group_ID).HasColumnName("Tmp_Log_Group_ID");
            this.Property(t => t.Header_ID).HasColumnName("Header_ID");
            this.Property(t => t.Header_Order).HasColumnName("Header_Order");
            this.Property(t => t.Sumary_Report_Display).HasColumnName("Sumary_Report_Display");

            // Relationships
            this.HasOptional(t => t.Template_Header)
                .WithMany(t => t.Tmp_Log_Header)
                .HasForeignKey(d => d.Header_ID);
            this.HasOptional(t => t.Tmp_Log_Group)
                .WithMany(t => t.Tmp_Log_Header)
                .HasForeignKey(d => d.Tmp_Log_Group_ID);

        }
    }
}
