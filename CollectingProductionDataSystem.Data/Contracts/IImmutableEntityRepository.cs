namespace CollectingProductionDataSystem.Data.Contracts
{
    using System.Linq;
    using CollectingProductionDataSystem.Models.Contracts;
    using System;

    public interface IImmutableEntityRepository<T> where T : IEntity
    {
        IQueryable<T> All();

        T GetById(object id);

        void Add(T entity);

        void Detach(T entity);
    }
}
