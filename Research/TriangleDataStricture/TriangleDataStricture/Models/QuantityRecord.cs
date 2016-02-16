/// <summary>
/// Summary description for RelatedRecords
/// </summary>
namespace TriangleDataStricture.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class QuantityRecord
    {
        public int FuelId { get; set; }

        public DateTime RecordMonth { get; set; }

        public decimal Quantity { get; set; }

    }
}
