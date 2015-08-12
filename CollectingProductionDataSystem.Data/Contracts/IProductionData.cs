namespace CollectingProductionDataSystem.Data
{
    using System;

    using System.Data.Entity;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models;
    using CollectingProductionDataSystem.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;

    public interface IProductionData : IDisposable
    {
        IDbContext Context { get; }

        IDeletableEntityRepository<ExciseStore> ExciseStores { get; }

        IDeletableEntityRepository<Plant> Plants { get; }

        IDeletableEntityRepository<Factory> Factories { get; }

        IDeletableEntityRepository<ProcessUnit> ProcessUnits { get; }

        IDeletableEntityRepository<UnitConfig> Units { get; }

        IDeletableEntityRepository<UnitsData> UnitsData { get; }

        IDeletableEntityRepository<UnitsInspectionData> UnitsInspectionData { get; }

        IDeletableEntityRepository<Area> Areas { get; }

        IDeletableEntityRepository<Park> Parks { get; }

        IDeletableEntityRepository<TankConfig> Tanks { get; }

        IDeletableEntityRepository<TankData> TanksData { get; }

        IDeletableEntityRepository<Product> Products { get; }

        IDeletableEntityRepository<ProductType> ProductTypes { get; }

        int SaveChanges(string userName);
    }
}
