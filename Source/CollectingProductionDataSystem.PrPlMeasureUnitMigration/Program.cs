using System.Data.Entity;
using CollectingProductionDataSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.PrPlMeasureUnitMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            var productionPlanConfigsErrors = new List<string>();
            using (var data = new CollectingDataSystemDbContext())
            {
                var planConfigs = data.ProductionPlanConfigs.Where(x => x.IsDeleted == false);
                foreach (var planConfig in planConfigs)
                {
                    string code = planConfig.QuantityFactMembers.Split('@').FirstOrDefault();
                    var unitDailyConfig = data.UnitDailyConfigs.FirstOrDefault(x => x.Code == code);
                    if (unitDailyConfig == null)
                    {
                        Console.WriteLine(string.Format("Id: {0} | code: {1}", planConfig.Id, code));
                    }
                    else
                    {
                        var dailyMeasureUnitId = unitDailyConfig.MeasureUnitId;
                        if (dailyMeasureUnitId > 0)
                        {
                            planConfig.MeasureUnitId = dailyMeasureUnitId;
                            if (planConfig.MaterialTypeId>1 && planConfig.MaterialDetailTypeId==null)
                            {
                                planConfig.MaterialDetailTypeId=1;
                            }
                        }
                    }
                }
                data.SaveChanges();
            }
        }
    }
}
