namespace CollectingProductionDataSystem.Application.Contracts
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;

    public interface IShiftService
    {
        int GetPreviousShiftAndDate(DateTime tagretDay, int targetShiftId, out DateTime previousShiftTargetDay);
    }
}