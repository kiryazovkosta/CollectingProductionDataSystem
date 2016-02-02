namespace CollectingProductionDataSystem.Web.Areas.MonthlyReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Data.Contracts;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using AutoMapper;
    using CollectingProductionDataSystem.Web.ViewModels.Nomenclatures;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using CollectingProductionDataSystem.Web.Areas.MonthlyReporting.ViewModels;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using Resources = App_GlobalResources.Resources;

    public class InProcessUnitsController : AreaBaseController
    {
        public InProcessUnitsController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        [HttpGet]
        public ActionResult InProcessUnitsData()
        {
            this.ViewData["products"] = Mapper.Map<IEnumerable<ProductViewModel>>(this.data.Products.All().ToList());
            this.ViewData["processunits"] = Mapper.Map<IEnumerable<ProcessUnitViewModel>>(this.data.ProcessUnits.All().ToList());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadInProcessUnitsData([DataSourceRequest]DataSourceRequest request, DateTime? date)
        {
            return Json(date);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create([DataSourceRequest]DataSourceRequest request, InProcessUnitsViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = Mapper.Map<InProcessUnitData>(inputViewModel);
                    this.data.InProcessUnitDatas.Add(entity);
                    var result = data.SaveChanges(this.UserProfile.UserName);

                    if (!result.IsValid)
                    {
                        result.ToModelStateErrors(ModelState);
                    }

                    inputViewModel.Id = entity.Id;
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
        public virtual ActionResult Update([DataSourceRequest]DataSourceRequest request, InProcessUnitsViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {
                var dbEntity = this.data.InProcessUnitDatas.All().Where(x => x.Id == inputViewModel.Id).FirstOrDefault();
                if(dbEntity == null)
                {
                    ModelState.AddModelError("", string.Format(Resources.ErrorMessages.InvalidRecordUpdate, inputViewModel.Id));
                }
                else
                {
                    Mapper.Map(inputViewModel, dbEntity);
                    this.data.InProcessUnitDatas.Update(dbEntity);
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
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, InProcessUnitsViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {
                var dbEntity = this.data.InProcessUnitDatas.All().Where(x => x.Id == inputViewModel.Id).FirstOrDefault();
                if(dbEntity == null)
                {
                    ModelState.AddModelError("", string.Format(Resources.ErrorMessages.InvalidRecordUpdate, inputViewModel.Id));
                }

                this.data.InProcessUnitDatas.Delete(dbEntity);
                var result = data.SaveChanges(this.UserProfile.UserName);
                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }
            }

            return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
        }
    }
}