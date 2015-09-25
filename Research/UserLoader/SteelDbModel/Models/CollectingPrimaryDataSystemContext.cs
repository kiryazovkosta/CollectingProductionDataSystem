using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SteelDbModel.Models.Mapping;

namespace SteelDbModel.Models
{
    public partial class CollectingPrimaryDataSystemContext : DbContext
    {
        static CollectingPrimaryDataSystemContext()
        {
            Database.SetInitializer<CollectingPrimaryDataSystemContext>(null);
        }

        public CollectingPrimaryDataSystemContext()
            : base("Name=CollectingPrimaryDataSystemContext")
        {
        }

        public DbSet<Area> Areas { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AuditLogRecord> AuditLogRecords { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<EditReason> EditReasons { get; set; }
        public DbSet<Factory> Factories { get; set; }
        public DbSet<Ikunk> Ikunks { get; set; }
        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<MeasurementPoint> MeasurementPoints { get; set; }
        public DbSet<MeasurementPointsProductsConfig> MeasurementPointsProductsConfigs { get; set; }
        public DbSet<MeasurementPointsProductsData> MeasurementPointsProductsDatas { get; set; }
        public DbSet<MeasureUnit> MeasureUnits { get; set; }
        public DbSet<Park> Parks { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<ProcessUnit> ProcessUnits { get; set; }
        public DbSet<ProductionPlanConfig> ProductionPlanConfigs { get; set; }
        public DbSet<ProductionShift> ProductionShifts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<TankConfig> TankConfigs { get; set; }
        public DbSet<TankData> TankDatas { get; set; }
        public DbSet<TankMasterProduct> TankMasterProducts { get; set; }
        public DbSet<TanksManualData> TanksManualDatas { get; set; }
        public DbSet<TransportType> TransportTypes { get; set; }
        public DbSet<UnitsApprovedDailyData> UnitsApprovedDailyDatas { get; set; }
        public DbSet<UnitsApprovedData> UnitsApprovedDatas { get; set; }
        public DbSet<UnitsConfig> UnitsConfigs { get; set; }
        public DbSet<UnitsDailyConfig> UnitsDailyConfigs { get; set; }
        public DbSet<UnitsDailyData> UnitsDailyDatas { get; set; }
        public DbSet<UnitsData> UnitsDatas { get; set; }
        public DbSet<UnitsInspectionData> UnitsInspectionDatas { get; set; }
        public DbSet<UnitsManualDailyData> UnitsManualDailyDatas { get; set; }
        public DbSet<UnitsManualData> UnitsManualDatas { get; set; }
        public DbSet<Zone> Zones { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AreaMap());
            modelBuilder.Configurations.Add(new AspNetRoleMap());
            modelBuilder.Configurations.Add(new AspNetUserClaimMap());
            modelBuilder.Configurations.Add(new AspNetUserLoginMap());
            modelBuilder.Configurations.Add(new AspNetUserMap());
            modelBuilder.Configurations.Add(new AuditLogRecordMap());
            modelBuilder.Configurations.Add(new DirectionMap());
            modelBuilder.Configurations.Add(new EditReasonMap());
            modelBuilder.Configurations.Add(new FactoryMap());
            modelBuilder.Configurations.Add(new IkunkMap());
            modelBuilder.Configurations.Add(new MaterialTypeMap());
            modelBuilder.Configurations.Add(new MeasurementPointMap());
            modelBuilder.Configurations.Add(new MeasurementPointsProductsConfigMap());
            modelBuilder.Configurations.Add(new MeasurementPointsProductsDataMap());
            modelBuilder.Configurations.Add(new MeasureUnitMap());
            modelBuilder.Configurations.Add(new ParkMap());
            modelBuilder.Configurations.Add(new PlantMap());
            modelBuilder.Configurations.Add(new ProcessUnitMap());
            modelBuilder.Configurations.Add(new ProductionPlanConfigMap());
            modelBuilder.Configurations.Add(new ProductionShiftMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new ProductTypeMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new TankConfigMap());
            modelBuilder.Configurations.Add(new TankDataMap());
            modelBuilder.Configurations.Add(new TankMasterProductMap());
            modelBuilder.Configurations.Add(new TanksManualDataMap());
            modelBuilder.Configurations.Add(new TransportTypeMap());
            modelBuilder.Configurations.Add(new UnitsApprovedDailyDataMap());
            modelBuilder.Configurations.Add(new UnitsApprovedDataMap());
            modelBuilder.Configurations.Add(new UnitsConfigMap());
            modelBuilder.Configurations.Add(new UnitsDailyConfigMap());
            modelBuilder.Configurations.Add(new UnitsDailyDataMap());
            modelBuilder.Configurations.Add(new UnitsDataMap());
            modelBuilder.Configurations.Add(new UnitsInspectionDataMap());
            modelBuilder.Configurations.Add(new UnitsManualDailyDataMap());
            modelBuilder.Configurations.Add(new UnitsManualDataMap());
            modelBuilder.Configurations.Add(new ZoneMap());
        }
    }
}
