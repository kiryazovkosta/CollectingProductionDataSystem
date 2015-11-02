using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ManyToManySelfRelation.Models.Mapping;
using ManyToManySelfRelation.Migrations;

namespace ManyToManySelfRelation.Models
{
    public partial class AppContext : DbContext
    {

        static AppContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppContext,Configuration>());
        }

        public AppContext()
            : base("Name=TestSelfRelationManyToManyContext")
        {
            this.Database.Log = System.Console.WriteLine;
        }

        public DbSet<Record> Records { get; set; }

        public DbSet<RelatedRecords> RelatedRecords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RecordMap());
            modelBuilder.Configurations.Add(new RelatedRecordsMap());

        }
    }
}
