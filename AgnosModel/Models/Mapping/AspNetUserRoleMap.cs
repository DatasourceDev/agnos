using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class AspNetUserRoleMap : EntityTypeConfiguration<AspNetUserRole>
    {
        public AspNetUserRoleMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.RoleId });

            // Properties
            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.RoleId)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("AspNetUserRoles");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
        }
    }
}
