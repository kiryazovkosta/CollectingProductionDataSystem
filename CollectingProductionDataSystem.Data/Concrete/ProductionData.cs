namespace CollectingProductionDataSystem.Data.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Contracts;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;

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

        public IDeletableEntityRepository<ExciseStore> ExciseStores
        {
            get
            {
                return this.GetDeletableEntityRepository<ExciseStore>();
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

        public IDeletableEntityRepository<UnitsData> UnitsData
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitsData>();
            }
        }

        public IDeletableEntityRepository<UnitsInspectionData> UnitsInspectionData
        {
            get
            {
                return this.GetDeletableEntityRepository<UnitsInspectionData>();
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

        public IDeletableEntityRepository<TankData> TanksData
        {
            get
            {
                return this.GetDeletableEntityRepository<TankData>();
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
