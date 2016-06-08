namespace CalcultedFieldTest.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CalcultedFieldTest : DbContext
    {
        public CalcultedFieldTest()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<TestTable> TestTables { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
