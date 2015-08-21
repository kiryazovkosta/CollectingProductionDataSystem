namespace CollectingProductionDataSystem.Data.Contracts
{
    using System.Linq;
    using CollectingProductionDataSystem.Contracts;
    using CollectingProductionDataSystem.Models.Contracts;

    public interface IDeletableEntityRepository<T> : IRepository<T> where T : IEntity
    {
        IQueryable<T> AllWithDeleted();

    }
}
