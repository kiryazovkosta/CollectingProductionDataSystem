namespace HistoricalProductionStructure.DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Contracts;

    public interface IApprovableEntityRepository<T> where T : IApprovableEntity
    {
        IQueryable<T> All();

        T GetById(object id);

        void Add(T entity);

        void Approve(T entity);

        void Detach(T entity);

        void Delete(T entity);

        void BulkInsert(IEnumerable<T> entities, string userName);
    }
}
