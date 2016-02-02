namespace CollectingProductionDataSystem.Models.Inventories
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TankReportConfigData : DeletableEntity, IEntity
    {
        public int Id { get; set; }

        public DateTime RecordTimestamp { get; set; }

        //public int TankReportConfigId { get; set; }

        //public virtual TankReportConfig TankReportConfig { get; set; }

        public bool IsExpediotion { get; set; }

        public bool IsProduction { get; set; }

        public bool IsBlending { get; set; }

        public bool IsOther { get; set; }
    }
}
