using System;
using System.Collections.Generic;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Models.Productions
{
    public partial class ProcessUnit: DeletableEntity, IEntity
    {
        public ProcessUnit()
        {
            this.UnitsConfigs = new HashSet<UnitConfig>();
            this.UnitsAggregateDailyConfigs = new HashSet<UnitsAggregateDailyConfig>();
        }

        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int FactoryId { get; set; }
        public virtual Factory Factory { get; set; }
        public virtual ICollection<UnitConfig> UnitsConfigs { get; set; }
        public virtual ICollection<UnitsAggregateDailyConfig> UnitsAggregateDailyConfigs { get; set; }
    }
}
