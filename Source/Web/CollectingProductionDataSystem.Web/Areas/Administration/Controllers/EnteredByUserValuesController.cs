namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    public class EnteredByUserValuesController : AreaBaseController
    {
        public EnteredByUserValuesController(IProductionData dataParam)
            : base(dataParam)
        {

        }

        // GET: Administration/EnteredByUserValues
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var shifts = this.data.Shifts.All().ToDictionary(x => x.Id, x => x.Name);
            var result = data.UnitEnteredForCalculationDatas.All()
                .Include(x => x.UnitsData)
                .Include(x => x.UnitsData.UnitConfig)
                .Include(x => x.UnitsData.UnitConfig.ProcessUnit)
                .OrderByDescending(x => x.Id)
                .ToList()
                .Select(y => new EnteredByUserValueViewModel
                {
                    Id = y.Id,
                    Code = y.UnitsData.UnitConfig.Code, 
                    CreatedFrom = y.CreatedFrom, 
                    CreatedOn = y.CreatedOn, 
                    Name = y.UnitsData.UnitConfig.Name, 
                    ProcessUnitName = y.UnitsData.UnitConfig.ProcessUnit.FullName, 
                    OldValue = y.OldValue, 
                    NewValue = y.NewValue,
                    RecordTimestamp = y.UnitsData.RecordTimestamp,
                    ShiftName = shifts[y.UnitsData.ShiftId]
                });
            return Json(result.ToDataSourceResult(request, ModelState, Mapper.Map<EnteredByUserValueViewModel>));
        }
    }
}