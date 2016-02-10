/// <summary>
/// Summary description for RelatedRecordsMap
/// </summary>
namespace ManyToManySelfRelation.Models.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    class RelatedRecordsMap : EntityTypeConfiguration<RelatedRecords>
    {
        public RelatedRecordsMap()
        {
            this.HasKey(t => new { t.RecordId, t.RelatedRecordId });

            this.HasRequired(p => p.RelatedRecord).WithMany().WillCascadeOnDelete(false);
        }
    }
}
