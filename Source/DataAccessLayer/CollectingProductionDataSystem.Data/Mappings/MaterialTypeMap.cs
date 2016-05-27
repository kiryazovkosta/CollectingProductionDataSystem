namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public class MaterialTypeMap : EntityTypeConfiguration<MaterialType>
    {
        public MaterialTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            //Relations
            this.HasMany(x => x.UnitDailyConfigs)
                .WithOptional(x => x.MaterialType)
                .HasForeignKey(y => y.MaterialTypeId);

            this.HasMany(x => x.MaterialDetailTypes)
                .WithOptional()
                .HasForeignKey(y => y.MaterialTypeId);

            this.HasMany(x => x.ProductionPlanConfigs)
                .WithOptional(x => x.MaterialType)
                .HasForeignKey(y => y.MaterialTypeId);

            // Not Mapped Properties
            // TODO: Remove this when Production Plan rewriting is finished
            this.Ignore(t => t.SortableName);

            // Table & Column Mappings
            this.ToTable("MaterialTypes");
            this.Property(t => t.Id).HasColumnName("Id");
        }
    }
}
