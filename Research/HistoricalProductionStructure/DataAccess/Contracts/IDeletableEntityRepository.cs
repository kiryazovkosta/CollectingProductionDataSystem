namespace HistoricalProductionStructure.DataAccess.Contracts
{
    using System.Linq;
    using Domain.Contracts;

    public interface IDeletableEntityRepository<T> : IRepository<T> where T : IEntity
    {
        IQueryable<T> AllWithDeleted();
    }
}
