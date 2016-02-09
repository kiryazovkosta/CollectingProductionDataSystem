using System;
using System.Collections.Generic;

namespace ManyToManySelfRelation.Models
{
    public partial class Record
    {
        private ICollection<RelatedRecords> relatedRecords;
        public Record()
        {
            this.relatedRecords = new HashSet<RelatedRecords>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<RelatedRecords> RelatedRecords 
        {
            get { return this.relatedRecords; }
            set { this.relatedRecords = value; } 
        }
    }
}
