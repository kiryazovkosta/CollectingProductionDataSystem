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
                    .Where(x => x.RecordTimestamp >= beginTimeStamp && x.RecordTimestamp <= endTimeStamp)
                    .ToList();
                   
                try
                {
                    var kendoPreparedResult = Mapper.Map<IEnumerable<InnerPipelineData>, IEnumerable<InnerPipelinesDataViewModel>>(dbResult);
                    var kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
                    return Json(kendoResult);
                }
                catch (Exception ex)
                {
                    var kendoResult1 = new List<InnerPipelinesDataViewModel>().ToDataSourceResult(request, ModelState);
                    return Json(kendoResult1);   
                }
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
                //try
                //{
                //    var entity = Mapper.Map<TModel>(inputViewModel);

                //    this.repository.Add(entity);

                //    var result = data.SaveChanges(this.UserProfile.UserName);

                //    if (!result.IsValid)
                //    {
                //        result.ToModelStateErrors(ModelState);
                //    }

                //    inputViewModel.Id = entity.Id;
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError("", ex.Message + ex.StackTrace);
                //}
            }

            return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Update([DataSourceRequest]DataSourceRequest request, InnerPipelinesDataViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {

                //var dbEntity = this.repository.GetById(inputViewModel.Id);

                //if (dbEntity == null)
                //{
                //    ModelState.AddModelError("", string.Format(Resources.ErrorMessages.InvalidRecordUpdate, inputViewModel.Id));
                //}
                //else
                //{
                //    Mapper.Map(inputViewModel, dbEntity);

                //    this.repository.Update(dbEntity);

                //    var result = data.SaveChanges(this.UserProfile.UserName);

                //    if (!result.IsValid)
                //    {
                //        result.ToModelStateErrors(ModelState);
                //    }
                //}
            }

            return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, InnerPipelinesDataViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {
                //this.repository.Delete(inputViewModel.Id);
                //var result = data.SaveChanges(this.UserProfile.UserName);

                //if (!result.IsValid)
                //{
                //    result.ToModelStateErrors(ModelState);
                //}
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