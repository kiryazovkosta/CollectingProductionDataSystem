using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Data.Contracts
{
    public interface IPersister
    {
        void PrepareSaveChanges(DbContext data, string userName);
        void PrepareSaveChanges(DbContext data);
    }
}
