namespace CollectingProductionDataSystem.Models.Mec
{
    using System.ComponentModel.DataAnnotations;

    public class Unit : EntityBase
    {
        public int Id { get; set; }

        /// <summary>
        /// Шифър
        /// </summary>
        [MaxLength(20)]
        public string Shifar { get; set; }

        /// <summary>
        /// Име на шифър
        /// </summary>
        [MaxLength(150)]
        public string ShifarName { get; set; }

        /// <summary>
        /// Код работно място
        /// </summary>
        public int WorkingPlaceId { get; set; }

        /// <summary>
        /// Вид на прибора)
        /// </summary>
        public DeviceType DeviceType { get; set; }

        /// <summary>
        /// Kод на формулата
        /// </summary>
        public int FormulaId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal max_razhod { get; set; }

        /// <summary>
        /// Плътност при нормални условия
        /// </summary>
        public decimal platnost_nu { get; set; }

        /// <summary>
        /// Разчетна плътност
        /// </summary>
        public decimal razchet_platnost { get; set; }

        /// <summary>
        /// Разчетно налягане
        /// </summary>
        public decimal razchet_naliagane { get; set; }

        /// <summary>
        /// Разчетна температура
        /// </summary>
        public decimal razchet_temperatura { get; set; }
        
        /// <summary>
        /// Разчетен коефициент на свиваемост
        /// </summary>
        public decimal razchet_ks { get; set; }

        /// <summary>
        /// Вид на продукта
        /// </summary>
        public string vid_produkt { get; set; }

        /// <summary>
        /// Цифреност на брояча
        /// </summary>
        public int cifri_broiach { get; set; }

        /// <summary>
        /// Изчисляема или не за позицията
        /// </summary>
        public int izchisliaemost { get; set; }

        /// <summary>
        /// Диаметър
        /// </summary>
        public decimal diametar { get; set; }

        /// <summary>
        /// Номенклатурен номер на продукта
        /// </summary>
        public string nomenklaturen_nomer { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string identifikator { get; set; }

        /// <summary>
        /// Входен/изходен
        /// </summary>
        public string vhod_izhod { get; set; }

        /// <summary>
        /// Пореден номер входен/изходен
        /// </summary>
        public string pn_vhod_izhod { get; set; }

        /// <summary>
        /// Вид гориво
        /// </summary>
        public string vid_gorivo { get; set; }

        /// <summary>
        /// Разходна норма - горива
        /// </summary>
        public string rn_goriva { get; set; }

        /// <summary>
        /// Разходна норма - топлоенергия
        /// </summary>
        public string rn_toplo { get; set; }

        /// <summary>
        /// Разходна норма - електроенергия
        /// </summary>
        public string rn_elektro { get; set; }

        /// <summary>
        /// Разходна норма - оборотна вода
        /// </summary>
        public string rn_ov  { get; set; }

        /// <summary>
        /// Разходна норма - въздух
        /// </summary>
        public string rn_vazdih { get; set; }

        /// <summary>
        /// Разходна норма - азот
        /// </summary>
        public string rn_azot { get; set; }

        /// <summary>
        /// Материален баланс - енергия
        /// </summary>
        public string mb_energia { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ProcessUnitId { get; set; }

            /// <summary>
        /// 
        /// </summary>
        public virtual ProcessUnit ProcessUnit { get; set; }
    }
}
