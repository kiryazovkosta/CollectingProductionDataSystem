namespace CollectingProductionDataSystem.Data
{
    using CollectingProductionDataSystem.Common;
    using CollectingProductionDataSystem.Data.Repositories;
    using CollectingProductionDataSystem.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class InventoryData : IInventoryData
    {
        private readonly IDbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public InventoryData(IDbContext context)
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

        public IActiveEntityRepository<Area> Areas
        {
            get
            {
                return this.GetIActiveEntityRepository<Area>();
            }
        }

        public IActiveEntityRepository<InventoryPark> Parks
        {
            get
            {
                return this.GetIActiveEntityRepository<InventoryPark>();
            }
        }

        public IActiveEntityRepository<InventoryTank> Tanks
        {
            get
            {
                return this.GetIActiveEntityRepository<InventoryTank>();
            }
        }

        public IRepository<InventoryTanksData> TanksData
        {
            get 
            {
                return this.GetRepository<InventoryTanksData>(); 
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
