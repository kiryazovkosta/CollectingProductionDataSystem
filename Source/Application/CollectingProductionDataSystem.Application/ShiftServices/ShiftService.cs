namespace CollectingProductionDataSystem.Application.ShiftServices
{
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Data.Contracts;
    using System;
    using System.Linq;

    public class ShiftService : IShiftService
    {
        private readonly IProductionData data;

        public ShiftService(IProductionData dataParam)
        {
            this.data = dataParam;
        }

        public int GetPreviousShiftAndDate(DateTime tagretDay, int targetShiftId, out DateTime previousShiftTargetDay)
        {
            var foundPreviousShift = false;
            var previousShiftId = -1;
            previousShiftTargetDay = DateTime.MinValue;

            var shifts = this.data.Shifts.All().ToList().OrderByDescending(s => s.EndTicks);
            var currentShift = shifts.FirstOrDefault(s => s.Id == targetShiftId);
            var currentShiftEndTime = tagretDay.Date + currentShift.EndTime;
            foreach (var shift in shifts)
            {
                var generatedShiftEndTime = tagretDay.Date + shift.EndTime;
                var differenceInDates = generatedShiftEndTime.CompareTo(currentShiftEndTime);
                if (differenceInDates < 0)
                {
                    foundPreviousShift = true;
                    previousShiftId = shift.Id;
                    previousShiftTargetDay = tagretDay.Date;
                    break;
                }
            }

            if (!foundPreviousShift)
            {
                var previousDay = tagretDay.AddDays(-1);
                foreach (var shift in shifts)
                {
                    var generatedShiftEndTime = previousDay.Date + shift.EndTime;
                    if (generatedShiftEndTime.CompareTo(currentShiftEndTime) < 0)
                    {
                        foundPreviousShift = true;
                        previousShiftId = shift.Id;
                        previousShiftTargetDay = previousDay;
                        break;
                    }
                }   
            }

            return previousShiftId;
        }
    }
}
