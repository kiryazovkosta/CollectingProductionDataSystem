namespace CollectingProductionDataSystem.Phd2SqlProductionData.Models
{
    public class Tank
    {
        /// <summary>
        /// Резервоар
        /// </summary>
        public int TankId { get; set; }
        /// <summary>
        /// Търговско наименование на продукта
        /// </summary>
        public string PhdTagProductId { get; set; }
        /// <summary>
        /// Ниво на течност
        /// </summary>
        public string PhdTagLiquidLevel { get; set; } 
        /// <summary>
        /// Ниво на продукт
        /// </summary>
        public string PhdTagProductLevel { get; set; }
        /// <summary>
        /// Ниво на вода
        /// </summary>
        public string PhdTagFreeWaterLevel { get; set; }    
        /// <summary>
        /// Плътност на продукт при 15°С
        /// </summary>
        public string PhdTagReferenceDensity  { get; set; }
        /// <summary>
        /// Обем на продукт
        /// </summary>
        public string PhdTagNetStandardVolume { get; set; }
        /// <summary>
        /// Маса на продукта във въздух
        /// </summary>
        public string PhdTagWeightInAir { get; set; }
        /// <summary>
        /// Маса на продукта във вакуум
        /// </summary>
        public string PhdTagWeightInVaccum { get; set; }
        /// <summary>
        /// Резервоарен парк,
        /// </summary>
        public int ParkId { get; set; }
        /// <summary>
        /// Контролна точка
        /// </summary>
        public string ControlPoint { get; set; }
        /// <summary>
        /// 
        /// Ниво на мъртъв остатък (МО),
        /// </summary>
        public decimal UnusableResidueLevel { get; set; }
    }
}
