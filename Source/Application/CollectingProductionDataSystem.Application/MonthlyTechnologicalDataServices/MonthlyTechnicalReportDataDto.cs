namespace CollectingProductionDataSystem.Application.MonthlyTechnologicalDataServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MonthlyTechnicalReportDataDto
    {
        /// <summary>
        /// HEADER
        /// </summary>
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Factory { get; set; }
        public int FactoryId { get; set; }
        public string ProcessUnit { get; set; }
        public int ProcessUnitId { get; set; }
        public string MaterialType { get; set; }
        public string MeasurementUnit { get; set; }
        public string DetailedMaterialType { get; set; }

        /// <summary>
        /// PLAN MONTH
        /// </summary>
        public decimal PlanValue { get; set; }
        public decimal PlanPercentage { get; set; }

        /// <summary>
        /// FACT MONTH
        /// </summary>
        public decimal FactValue { get; set; }
        public decimal FactPercentage { get; set; }
        public decimal FactValueDifference { get; set; }
        public decimal FactPercentageDifference { get; set; }

        /// <summary>
        /// FACT YEAR
        /// </summary>
        public decimal YearValue { get; set; }
        public decimal YearPercentage { get; set; }
        public decimal YearValueDifference { get; set; }
        public decimal YearPercentageDifference { get; set; }

        public override string ToString()
        {
            return string.Format("{0};{1};{2};{3};{4};{5};{6};;{7};{8};;{9};{10};{11};{12};;{13};{14};{15};{16}",
                Id,Code,Name,Factory,ProcessUnit,MaterialType,MeasurementUnit
                ,PlanValue,PlanPercentage
                ,FactValue,FactPercentage,FactValueDifference,FactPercentageDifference
                ,YearValue,YearPercentage,YearValueDifference,YearPercentageDifference
                );
        }
    }
}
