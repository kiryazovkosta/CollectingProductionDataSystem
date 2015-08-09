namespace CollectingProductionDataSystem.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Migrations;
    using CollectingProductionDataSystem.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using CollectingProductionDataSystem.Common;
    
    public class CollectingDataSystemDbContext : IdentityDbContext<User>, IDbContext
    {
        public CollectingDataSystemDbContext()
            : base("CollectingPrimaryDataSystemConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CollectingDataSystemDbContext, Configuration>());
        }

        public IDbSet<ExciseStore> ExciseStores { get; set; }
        
        public IDbSet<Product> Products { get; set; }

        public IDbSet<ProductType> ProductTypes { get; set; }

        public IDbSet<MeasureUnit> MeasureUnits { get; set; }

        public IDbSet<Direction> Directions { get; set; }

        public IDbSet<Area> Areas { get; set; }

        public IDbSet<InventoryPark> InventoryParks { get; set; }

        public IDbSet<InventoryTank> InventoryTanks { get; set; }

        public IDbSet<InventoryTanksData> InventoryTanksData { get; set; }

        public IDbSet<Plant> Plants { get; set; }

        public IDbSet<Factory> Factories { get; set; }

        public IDbSet<ProcessUnit> ProcessUnit { get; set; }

        public IDbSet<Unit> Units { get; set; }

        public IDbSet<UnitsData> UnitsData { get; set; }

        public IDbSet<UnitsInspectionData> UnitsInspectionData { get; set; }

        public IDbSet<MaterialType> MaterialTypes { get; set; }

        public static CollectingDataSystemDbContext Create()
        {
            return new CollectingDataSystemDbContext();
        }

        public override int SaveChanges()
        {
            this.ApplyActiveEntityRules();
            return base.SaveChanges();
        }

        public DbContext DbContext { get; set; }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        private void ApplyActiveEntityRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (
                var entry in
                    this.ChangeTracker.Entries()
                        .Where(e => e.Entity is IActiveEntity && (e.State == EntityState.Deleted)))
            {
                var entity = (IActiveEntity)entry.Entity;

                entity.IsActive = false;
                entry.State = EntityState.Modified;
            }
        }
    }
}
