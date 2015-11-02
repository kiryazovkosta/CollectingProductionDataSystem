namespace CollectingProductionDataSystem.Models.Inventories
{
    using System;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System.ComponentModel.DataAnnotations.Schema;
    using MathExpressions.Application;

    public partial class TankData : AuditInfo, IApprovableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int TankConfigId { get; set; }
        public int ParkId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? LiquidLevel { get; set; }
        public decimal? TotalObservableVolume { get; set; }
        public decimal? ProductLevel { get; set; }
        public decimal? GrossObservableVolume { get; set; }
        public decimal? ObservableDensity { get; set; }
        public decimal? GrossStandardVolume { get; set; }
        public decimal? AverageTemperature { get; set; }
        public decimal? NetStandardVolume { get; set; }
        public decimal? ReferenceDensity { get; set; }
        public decimal? WeightInAir { get; set; }
        public decimal? WeightInVacuum { get; set; }
        public decimal? FreeWaterLevel { get; set; }
        public decimal? FreeWaterVolume { get; set; }
        public decimal? MaxVolume { get; set; }
        public decimal? AvailableRoom { get; set; }
        public decimal? UnusableResidueLevel { get; set; }
        public string CorrectionFactor { get; set; }
        public bool IsApproved { get; set; }
        public virtual TankConfig TankConfig { get; set; }
        public virtual Product Product { get; set; }

        [NotMapped]
        public decimal? CorrectedLiquidLevel
        { 
            get
            {
                return GetCorrectedLevelValue(this.LiquidLevel);
            }
        }

        [NotMapped]
        public decimal? CorrectedProductLevel
        { 
            get
            {
                return GetCorrectedLevelValue(this.ProductLevel);
            }
        }

        [NotMapped]
        public decimal? CorrectedFreeWaterLevel
        { 
            get
            {
                return GetCorrectedLevelValue(this.FreeWaterLevel);
            }
        }
        private decimal? GetCorrectedLevelValue(decimal? levelParam)
        {
            if (levelParam.HasValue)
            {
                var calc = new Calculator();
                var inputParams = new Dictionary<string, double>();
                inputParams.Add("p0", (double)levelParam);  
                var formula = string.Format("p.p0 {0}", this.TankConfig.CorrectionFactor);
                var value = calc.Calculate(formula, "p", inputParams.Count, inputParams);
                return (decimal)value;
            }
            else
            {
                return levelParam;
            }
        }
    }
}
