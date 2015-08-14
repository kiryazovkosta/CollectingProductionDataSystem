using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Models.UtilityEntities;

namespace CollectingProductionDataSystem.Data.Contracts
{
    public interface IPersister
    {
        IEnumerable<AuditLogRecord> PrepareSaveChanges(DbContext data, string userName);
        void PrepareSaveChanges(DbContext data);
    }
}
