using System;
using System.Collections.Generic;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    public partial class MaterialType : DeletableEntity, IEntity
    {
        private ICollection<UnitDailyConfig> unitDailyConfigs;
        private ICollection<MaterialDetailType> materialDetailTypes;

        public MaterialType()
        {
            this.Units = new HashSet<UnitConfig>();
            this.unitDailyConfigs = new HashSet<UnitDailyConfig>();
            this.materialDetailTypes = new HashSet<MaterialDetailType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UnitConfig> Units { get; set; }

        public bool IsDetailRequired { get; set; }

        public virtual ICollection<UnitDailyConfig> UnitDailyConfigs
        {
            get { return this.unitDailyConfigs; }
            set { this.unitDailyConfigs = value; }
        }

        public ICollection<MaterialDetailType> MaterialDetailTypes
        {
            get { return this.materialDetailTypes; }
            set { this.materialDetailTypes = value; }
        }
    }
}
