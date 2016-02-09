namespace CollectingProductionDataSystem.Web.Areas.MonthlyReporting.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Resources = App_GlobalResources.Resources;
    using AutoMapper;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Web.Areas.MonthlyReporting.ViewModels;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using CollectingProductionDataSystem.Web.ViewModels.Nomenclatures;

    public class InnerPipelinesController : AreaBaseController
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
                var beginTimeStamp = new DateTime(date.Value.Year, date.Value.Month, 1, 0, 0, 0);
                var endTimeStamp = new DateTime(date.Value.Year, date.Value.Month, DateTime.DaysInMonth(date.Value.Year, date.Value.Month));

                var dbResult = this.data.InnerPipelineDatas
                    .All()
                    .Include(x => x.Product)
                    .Where(x => x.RecordTimestamp >= beginTimeStamp && x.RecordTimestamp <= endTimeStamp);

                var kendoResult = dbResult.ToDataSourceResult(request, ModelState, Mapper.Map<InnerPipelinesDataViewModel>);
                return Json(kendoResult);
            }
            else
            {
                var kendoResult = new List<InnerPipelinesDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create([DataSourceRequest]DataSourceRequest request, InnerPipelinesDataViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = Mapper.Map<InnerPipelineData>(inputViewModel);
                    this.data.InnerPipelineDatas.Add(entity);
                    var result = data.SaveChanges(this.UserProfile.UserName);

                    if (!result.IsValid)
                    {
                        result.ToModelStateErrors(ModelState);
                    }

                    inputViewModel.Id = entity.Id;
                }
                catch (DbUpdateException ex)
                {
                    var entity = this.data.InnerPipelineDatas.All().Include(x=>x.Product)
                                            .FirstOrDefault(x =>
                                                x.RecordTimestamp == inputViewModel.RecordTimestamp
                                                && x.ProductId == inputViewModel.ProductId);
                    if (entity != null)
                    {
                        ModelState.AddModelError("", string.Format(Resources.ErrorMessages.InnerPipelineDataDublicatedRecords, entity.RecordTimestamp, entity.Product.Name));
                    }
                    else 
                    {
                        throw (ex);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message + ex.StackTrace);
                }
            }

            return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, InnerPipelinesDataViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {
                var dbEntity = this.data.InnerPipelineDatas.All().Where(x => x.Id == inputViewModel.Id).FirstOrDefault();
                if (dbEntity == null)
                {
                    ModelState.AddModelError("", string.Format(Resources.ErrorMessages.InvalidRecordUpdate, inputViewModel.Id));
                }
                else
                {
                    Mapper.Map(inputViewModel, dbEntity);
                    this.data.InnerPipelineDatas.Update(dbEntity);
                    var result = this.data.SaveChanges(this.UserProfile.UserName);
                    if (!result.IsValid)
                    {
                        result.ToModelStateErrors(ModelState);
                    }
                }
            }

            return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, InnerPipelinesDataViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {
                var dbEntity = this.data.InnerPipelineDatas.All().Where(x => x.Id == inputViewModel.Id).FirstOrDefault();
                if (dbEntity == null)
                {
                    ModelState.AddModelError("", string.Format(Resources.ErrorMessages.InvalidRecordUpdate, inputViewModel.Id));
                }

                this.data.InnerPipelineDatas.Delete(dbEntity);
                var result = data.SaveChanges(this.UserProfile.UserName);
                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }
            }

            return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
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