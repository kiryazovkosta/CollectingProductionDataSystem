namespace CollectingProductionDataSystem.Data
{
    using CollectingProductionDataSystem.Common;
    using CollectingProductionDataSystem.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IInventoryData : IDisposable
    {
        IDbContext Context { get; }

        IActiveEntityRepository<ExciseStore> ExciseStores { get; }

        IActiveEntityRepository<Area> Areas { get; }

        IActiveEntityRepository<InventoryPark> Parks { get; }

        IActiveEntityRepository<InventoryTank> Tanks { get; }

        IRepository<InventoryTanksData> TanksData { get; }

        IActiveEntityRepository<Product> Products { get; }

        IActiveEntityRepository<ProductType> ProductTypes { get; }

        int SaveChanges();
    }
}
