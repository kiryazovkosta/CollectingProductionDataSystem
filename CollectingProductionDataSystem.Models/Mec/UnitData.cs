namespace CollectingProductionDataSystem.Models.Mec
{
    using System;

    public class UnitData
    {
        public long Id { get; set; }

        /// <summary>
        /// Дата на обработка
        /// </summary>
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// Номер на шифър
        /// </summary>
        public int UnitId { get; set; }

        /// <summary>
        /// Шифър
        /// </summary>
        public virtual Unit Unit { get; set;}

        /// <summary>
        /// Площ/показание
        /// </summary>
        public decimal plosht { get; set; }

        /// <summary>
        /// Налягане
        /// </summary>
        public decimal naliagane { get; set; }

        /// <summary>
        /// температура
        /// </summary>
        public decimal temperatura { get; set; }

        /// <summary>
        /// плътност
        /// </summary>
        public decimal platnost { get; set; }

        /// <summary>
        /// състояние на брояча
        /// </summary>
        public int sastoianie { get; set; }

        /// <summary>
        /// начално показание - нов брояч
        /// </summary>
        public int nachalno_pokazanie { get; set; }

        /// <summary>
        /// крайно показание - нов брояч
        /// </summary>
        public int kraino_pokazanie { get; set; }

        /// <summary>
        /// показание след повторно превключване
        /// </summary>
        public int povtorno_pokazanie { get; set; }

        /// <summary>
        /// разход за деня
        /// </summary>
        public decimal razhod_za_denia { get; set; }

        /// <summary>
        /// разход до деня
        /// </summary>
        public decimal razhod_do_denia { get; set; }

        /// <summary>
        /// корегиращо количество
        /// </summary>
        public decimal korekcia { get; set; }

        /// <summary>
        /// корегиращо количество за газове (неизмерена пара)
        /// </summary>
        public decimal korekcia_gazove { get; set; }

        /// <summary>
        /// корегиращо количество ХОВ (пара за пароспътници)
        /// </summary>
        public decimal korekcia_hov { get; set; }

        /// <summary>
        /// пара за топла вода
        /// </summary>
        public decimal para_tv { get; set; }
    }
}
