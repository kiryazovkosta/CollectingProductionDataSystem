namespace CollectingProductionDataSystem.Models.Productions
{
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Models.Abstract;
    using System;
    using CollectingProductionDataSystem.Models.Contracts;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;

    public class ProductionPlanConfig : DeletableEntity, IEntity, IValidatableObject
    {
        private ICollection<UnitDailyConfig> unitsDailyConfigs;
        private ICollection<ProductionPlanData> productionPlanDatas;
        private ICollection<PlanNorm> planNorms; //Monthly data for production plan from RPMS system
        private ICollection<UnitMonthlyConfig> unitMonthlyConfigs;
        private ICollection<ProductionPlanConfigUnitMonthlyConfigPlanMembers> productionPlanConfigUnitMonthlyConfigPlanMembers;
        private ICollection<ProductionPlanConfigUnitMonthlyConfigFactFractionMembers> productionPlanConfigUnitMonthlyConfigFactFractionMembers;

        public ProductionPlanConfig()
        {
            this.unitsDailyConfigs = new HashSet<UnitDailyConfig>();
            this.productionPlanDatas = new HashSet<ProductionPlanData>();
            this.planNorms = new HashSet<PlanNorm>();
            this.unitMonthlyConfigs = new HashSet<UnitMonthlyConfig>();
            this.productionPlanConfigUnitMonthlyConfigPlanMembers = new HashSet<ProductionPlanConfigUnitMonthlyConfigPlanMembers>();
            this.productionPlanConfigUnitMonthlyConfigFactFractionMembers = new HashSet<ProductionPlanConfigUnitMonthlyConfigFactFractionMembers>();

        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Percentages { get; set; }

        public string QuantityPlanFormula { get; set; }

        public string QuantityPlanMembers { get; set; }

        public string QuantityFactFormula { get; set; }

        public string QuantityFactMembers { get; set; }

        public string MonthlyValuePlanFormula { get; set; }

        public virtual ICollection<ProductionPlanConfigUnitMonthlyConfigPlanMembers> ProductionPlanConfigUnitMonthlyConfigPlanMembers
        {
            get { return this.productionPlanConfigUnitMonthlyConfigPlanMembers; }
            set { this.productionPlanConfigUnitMonthlyConfigPlanMembers = value; }
        }

        public string MonthlyFactFractionFormula { get; set; }

        public virtual ICollection<ProductionPlanConfigUnitMonthlyConfigFactFractionMembers> ProductionPlanConfigUnitMonthlyConfigFactFractionMembers
        {
            get { return this.productionPlanConfigUnitMonthlyConfigFactFractionMembers; }
            set { this.productionPlanConfigUnitMonthlyConfigFactFractionMembers = value; }
        }


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

        public virtual ICollection<PlanNorm> PlanNorms
        {
            get { return this.planNorms; }
            set { this.planNorms = value; }
        }

        public virtual ICollection<UnitMonthlyConfig> UnitMonthlyConfigs
        {
            get { return this.unitMonthlyConfigs; }
            set { this.unitMonthlyConfigs = value; }
        }

        public bool IsSummaryOfProcessing { get; set; }

        public string UsageRateFormula { get; set; }

        public string UsageRateMembers { get; set; }

        public int? MaterialTypeId { get; set; }

        public virtual MaterialType MaterialType { get; set; }

        public int? MaterialDetailTypeId { get; set; }
        public virtual MaterialDetailType MaterialDetailType { get; set; }

        public int? MeasureUnitId { get; set; }

        public bool IsPropductionPlan { get; set; }
        public bool IsMonthlyPlan { get; set; }

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