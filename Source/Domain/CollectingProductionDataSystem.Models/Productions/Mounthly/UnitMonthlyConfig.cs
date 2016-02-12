namespace CollectingProductionDataSystem.Models.Productions.Mounthly
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;

    public class UnitMonthlyConfig : DeletableEntity, IEntity, IValidatableObject
    {
        private ICollection<UnitMonthlyData> unitMonthlyDatas;
        private ICollection<RelatedUnitMonthlyConfigs> relatedUnitMonthlyConfigs;
        private ICollection<UnitDailyConfigUnitMonthlyConfig> unitDailyConfigUnitMonthlyConfig;

        public UnitMonthlyConfig()
        {
            this.unitMonthlyDatas = new HashSet<UnitMonthlyData>();
            this.relatedUnitMonthlyConfigs = new HashSet<RelatedUnitMonthlyConfigs>();
            this.unitDailyConfigUnitMonthlyConfig = new HashSet<UnitDailyConfigUnitMonthlyConfig>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int ProcessUnitId { get; set; }

        public int ProductId { get; set; }

        public int MonthlyReportTypeId { get; set; }

        public int MeasureUnitId { get; set; }

        public int ProductTypeId { get; set; }

        public string AggregationFormula { get; set; }

        public bool AggregationCurrentLevel { get; set; }

        //public string AggregationMembers { get; set; }

        [DefaultValue(true)]
        public bool IsEditable { get; set; }

        public virtual MeasureUnit MeasureUnit { get; set; }

        public virtual ProcessUnit ProcessUnit { get; set; }

        public virtual Product Product { get; set; }

        public virtual MonthlyReportType MonthlyReportType { get; set; }

        public virtual DailyProductType ProductType { get; set; }

        public bool IsConverted { get; set; }

        public virtual ICollection<UnitMonthlyData> UnitMonthlyDatas
        {
            get
            {
                return this.unitMonthlyDatas;
            }
            set
            {
                this.unitMonthlyDatas = value;
            }
        }

        public virtual ICollection<RelatedUnitMonthlyConfigs> RelatedUnitMonthlyConfigs
        {
            get
            {
                return this.relatedUnitMonthlyConfigs;
            }
            set
            {
                this.relatedUnitMonthlyConfigs = value;
            }
        }

        public virtual ICollection<UnitDailyConfigUnitMonthlyConfig> UnitDailyConfigUnitMonthlyConfigs
        {
            get
            {
                return this.unitDailyConfigUnitMonthlyConfig;
            }
            set
            {
                this.unitDailyConfigUnitMonthlyConfig = value;
            }
        }

        public string ProcessUnitAlias { get; set; }

        //public int? MaterialTypeId { get; set; }

        //public virtual MaterialType MaterialType { get; set; }

        //public int? MaterialDetailTypeId { get; set; }

        //public virtual MaterialDetailType MaterialDetailType { get; set; }

        public bool NotATotalizedPosition { get; set; }

        public bool IsManualEntry { get; set; }


         /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A collection that holds failed-validation information.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var currentObject = validationContext.ObjectInstance as UnitMonthlyConfig;
            if (currentObject.IsManualEntry == false && string.IsNullOrEmpty(currentObject.AggregationFormula))
            {
                yield return new ValidationResult(string.Format(Resources.ModelErrors.Required, "AggregationFormula"));
            }
        }

    }
}