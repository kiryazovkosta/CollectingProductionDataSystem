namespace CollectingProductionDataSystem.Web.Areas.MonthlyTechReporting.Controllers
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;

    public class AjaxController : AreaBaseController
    {
        public AjaxController(IProductionData productionDataParam)
            : base(productionDataParam)
        { }
    }
}