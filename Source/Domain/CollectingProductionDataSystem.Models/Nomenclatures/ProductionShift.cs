namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public partial class ProductionShift : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public int BeginMinutes { get; set; }
        public int OffsetMinutes { get; set; }
    }
}
