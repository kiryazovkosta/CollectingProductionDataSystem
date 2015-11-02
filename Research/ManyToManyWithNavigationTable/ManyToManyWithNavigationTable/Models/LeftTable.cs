using System;
using System.Collections.Generic;

namespace ManyToManyWithNavigationTable.Models
{
    public partial class LeftTable
    {
        public LeftTable()
        {
            this.RightTables = new List<RightTable>();
        }

        public int Id { get; set; }
        public string Data { get; set; }
        public virtual ICollection<RightTable> RightTables { get; set; }
    }
}
