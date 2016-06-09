using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Data.Concrete;
using CollectingProductionDataSystem.Models.Contracts;

namespace CollectingProductionDataSystem.Extentions
{
    public static class DeletableEntityRepositoryExtentions
    {
        public static IQueryable<T> AllAnual<T>(this DeletableEntityRepository<T> repository, int year) 
            where T: class, IDeletableEntity, IEntity, IDateable
        {
            return repository.AllWithDeleted().Where(x => x.DeletedOn <= new DateTime(year, 1, 1));
        }
    }
}
