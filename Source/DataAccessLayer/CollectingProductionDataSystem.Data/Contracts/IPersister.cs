using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Models.UtilityEntities;

namespace CollectingProductionDataSystem.Data.Contracts
{
    public interface IPersister
    {
        /// <summary>
        /// Gets the added entityes.
        /// </summary>
        /// <param name="addedRecords">The added records.</param>
        IEnumerable<AuditLogRecord> GetAddedEntityes(IEnumerable<DbEntityEntry> addedRecords, IDbSet<AuditLogRecord> auditRecords);

        IEnumerable<DbEntityEntry> PrepareSaveChanges(IAuditableDbContext data, string userName);
    }
}
