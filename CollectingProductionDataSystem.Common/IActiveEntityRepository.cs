namespace CollectingProductionDataSystem.Common
{
    using System.Linq;

    public interface IActiveEntityRepository<T> : IRepository<T> where T : class
    {
        IQueryable<T> AllActive();
    }
}
