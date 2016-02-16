/// <summary>
/// Summary description for RelatedRecordsMap
/// </summary>
namespace TriangleDataStricture.Models.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    class QuantityRecordMap : EntityTypeConfiguration<QuantityRecord>
    {
        public QuantityRecordMap()
        {
            this.HasKey(t => new { t.FuelId, t.RecordMonth });
        }
    }
}
