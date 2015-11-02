using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class UnitsConfig
    {
        public UnitsConfig()
        {
            this.UnitsDatas = new List<UnitsData>();
            this.UnitsInspectionDatas = new List<UnitsInspectionData>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Position { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public int ProcessUnitId { get; set; }
        public int DirectionId { get; set; }
        public int MeasureUnitId { get; set; }
        public int MaterialTypeId { get; set; }
        public bool IsMaterial { get; set; }
        public bool IsEnergy { get; set; }
        public bool IsInspectionPoint { get; set; }
        public string CollectingDataMechanism { get; set; }
        public string AggregateGroup { get; set; }
        public bool IsCalculated { get; set; }
        public string PreviousShiftTag { get; set; }
        public string CurrentInspectionDataTag { get; set; }
        public string Notes { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
        public string AggregateParameter { get; set; }
        public Nullable<decimal> MaximumCost { get; set; }
        public Nullable<decimal> EstimatedDensity { get; set; }
        public Nullable<decimal> EstimatedPressure { get; set; }
        public Nullable<decimal> EstimatedTemperature { get; set; }
        public Nullable<decimal> EstimatedCompressibilityFactor { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual MaterialType MaterialType { get; set; }
        public virtual MeasureUnit MeasureUnit { get; set; }
        public virtual ProcessUnit ProcessUnit { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<UnitsData> UnitsDatas { get; set; }
        public virtual ICollection<UnitsInspectionData> UnitsInspectionDatas { get; set; }
    }
}
