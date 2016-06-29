namespace CollectingProductionDataSystem.Web.Areas.MonthlyTechnicalReporting.Controllers
{
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Controllers;
    using System.Web.Mvc;

    [Authorize(Roles="Administrator, MonthlyTechnologicalReporter")]
    public abstract class AreaBaseController : BaseController
    {
        public AreaBaseController(IProductionData dataParam)
            : base(dataParam)
        { }
    }
}