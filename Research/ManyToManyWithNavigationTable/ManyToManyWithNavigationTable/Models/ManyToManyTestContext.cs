using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ManyToManyWithNavigationTable.Models.Mapping;

namespace ManyToManyWithNavigationTable.Models
{
    public partial class ManyToManyTestContext : DbContext
    {
        static ManyToManyTestContext()
        {
            Database.SetInitializer<ManyToManyTestContext>(null);
        }

        public ManyToManyTestContext()
            : base("Name=ManyToManyTestContext")
        {
        }

        public DbSet<LeftTable> LeftTables { get; set; }
        public DbSet<RightTable> RightTables { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new LeftTableMap());
            modelBuilder.Configurations.Add(new RightTableMap());
           
    }
}
