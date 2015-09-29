namespace CollectingProductionDataSystem.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using CollectingProductionDataSystem.Models.Contracts;

    public interface IRepository<T> where T : IEntity
    {
        IQueryable<T> All();

        T GetById(object id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(object id);

        void Detach(T entity);

        void UpdateValues(Expression<Func<T, object>> entity);

        void BulkInsert(IEnumerable<T> entities, string userName);
    }
}
