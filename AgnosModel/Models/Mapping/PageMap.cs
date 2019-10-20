using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class PageMap : EntityTypeConfiguration<Page>
    {
        public PageMap()
        {
            // Primary Key
            this.HasKey(t => t.Page_ID);

            // Properties
            this.Property(t => t.Page_Code)
                .HasMaxLength(50);

            this.Property(t => t.Page_Name)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Page");
            this.Property(t => t.Page_ID).HasColumnName("Page_ID");
            this.Property(t => t.Page_Code).HasColumnName("Page_Code");
            this.Property(t => t.Page_Name).HasColumnName("Page_Name");
        }
    }
}
