using System;
using System.Collections.Generic;

namespace ManyToManyWithNavigationTable.Models
{
    public partial class RightTable
    {
        public RightTable()
        {
            this.LeftTables = new List<LeftTable>();
        }

        public int Id { get; set; }
        public string Data { get; set; }
        public virtual ICollection<LeftTable> LeftTables { get; set; }
    }
}
