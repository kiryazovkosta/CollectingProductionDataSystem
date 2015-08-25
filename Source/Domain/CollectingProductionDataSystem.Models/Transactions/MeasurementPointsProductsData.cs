using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;

namespace CollectingProductionDataSystem.Models.Transactions
{
    public class MeasurementPointsProductsData : AuditInfo, IApprovableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int MeasurementPointsProductsConfigId { get; set; }
        public decimal? Value { get; set; }
        public bool IsApproved { get; set; }
        public virtual MeasurementPointsProductsConfig MeasurementPointsProductsConfig { get; set; }
        
    }
}
