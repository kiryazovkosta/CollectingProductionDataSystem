namespace CollectingProductionDataSystem.Data.Contracts
{
    using System;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Transactions;

    public interface IProductionData : IDisposable
    {
        IDbContext Context { get; }

        IDeletableEntityRepository<Plant> Plants { get; }

        IDeletableEntityRepository<Factory> Factories { get; }

        IDeletableEntityRepository<ProcessUnit> ProcessUnits { get; }

        IDeletableEntityRepository<UnitConfig> Units { get; }

        IApprovableEntityRepository<UnitsData> UnitsData { get; }

        IDeletableEntityRepository<UnitsManualData> UnitsManualData { get; }

        IImmutableEntityRepository<UnitsInspectionData> UnitsInspectionData { get; }

        IDeletableEntityRepository<Area> Areas { get; }

        IDeletableEntityRepository<Park> Parks { get; }

        IDeletableEntityRepository<TankConfig> Tanks { get; }

        IApprovableEntityRepository<TankData> TanksData { get; }

        IApprovableEntityRepository<MeasurementPointsProductsData> MeasurementPointsProductsDatas { get; }

        IDeletableEntityRepository<TanksManualData> TanksManualData { get; }

        IDeletableEntityRepository<Product> Products { get; }

        IDeletableEntityRepository<ProductType> ProductTypes { get; }

        IDeletableEntityRepository<Ikunk> Ikunks { get; }

        IDeletableEntityRepository<Zone> Zones { get; }

        IDeletableEntityRepository<MeasurementPoint> MeasurementPoints { get; }

        IDeletableEntityRepository<MeasurementPointsProductsConfig> MeasurementPointsProductConfigs { get; }

        IDeletableEntityRepository<TransportType> TransportTypes { get; }

        IDeletableEntityRepository<EditReason> EditReasons { get; }

        IDeletableEntityRepository<TankMasterProduct> TankMasterProducts { get; }

        IRepository<ApplicationUser> Users{get;}


        IRepository<ApplicationRole> Roles { get; }
       


        IDbContext DbContext { get; }

        int SaveChanges(string userName);
    }
}
