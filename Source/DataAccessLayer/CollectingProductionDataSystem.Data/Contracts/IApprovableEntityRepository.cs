using CollectingProductionDataSystem.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Data.Contracts
{
    public interface IApprovableEntityRepository<T> where T : IApprovableEntity
    {
        IQueryable<T> All();

        T GetById(object id);

        void Add(T entity);

        void Approve(T entity);

        void Detach(T entity);
    }
}
