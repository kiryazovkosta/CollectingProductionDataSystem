namespace CollectingProductionDataSystem.Data.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Contracts;
    using CollectingProductionDataSystem.Models.Contracts;
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

    public class ProductionData : IProductionData
    {
        private readonly IDbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public ProductionData(IPersister persisterParam, ILogger loggerParam)
            : this(new CollectingDataSystemDbContext(persisterParam, loggerParam))
        {
        }

        public ProductionData(IPersister persisterParam, ILogger loggerParam, string connectionString)
            : this(new CollectingDataSystemDbContext(persisterParam, loggerParam, connectionString))
        {
        }

        public ProductionData(IDbContext context)
        {
            this.context = context;
        }

        public IDbContext Context
        {
            get
            {
                return this.context;
            }
        }

        public IDeletableEntityRepository<Plant> Plants
        {
            get
            {
                return this.GetDeletableEntityRepository<Plant>();
            }
        }

        public IDeletableEntityRepository<Factory> Factories
        {
            get
            {
                return this.GetDeletableEntityRepository<Factory>();
            }
        }

        public IDeletableEntityRepository<ProcessUnit> ProcessUnits
        {
            get
            {
                return this.GetDeletableEntityRepository<ProcessUnit>();
            }
        }

        public IDeletableEntityRepository<UnitConfig> UnitConfigs
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitConfig>();
            }
        }

        public IApprovableEntityRepository<UnitsData> UnitsData
        {
            get
            {
                return this.GetApprovableEntityRepository<UnitsData>();
            }
        }

        public IDeletableEntityRepository<UnitsManualData> UnitsManualData
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitsManualData>();
            }
        }

        public IApprovableEntityRepository<UnitsDailyData> UnitsDailyDatas
        {
            get
            {
                return this.GetApprovableEntityRepository<UnitsDailyData>();
            }
        }

        /// <summary>
        /// Gets or sets the unit monthly datas.
        /// </summary>
        /// <value>The unit monthly datas.</value>
        public IApprovableEntityRepository<UnitMonthlyData> UnitMonthlyDatas
        {
            get
            {
                return this.GetApprovableEntityRepository<UnitMonthlyData>();
            }
        }

        public IDeletableEntityRepository<UnitsManualDailyData> UnitsManualDailyDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitsManualDailyData>();
            }
        }

        /// <summary>
        /// Gets or sets the unit manual monthly datas.
        /// </summary>
        /// <value>The unit manual monthly datas.</value>
        public IDeletableEntityRepository<UnitManualMonthlyData> UnitManualMonthlyDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitManualMonthlyData>();
            }
        }

        public IDeletableEntityRepository<Area> Areas
        {
            get
            {
                return this.GetDeletableEntityRepository<Area>();
            }
        }

        public IDeletableEntityRepository<Park> Parks
        {
            get
            {
                return this.GetDeletableEntityRepository<Park>();
            }
        }

        public IDeletableEntityRepository<TankConfig> Tanks
        {
            get
            {
                return this.GetDeletableEntityRepository<TankConfig>();
            }
        }

        public IDeletableEntityRepository<UnitDailyConfig> UnitsDailyConfigs
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitDailyConfig>();
            }
        }

        /// <summary>
        /// Gets or sets the unit monthly configs.
        /// </summary>
        /// <value>The unit monthly configs.</value>
        public IDeletableEntityRepository<UnitMonthlyConfig> UnitMonthlyConfigs
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitMonthlyConfig>();
            }
        }

        public IDeletableEntityRepository<PhdConfig> PhdConfigs
        {
            get
            {
                return this.GetDeletableEntityRepository<PhdConfig>();
            }
        }

        public IApprovableEntityRepository<TankData> TanksData
        {
            get
            {
                return this.GetApprovableEntityRepository<TankData>();
            }
        }

        public IDeletableEntityRepository<MeasuringPointProductsData> MeasurementPointsProductsDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<MeasuringPointProductsData>();
            }
        }

        public IDeletableEntityRepository<Product> Products
        {
            get
            {
                return this.GetDeletableEntityRepository<Product>();
            }
        }

        public IDeletableEntityRepository<ProductType> ProductTypes
        {
            get
            {
                return this.GetDeletableEntityRepository<ProductType>();
            }
        }

        public IDeletableEntityRepository<ShiftProductType> ShiftProductTypes
        {
            get
            {
                return this.GetDeletableEntityRepository<ShiftProductType>();
            }
        }

        public IDeletableEntityRepository<DailyProductType> DailyProductTypes
        {
            get
            {
                return this.GetDeletableEntityRepository<DailyProductType>();
            }
        }

        public IDeletableEntityRepository<Ikunk> Ikunks
        {
            get
            {
                return this.GetDeletableEntityRepository<Ikunk>();
            }
        }

        public IDeletableEntityRepository<Zone> Zones
        {
            get
            {
                return this.GetDeletableEntityRepository<Zone>();
            }
        }

        public IDeletableEntityRepository<TransportType> TransportTypes
        {
            get
            {
                return this.GetDeletableEntityRepository<TransportType>();
            }
        }

        public IDeletableEntityRepository<EditReason> EditReasons
        {
            get
            {
                return this.GetDeletableEntityRepository<EditReason>();
            }
        }

        public IDeletableEntityRepository<TankMasterProduct> TankMasterProducts
        {
            get
            {
                return this.GetDeletableEntityRepository<TankMasterProduct>();
            }
        }

        public IDeletableEntityRepository<UnitsApprovedData> UnitsApprovedDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitsApprovedData>();
            }
        }

        public IDeletableEntityRepository<ApplicationUser> Users
        {
            get
            {
                return this.GetDeletableEntityRepository<ApplicationUser>();
            }
        }

        public IDeletableEntityRepository<ApplicationRole> Roles
        {
            get
            {
                return this.GetDeletableEntityRepository<ApplicationRole>();
            }
        }

        public IDeletableEntityRepository<UnitsApprovedDailyData> UnitsApprovedDailyDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitsApprovedDailyData>();
            }
        }

        /// <summary>
        /// Gets or sets the unit approved monthly datas.
        /// </summary>
        /// <value>The unit approved monthly datas.</value>
        public IDeletableEntityRepository<UnitApprovedMonthlyData> UnitApprovedMonthlyDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitApprovedMonthlyData>();
            }
        }

        public IDeletableEntityRepository<ProductionPlanConfig> ProductionPlanConfigs
        {
            get
            {
                return this.GetDeletableEntityRepository<ProductionPlanConfig>();
            }
        }

        public IDeletableEntityRepository<MeasuringPointConfig> MeasuringPointConfigs
        {
            get
            {
                return this.GetDeletableEntityRepository<MeasuringPointConfig>();
            }
        }

        public IDeletableEntityRepository<MeasuringPointsConfigsData> MeasuringPointsConfigsDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<MeasuringPointsConfigsData>();
            }
        }

        public IDeletableEntityRepository<MaxAsoMeasuringPointDataSequenceNumber> MaxAsoMeasuringPointDataSequenceNumberMap
        {
            get
            {
                return this.GetDeletableEntityRepository<MaxAsoMeasuringPointDataSequenceNumber>();
            }
        }

        public IDeletableEntityRepository<Direction> Directions
        {
            get
            {
                return this.GetDeletableEntityRepository<Direction>();
            }
        }

        public IDeletableEntityRepository<MaterialType> MaterialTypes
        {
            get
            {
                return this.GetDeletableEntityRepository<MaterialType>();
            }
        }

        public IDeletableEntityRepository<MeasureUnit> MeasureUnits
        {
            get
            {
                return this.GetDeletableEntityRepository<MeasureUnit>();
            }
        }

        public IDeletableEntityRepository<ActiveTransactionsData> ActiveTransactionsDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<ActiveTransactionsData>();
            }
        }

        public IDeletableEntityRepository<MaterialDetailType> MaterialDetailTypes
        {
            get
            {
                return this.GetDeletableEntityRepository<MaterialDetailType>();
            }
        }

        public IDeletableEntityRepository<MonthlyReportType> MonthlyReportTypes
        {
            get
            {
                return this.GetDeletableEntityRepository<MonthlyReportType>();
            }
        }

        public IDeletableEntityRepository<PlanNorm> PlanNorms
        {
            get
            {
                return this.GetDeletableEntityRepository<PlanNorm>();
            }
        }

        public IDeletableEntityRepository<PlanValue> PlanValues
        {
            get
            {
                return this.GetDeletableEntityRepository<PlanValue>();
            }
        }

        public IRepository<UnitConfigUnitDailyConfig> UnitConfigUnitDailyConfigs
        {
            get
            {
                return this.GetRepository<UnitConfigUnitDailyConfig>();
            }
        }

        /// <summary>
        /// Gets or sets the unit daily config unit monthly configs.
        /// </summary>
        /// <value>The unit daily config unit monthly configs.</value>
        public IRepository<UnitDailyConfigUnitMonthlyConfig> UnitDailyConfigUnitMonthlyConfigs
        {
            get
            {
                return this.GetRepository<UnitDailyConfigUnitMonthlyConfig>();
            }
        }

        public IRepository<LogedInUser> LogedInUsers
        {
            get
            {
                return this.GetRepository<LogedInUser>();
            }
        }

        public IRepository<ProductionPlanConfigUnitMonthlyConfigPlanMembers> ProductionPlanConfigUnitMonthlyConfigPlanMembers
        {
            get
            {
                return this.GetRepository<ProductionPlanConfigUnitMonthlyConfigPlanMembers>();
            }
        }

        public IRepository<ProductionPlanConfigUnitMonthlyConfigFactFractionMembers> ProductionPlanConfigUnitMonthlyConfigFactFractionMembers
        {
            get
            {
                return this.GetRepository<ProductionPlanConfigUnitMonthlyConfigFactFractionMembers>();
            }
        }

        public IDeletableEntityRepository<ProductionPlanData> ProductionPlanDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<ProductionPlanData>();
            }
        }

        public IDeletableEntityRepository<MathExpression> MathExpressions
        {
            get
            {
                return this.GetDeletableEntityRepository<MathExpression>();
            }
        }

        public IDeletableEntityRepository<HighwayPipelineConfig> HighwayPipelineConfigs
        {
            get
            {
                return this.GetDeletableEntityRepository<HighwayPipelineConfig>();
            }
        }

        public IDeletableEntityRepository<HighwayPipelineData> HighwayPipelineDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<HighwayPipelineData>();
            }
        }

        public IDeletableEntityRepository<HighwayPipelineManualData> HighwayPipelineManualDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<HighwayPipelineManualData>();
            }
        }

        public IDeletableEntityRepository<HighwayPipelineApprovedData> HighwayPipelineApprovedDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<HighwayPipelineApprovedData>();
            }
        }

        public IDeletableEntityRepository<InnerPipelineData> InnerPipelineDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<InnerPipelineData>();
            }
        }

        public IDeletableEntityRepository<InProcessUnitData> InProcessUnitDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<InProcessUnitData>();
            }
        }

        public IDeletableEntityRepository<InnerPipelinesApprovedDate> InnerPipelinesApprovedDates
        {
            get
            {
                return this.GetDeletableEntityRepository<InnerPipelinesApprovedDate>();
            }
        }

        public IDeletableEntityRepository<InProcessUnitsApprovedDate> InProcessUnitsApprovedDates
        {
            get
            {
                return this.GetDeletableEntityRepository<InProcessUnitsApprovedDate>();
            }
        }

        public IDeletableEntityRepository<TankStatus> TankStatuses
        {
            get
            {
                return this.GetDeletableEntityRepository<TankStatus>();
            }
        }

        public IDeletableEntityRepository<TankStatusData> TankStatusDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<TankStatusData>();
            }
        }

        public IRepository<AuditLogRecord> AuditLogRecords
        {
            get
            {
                return this.GetRepository<AuditLogRecord>();
            }
        }

        public IRepository<UnitDatasTemp> UnitDatasTemps
        {
            get
            {
                return this.GetRepository<UnitDatasTemp>();
            }
        }

        public IDeletableEntityRepository<UnitRecalculatedMonthlyData> UnitRecalculatedMonthlyDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitRecalculatedMonthlyData>();
            }
        }

        public IRepository<RelatedProductionPlanConfigs> RelatedProductionPlanConfigs
        {
            get
            {
                return this.GetRepository<RelatedProductionPlanConfigs>();
            }
        }

        public IRepository<MeasuringPointsConfigsReportData> MeasuringPointsConfigsReportDatas
        {
            get
            {
                return this.GetRepository<MeasuringPointsConfigsReportData>();
            }
        }

        public IEfStatus SaveChanges(string userName)
        {
            return this.context.SaveChangesWithValidation(userName);
        }

        public IDeletableEntityRepository<Shift> Shifts
        {
            get
            {
                return this.GetDeletableEntityRepository<Shift>();
            }
        }

        public IRepository<RelatedUnitConfigs> RelatedUnitConfigs
        {
            get
            {
                return this.GetRepository<RelatedUnitConfigs>();
            }
        }

        public IRepository<RelatedUnitDailyConfigs> RelatedUnitDailyConfigs
        {
            get
            {
                return this.GetRepository<RelatedUnitDailyConfigs>();
            }
        }

        /// <summary>
        /// Gets or sets the related unit monthly configs.
        /// </summary>
        /// <value>The related unit monthly configs.</value>
        public IRepository<RelatedUnitMonthlyConfigs> RelatedUnitMonthlyConfigs
        {
            get
            {
                return this.GetRepository<RelatedUnitMonthlyConfigs>();
            }
        }

        public IDeletableEntityRepository<UnitEnteredForCalculationData> UnitEnteredForCalculationDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitEnteredForCalculationData>();
            }
        }

        public IDeletableEntityRepository<MeasuringPointProductsConfig> MeasuringPointProductsConfigs
        {
            get
            {
                return this.GetDeletableEntityRepository<MeasuringPointProductsConfig>();
            }
        }

        public IDeletableEntityRepository<Density2FactorAlpha> Density2FactorAlphas
        {
            get
            {
                return this.GetDeletableEntityRepository<Density2FactorAlpha>();
            }
        }

        public MessageRepository Messages
        {
            get
            {
                return this.GetMessageRepository();
            }
        }

        //public IDeletableEntityRepository<Message> Messages
        //{
        //    get
        //    {
        //        return this.GetDeletableEntityRepository<Message>();
        //    }
        //}

        public IRepository<Event> Events
        {
            get
            {
                return this.GetRepository<Event>();
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                }
            }
        }

        private IRepository<T> GetRepository<T>() where T : class, IEntity
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }


        private MessageRepository GetMessageRepository()
        {
            if (!this.repositories.ContainsKey(typeof(MessageRepository)))
            {
                var type = typeof(MessageRepository);
                this.repositories.Add(typeof(MessageRepository), Activator.CreateInstance(type, this.context));
            }

            return (MessageRepository)this.repositories[typeof(MessageRepository)];
        }

        private IDeletableEntityRepository<T> GetDeletableEntityRepository<T>() where T : class, IEntity, IDeletableEntity
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(DeletableEntityRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IDeletableEntityRepository<T>)this.repositories[typeof(T)];
        }

        private IImmutableEntityRepository<T> GetImmutableEntityRepository<T>() where T : class, IEntity, IAuditInfo
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(ImmutableEntityRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IImmutableEntityRepository<T>)this.repositories[typeof(T)];
        }
        private IApprovableEntityRepository<T> GetApprovableEntityRepository<T>() where T : class, IEntity, IApprovableEntity, IAuditInfo
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(ApprovableEntityRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IApprovableEntityRepository<T>)this.repositories[typeof(T)];
        }

        /// <summary>
        /// Gets or sets the db context.
        /// </summary>
        /// <value>The db context.</value>
        public IDbContext DbContext
        {
            get
            {
                return this.context;
            }
        }

    }
}
