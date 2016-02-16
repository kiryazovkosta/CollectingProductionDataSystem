using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using CollectingProductionDataSystem.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Data.Common;

namespace CollectingProductionDataSystem.Application.HighwayPipelinesDataServices
{
    public class HighwayPipelinesDataService : IHighwayPipelinesDataService
    {
        private readonly IProductionData data;

        public HighwayPipelinesDataService(IProductionData dataParam)
        {
            this.data = dataParam;
        }

        public IEfStatus CheckIfPreviousDaysAreReady(DateTime targetDate)
        {
            var status = new EfStatus();
            if (targetDate.Day > 1)
            {
                var validationResults = new List<ValidationResult>();

                var firstDay = new DateTime(targetDate.Year, targetDate.Month, 1);
                var previousDay = targetDate.AddDays(-1);

                var approvedInPeriod = this.data.HighwayPipelineApprovedDatas
                    .All()
                    .Where(x => x.RecordDate >= firstDay 
                        && x.RecordDate <= previousDay)
                    .ToDictionary(x => x.RecordDate, x => x);

                while (firstDay <= previousDay)
                {
                    if (!approvedInPeriod.ContainsKey(firstDay))
                    {
                        if (validationResults.Count == 0)
                        {
                            validationResults.Add(new ValidationResult("Не са потвърдени наличностите за следните дати:"));     
                        }
                        validationResults.Add(new ValidationResult(string.Format("{0:dd.MMMM.yyyy}", firstDay)));       
                    }
                    firstDay = firstDay.AddDays(1);  
                }

                if (validationResults.Count > 0)
                {
                    status.SetErrors(validationResults);    
                }
            }

            return status;
        }

        public IEnumerable<HighwayPipelineDto> ReadDataForDay(DateTime targetDay)
        {
            return this.data.HighwayPipelineConfigs.All()
                .Include(x => x.Product)
                .Include(x => x.HighwayPipelineDatas)
                .Select(x => new HighwayPipelineDto
                {
                    HighwayPipeline = x,
                    Quantity = x.HighwayPipelineDatas
                            .Where(z => z.RecordTimestamp <= targetDay && z.IsDeleted == false)
                            .OrderByDescending(y => y.RecordTimestamp)
                            .FirstOrDefault()
                });
        }
    }
}
