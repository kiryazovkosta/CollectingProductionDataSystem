

namespace CollectingProductionDataSystem.Models.Transactions
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public class ActiveTransactionsData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public decimal Mass { get; set; }
        public int MeasuringPointConfigId { get; set; }
        public virtual MeasuringPointConfig MeasuringPointConfig { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
