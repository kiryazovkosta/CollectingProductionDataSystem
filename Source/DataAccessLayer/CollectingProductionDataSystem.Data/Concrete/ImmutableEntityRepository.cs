namespace CollectingProductionDataSystem.Data.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using System.Data.Entity;
    using CollectingProductionDataSystem.Infrastructure.Extentions;
    using CollectingProductionDataSystem.Models.Contracts;
    using System.Data.Entity.Infrastructure;
    using EntityFramework.BulkInsert.Extensions;

    public class ImmutableEntityRepository<T> : IImmutableEntityRepository<T> where T : class, IEntity
    {
        public ImmutableEntityRepository(IDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", "context");
            }

            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }
        protected IDbSet<T> DbSet { get; set; }

        protected IDbContext Context { get; set; }

        public IQueryable<T> All()
        {
 	        return this.DbSet.AsQueryable();
        }

        public T GetById(object id)
        {
 	        return this.DbSet.Find(id);
        }

        public void Add(T entity)
        {
 	        DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.DbSet.Add(entity);
            }
        }

        public void Detach(T entity)
        {
 	        DbEntityEntry entry = this.Context.Entry(entity);
            entry.State = EntityState.Detached;
        }
        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void BulkInsert(IEnumerable<T> entities, string userName)
        {
            this.Context.BulkInsert(entities, userName);
        }
    }
}
