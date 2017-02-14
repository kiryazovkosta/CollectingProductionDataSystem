namespace HistoricalProductionStructure.DataAccess.Contracts
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Domain.Models;

    public interface IPersister
    {
        /// <summary>
        /// Gets the added entityes.
        /// </summary>
        /// <param name="addedRecords">The added records.</param>
        /// <param name="auditRecords"></param>
        IEnumerable<AuditLogRecord> GetAddedEntityes(IEnumerable<DbEntityEntry> addedRecords, IDbSet<AuditLogRecord> auditRecords);

        IEnumerable<DbEntityEntry> PrepareSaveChanges(IAuditableDbContext data, string userName);
    }
}
