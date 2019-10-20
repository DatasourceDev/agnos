using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // Primary Key
            this.HasKey(t => t.Role_ID);

            // Properties
            this.Property(t => t.Role_Name)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("Role");
            this.Property(t => t.Role_ID).HasColumnName("Role_ID");
            this.Property(t => t.Role_Name).HasColumnName("Role_Name");
        }
    }
}
