namespace CollectingProductionDataSystem.Application.ProductionDataServices
{
    using System;
    using System.Linq;

    public class FormulaArguments
    {
        //public FormulaArguments(string code)
        //{
        //}
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

        /// <summary>
        /// Процент калкулация за изчисление по формула
        /// </summary>
        public double? CalculationPercentage { get; set; }

        /// <summary>
        /// Текст на формула, която се използва при калкулиране на потребителски вход
        /// </summary>
        public string CustomFormulaExpression { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        ///// <summary>
        ///// Уникален номер на позицията в сменен аспект
        ///// </summary>
        //public int UnitConfigId { get; set; }

        //public int ShiftId { get; set; }

        //public DateTime RecordTimestamp { get; set; }
    }
}
