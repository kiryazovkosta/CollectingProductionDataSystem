namespace CollectingProductionDataSystem.Application.ProductionDataServices
{
    using System;
    using System.Linq;

    public class FormulaArguments
    {
        /// <summary>
        /// PL-ПЛОЩ/ПОКАЗАНИЕ НА БРОЯЧА
        /// </summary>
        public double? InputValue { get; set; }

        /// <summary>
        /// P-НАЛЯГАНЕ
        /// </summary>
        public double? Pressure { get; set; }

        /// <summary>
        /// T-ТЕМПЕРАТУРА
        /// </summary>
        public double? Temperature { get; set; }

        /// <summary>
        /// D-ПЛЪТНОСТ
        /// </summary>
        public double? Density { get; set; }

        /// <summary>
        /// D(2) - МАКС. РАЗХОД (ОТ ПАСП.ДАННИ)
        /// </summary>
        public double? MaximumFlow{ get; set; }

        /// <summary>
        /// D(4) - РАЗЧЕТНА ПЛЪТНОСТ (ОТ ПАСП.ДАННИ)
        /// </summary>
        public double? EstimatedDensity { get; set; }

        /// <summary>
        /// D(5) - РАЗЧЕТНО НАЛЯГАНЕ (ОТ ПАСП.ДАННИ)
        /// </summary>
        public double? EstimatedPressure { get; set; }

        /// <summary>
        /// D(6) - РАЗЧЕТНА ТЕМПЕРАТУРА (ОТ ПАСП.ДАННИ)
        /// </summary>
        public double? EstimatedTemperature { get; set; }

        /// <summary>
        /// D(7) - РАЗЧЕТЕН КОЕФ. СВИВАЕМОСТ (ОТ ПАСП.ДАННИ)
        /// </summary>
        public double? EstimatedCompressibilityFactor { get; set; }

        /// <summary>
        /// PL1 - СТАРО ПОКАЗАНИЕ НА БРОЯЧА
        /// </summary>
        public double? OldValue { get; set; }

        /// <summary>
        /// Коригиращ фактор алфа
        /// </summary>
        public double? FactorAlpha { get; set; }

    }
}
