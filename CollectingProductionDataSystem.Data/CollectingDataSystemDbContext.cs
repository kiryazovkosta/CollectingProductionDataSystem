namespace CollectingProductionDataSystem.Data
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Data.Mappings.Configuration;
    using CollectingProductionDataSystem.Data.Migrations;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Transactions;
    using CollectingProductionDataSystem.Models.UtilityEntities;
    using Microsoft.AspNet.Identity.EntityFramework;


    public class CollectingDataSystemDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int,
        UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>, IDbContext
    {
        private readonly IPersister persister;

        static CollectingDataSystemDbContext() 
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CollectingDataSystemDbContext, Configuration>());
        }

        public CollectingDataSystemDbContext()
            : base("CollectingPrimaryDataSystemConnection")
        {
            //#if DEBUG
            //            this.Database.Log = (c) => { Debug.WriteLine(c); };
            //#endif
            this.persister = new AuditablePersister();
        }

        public CollectingDataSystemDbContext(IPersister param)
            : base("CollectingPrimaryDataSystemConnection")
        {
            this.persister = param;
        }

        public IDbSet<Product> Products { get; set; }

        public IDbSet<ProductType> ProductTypes { get; set; }

        public IDbSet<MeasureUnit> MeasureUnits { get; set; }

        public IDbSet<Direction> Directions { get; set; }

        public IDbSet<Area> Areas { get; set; }

        public IDbSet<Park> InventoryParks { get; set; }

        public IDbSet<TankConfig> InventoryTanks { get; set; }

        public IDbSet<TankData> TanksData { get; set; }

        public IDbSet<TanksManualData> TanksManualData { get; set; }

        public IDbSet<Plant> Plants { get; set; }

        public IDbSet<Factory> Factories { get; set; }

        public IDbSet<ProcessUnit> ProcessUnit { get; set; }

        public IDbSet<UnitConfig> Units { get; set; }

        public IDbSet<UnitsData> UnitsData { get; set; }

        public IDbSet<UnitsInspectionData> UnitsInspectionData { get; set; }

        public IDbSet<MaterialType> MaterialTypes { get; set; }

        public IDbSet<AuditLogRecord> AuditLogRecords { get; set; }

        public IDbSet<Ikunk> Ikunks { get; set; }

        public IDbSet<MeasurementPoint> MeasurementPoints { get; set; }

        public IDbSet<MeasurementPointsProductsConfig> MeasurementPointsProductConfigs { get; set; }

        public IDbSet<TransportType> TransportTypes { get; set; }

        public DbSet<AggregationsFormula> AggregationsFormulas { get; set; }

        public DbSet<UnitsAggregateDailyConfig> UnitsAggregateDailyConfigs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelBingConfig.RegisterMappings(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public static CollectingDataSystemDbContext Create()
        {
            return new CollectingDataSystemDbContext();
        }

        public override int SaveChanges()
        {
            return SaveChanges(null);
        }

        public int SaveChanges(string userName)
        {
            if (userName == null)
            {
                // performed only if some MicroSoft API calls original SaveChanges
                userName = "System Change";
            }

            List<DbEntityEntry> addedRecords;

            List<AuditLogRecord> changes = persister.PrepareSaveChanges(this, userName, out addedRecords);

            var returnValue = base.SaveChanges();

            if (addedRecords != null && addedRecords.Count() > 0)
            {
                changes.AddRange(persister.GetAddedEntityes(addedRecords));
            }

            if (changes.Count() > 0)
            {
                foreach (var change in changes)
                {
                    this.AuditLogRecords.Add(change);
                }
            }

            base.SaveChanges();
            return returnValue;
        }



        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }
    }


}
