namespace CollectingProductionDataSystem.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;

    public interface IDbContext
    {
        DbContext DbContext { get; }

        //string UserName { get; set; }

        int SaveChanges(string userName);

        void Dispose();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        
        IDbSet<T> Set<T>() where T : class;

        IEfStatus SaveChangesWithValidation(string userName);
    }
}
