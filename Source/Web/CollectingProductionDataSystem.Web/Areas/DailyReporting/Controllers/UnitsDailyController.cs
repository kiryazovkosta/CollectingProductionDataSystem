using System.Data.Entity;
using AutoMapper;
using CollectingProductionDataSystem.Application.UnitsDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Web.ViewModels.Units;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    [Authorize]
    public class UnitsDailyController : AreaBaseController
    {
        private readonly IUnitsDataService unitsData;

        public UnitsDailyController(IProductionData dataParam, IUnitsDataService unitsDataParam) : base(dataParam)
        {
            this.unitsData = unitsDataParam;
        }

        [HttpGet]
        public ActionResult UnitsDailyData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReadDailyUnitsData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId)
        {
            if (this.ModelState.IsValid)
            {
                var dbResult = this.data.UnitsDailyDatas
                    .All()
                    .Include(u => u.UnitsDailyConfig)
                    .Include(u => u.UnitsDailyConfig.MeasureUnit)
                    .Include(u => u.UnitsDailyConfig.ProductType)
                    .Where(u => u.RecordTimestamp == date && u.UnitsDailyConfig.ProcessUnitId == processUnitId)
                    .ToList();
                var kendoResult = dbResult.ToDataSourceResult(request, ModelState);
                kendoResult.Data = Mapper.Map<IEnumerable<UnitsDailyData>, IEnumerable<UnitDailyDataViewModel>>((IEnumerable<UnitsDailyData>)kendoResult.Data);
                return Json(kendoResult);
            }
            else
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }
    }
}