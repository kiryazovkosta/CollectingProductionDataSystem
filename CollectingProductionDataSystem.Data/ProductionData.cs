using CollectingProductionDataSystem.Common;
using CollectingProductionDataSystem.Data.Repositories;
using CollectingProductionDataSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Data
{
    public class ProductionData : IProductionData
    {
        private readonly IDbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

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

        public IActiveEntityRepository<ExciseStore> ExciseStores
        {
            get
            {
                return this.GetIActiveEntityRepository<ExciseStore>();
            }
        }

        public Common.IActiveEntityRepository<Plant> Plants
        {
            get
            {
                return this.GetIActiveEntityRepository<Plant>();
            }
        }

        public Common.IActiveEntityRepository<Factory> Factories
        {
            get
            {
                return this.GetIActiveEntityRepository<Factory>();
            }
        }

        public IActiveEntityRepository<ProcessUnit> ProcessUnits
        {
            get
            {
                return this.GetIActiveEntityRepository<ProcessUnit>();
            }
        }

        public IActiveEntityRepository<Unit> Units
        {
            get
            {
                return this.GetIActiveEntityRepository<Unit>();
            }
        }

        public IRepository<UnitsData> UnitsData
        {
            get
            {
                return this.GetRepository<UnitsData>();
            }
        }

        public IRepository<UnitsInspectionData> UnitsInspectionData
        {
            get
            {
                return this.GetRepository<UnitsInspectionData>();
            }
        }

        public IActiveEntityRepository<Product> Products
        {
            get
            {
                return this.GetIActiveEntityRepository<Product>();
            }
        }

        public IActiveEntityRepository<ProductType> ProductTypes
        {
            get
            {
                return this.GetIActiveEntityRepository<ProductType>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
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

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }

        private IActiveEntityRepository<T> GetIActiveEntityRepository<T>() where T : class, IActiveEntity
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(ActiveEntityRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IActiveEntityRepository<T>)this.repositories[typeof(T)];
        }
    }
}
