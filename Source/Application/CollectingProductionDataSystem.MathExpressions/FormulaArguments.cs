namespace CollectingProductionDataSystem.Models.UtilityEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FormulaArguments
    {
        public int Id { get; set; }
        /// <summary>
        /// PL-ПЛОЩ/ПОКАЗАНИЕ НА БРОЯЧА
        /// </summary>
        public double? IndicatorCounter { get; set; }

        /// <summary>
        /// P-НАЛЯГАНЕ
        /// </summary>
        public double? Volume { get; set; }

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

    }
}
