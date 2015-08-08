namespace CollectingProductionDataSystem.Data.Repositories
{
    using System.Data.Entity;
    using System.Linq;

    using CollectingProductionDataSystem.Common.Contracts;
    using CollectingProductionDataSystem.Common.Repositories;

    public class DeletableEntityRepository<T> : GenericRepository<T>, IActiveEntityRepository<T>
        where T : class, IActiveEntity
    {
        public DeletableEntityRepository(DbContext context)
            : base(context)
        {
        }

        public override IQueryable<T> All()
        {
            return base.All().Where(x => x.IsActive);
        }

        public IQueryable<T> AllActive()
        {
            return base.All();
        }
    }
}
