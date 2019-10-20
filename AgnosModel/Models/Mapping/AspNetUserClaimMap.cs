using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class AspNetUserClaimMap : EntityTypeConfiguration<AspNetUserClaim>
    {
        public AspNetUserClaimMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.User_Id)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("AspNetUserClaims");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ClaimType).HasColumnName("ClaimType");
            this.Property(t => t.ClaimValue).HasColumnName("ClaimValue");
            this.Property(t => t.User_Id).HasColumnName("User_Id");

            // Relationships
            this.HasOptional(t => t.AspNetUser)
                .WithMany(t => t.AspNetUserClaims)
                .HasForeignKey(d => d.User_Id);

        }
    }
}
