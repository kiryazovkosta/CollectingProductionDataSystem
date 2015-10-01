namespace CollectingProductionDataSystem.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Transactions;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Data.Mappings.Configuration;
    using CollectingProductionDataSystem.Data.Migrations;
    using CollectingProductionDataSystem.Infrastructure.Extentions;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Transactions;
    using CollectingProductionDataSystem.Models.UtilityEntities;
    using Microsoft.AspNet.Identity.EntityFramework;
    using EntityFramework.BulkInsert.Extensions;

    public class CollectingDataSystemDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int,
        UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>, IAuditableDbContext
    {
        private readonly IPersister persister;

        static CollectingDataSystemDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CollectingDataSystemDbContext, Configuration>());
        }

        public CollectingDataSystemDbContext()
            : base("CollectingPrimaryDataSystemConnection")
        {
#if DEBUG
            this.Database.Log = (c) => { Debug.WriteLine(c); };
#endif
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

        public IDbSet<UnitConfig> UnitsConfigs { get; set; }

        public IDbSet<UnitsData> UnitsData { get; set; }

        public IDbSet<UnitsInspectionData> UnitsInspectionData { get; set; }

        public IDbSet<MaterialType> MaterialTypes { get; set; }

        public IDbSet<AuditLogRecord> AuditLogRecords { get; set; }

        public IDbSet<Ikunk> Ikunks { get; set; }

        public IDbSet<MeasurementPoint> MeasurementPoints { get; set; }

        public IDbSet<MeasurementPointsProductsConfig> MeasurementPointsProductConfigs { get; set; }

        public IDbSet<MeasurementPointsProductsData> MeasurementPointsProductsDatas { get; set; }

        public IDbSet<TransportType> TransportTypes { get; set; }

        public IDbSet<EditReason> EditReasons { get; set; }

        public IDbSet<UnitsDailyConfig> UnitsDailyConfigs { get; set; }

        public IDbSet<UnitsDailyData> UnitsDailyDatas { get; set; }

        public IDbSet<UnitsManualDailyData> UnitsManualDailyDatas { get; set; }

        public IDbSet<TankMasterProduct> TankMasterProducts { get; set; }

        public IDbSet<ProductionShift> ProductionShifts { get; set; }

        public IDbSet<UnitsApprovedData> UnitsApprovedDatas { get; set; }

        public IDbSet<UnitsApprovedDailyData> UnitsApprovedDailyDatas { get; set; }

        public IDbSet<ProductionPlanConfig> ProductionPlanConfigs { get; set; }

        public IDbSet<TanksApprovedData> TanksApprovedDatas { get; set; }

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

        /// <summary>
        /// Gets or sets the db context.
        /// </summary>
        /// <value>The db context.</value>
        public DbContext DbContext
        {
            get
            {
                return this;
            }
        }

        public int SaveChanges(string userName)
        {
            using (var transaction = new TransactionScope())
            {
                if (userName == null)
                {
                    // performed only if some MicroSoft API calls original SaveChanges
                    userName = "System Change";
                }

                var addedRecords = persister.PrepareSaveChanges(this, userName);
                var returnValue = base.SaveChanges();

                //Append added records with their Ids into audit log
                if (addedRecords != null && addedRecords.Count() > 0)
                {
                    persister.GetAddedEntityes(addedRecords, this.AuditLogRecords);
                    base.SaveChanges();
                }
                transaction.Complete();
                return returnValue;
            } 
        }

        public IEfStatus SaveChangesWithValidation(string userName)
        {
            var result = new EfStatus();
            try
            {
                result.ResultRecordsCount = SaveChanges(userName); //then update it
            }
            catch (DbEntityValidationException ex)
            {
                return result.SetErrors(ex.EntityValidationErrors);
            }
            //else it isn't an exception we understand so it throws in the normal way

            return result;
        }

        public void BulkInsert<T>(IEnumerable<T> entities, string userName) where T : class
        {
            if (entities.FirstOrDefault() is IAuditInfo)
            {
                entities.ForEach(x =>
                {
                    ((IAuditInfo)x).CreatedFrom = userName;
                    ((IAuditInfo)x).CreatedOn = DateTime.Now;
                });
            }

            this.BulkInsert(entities);
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }
    }
}
