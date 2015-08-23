namespace CollectingProductionDataSystem.Data.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Models.Transactions;

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

        public IDeletableEntityRepository<UnitConfig> Units
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

        public IImmutableEntityRepository<UnitsInspectionData> UnitsInspectionData
        {
            get
            {
                return this.GetImmutableEntityRepository<UnitsInspectionData>();
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

        public IApprovableEntityRepository<TankData> TanksData
        {
            get
            {
                return this.GetApprovableEntityRepository<TankData>();
            }
        }

        public IDeletableEntityRepository<TanksManualData> TanksManualData
        {
            get
            {
                return this.GetDeletableEntityRepository<TanksManualData>();
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

        public IDeletableEntityRepository<MeasurementPoint> MeasurementPoints
        {
            get
            {
                return this.GetDeletableEntityRepository<MeasurementPoint>();
            }
        }

        public IDeletableEntityRepository<MeasurementPointsProductsConfig> MeasurementPointsProductConfigs
        {
            get
            {
                return this.GetDeletableEntityRepository<MeasurementPointsProductsConfig>();
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

        public int SaveChanges(string userName)
        {
            return this.context.SaveChanges(userName);
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
