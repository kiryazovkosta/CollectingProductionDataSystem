namespace CollectingProductionDataSystem.Web.Areas.MonthlyTechReporting.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Controllers;

    [Authorize(Roles = "Administrator, OverseerReporter")]
    public abstract class AreaBaseController : BaseController
    {
        public AreaBaseController(IProductionData dataParam)
            : base(dataParam)
        { }
    }
}