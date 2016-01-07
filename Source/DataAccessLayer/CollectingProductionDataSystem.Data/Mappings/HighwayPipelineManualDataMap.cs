namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Productions.HighwayPipelines;
    using System.ComponentModel.DataAnnotations.Schema;

    public class HighwayPipelineManualDataMap : EntityTypeConfiguration<HighwayPipelineManualData>
    {
        public HighwayPipelineManualDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("HighwayPipelineManualDatas");
        }
    }
}
