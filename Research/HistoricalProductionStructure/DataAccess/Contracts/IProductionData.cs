namespace HistoricalProductionStructure.DataAccess.Contracts
{
    using System;
    using Domain.Models;

    public interface IProductionData : IDisposable
    {
        IDbContext Context { get; }

        IDeletableEntityRepository<Plant> Plants { get; }

        IDeletableEntityRepository<Factory> Factories { get; }

        IDeletableEntityRepository<ProcessUnit> ProcessUnits { get; }

        IDeletableEntityRepository<ProcessUnitToFactoryHistory> ProcessUnitToFactoryHistory { get;}

        IRepository<AuditLogRecord> AuditLogRecords { get; }

        IDbContext DbContext { get; }

        IEfStatus SaveChanges(string userName);
    }
}
