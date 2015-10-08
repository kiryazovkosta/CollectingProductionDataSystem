using System;
using System.Collections.Generic;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using CollectingProductionDataSystem.Models.Nomenclatures;
using CollectingProductionDataSystem.Models.Productions;
using System.ComponentModel;

namespace CollectingProductionDataSystem.Models.Productions
{
    public partial class UnitConfig : DeletableEntity, IEntity
    {
        private ICollection<UnitsData> unitsDatas;
        private ICollection<UnitsInspectionData> unitsInspectionDatas;

        public UnitConfig()
        {
            this.unitsDatas = new HashSet<UnitsData>();
            this.unitsInspectionDatas = new HashSet<UnitsInspectionData>();
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
        public string AggregateParameter { get; set; }
        public bool IsCalculated { get; set; }
        public string PreviousShiftTag { get; set; }
        public string Notes { get; set; }
        public decimal? MaximumCost { get; set; }
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
        public virtual ICollection<UnitsData> UnitsDatas 
        { 
            get {return this.unitsDatas; } 
            set {this.unitsDatas = value; }
        }
        public virtual ICollection<UnitsInspectionData> UnitsInspectionDatas 
        {
            get { return this.unitsInspectionDatas; }
            set { this.unitsInspectionDatas = value; } 
        }
    }
}
