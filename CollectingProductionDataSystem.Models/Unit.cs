namespace CollectingProductionDataSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using CollectingProductionDataSystem.Models;
    using System.ComponentModel.DataAnnotations.Schema;
using CollectingProductionDataSystem.Common;

    public class Unit : IActiveEntity
    {
        private ICollection<UnitsData> unitsData;

        private ICollection<UnitsInspectionData> inspectionPointData;

        public Unit()
        {
            this.unitsData = new HashSet<UnitsData>();
            this.inspectionPointData = new HashSet<UnitsInspectionData>();
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

        public string AggregateGroup { get; set; }

        [MaxLength(50)]
        public string CurrentShiftTag { get; set; }
        
        [MaxLength(50)]
        public string PreviousShiftTag { get; set; }
        
        [MaxLength(50)]
        public string CurrentDayTag { get; set; }
        
        [MaxLength(50)]
        public string PreviousDayTag { get; set; }

        [MaxLength(50)]
        public string CurrentMonthTag { get; set; }

        [MaxLength(50)]
        public string CurrentInspectionDataTag { get; set; }

        [MaxLength(200)]
        public string Notes { get; set; }

        [Index]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

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

        public virtual ICollection<UnitsInspectionData> InspectionPointData
        {
            get { return this.inspectionPointData; }
            set { this.inspectionPointData = value; }
        }
    }
}