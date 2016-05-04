namespace CollectingProductionDataSystem.Models.Productions
{
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Models.Abstract;
    using System;
    using CollectingProductionDataSystem.Models.Contracts;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public class ProductionPlanConfig : DeletableEntity, IEntity, IValidatableObject
    {
        private ICollection<UnitDailyConfig> unitsDailyConfigs;
        private ICollection<ProductionPlanData> productionPlanDatas;

        public ProductionPlanConfig()
        {
            this.unitsDailyConfigs = new HashSet<UnitDailyConfig>();
            this.productionPlanDatas = new HashSet<ProductionPlanData>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Percentages { get; set; }

        public string QuantityPlanFormula { get; set; }

        public string QuantityPlanMembers { get; set; }

        public string QuantityFactFormula { get; set; }

        public string QuantityFactMembers { get; set; }

        public int ProcessUnitId { get; set; }

        public int Position { get; set; }

        public virtual ProcessUnit ProcessUnit { get; set; }

        public virtual ICollection<UnitDailyConfig> UnitsDailyConfigsFact
        {
            get { return this.unitsDailyConfigs; }
            set { this.unitsDailyConfigs = value; }
        }

        public virtual ICollection<ProductionPlanData> ProductionPlanDatas
        {
            get { return this.productionPlanDatas; }
            set { this.productionPlanDatas = value; }
        }

        public bool IsSummaryOfProcessing { get; set; }

        public string UsageRateFormula { get; set; }

        public string UsageRateMembers { get; set; }

        public int? MaterialTypeId { get; set; }

        public virtual MaterialType MaterialType { get; set; }

        public int? MaterialDetailTypeId { get; set; }
        public virtual MaterialDetailType MaterialDetailType { get; set; }

        public int? MeasureUnitId { get; set; }
        
        public bool IsPercentagesPlanVisibleInChast { get; set; }
        public bool IsPercentagesFactVisibleInChast { get; set; }
        public bool IsQuanityPlanVisibleInChast { get; set; }
        public bool IsQuantityFactVisibleInChast { get; set; }

        public virtual MeasureUnit MeasureUnit { get; set; }

        public string Code { get; set; }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A collection that holds failed-validation information.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationObject = validationContext.ObjectInstance as ProductionPlanConfig;
            if (validationObject.MaterialType != null)
            {
                if (validationObject.MaterialType.IsDetailRequired && (validationObject.MaterialDetailTypeId == null || validationObject.MaterialDetailTypeId == 0))
                {
                    yield return new ValidationResult(string.Format(Resources.ModelErrors.Required, Resources.ModelErrors.MaterialDetailType), new string[] { "MaterialDetailTypeId" });
                }
            }
            else
            {
                yield return new ValidationResult(string.Format(Resources.ModelErrors.Required, Resources.ModelErrors.MaterialType), new string[] { "MaterialTypeId" });
            }
        }
    }
}