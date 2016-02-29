namespace CollectingProductionDataSystem.Web.Areas.OverseersReporting.Controllers
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;

    public class AjaxController : AreaBaseController
    {
        public AjaxController(IProductionData productionDataParam)
            :base(productionDataParam)
        { }
    }
}