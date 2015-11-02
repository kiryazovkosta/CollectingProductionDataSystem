namespace CollectingProductionDataSystem.Data.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Transactions;
    using CollectingProductionDataSystem.Models.UtilityEntities;

    public class ProductionData : IProductionData
    {
        private readonly IDbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public ProductionData(IPersister persisterParam)
            : this(new CollectingDataSystemDbContext(persisterParam))
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

        public IDeletableEntityRepository<UnitsManualDailyData> UnitsManualDailyDatas
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitsManualDailyData>();
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

        public IRepository<UnitConfigUnitDailyConfig> UnitConfigUnitDailyConfigs
        {
            get
            {
                return this.GetRepository<UnitConfigUnitDailyConfig>();
            }
        }

        public IRepository<AuditLogRecord> AuditLogRecords
        {
            get
            {
                return this.GetRepository<AuditLogRecord>();
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
