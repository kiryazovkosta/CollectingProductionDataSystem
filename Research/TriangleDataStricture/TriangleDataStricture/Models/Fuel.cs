using System;
using System.Collections.Generic;

namespace TriangleDataStricture.Models
{
    public class Fuel
    {
        private ICollection<QuantityRecord> quantityRecords;
        public Fuel()
        {
            this.quantityRecords = new HashSet<QuantityRecord>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }

        public virtual ICollection<QuantityRecord> QuantityRecords 
        {
            get { return this.quantityRecords; }
            set { this.quantityRecords = value; } 
        }
    }
}
