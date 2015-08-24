/// <summary>
/// Summary description for TankDataKendoService
/// </summary>
namespace CollectingProductionDataSystem.Application.TankDataServices
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;

    public class TankDataKendoService : ITankDataKendoService
    {
        private readonly IProductionData data;
        
        public TankDataKendoService(IProductionData dataParam) 
        {
            this.data = dataParam;
        }
 
        public IQueryable<TankData> GetTankDataForDateTime(DateTime? date)
        {
            IQueryable<TankData> dbResult = this.data.TanksData
                .All()
                .Include(x => x.TankConfig)
                .Include(x => x.TankConfig.Park);
            if (date != null)
            {
                dbResult = dbResult.Where(x => x.RecordTimestamp == date);
            }
            return dbResult;
        }
    }
}
