namespace CollectingProductionDataSystem.Web.Areas.MonthlyReporting.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Resources = App_GlobalResources.Resources;
    using AutoMapper;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Web.Areas.MonthlyReporting.ViewModels;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers;

    public class InnerPipelinesController : GenericNomController<InnerPipelineData, InnerPipelinesDataViewModel>
    {
        public InnerPipelinesController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        [HttpGet]
        public ActionResult InnerPipelinesData()
        {
            this.ViewData["products"] = Mapper.Map<IEnumerable<ProductViewModel>>(this.data.Products.All().ToList());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadInnerPipelinesData([DataSourceRequest]DataSourceRequest request, DateTime? date)
        {
            ValidateModelState(date);

            if (this.ModelState.IsValid)
            {
                var beginTimeStamp = new DateTime(date.Value.Year, date.Value.Month, 1, 0, 0, 1);
                var endTimeStamp = new DateTime(date.Value.Year, date.Value.Month, DateTime.DaysInMonth(date.Value.Year, date.Value.Month));

                var dbResult = this.data.InnerPipelineDatas
                    .All()
                    .Include(x => x.Product)
                    .Where(x => x.RecordTimestamp >= beginTimeStamp && x.RecordTimestamp <= endTimeStamp)
                    .ToList();
                    
                var kendoPreparedResult = Mapper.Map<IEnumerable<InnerPipelineData>, IEnumerable<InnerPipelinesDataViewModel>>(dbResult);
                var kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
            else
            {
                var kendoResult = new List<InnerPipelinesDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }

        private void ValidateModelState(DateTime? date)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }
        }
    }
}