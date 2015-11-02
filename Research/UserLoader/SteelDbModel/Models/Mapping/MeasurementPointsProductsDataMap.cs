using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class MeasurementPointsProductsDataMap : EntityTypeConfiguration<MeasurementPointsProductsData>
    {
        public MeasurementPointsProductsDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("MeasurementPointsProductsDatas");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RecordTimestamp).HasColumnName("RecordTimestamp");
            this.Property(t => t.MeasurementPointsProductsConfigId).HasColumnName("MeasurementPointsProductsConfigId");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");

            // Relationships
            this.HasRequired(t => t.MeasurementPointsProductsConfig)
                .WithMany(t => t.MeasurementPointsProductsDatas)
                .HasForeignKey(d => d.MeasurementPointsProductsConfigId);

        }
    }
}
