namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
using CollectingProductionDataSystem.Models.Inventories;
using CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels;

    public class TanksStatusesController : AreaBaseController
    {

        public TanksStatusesController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        [HttpGet]
        public ActionResult TanksStatusesData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReadTanksStatusesData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? areaId, int? parkId)
        {
            var dbResult = this.data.Tanks.All().Include(x => x.Park).Include(x => x.TankStatusDatas).GroupJoin(
                this.data.TankStatusDatas.All(),
                p => p.Id,
                c => c.TankConfigId,
                (p, g) => g
                    .Select(c => new StatusOfTankViewModel()
                    {
                        Id = p.Id,
                        TankName = p.TankName,
                        ParkName = p.Park.Name,
                        RecordTimestamp = c.RecordTimestamp,
                        Status = c.TankStatus
                    })
                    .OrderByDescending(w => w.RecordTimestamp)
                    .FirstOrDefault()
                )
                .Select(g => g);


            var kendoResult = dbResult.ToDataSourceResult(request, ModelState);
            kendoResult.Data = Mapper.Map<IEnumerable<StatusOfTankViewModel>, IEnumerable<TanksStatusesViewModel>>((IEnumerable<StatusOfTankViewModel>)kendoResult.Data);
            return Json(kendoResult);
        }

        private void ValidateInputModel(DateTime? date, int? parkId)
        {
            throw new NotImplementedException();
        }
    }
}