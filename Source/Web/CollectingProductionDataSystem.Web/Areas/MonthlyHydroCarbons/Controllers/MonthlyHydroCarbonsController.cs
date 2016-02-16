using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Application.MonthlyServices;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions.Mounthly;
using CollectingProductionDataSystem.Web.Areas.MonthlyHydroCarbons.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyHydroCarbons.Controllers
{
    public class MonthlyHydroCarbonsController : AreaBaseController
    {
        public MonthlyHydroCarbonsController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        // GET: MonthlyHydroCarbons/MonthlyHydroCarbons
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReadMonthlyUnitsData([DataSourceRequest]DataSourceRequest request, DateTime date)
        {
            try
            {
                this.data.DbContext.DbContext.Database.ExecuteSqlCommand("DELETE FROM [CollectingPrimaryDataSystem].[dbo].[UnitMonthlyDatas]");
                var service = DependencyResolver.Current.GetService<UnitMothlyDataService>();
                var result = service.CalculateMonthlyDataForReportType(date, false, 1);
                data.UnitMonthlyDatas.BulkInsert(result, "Test");
                data.SaveChanges("Test");
                var dbResult = data.UnitMonthlyDatas.All().ToList();
                return Json(dbResult.ToDataSourceResult(request, ModelState,  Mapper.Map<MonthlyHydroCarbonViewModel>));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }

        }
    }
}