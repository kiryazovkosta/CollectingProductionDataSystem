using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class MeasurementPointsProductsConfigMap : EntityTypeConfiguration<MeasurementPointsProductsConfig>
    {
        public MeasurementPointsProductsConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PhdTotalCounterTag)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MeasurementPointsProductsConfigs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PhdTotalCounterTag).HasColumnName("PhdTotalCounterTag");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.MeasurementPointId).HasColumnName("MeasurementPointId");
            this.Property(t => t.DirectionId).HasColumnName("DirectionId");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.DeletedOn).HasColumnName("DeletedOn");
            this.Property(t => t.DeletedFrom).HasColumnName("DeletedFrom");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");

            // Relationships
            this.HasRequired(t => t.Direction)
                .WithMany(t => t.MeasurementPointsProductsConfigs)
                .HasForeignKey(d => d.DirectionId);
            this.HasRequired(t => t.MeasurementPoint)
                .WithMany(t => t.MeasurementPointsProductsConfigs)
                .HasForeignKey(d => d.MeasurementPointId);
            this.HasRequired(t => t.Product)
                .WithMany(t => t.MeasurementPointsProductsConfigs)
                .HasForeignKey(d => d.ProductId);

        }
    }
}
