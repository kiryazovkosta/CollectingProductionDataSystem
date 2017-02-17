﻿namespace HistoricalProductionStructure.DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public interface IDbContext
    {
        DbContext DbContext { get; }

        //string UserName { get; set; }

        int SaveChanges(string userName);

        void Dispose();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        
        IDbSet<T> Set<T>() where T : class;

        IEfStatus SaveChangesWithValidation(string userName);

        void BulkInsert<T>(IEnumerable<T> entities, string userName) where T : class;
    }
}