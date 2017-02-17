namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class PlanValue : DeletableEntity, IEntity, ICloneable<PlanValue>, IProcessUnitCangeable
    {
        public int Id { get; set; }
        public int ProcessUnitId { get; set; }
        public virtual ProcessUnit ProcessUnit { get; set; }
        public DateTime Month { get; set; }
        public decimal Value { get; set; }
        public decimal? ValueLiquid { get; set; }
        public PlanValue Clone()
        {
            return new PlanValue()
            {
                ProcessUnitId = this.ProcessUnitId,
                Month = this.Month,
                Value = this.Value,
                ValueLiquid = this.ValueLiquid
            };
        }

        public ProcessUnit HistorycalProcessUnit { get; set; }
    }
}
