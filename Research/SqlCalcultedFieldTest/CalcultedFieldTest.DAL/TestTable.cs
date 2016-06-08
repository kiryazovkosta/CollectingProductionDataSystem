namespace CalcultedFieldTest.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TestTable")]
    public partial class TestTable
    {
        public int Id { get; set; }

        public int Value { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? TotalValue { get; set; }
    }
}
