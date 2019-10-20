using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Logsheet_HeaderMap : EntityTypeConfiguration<Logsheet_Header>
    {
        public Logsheet_HeaderMap()
        {
            // Primary Key
            this.HasKey(t => t.Logsheet_Header_ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Logsheet_Header");
            this.Property(t => t.Logsheet_Header_ID).HasColumnName("Logsheet_Header_ID");
            this.Property(t => t.Header_ID).HasColumnName("Header_ID");
            this.Property(t => t.Logsheet_Group_ID).HasColumnName("Logsheet_Group_ID");

            // Relationships
            this.HasOptional(t => t.Logsheet_Group)
                .WithMany(t => t.Logsheet_Header)
                .HasForeignKey(d => d.Logsheet_Group_ID);
            this.HasOptional(t => t.Template_Header)
                .WithMany(t => t.Logsheet_Header)
                .HasForeignKey(d => d.Header_ID);

        }
    }
}
