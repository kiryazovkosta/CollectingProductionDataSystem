namespace CollectingProductionDataSystem.Web.InputModels
{
    using System;
    using System.Linq;

    public class ProcessUnitConfirmShiftInputModel
    {
        public DateTime? date { get; set; }
        public int? processUnitId { get; set; }
        public int? shiftId { get; set; }
        public bool IsConfirmed { get; set; }
    }
}