namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.Controllers
{
    using System.Data.Entity;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Models.Transactions.HighwayPipelines;
    using CollectingProductionDataSystem.Web.Areas.ShiftReporting.ViewModels;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Resources = App_GlobalResources.Resources;

    public class HighwayPipelinesReportController : AreaBaseController
    {
        public HighwayPipelinesReportController(IProductionData dataParam) 
            : base(dataParam)
        {
        }

        [HttpGet]
        public ActionResult HighwayPipelinesData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReadHighwayPipelinesData([DataSourceRequest]DataSourceRequest request, DateTime? date)
        {
            ValidateModelState(date);

            if (this.ModelState.IsValid)
            {
                if (!this.data.HighwayPipelineApprovedDatas.All().Where(x => x.RecordDate == date).Any())
                {
                    this.ModelState.AddModelError("", string.Format("Не са потвърдени данните за {0}", date));   
                }

                if (ModelState.IsValid)
                {
                    var highwayPipesData = this.data.HighwayPipelineDatas.All()
                        .Include(x => x.HighwayPipelineConfig)
                        .Where(x => x.RecordTimestamp == date 
                            && (x.Mass > 0 || x.Volume > 0))
                        .ToList();
                    var kendoPreparedResult = Mapper.Map<IEnumerable<HighwayPipelineData>, IEnumerable<HighwayPipelinesDataViewModel>>(highwayPipesData);
                    var kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
                    return Json(kendoResult);    
                }
                else
                {
                    var kendoResult = new List<HighwayPipelinesDataViewModel>().ToDataSourceResult(request, ModelState);
                    return Json(kendoResult);
                }
            }
            else
            {
                var kendoResult = new List<HighwayPipelinesDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }
 
        private void ValidateModelState(DateTime? date)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }
            if (date.HasValue && date.Value.CompareTo(DateTime.Today) > 0)
            {
                this.ModelState.AddModelError("date", Resources.Layout.UnitsDateSelectorFuture);
            }
            if (date != null && DateTime.Today.AddDays(1) < date.Value)
            {
                if (date == null)
                {
                    this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
                }
            }
        }

        private List<string> GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }
    }
}