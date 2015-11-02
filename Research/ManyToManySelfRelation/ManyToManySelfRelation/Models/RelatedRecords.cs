/// <summary>
/// Summary description for RelatedRecords
/// </summary>
namespace ManyToManySelfRelation.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class RelatedRecords
    {
        public int RecordId { get; set; }
        public int RelatedRecordId { get; set; }

        public virtual Record Record { get; set; }

        public virtual Record RelatedRecord { get; set; }
    }
}
