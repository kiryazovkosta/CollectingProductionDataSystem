using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Models.UtilityEntities;

namespace CollectingProductionDataSystem.Data.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuditableDbContext:IDbContext
    {
        IDbSet<AuditLogRecord> AuditLogRecords { get; set; }
    }
}
