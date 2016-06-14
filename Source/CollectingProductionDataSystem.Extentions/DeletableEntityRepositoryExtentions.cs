namespace CollectingProductionDataSystem.Extentions
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Contracts;

    public static class DeletableEntityRepositoryExtentions
    {
        //[System.Runtime.CompilerServices.ExtensionAttribute()]
        public static IQueryable<T> AllAnual<T>(this IDeletableEntityRepository<T> repository, int year) 
            where T: class, IDeletableEntity, IEntity
        {
            var targetDate = new DateTime(year, 1, 1);
            return repository.AllWithDeleted()
                             .Where(x => (!x.IsDeleted) 
                                 || (x.DeletedOn >= targetDate));
        }
    }
}
