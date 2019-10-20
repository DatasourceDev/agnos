using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AgnosModel.Models.Mapping
{
    public class CMS_DeliveryMap : EntityTypeConfiguration<CMS_Delivery>
    {
        public CMS_DeliveryMap()
        {
            // Primary Key
            this.HasKey(t => t.Delivery_ID);

            // Properties
            this.Property(t => t.Delivery_Order_No)
                .HasMaxLength(150);

            this.Property(t => t.Create_By)
                .HasMaxLength(150);

            this.Property(t => t.Update_By)
                .HasMaxLength(150);

            this.Property(t => t.Record_Status)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("CMS_Delivery");
            this.Property(t => t.Delivery_ID).HasColumnName("Delivery_ID");
            this.Property(t => t.Delivery_Order_No).HasColumnName("Delivery_Order_No");
            this.Property(t => t.Create_By).HasColumnName("Create_By");
            this.Property(t => t.Create_On).HasColumnName("Create_On");
            this.Property(t => t.Update_By).HasColumnName("Update_By");
            this.Property(t => t.Update_On).HasColumnName("Update_On");
            this.Property(t => t.Record_Status).HasColumnName("Record_Status");
            this.Property(t => t.Completed).HasColumnName("Completed");
        }
    }
}
