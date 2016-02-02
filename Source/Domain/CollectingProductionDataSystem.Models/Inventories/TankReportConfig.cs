using CollectingProductionDataSystem.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Models.Contracts;

namespace CollectingProductionDataSystem.Models.Inventories
{
    public class TankReportConfig : DeletableEntity, IEntity
    {
        public int Id { get; set; }

        //public int TankConfigId { get; set; }

        //public virtual TankConfig TankConfig { get; set; }

        public bool IsExpediotion { get; set; }

        public bool IsProduction { get; set; }

        public bool IsBlending { get; set; }

        public bool IsOther { get; set; }

        public int TankReportConfigDataId { get; set; }

        public virtual TankReportConfigData TankReportConfigData { get; set; }
    }
}
