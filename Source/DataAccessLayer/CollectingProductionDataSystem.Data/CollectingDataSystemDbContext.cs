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

        public IDbSet<Plant> Plants { get; set; }

        public IDbSet<Factory> Factories { get; set; }

        public IDbSet<ProcessUnit> ProcessUnit { get; set; }

        public IDbSet<UnitConfig> UnitConfigs { get; set; }

        public IDbSet<UnitsData> UnitsData { get; set; }

        public IDbSet<MaterialType> MaterialTypes { get; set; }

        public IDbSet<AuditLogRecord> AuditLogRecords { get; set; }

        public IDbSet<Ikunk> Ikunks { get; set; }

        public IDbSet<MeasuringPointProductsData> MeasurementPointsProductsDatas { get; set; }

        public IDbSet<TransportType> TransportTypes { get; set; }

        public IDbSet<EditReason> EditReasons { get; set; }

        public IDbSet<UnitDailyConfig> UnitDailyConfigs { get; set; }

        public IDbSet<UnitsDailyData> UnitsDailyDatas { get; set; }

        public IDbSet<UnitsManualDailyData> UnitsManualDailyDatas { get; set; }

        public IDbSet<TankMasterProduct> TankMasterProducts { get; set; }

        public IDbSet<UnitsApprovedData> UnitsApprovedDatas { get; set; }

        public IDbSet<UnitsApprovedDailyData> UnitsApprovedDailyDatas { get; set; }

        public IDbSet<ProductionPlanConfig> ProductionPlanConfigs { get; set; }

        public IDbSet<ShiftProductType> ShiftProductTypes { get; set; }

        public IDbSet<DailyProductType> DailyProductTypes { get; set; }

        public IDbSet<Shift> Shifts { get; set; }

        public IDbSet<ActiveTransactionsData> ActiveTransactionsDatas { get; set; }

        public IDbSet<RelatedMeasuringPointConfigs> RelatedMeasuringPointConfigs { get; set; }

        public IDbSet<RelatedTankConfigs> RelatedTankConfigs { get; set; }

        public IDbSet<RelatedUnitConfigs> RelatedUnitConfigs { get; set; }

        public IDbSet<RelatedUnitDailyConfigs> RelatedUnitDailyConfigs { get; set; }

        public IDbSet<UnitConfigUnitDailyConfig> UnitConfigUnitDailyConfigs { get; set; }

        public IDbSet<ApplicationUserPark> ApplicationUserParks { get; set; }

        public IDbSet<ApplicationUserProcessUnit> ApplicationUserProcessUnits { get; set; }

        public IDbSet<UnitEnteredForCalculationData> UnitEnteredForCalculationDatas { get; set; }

        public IDbSet<MeasuringPointProductsConfig> MeasuringPointProductsConfigs { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelBingConfig.RegisterMappings(modelBuilder);
            base.OnModelCreating(modelBuilder);
//#if DEBUG
            this.Database.Log = (c) => { Debug.WriteLine(c); };
//#endif
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
            using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, 30)))
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
