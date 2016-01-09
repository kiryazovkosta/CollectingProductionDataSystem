namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    using System.Data.Entity;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Models.Transactions.HighwayPipelines;
    using CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using System.Diagnostics;
    using System.Data.Entity.Infrastructure;
    using Resources = App_GlobalResources.Resources;

    public class HighwayPipelinesController : AreaBaseController
    {
        public HighwayPipelinesController(IProductionData dataParam) 
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
                if ( !this.data.HighwayPipelineDatas.All().Where(x => x.RecordTimestamp == date).Any())
                {
                    var pipelinesConfigs = this.data.HighwayPipelineConfigs.All().ToList();
                    foreach (var pipelinesConfig in pipelinesConfigs)
                    {
                        this.data.HighwayPipelineDatas.Add(new HighwayPipelineData
                        {
                            RecordTimestamp = date.Value,
                            HighwayPipelineConfigId = pipelinesConfig.Id,
                            ProductName = pipelinesConfig.Product.Name,
                            ProductCode = pipelinesConfig.Product.Code,
                            Volume = 0,
                            Mass = 0,
                        });   
                    }

                    this.data.SaveChanges(this.UserProfile.UserName);
                }
                var highwayPipesData = this.data.HighwayPipelineDatas
                    .All()
                    .Include(x => x.HighwayPipelineConfig)
                    .Where(x => x.RecordTimestamp == date)
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
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([DataSourceRequest]DataSourceRequest request, HighwayPipelinesDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var exsistRecord = this.data.HighwayPipelineDatas.All().Where(x => x.Id == model.Id).First();
                exsistRecord.Volume = model.Volume;
                exsistRecord.Mass = model.Mass;
                this.data.HighwayPipelineDatas.Update(exsistRecord);
                try
                    {
                        IEfStatus result = this.data.SaveChanges(UserProfile.UserName);
                        if (!result.IsValid)
                        {
                            result.ToModelStateErrors(this.ModelState);
                        }
                    }
                    catch (DbUpdateException)
                    {
                        this.ModelState.AddModelError("", "Записът не можа да бъде осъществен. Моля опитайте на ново!");
                    }
                    catch(Exception ex)
                    {
                        this.ModelState.AddModelError("", ex.ToString());
                    }
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}