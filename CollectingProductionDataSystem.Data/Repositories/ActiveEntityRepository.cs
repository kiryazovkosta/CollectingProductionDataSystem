namespace CollectingProductionDataSystem.Data.Repositories
{
    using System.Data.Entity;
    using System.Linq;

    using CollectingProductionDataSystem.Common;

    public class ActiveEntityRepository<T> : GenericRepository<T>, IActiveEntityRepository<T>
        where T : class, IActiveEntity
    {
        public ActiveEntityRepository(IDbContext context)
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
