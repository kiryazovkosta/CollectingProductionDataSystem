using System;
using System.Collections.Generic;
using System.Text;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Models.Productions.Mounthly;

namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    public partial class MaterialType : DeletableEntity, IEntity
    {
        private ICollection<UnitDailyConfig> unitDailyConfigs;
        private ICollection<MaterialDetailType> materialDetailTypes;
        private ICollection<ProductionPlanConfig> productionPlanConfigs;
        private ICollection<UnitMonthlyConfig> unitMonthlyConfigs = new HashSet<UnitMonthlyConfig>();

        public MaterialType()
        {
            this.Units = new HashSet<UnitConfig>();
            this.unitDailyConfigs = new HashSet<UnitDailyConfig>();
            this.materialDetailTypes = new HashSet<MaterialDetailType>();
            this.productionPlanConfigs = new HashSet<ProductionPlanConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UnitConfig> Units { get; set; }

        public bool IsDetailRequired { get; set; }

        public string SortableName
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(this.Id.ToString("d2"));
                sb.Append(" ");
                sb.Append(this.Name);
                return sb.ToString();
            }
        }

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

        public virtual ICollection<ProductionPlanConfig> ProductionPlanConfigs
        {
            get { return this.productionPlanConfigs; }
            set { this.productionPlanConfigs = value; }
        }
    }
}
