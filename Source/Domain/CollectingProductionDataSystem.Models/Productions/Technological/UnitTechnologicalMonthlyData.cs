namespace CollectingProductionDataSystem.Models.Productions.Technological
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UnitTechnologicalMonthlyData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public int UnitMonthlyConfigId { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Factory { get; set; }
        public int FactoryId { get; set; }
        public string ProcessUnit { get; set; }
        public int ProcessUnitId { get; set; }
        public string MaterialType { get; set; }
        public string MeasurementUnit { get; set; }
        public string DetailedMaterialType { get; set; }
        public decimal PlanValue { get; set; }
        public decimal PlanPercentage { get; set; }
        public decimal FactValue { get; set; }
        public decimal FactPercentage { get; set; }
        public decimal FactValueDifference { get; set; }
        public decimal FactPercentageDifference { get; set; }
        public decimal YearValue { get; set; }
        public decimal YearPercentage { get; set; }
        public decimal YearValueDifference { get; set; }
        public decimal YearPercentageDifference { get; set; }
    }
}
