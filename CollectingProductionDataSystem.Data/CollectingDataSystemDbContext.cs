

namespace CollectingProductionDataSystem.Data
{
    using System.Data.Entity;
    using CollectingProductionDataSystem.Data.Migrations;

    using CollectingProductionDataSystem.Models;
    using CollectingProductionDataSystem.Models.Inventory;
using CollectingProductionDataSystem.Models.Mec;

    public class CollectingDataSystemDbContext : DbContext
    {
        public CollectingDataSystemDbContext()
            : base("CollectingProductionDataSystemConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CollectingDataSystemDbContext, Configuration>());
        }

        public virtual IDbSet<Area> Areas { get; set; }

        public virtual IDbSet<InventoryPark> InventoryParks { get; set; }

        public virtual IDbSet<InventoryTank> InventoryTanks { get; set; }

        public virtual IDbSet<ExciseStore> ExciseStores { get; set; }
        
        public virtual IDbSet<Product> Products { get; set; }

        public virtual IDbSet<ProductType> ProductTypes { get; set; }

        public virtual IDbSet<Plant> Plants { get; set; }

        public virtual IDbSet<Factory> Factories { get; set; }

        public virtual IDbSet<ProcessUnit> ProcessUnit { get; set; }

        public virtual IDbSet<Unit> Units { get; set; }

        public virtual IDbSet<UnitData> UnitData { get; set; }
    }
}
