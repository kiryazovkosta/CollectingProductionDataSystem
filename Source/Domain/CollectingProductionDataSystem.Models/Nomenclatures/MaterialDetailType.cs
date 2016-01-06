namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Productions;

    public class MaterialDetailType : DeletableEntity, IEntity
    {
        private ICollection<UnitDailyConfig> unitDailyConfigs;

        public MaterialDetailType()
        {
            this.unitDailyConfigs = new HashSet<UnitDailyConfig>();
        }
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        public int? Position { get; set; }

        public string Name { get; set; }

        public int MaterialTypeId { get; set; }

        public virtual MaterialType MaterialType { get; set; }

        public virtual ICollection<UnitDailyConfig> UnitDailyConfigs
        {
            get { return this.unitDailyConfigs; }
            set { this.unitDailyConfigs = value; }
        }
    }
}