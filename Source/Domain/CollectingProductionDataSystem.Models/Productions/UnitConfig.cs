namespace CollectingProductionDataSystem.Models.Productions
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using Productions;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class UnitConfig : DeletableEntity, IEntity, IAggregatable, IConvertable, ICloneable<UnitConfig>
    {
        private ICollection<UnitsData> unitsDatas;
        private ICollection<RelatedUnitConfigs> relatedUnitConfigs;
        private ICollection<UnitConfigUnitDailyConfig> unitConfigUnitDailyConfigs;
        private ICollection<UnitDatasTemp> unitDatasTemps;

        public UnitConfig()
        {
            this.unitsDatas = new HashSet<UnitsData>();
            this.relatedUnitConfigs = new HashSet<RelatedUnitConfigs>();
            this.unitConfigUnitDailyConfigs = new HashSet<UnitConfigUnitDailyConfig>();
            this.unitDatasTemps = new HashSet<UnitDatasTemp>();
        }

        public int Id { get; set; }
        public string Code { get; set; }

        public string Position { get; set; }

        public string Name { get; set; }

        public int? ShiftProductTypeId { get; set; }
        public int ProductId { get; set; }
        public int ProcessUnitId { get; set; }
        public int DirectionId { get; set; }
        public int MeasureUnitId { get; set; }
        public int MaterialTypeId { get; set; }
        public string CollectingDataMechanism { get; set; }
        public string CalculatedFormula { get; set; }
        public string AggregateGroup { get; set; }
        public string AggregationMembers { get; set; }
        public string CustomFormulaExpression { get; set; }
        public bool IsCalculated { get; set; }
        public string PreviousShiftTag { get; set; }
        public string Notes { get; set; }
        public decimal? MaximumFlow { get; set; }
        public decimal? EstimatedDensity { get; set; }
        public decimal? EstimatedPressure { get; set; }
        public decimal? EstimatedTemperature { get; set; }
        public decimal? EstimatedCompressibilityFactor { get; set; }

        [DefaultValue(true)]
        public bool IsEditable { get; set; }

        public virtual Direction Direction { get; set; }
        public virtual MaterialType MaterialType { get; set; }
        public virtual MeasureUnit MeasureUnit { get; set; }
        public virtual ProcessUnit ProcessUnit { get; set; }
        public virtual Product Product { get; set; }
        public virtual ShiftProductType ShiftProductType { get; set; }

        /// <summary>
        /// Gets or sets the is converted.
        /// </summary>
        /// <value>The is converted.</value>
        public bool IsConverted { get; set; }

        public virtual ICollection<UnitsData> UnitsDatas
        {
            get { return this.unitsDatas; }
            set { this.unitsDatas = value; }
        }

        public virtual ICollection<RelatedUnitConfigs> RelatedUnitConfigs
        {
            get { return this.relatedUnitConfigs; }
            set { this.relatedUnitConfigs = value; }
        }

        public virtual ICollection<UnitConfigUnitDailyConfig> UnitConfigUnitDailyConfigs
        {
            get { return this.unitConfigUnitDailyConfigs; }
            set { this.unitConfigUnitDailyConfigs = value; }
        }

        public virtual ICollection<UnitDatasTemp> UnitDatasTemps
        {
            get { return this.unitDatasTemps; }
            set { this.unitDatasTemps = value; }
        }

        public decimal? StartupValue { get; set; }

        public bool IsMemberOfShiftsReport { get; set; }

        public int? EnteredMeasureUnitId { get; set; }
        public virtual MeasureUnit EnteredMeasureUnit { get; set; }

        public decimal? CalculationPercentage { get; set; }
        public PrimaryDataSourceType? DataSource { get; set; }

        public string ProcessUnitAlias { get; set; }

        public bool NotATotalizedPosition { get; set; }

        public int NeedToGetOnlyLastShiftValue { get; set; }

        public bool RemoveAndAddInsteadUpdate { get; set; }
        public bool HasRawDataReading { get; set; }

        //<summary>
        //If this property is set to true and value is negative number need to set value to zero. :)
        //</summary>
        public bool NeedToSetValueToZero { get; set; }

        public UnitConfig Clone()
        {
            var result = new UnitConfig()
            {

                Code = this.Code,
                Position = this.Position,
                Name = this.Name,
                ShiftProductTypeId = this.ShiftProductTypeId,
                ProductId = this.ProductId,
                ProcessUnitId = this.ProcessUnitId,
                DirectionId = this.DirectionId,
                MeasureUnitId = this.MeasureUnitId,
                MaterialTypeId = this.MaterialTypeId,
                CollectingDataMechanism = this.CollectingDataMechanism,
                CalculatedFormula = this.CalculatedFormula,
                AggregateGroup = this.AggregateGroup,
                AggregationMembers = this.AggregationMembers,
                CustomFormulaExpression = this.CustomFormulaExpression,
                IsCalculated = this.IsCalculated,
                PreviousShiftTag = this.PreviousShiftTag,
                Notes = this.Notes,
                MaximumFlow = this.MaximumFlow,
                EstimatedDensity = this.EstimatedDensity,
                EstimatedPressure = this.EstimatedPressure,
                EstimatedTemperature = this.EstimatedTemperature,
                EstimatedCompressibilityFactor = this.EstimatedCompressibilityFactor,
                IsEditable = this.IsEditable,
                ShiftProductType = this.ShiftProductType,
                NeedToSetValueToZero = this.NeedToSetValueToZero,
                StartupValue = this.StartupValue,
                IsMemberOfShiftsReport = this.IsMemberOfShiftsReport,
                EnteredMeasureUnitId = this.EnteredMeasureUnitId,
                EnteredMeasureUnit = this.EnteredMeasureUnit,
                CalculationPercentage = this.CalculationPercentage,
                DataSource = this.DataSource,
                ProcessUnitAlias = this.ProcessUnitAlias,
                NotATotalizedPosition = this.NotATotalizedPosition,
                NeedToGetOnlyLastShiftValue = this.NeedToGetOnlyLastShiftValue,
                RemoveAndAddInsteadUpdate = this.RemoveAndAddInsteadUpdate,
                HasRawDataReading = this.HasRawDataReading
            };

            result.RelatedUnitConfigs = new HashSet<RelatedUnitConfigs>();
            result.UnitConfigUnitDailyConfigs = new HashSet<UnitConfigUnitDailyConfig>();

            return result;

            //TODO: Make RelatedUnitConfigs 
            //public virtual ICollection<RelatedUnitConfigs> RelatedUnitConfigs
            //{
            //    get { return this.relatedUnitConfigs; }
            //    set { this.relatedUnitConfigs = value; }
            //}

            //TODO: Make UnitConfigUnitDailyConfig in next level 
            //public virtual ICollection<UnitConfigUnitDailyConfig> UnitConfigUnitDailyConfigs
            //{
            //    get { return this.unitConfigUnitDailyConfigs; }
            //    set { this.unitConfigUnitDailyConfigs = value; }
            //}


        }
    }
}
