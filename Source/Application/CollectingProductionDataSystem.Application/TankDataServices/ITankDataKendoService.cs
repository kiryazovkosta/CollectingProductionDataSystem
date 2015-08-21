using System;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Inventories;

namespace CollectingProductionDataSystem.Application.TankDataServices
{
    public interface ITankDataKendoService
    {
        System.Linq.IQueryable<TankData> GetTankDataForDateTime(DateTime? date);
    }
}
