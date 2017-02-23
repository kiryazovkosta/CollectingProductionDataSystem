namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using CollectingProductionDataSystem.Models.Productions;
    using Resources = App_GlobalResources.Resources;

    public class ProcessUnitController : GenericNomController<ProcessUnit, ProcessUnitViewModel>
    {
        public ProcessUnitController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        public override ActionResult Index()
        {
            ViewData["factories"] = data.Factories.All().ToList();
            return base.Index();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Create([DataSourceRequest]DataSourceRequest request, ProcessUnitViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //var entity = Mapper.Map<TModel>(inputViewModel);
                    // this way we create proxy object associated with database which can
                    // update navigation properties during change of the foreign keys
                    var processUnit = new ProcessUnit();
                    
                    Mapper.Map(inputViewModel, processUnit);

                    //TODO: Comment this next week
                    processUnit.ActiveFrom = new DateTime(processUnit.ActiveFrom.Year, processUnit.ActiveFrom.Month, 1);

                    this.data.ProcessUnits.Add(processUnit);

                    var result = data.SaveChanges(this.UserProfile.UserName);

                    if (!result.IsValid)
                    {
                        result.ToModelStateErrors(ModelState);
                    }

                    inputViewModel.Id = processUnit.Id;
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
        public override ActionResult Update([DataSourceRequest]DataSourceRequest request, ProcessUnitViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {

                var processUnit = this.data.ProcessUnits.GetById(inputViewModel.Id);

                if (processUnit == null)
                {
                    ModelState.AddModelError("", string.Format(Resources.ErrorMessages.InvalidRecordUpdate, inputViewModel.Id));
                }
                else
                {
                    Mapper.Map(inputViewModel, processUnit);

                    //TODO: Comment this next week
                    processUnit.ActiveFrom = new DateTime(processUnit.ActiveFrom.Year, processUnit.ActiveFrom.Month, 1);

                    this.data.ProcessUnits.Update(processUnit);

                    var result = data.SaveChanges(this.UserProfile.UserName);

                    if (!result.IsValid)
                    {
                        result.ToModelStateErrors(ModelState);
                    }
                }
            }

            return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
        }
    }
}
