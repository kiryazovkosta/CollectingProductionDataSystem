namespace CollectingProductionDataSystem.Data.Mappings.Configuration
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public static class ModelBingConfig
    {
        internal static void RegisterMappings(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Add(new GlobalTypeMappingConfig());
            modelBuilder.Configurations.Add(new AreaMap());
            modelBuilder.Configurations.Add(new AuditLogRecordMap());
            modelBuilder.Configurations.Add(new DirectionMap());
            modelBuilder.Configurations.Add(new EditReasonMap());
            modelBuilder.Configurations.Add(new FactoryMap());
            modelBuilder.Configurations.Add(new ParkMap());
            modelBuilder.Configurations.Add(new TankConfigMap());
            modelBuilder.Configurations.Add(new TankDataMap());
            modelBuilder.Configurations.Add(new MaterialTypeMap());
            modelBuilder.Configurations.Add(new MeasureUnitMap());
            modelBuilder.Configurations.Add(new PlantMap());
            modelBuilder.Configurations.Add(new ProcessUnitMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new ProductTypeMap());
            modelBuilder.Configurations.Add(new UnitConfigMap());
            modelBuilder.Configurations.Add(new UnitsDataMap());
            modelBuilder.Configurations.Add(new UnitsManualDataMap());
            modelBuilder.Configurations.Add(new IkunkMap());
            modelBuilder.Configurations.Add(new ZoneMap());
            modelBuilder.Configurations.Add(new TransportTypeMap());
            modelBuilder.Configurations.Add(new UnitDailyConfigMap());
            modelBuilder.Configurations.Add(new TankMasterProductMap());
            modelBuilder.Configurations.Add(new UnitsDailyDataMap());
            modelBuilder.Configurations.Add(new UnitsManualDailyDataMap());
            modelBuilder.Configurations.Add(new ApplicationUserMapping());
            modelBuilder.Configurations.Add(new UnitsApprovedDataMap());
            modelBuilder.Configurations.Add(new UnitsApprovedDailyDataMap());
            modelBuilder.Configurations.Add(new ProductionPlanConfigMap());
            modelBuilder.Configurations.Add(new MeasuringPointConfigMap());
            modelBuilder.Configurations.Add(new MeasuringPointConfigsDataMap());
            modelBuilder.Configurations.Add(new MaxAsoMeasuringPointDataSequenceNumberMap());
            modelBuilder.Configurations.Add(new ShiftProductTypeMap());
            modelBuilder.Configurations.Add(new DailyProductTypeMap());
            modelBuilder.Configurations.Add(new ActiveTransactionsDataMap());
            modelBuilder.Configurations.Add(new RelatedMeasuringPointConfigsMap());
            modelBuilder.Configurations.Add(new RelatedTankConfigsMap());
            modelBuilder.Configurations.Add(new RelatedUnitConfigsMap());
            modelBuilder.Configurations.Add(new RelatedUnitDailyConfigMap());
            modelBuilder.Configurations.Add(new UnitConfigUnitDailyConfigMap());
            modelBuilder.Configurations.Add(new ApplicationUserParkMap());
            modelBuilder.Configurations.Add(new ApplicationUserProcessUnitMap());
            modelBuilder.Configurations.Add(new UnitEnteredForCalculationDataMap());
            modelBuilder.Configurations.Add(new MeasuringPointProductsConfigMap());
            modelBuilder.Configurations.Add(new EventMap());
            modelBuilder.Configurations.Add(new LogedInUserMap());
            modelBuilder.Configurations.Add(new ProductionPlanDataMap());
            modelBuilder.Configurations.Add(new MathExpressionMap());
            modelBuilder.Configurations.Add(new MaterialDetailTypeMap());
            modelBuilder.Configurations.Add(new HighwayPipelineConfigMap());
            modelBuilder.Configurations.Add(new HighwayPipelineDataMap());
            modelBuilder.Configurations.Add(new HighwayPipelineManualDataMap());
            modelBuilder.Configurations.Add(new HighwayPipelineApprovedDataMap());
            modelBuilder.Configurations.Add(new InnerPipelineDataMap());
            modelBuilder.Configurations.Add(new InProcessUnitDataMap());
            modelBuilder.Configurations.Add(new MessageMap());
            modelBuilder.Configurations.Add(new MonthlyProductTypeMap());
            modelBuilder.Configurations.Add(new RelatedUnitMonthlyConfigMap());
            modelBuilder.Configurations.Add(new UnitApprovedMonthlyDataMap());
            modelBuilder.Configurations.Add(new UnitDailyConfigUnitMonthlyConfigMap());
            modelBuilder.Configurations.Add(new UnitManualMonthlyDataMap());
            modelBuilder.Configurations.Add(new UnitMonthlyConfigMap());
            modelBuilder.Configurations.Add(new UnitMonthlyDataMap());

        }
    }
}
