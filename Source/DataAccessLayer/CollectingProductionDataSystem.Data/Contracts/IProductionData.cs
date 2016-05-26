namespace CollectingProductionDataSystem.Data.Contracts
{
    using System;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;
    using CollectingProductionDataSystem.Models.Productions.Qpt;
    using CollectingProductionDataSystem.Models.SystemLog;
    using CollectingProductionDataSystem.Models.Transactions;
    using CollectingProductionDataSystem.Models.Transactions.HighwayPipelines;
    using CollectingProductionDataSystem.Models.UtilityEntities;

    public interface IProductionData : IDisposable
    {
        IDbContext Context { get; }

        IDeletableEntityRepository<Plant> Plants { get; }

        IDeletableEntityRepository<Factory> Factories { get; }

        IDeletableEntityRepository<ProcessUnit> ProcessUnits { get; }

        IDeletableEntityRepository<UnitConfig> UnitConfigs { get; }

        IApprovableEntityRepository<UnitsData> UnitsData { get; }

        IDeletableEntityRepository<UnitsManualData> UnitsManualData { get; }

        IApprovableEntityRepository<UnitsDailyData> UnitsDailyDatas { get; }

        IApprovableEntityRepository<UnitMonthlyData> UnitMonthlyDatas { get; }

        IDeletableEntityRepository<UnitsManualDailyData> UnitsManualDailyDatas { get; }

        IDeletableEntityRepository<UnitManualMonthlyData> UnitManualMonthlyDatas { get; }


        IDeletableEntityRepository<Area> Areas { get; }

        IDeletableEntityRepository<Park> Parks { get; }

        IDeletableEntityRepository<TankConfig> Tanks { get; }

        IDeletableEntityRepository<UnitDailyConfig> UnitsDailyConfigs { get; }

        IDeletableEntityRepository<UnitMonthlyConfig> UnitMonthlyConfigs { get; }

        IApprovableEntityRepository<TankData> TanksData { get; }

        IDeletableEntityRepository<MeasuringPointProductsData> MeasurementPointsProductsDatas { get; }

        IDeletableEntityRepository<Product> Products { get; }

        IDeletableEntityRepository<ProductType> ProductTypes { get; }

        IDeletableEntityRepository<ShiftProductType> ShiftProductTypes { get; }

        IDeletableEntityRepository<DailyProductType> DailyProductTypes { get; }

        IDeletableEntityRepository<Ikunk> Ikunks { get; }

        IDeletableEntityRepository<Zone> Zones { get; }

        IDeletableEntityRepository<TransportType> TransportTypes { get; }

        IDeletableEntityRepository<EditReason> EditReasons { get; }

        IDeletableEntityRepository<TankMasterProduct> TankMasterProducts { get; }

        IDeletableEntityRepository<UnitsApprovedData> UnitsApprovedDatas { get; }

        IDeletableEntityRepository<ApplicationUser> Users { get; }

        IDeletableEntityRepository<ApplicationRole> Roles { get; }

        IDeletableEntityRepository<UnitsApprovedDailyData> UnitsApprovedDailyDatas { get; }

        IDeletableEntityRepository<UnitApprovedMonthlyData> UnitApprovedMonthlyDatas { get; }


        IDeletableEntityRepository<ProductionPlanConfig> ProductionPlanConfigs { get; }

        IDeletableEntityRepository<MeasuringPointConfig> MeasuringPointConfigs { get; }

        IDeletableEntityRepository<MeasuringPointsConfigsData> MeasuringPointsConfigsDatas { get; }

        IDeletableEntityRepository<MaxAsoMeasuringPointDataSequenceNumber> MaxAsoMeasuringPointDataSequenceNumberMap { get; }

        IDeletableEntityRepository<Direction> Directions { get; }

        IDeletableEntityRepository<MaterialType> MaterialTypes { get; }

        IDeletableEntityRepository<MeasureUnit> MeasureUnits { get; }

        IDeletableEntityRepository<MaterialDetailType> MaterialDetailTypes { get; }

        IDeletableEntityRepository<MonthlyReportType> MonthlyReportTypes { get; }

        IDeletableEntityRepository<ActiveTransactionsData> ActiveTransactionsDatas { get; }

        IDeletableEntityRepository<PhdConfig> PhdConfigs { get; }

        IDeletableEntityRepository<PlanNorm> PlanNorms { get; }

        IDeletableEntityRepository<PlanValue> PlanValues { get; }

        IRepository<UnitConfigUnitDailyConfig> UnitConfigUnitDailyConfigs { get; }

        IRepository<UnitDailyConfigUnitMonthlyConfig> UnitDailyConfigUnitMonthlyConfigs { get; }


        IRepository<AuditLogRecord> AuditLogRecords { get; }

        IDeletableEntityRepository<Shift> Shifts { get; }

        IRepository<RelatedUnitConfigs> RelatedUnitConfigs { get; }

        IRepository<RelatedUnitDailyConfigs> RelatedUnitDailyConfigs { get; }

        IRepository<RelatedUnitMonthlyConfigs> RelatedUnitMonthlyConfigs { get; }

        IRepository<ProductionPlanConfigUnitMonthlyConfigPlanMembers> ProductionPlanConfigUnitMonthlyConfigPlanMembers { get; }

        IRepository<ProductionPlanConfigUnitMonthlyConfigFactFractionMembers> ProductionPlanConfigUnitMonthlyConfigFactFractionMembers { get; }

        IDeletableEntityRepository<UnitEnteredForCalculationData> UnitEnteredForCalculationDatas { get; }

        IDeletableEntityRepository<MeasuringPointProductsConfig> MeasuringPointProductsConfigs { get; }

        IDeletableEntityRepository<Density2FactorAlpha> Density2FactorAlphas { get; }

        MessageRepository Messages { get; }

        IRepository<Event> Events { get; }

        IRepository<LogedInUser> LogedInUsers { get; }

        IDeletableEntityRepository<ProductionPlanData> ProductionPlanDatas { get; }

        IDeletableEntityRepository<MathExpression> MathExpressions { get; }

        IDeletableEntityRepository<HighwayPipelineConfig> HighwayPipelineConfigs { get; }

        IDeletableEntityRepository<HighwayPipelineData> HighwayPipelineDatas { get; }

        IDeletableEntityRepository<HighwayPipelineManualData> HighwayPipelineManualDatas { get; }

        IDeletableEntityRepository<HighwayPipelineApprovedData> HighwayPipelineApprovedDatas { get; }

        IDeletableEntityRepository<InnerPipelineData> InnerPipelineDatas { get; }

        IDeletableEntityRepository<InProcessUnitData> InProcessUnitDatas { get; }

        IDeletableEntityRepository<InnerPipelinesApprovedDate> InnerPipelinesApprovedDates { get; }

        IDeletableEntityRepository<InProcessUnitsApprovedDate> InProcessUnitsApprovedDates { get; }

        IDeletableEntityRepository<TankStatus> TankStatuses { get; }

        IDeletableEntityRepository<TankStatusData> TankStatusDatas { get; }

        IRepository<UnitDatasTemp> UnitDatasTemps { get; }

        IDeletableEntityRepository<UnitRecalculatedMonthlyData> UnitRecalculatedMonthlyDatas { get; }

        IRepository<RelatedProductionPlanConfigs> RelatedProductionPlanConfigs { get; }

        IDbContext DbContext { get; }

        IEfStatus SaveChanges(string userName);
    }
}
