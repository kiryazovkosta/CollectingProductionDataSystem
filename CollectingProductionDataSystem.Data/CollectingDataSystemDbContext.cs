namespace CollectingProductionDataSystem.Data
{
    using System.Data.Entity;
    using CollectingProductionDataSystem.Data.Migrations;
    using CollectingProductionDataSystem.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    
    public class CollectingDataSystemDbContext : IdentityDbContext<ApplicationUser>
    {
        public CollectingDataSystemDbContext()
            : base("CollectingPrimaryDataSystemConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CollectingDataSystemDbContext, Configuration>());
        }

        public virtual IDbSet<ExciseStore> ExciseStores { get; set; }
        
        public virtual IDbSet<Product> Products { get; set; }

        public virtual IDbSet<ProductType> ProductTypes { get; set; }

        public virtual IDbSet<MeasureUnit> MeasureUnits { get; set; }

        public virtual IDbSet<Direction> Directions { get; set; }

        public virtual IDbSet<Area> Areas { get; set; }

        public virtual IDbSet<InventoryPark> InventoryParks { get; set; }

        public virtual IDbSet<InventoryTank> InventoryTanks { get; set; }

        public virtual IDbSet<InventoryTanksData> InventoryTanksData { get; set; }

        public virtual IDbSet<Plant> Plants { get; set; }

        public virtual IDbSet<Factory> Factories { get; set; }

        public virtual IDbSet<ProcessUnit> ProcessUnit { get; set; }

        public virtual IDbSet<Unit> Units { get; set; }

        public virtual IDbSet<UnitsData> UnitsData { get; set; }

        public virtual IDbSet<UnitsInspectionData> UnitsInspectionData { get; set; }

        public virtual IDbSet<MaterialType> MaterialTypes { get; set; }

        public static CollectingDataSystemDbContext Create()
        {
            return new CollectingDataSystemDbContext();
        }
    }
}
