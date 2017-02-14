namespace HistoricalProductionStructure.DataAccess.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Security.Policy;
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;

    public class ProductionData : IProductionData
    {
        private readonly IDbContext context;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public ProductionData(IPersister persisterParam)
            : this(new AppDbContext(persisterParam))
        {
        }

        public ProductionData(IPersister persisterParam, string connectionString)
            : this(new AppDbContext(persisterParam, connectionString))
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

        public IDeletableEntityRepository<ProcessUnitToFactoryHistory> ProcessUnitToFactoryHistory
        {
            get
            {
                return this.GetDeletableEntityRepository<ProcessUnitToFactoryHistory>();
            }
        }

        public IRepository<AuditLogRecord> AuditLogRecords
        {
            get
            {
                return this.GetRepository<AuditLogRecord>();
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

        public IEfStatus SaveChanges(string userName)
        {
            return this.context.SaveChangesWithValidation(userName);
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
