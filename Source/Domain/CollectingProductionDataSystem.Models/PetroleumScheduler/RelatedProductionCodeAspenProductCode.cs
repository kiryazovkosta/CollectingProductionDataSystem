using CollectingProductionDataSystem.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Models.PetroleumScheduler
{
    public class RelatedProductionCodeAspenProductCode : IEntity
    {
        public int Id { get; set; }

        public int ProductCode { get; set; }

        public string AspenCode { get; set; }
    }
}
