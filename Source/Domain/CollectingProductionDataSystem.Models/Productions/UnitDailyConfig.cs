namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class UnitDailyConfig : DeletableEntity, IEntity, IAggregatable, IConvertable, IValidatableObject
    {
        private ICollection<UnitsDailyData> unitsDailyDatas;
        private ICollection<RelatedUnitDailyConfigs> relatedUnitDailyConfigs;
        private ICollection<UnitConfigUnitDailyConfig> unitConfigUnitDailyConfig;

        public UnitDailyConfig()
        {
            this.unitsDailyDatas = new HashSet<UnitsDailyData>();
            this.relatedUnitDailyConfigs = new HashSet<RelatedUnitDailyConfigs>();
            this.unitConfigUnitDailyConfig = new HashSet<UnitConfigUnitDailyConfig>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ProcessUnitId { get; set; }

        public int ProductId { get; set; }
        public int DailyProductTypeId { get; set; }
        public int MeasureUnitId { get; set; }
        public string AggregationFormula { get; set; }
        public bool AggregationCurrentLevel { get; set; }
        public string AggregationMembers { get; set; }

        [DefaultValue(true)]
        public bool IsEditable { get; set; }

        public virtual MeasureUnit MeasureUnit { get; set; }
        public virtual ProcessUnit ProcessUnit { get; set; }
        public virtual Product Product { get; set; }
        public virtual DailyProductType DailyProductType { get; set; }

        public bool IsConverted { get; set; }

        public virtual ICollection<UnitsDailyData> UnitsDailyDatas
        {
            get { return this.unitsDailyDatas; }
            set { this.unitsDailyDatas = value; }
        }

        public virtual ICollection<RelatedUnitDailyConfigs> RelatedUnitDailyConfigs
        {
            get { return this.relatedUnitDailyConfigs; }
            set { this.relatedUnitDailyConfigs = value; }
        }

        public virtual ICollection<UnitConfigUnitDailyConfig> UnitConfigUnitDailyConfigs
        {
            get { return this.unitConfigUnitDailyConfig; }
            set { this.unitConfigUnitDailyConfig = value; }
        }

        public string ProcessUnitAlias { get; set; }

        public int? MaterialTypeId { get; set; }

        public virtual MaterialType MaterialType { get; set; }

        public int? MaterialDetailTypeId { get; set; }
        public virtual MaterialDetailType MaterialDetailType { get; set; }

        public bool NotATotalizedPosition { get; set; }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A collection that holds failed-validation information.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationObject = validationContext.ObjectInstance as UnitDailyConfig;
            if (validationObject.MaterialType != null)
            {
                if (validationObject.MaterialType.IsDetailRequired && (validationObject.MaterialDetailTypeId == null || validationObject.MaterialDetailTypeId == 0))
                {
                    yield return new ValidationResult(string.Format(Resources.ModelErrors.Required, Resources.ModelErrors.MaterialDetailType), new string[]{ "MaterialDetailTypeId" });
                }
            }
            else 
            {
                yield return new ValidationResult(string.Format(Resources.ModelErrors.Required, Resources.ModelErrors.MaterialType), new string[]{ "MaterialTypeId" });
            }
        }
    }
}
