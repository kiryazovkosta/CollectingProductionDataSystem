namespace CollectingProductionDataSystem.Data
{
    using CollectingProductionDataSystem.Common;
    using CollectingProductionDataSystem.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IProductionData : IDisposable
    {
        IDbContext Context { get; }

        IActiveEntityRepository<ExciseStore> ExciseStores { get; }

        IActiveEntityRepository<Plant> Plants { get; }

        IActiveEntityRepository<Factory> Factories { get; }

        IActiveEntityRepository<ProcessUnit> ProcessUnits { get; }

        IActiveEntityRepository<Unit> Units { get; }

        IRepository<UnitsData> UnitsData { get; }

        IRepository<UnitsInspectionData> UnitsInspectionData { get; } 

        IActiveEntityRepository<Product> Products { get; }

        IActiveEntityRepository<ProductType> ProductTypes { get; }

        int SaveChanges();
    }
}
