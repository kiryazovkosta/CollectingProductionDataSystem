namespace HistoricalProductionStructure.DataAccess.Contracts
{
    using System.Data.Entity;
    using Domain.Models;

    /// <summary>
    /// 
    /// </summary>
    public interface IAuditableDbContext:IDbContext
    {
        IDbSet<AuditLogRecord> AuditLogRecords { get; set; }
    }
}
