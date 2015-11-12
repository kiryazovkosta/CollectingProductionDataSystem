using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    public class EventsController : AreaBaseController
    {
        public EventsController(IProductionData dataParam)
            : base(dataParam)
        {
        }
        // GET: Administration/Events
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var result = data.Events.All().OrderByDescending(x => x.EventTime).ToList();
            return Json(result.ToDataSourceResult(request, ModelState, Mapper.Map<EventViewModel>));
        }

        [HttpGet]
        public virtual ActionResult Details(string eventId)
        {
            var result = Mapper.Map<EventViewModel>(data.Events.All().FirstOrDefault(x => x.EventId == eventId));
            return PartialView(result);
        }
    }
}