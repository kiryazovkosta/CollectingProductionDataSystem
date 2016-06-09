namespace CollectingProductionDataSystem.Models.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDateable
    {
         DateTime RecordTimestamp { get; set; }
    }
}
