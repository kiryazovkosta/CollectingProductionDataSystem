namespace CollectingProductionDataSystem.Data.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Extentions;
    using CollectingProductionDataSystem.Models.Contracts;
    using EntityFramework.BulkInsert.Extensions;

    public class ApprovableEntityRepository<T> : IApprovableEntityRepository<T> where T : class, IApprovableEntity
    {
        public ApprovableEntityRepository(IDbContext context)
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
        public void Approve(T entity)
        {
            entity.IsApproved = true;
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
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
            if (entities.FirstOrDefault() is IAuditInfo)
            {
                entities.ForEach(x =>
                {
                    ((IAuditInfo)x).CreatedFrom = userName;
                    ((IAuditInfo)x).CreatedOn = DateTime.Now;
                });
            }

            this.Context.DbContext.BulkInsert(entities);
        }
    }
}
