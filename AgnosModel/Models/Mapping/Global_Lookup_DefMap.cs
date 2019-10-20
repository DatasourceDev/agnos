using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class Global_Lookup_DefMap : EntityTypeConfiguration<Global_Lookup_Def>
    {
        public Global_Lookup_DefMap()
        {
            // Primary Key
            this.HasKey(t => t.Def_ID);

            // Properties
            this.Property(t => t.Def_Code)
                .HasMaxLength(150);

            this.Property(t => t.Name)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Global_Lookup_Def");
            this.Property(t => t.Def_ID).HasColumnName("Def_ID");
            this.Property(t => t.Def_Code).HasColumnName("Def_Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
        }
    }
}
