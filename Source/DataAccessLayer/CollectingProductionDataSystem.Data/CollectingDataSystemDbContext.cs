namespace CollectingProductionDataSystem.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Transactions;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Data.Mappings;
    using CollectingProductionDataSystem.Data.Mappings.Configuration;
    using CollectingProductionDataSystem.Data.Migrations;
    using CollectingProductionDataSystem.Infrastructure.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Extentions;
    using CollectingProductionDataSystem.Infrastructure.Log;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;
    using CollectingProductionDataSystem.Models.Productions.Qpt;
    using CollectingProductionDataSystem.Models.SystemLog;
    using CollectingProductionDataSystem.Models.Transactions;
    using CollectingProductionDataSystem.Models.Transactions.HighwayPipelines;
    using CollectingProductionDataSystem.Models.UtilityEntities;
    using Microsoft.AspNet.Identity.EntityFramework;
    using EntityFramework.BulkInsert.Extensions;
    using Models.Productions.Technological;
    using Models.PetroleumScheduler;

    public class CollectingDataSystemDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int,
        UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>, IAuditableDbContext
    {
        private readonly IPersister persister;
        private ILogger logger;
        private TransactionOptions transantionOption;

        static CollectingDataSystemDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CollectingDataSystemDbContext, Configuration>());
        }

        public CollectingDataSystemDbContext()
            : this(new AuditablePersister(), new Logger())
        {
        }

        public CollectingDataSystemDbContext(IPersister param, ILogger loggerParam)
            : base("CollectingPrimaryDataSystemConnection")
        {
            this.persister = param;
            this.logger = loggerParam;
            this.transantionOption = DefaultTransactionOptions.Instance.TransactionOptions;
        }

        public CollectingDataSystemDbContext(IPersister param, ILogger loggerParam, string connectionString)
            : base(connectionString)
        {
            this.persister = param;
            this.logger = loggerParam;
            this.transantionOption = DefaultTransactionOptions.Instance.TransactionOptions;
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

        public IDbSet<Density2FactorAlpha> Density2FactorAlphas { get; set; }

        public IDbSet<Event> Events { get; set; }

        public IDbSet<LogedInUser> LogedInUsers { get; set; }

        public IDbSet<ProductionPlanData> ProductionPlanDatas { get; set; }

        public IDbSet<MathExpression> MathExpressions { get; set; }

        public IDbSet<MaterialDetailType> MaterialDetailTypes { get; set; }

        public IDbSet<HighwayPipelineConfig> HighwayPipelineConfigs { get; set; }

        public IDbSet<HighwayPipelineData> HighwayPipelineDatas { get; set; }

        public IDbSet<HighwayPipelineManualData> HighwayPipelineManualDatas { get; set; }

        public IDbSet<HighwayPipelineApprovedData> HighwayPipelineApprovedDatas { get; set; }

        public IDbSet<InnerPipelineData> InnerPipelineDatas { get; set; }

        public IDbSet<InnerPipelinesApprovedDate> InnerPipelinesApprovedDates { get; set; }

        public IDbSet<InProcessUnitData> InProcessUnitDatas { get; set; }

        public IDbSet<InProcessUnitsApprovedDate> InProcessUnitsApprovedDates { get; set; }

        public IDbSet<Message> Messages { get; set; }

        public IDbSet<MonthlyReportType> MonthlyProductTypes { get; set; }

        public IDbSet<RelatedUnitMonthlyConfigs> RelatedUnitMonthlyConfigs { get; set; }

        public IDbSet<UnitApprovedMonthlyData> UnitApprovedMonthlyDatas { get; set; }

        public IDbSet<UnitDailyConfigUnitMonthlyConfig> UnitDailyConfigUnitMonthlyConfigs { get; set; }

        public IDbSet<UnitManualMonthlyData> UnitManualMonthlyDatas { get; set; }

        public IDbSet<UnitMonthlyConfig> UnitMonthlyConfigs { get; set; }

        public IDbSet<UnitMonthlyData> UnitMonthlyDatas { get; set; }

        public IDbSet<TankStatus> TankStatuses { get; set; }

        public IDbSet<TankStatusData> TankStatusDatas { get; set; }

        public IDbSet<UnitDatasTemp> UnitDatasTemps { get; set; }

        public IDbSet<UnitRecalculatedMonthlyData> UnitRecalculatedMonthlyDatas { get; set; }
        public IDbSet<PhdConfig> PhdConfigs { get; set; }

        public IDbSet<PlanNorm> PlanNorms { get; set; }

        public IDbSet<PlanValue> PlanValues { get; set; }

        public IDbSet<RelatedProductionPlanConfigs> RelatedProductionPlanConfigs { get; set; }

        public IDbSet<ProductionPlanConfigUnitMonthlyConfigFactFractionMembers> ProductionPlanConfigUnitMonthlyConfigFactFractionMembers { get; set; }

        public IDbSet<ProductionPlanConfigUnitMonthlyConfigPlanMembers> ProductionPlanConfigUnitMonthlyConfigPlanMembers { get; set; }

        public IDbSet<UnitTechnologicalMonthlyData> UnitTechnologicalMonthlyDatas { get; set; }


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
            using (var transaction = new TransactionScope(TransactionScopeOption.Required, this.transantionOption))
            {
                if (userName == null)
                {
                    // performed only if some MicroSoft API calls original SaveChanges
                    userName = "System Change";
                }

                var addedRecords = persister.PrepareSaveChanges(this, userName);
                int returnValue = 0;
                returnValue = base.SaveChanges();
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
                logger.Error(ex.Message, this, ex);
                return result.SetErrors(ex.EntityValidationErrors);
            }
            catch(DbUpdateException ex)
            {
                logger.Error(ex.Message, this, ex);
                var sqlException = ex.InnerException.InnerException as System.Data.SqlClient.SqlException;

                if ( (sqlException != null) && (sqlException.Number == 2601 || sqlException.Number == 2627))
                {
                    return result.SetErrors(new List<ValidationResult>{new ValidationResult( Errors.UnikConstraintViolation)});
                }
                else
                {
                    return result.SetErrors(new List<ValidationResult> { new ValidationResult(Errors.SavingDataError) });
                }
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

        protected override DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            if (typeof(System.ComponentModel.DataAnnotations.IValidatableObject).IsAssignableFrom(entityEntry.Entity.GetType()))
            {
                var type = entityEntry.Entity.GetType();
                if(IsProxy(type))
                {
                    type = entityEntry.Entity.GetType().BaseType;
                }

                MethodInfo method = this.GetType().GetMethod("GetNavigationProperties");
                MethodInfo generic = method.MakeGenericMethod(type);
                var navigationProperties = generic.Invoke(this, new object[] { entityEntry.Entity }) as List<PropertyInfo>;

                foreach (var property in navigationProperties)
                {
                    if (typeof(System.Collections.IEnumerable).IsAssignableFrom(property.PropertyType))
                    {
                        this.Entry(entityEntry.Entity).Collection(property.Name);
                    }
                    else
                    {
                        this.Entry(entityEntry.Entity).Reference(property.Name);
                    }
                }

                var result = new DbEntityValidationResult(entityEntry, ((IValidatableObject)entityEntry.Entity).Validate(new System.ComponentModel.DataAnnotations.ValidationContext(entityEntry.Entity)).Select(x => new DbValidationError(x.MemberNames.FirstOrDefault(), x.ErrorMessage)));
                if (!result.IsValid)
                {
                    return result;
                }
            }

            return base.ValidateEntity(entityEntry, items);
        }

        /// <summary>
        /// Determines whether the specified type is proxy.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private bool IsProxy(Type type)
        {
            return type != null && System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(type) != type;
        }

        public List<PropertyInfo> GetNavigationProperties<T>(T entity) where T : class
        {
            var entityType = typeof(T);
            var elementType = ((IObjectContextAdapter)this).ObjectContext.CreateObjectSet<T>().EntitySet.ElementType;
            return elementType.NavigationProperties.Select(property => entityType.GetProperty(property.Name)).ToList();
        }
    }
}
