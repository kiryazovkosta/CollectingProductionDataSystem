namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Productions.Daily;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    public class RelatedUnitDailyConfigAspenCodeMap : EntityTypeConfiguration<RelatedUnitDailyConfigAspenCode>
    {
        public RelatedUnitDailyConfigAspenCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

             this.Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.UnitDailyConfigId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("Ix_unit_daily_conf_aspen_code", 1)))
                .IsRequired();

            this.Property(u => u.AspenCode)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("Ix_unit_daily_conf_aspen_code", 2)))
                .IsRequired();

            // Properties
            this.Property(t => t.AspenCode)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("UnitDailyConfigs2AspenCodes");
        }
    }
}
