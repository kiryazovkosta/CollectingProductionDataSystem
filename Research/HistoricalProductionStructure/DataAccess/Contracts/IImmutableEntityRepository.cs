namespace HistoricalProductionStructure.DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Contracts;

    public interface IImmutableEntityRepository<T> where T : IEntity
    {
        IQueryable<T> All();

        T GetById(object id);

        void Add(T entity);

        void Detach(T entity);

        void BulkInsert(IEnumerable<T> entities, string userName);
    }
}
