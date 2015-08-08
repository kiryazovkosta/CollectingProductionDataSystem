namespace CollectingProductionDataSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Unit : EntityBase
    {
        private ICollection<UnitsData> unitsData;

        public Unit()
        {
            this.unitsData = new HashSet<UnitsData>();
        }

        [Key]
        public int Id { get; set; }

        public string Position { get; set; }

        public string Name { get; set; }

        public int ProcessUnitId { get; set; }

        public int DirectionId { get; set; }

        public int MeasureUnitId { get; set; }

        public int ProductTypetId { get; set; }

        public int MaterialTypeId { get; set; }

        public bool IsMaterial { get; set; }

        public bool IsEnergy { get; set; }

        [DefaultValue(false)]
        public bool IsInspectionPoint { get; set; }

        [MaxLength(1)]
        public string CollectingDataMechanism { get; set; }

        public string CurrentShiftTag { get; set; }

        public string PreviousShiftTag { get; set; }

        public string CurrentDayTag { get; set; }

        public string PreviousDayTag { get; set; }

        public string CurrentMonthTag { get; set; }

        public string AggregateGroup { get; set; }

        [MaxLength(200)]
        public string Notes { get; set; }

        public virtual ProcessUnit ProcessUnit { get; set; }

        public virtual Direction Direction { get; set; }

        public virtual MeasureUnit MeasureUnit { get; set; }

        public virtual MaterialType MaterialType { get; set; }

        public virtual ProductType ProductType { get; set; }
        
        public virtual ICollection<UnitsData> UnitsData
        {
            get { return this.unitsData; }
            set { this.unitsData = value; }
        }
    }
}

