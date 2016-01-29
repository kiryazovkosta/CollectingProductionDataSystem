using System.Data.Entity;
using CollectingProductionDataSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.PrPlMeasureUnitMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var data = new CollectingDataSystemDbContext())
            {
                var planConfigs = data.ProductionPlanConfigs;
                foreach (var planConfig in planConfigs)
                {
                    string code = planConfig.QuantityFactMembers.Split('@').FirstOrDefault();
                    var dailyMeasureUnitId = data.UnitDailyConfigs.FirstOrDefault(x => x.Code == code).MeasureUnitId;
                    if (dailyMeasureUnitId > 0)
                    {
                        planConfig.MeasureUnitId = dailyMeasureUnitId;
                    }
                }

                data.SaveChanges();
            }
        }
    }
}
