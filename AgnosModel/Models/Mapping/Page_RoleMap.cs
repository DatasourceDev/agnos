using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Page_RoleMap : EntityTypeConfiguration<Page_Role>
    {
        public Page_RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.Page_Role_ID);

            // Properties
            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Page_Role");
            this.Property(t => t.Page_Role_ID).HasColumnName("Page_Role_ID");
            this.Property(t => t.Page_ID).HasColumnName("Page_ID");
            this.Property(t => t.Role_ID).HasColumnName("Role_ID");
            this.Property(t => t.Modify).HasColumnName("Modify");
            this.Property(t => t.View).HasColumnName("View");
            this.Property(t => t.Close).HasColumnName("Close");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");

            // Relationships
            this.HasOptional(t => t.Page)
                .WithMany(t => t.Page_Role)
                .HasForeignKey(d => d.Page_ID);
            this.HasOptional(t => t.Role)
                .WithMany(t => t.Page_Role)
                .HasForeignKey(d => d.Role_ID);

        }
    }
}
