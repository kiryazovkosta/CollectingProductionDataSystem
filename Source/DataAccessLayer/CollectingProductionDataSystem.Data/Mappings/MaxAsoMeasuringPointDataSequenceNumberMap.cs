namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.Transactions;
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;

    class MaxAsoMeasuringPointDataSequenceNumberMap : EntityTypeConfiguration<MaxAsoMeasuringPointDataSequenceNumber>
    {
        public MaxAsoMeasuringPointDataSequenceNumberMap()
        {
             // Primary Key
            this.HasKey(t => t.Id);

            // Properties

            // Table & Column Mappings
            this.ToTable("MaxAsoMeasuringPointDataSequenceNumber");
        }
    }
}
