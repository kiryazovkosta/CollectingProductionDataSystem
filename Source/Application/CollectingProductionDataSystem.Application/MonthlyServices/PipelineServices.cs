/// <summary>
/// Summary description for PipelineServices
/// </summary>
namespace CollectingProductionDataSystem.Application.MonthlyServices
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Data.Contracts;

    public class PipelineServices : CollectingProductionDataSystem.Application.Contracts.IPipelineServices
    {
        private readonly IProductionData data;
        public PipelineServices(IProductionData dataParam)
        {
            this.data = dataParam;
        }

        public IEnumerable<InnerPipelineDto> ReadDataForMonth(DateTime targetMonth)
        {
            return this.data.Products.All().Where(x=>x.IsAvailableForInnerPipeLine == true)
                          .Include(x => x.ProductType)
                          .Include(x => x.InnerPipelineDatas)
                          .Select(x => new InnerPipelineDto
                          {
                              Product = x,
                              Quantity = x.InnerPipelineDatas
                                      .Where(z => z.RecordTimestamp <= targetMonth && z.IsDeleted == false)
                                      .OrderByDescending(y => y.RecordTimestamp)
                                      .FirstOrDefault()
                          });
        }
    }
}
