
namespace TriangleDataStricture.Models
{
    using System.Data.Entity;
    using TriangleDataStricture.Migrations;
    using TriangleDataStricture.Models.Mapping;

    public partial class AppContext : DbContext
    {

        static AppContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppContext,Configuration>());
        }

        public AppContext()
            : base("Name=Default")
        {
            //this.Database.Log = System.Console.WriteLine;
        }

        public DbSet<Fuel> Fuels { get; set; }

        public DbSet<QuantityRecord> QuantityRecords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FuelMap());
            modelBuilder.Configurations.Add(new QuantityRecordMap());

        }
    }
}
