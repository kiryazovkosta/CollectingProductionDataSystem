using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using CollectingProductionDataSystem.Constants;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Inventories;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Web.InputModels;
using CollectingProductionDataSystem.Web.ViewModels.Tank;
using CollectingProductionDataSystem.Web.ViewModels.Nomenclatures;
using CollectingProductionDataSystem.Web.ViewModels.Units;
using CollectingProductionDataSystem.Web.ViewModels.Utility;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNet.Identity;
using CollectingProductionDataSystem.Models.Productions.Mounthly;

namespace CollectingProductionDataSystem.Web.Controllers
{
    [Authorize]
    public class AjaxController : BaseController
    {
        public AjaxController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        public JsonResult GetReasons()
        {
            var reasons = this.data.EditReasons.All().ToList();
            var reasonView = Mapper.Map<IEnumerable<EditReasonInputModel>>(reasons);
            return this.Json(reasonView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAreas()
        {
            IEnumerable<Area> areas = new HashSet<Area>();
            if (this.IsPowerUser())
            {
                areas = this.data.Areas.All().ToList();
            }
            else
            {
                areas = Mapper.Map<IEnumerable<Area>>(this.UserProfile.Parks.Select(x => x.Area).Distinct());
            }
            var areaView = Mapper.Map<IEnumerable<AreaViewModel>>(areas);
            return this.Json(areaView, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetParks(int? areaId, string parksFilter)
        {
            IEnumerable<Park> parks = new HashSet<Park>();
            if (this.IsPowerUser())
            {
                parks = this.data.Parks.All();
            }
            else
            {
                parks = Mapper.Map<IEnumerable<Park>>(this.UserProfile.Parks);
            }

            if (areaId != null)
            {
                parks = parks.Where(p => p.AreaId == areaId);
            }

            if (!string.IsNullOrEmpty(parksFilter))
            {
                parks = parks.Where(p => p.Name.Contains(parksFilter));
            }

            var parkView = Mapper.Map<IEnumerable<ParkViewModel>>(parks);
            return this.Json(parkView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShifts()
        {
            var shifts = this.data.Shifts.All().ToList();
            var shiftView = Mapper.Map<IEnumerable<ShiftViewModel>>(shifts);
            return this.Json(shiftView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMaterialTypes()
        {
            var materialTypes = this.data.MaterialTypes.All().ToList();
            var materialTypeView = Mapper.Map<IEnumerable<Areas.NomManagement.Models.ViewModels.MaterialTypeViewModel>>(materialTypes);
            return this.Json(materialTypeView, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetFactories()
        {
            IEnumerable<Factory> factories = new HashSet<Factory>();

            if (this.IsPowerUser())
            {
                factories = this.data.Factories.All().ToList();
            }
            else
            {
                factories = Mapper.Map<IEnumerable<Factory>>(this.UserProfile.ProcessUnits.Select(x => x.Factory).Distinct().ToList());
            }
            var factoryView = Mapper.Map<IEnumerable<FactoryViewModel>>(factories);
            return this.Json(factoryView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProcessUnits(int? factoryId)
        {
            IEnumerable<ProcessUnit> processUnits = new HashSet<ProcessUnit>();
            if (this.IsPowerUser())
            {
                processUnits = this.data.ProcessUnits.All();
            }
            else
            {
                processUnits = Mapper.Map<IEnumerable<ProcessUnit>>(this.UserProfile.ProcessUnits);
            }

            if (factoryId.HasValue)
            {
                processUnits = processUnits.Where(p => p.FactoryId == factoryId);
            }

            processUnits = processUnits.OrderBy(pu => pu.Position);
            var processUnitView = Mapper.Map<IEnumerable<ProcessUnitViewModel>>(processUnits.ToList());
            return this.Json(processUnitView, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveGridState(string data)
        {
            this.Session["data"] = data;
            return new EmptyResult();
        }

        public ActionResult LoadGridState()
        {
            return this.Json(this.Session["data"], JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDirections()
        {
            var directions = this.data.Directions.All().Where(x => x.Id <= 2).ToList();
            var directionsView = Mapper.Map<IEnumerable<DirectionsViewModel>>(directions);
            return this.Json(directionsView, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserCloseWindow()
        {
             if (this.User.Identity.IsAuthenticated)
            {
               bool isLogOff = false;
                var user = this.data.Users.All().FirstOrDefault(x => x.UserName == this.UserProfile.UserName);
                if (user != null)
                {
                    user.IsUserLoggedIn -= 1;
                }
                if (user.IsUserLoggedIn <= 0)
                {
                    user.IsUserLoggedIn = 0;
                    isLogOff = true;
                }
                this.data.Users.Update(user);
                this.data.SaveChanges(this.UserProfile.UserName);
                if (isLogOff)
                {
                    this.HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    this.Session["user"] = null;
                }
            }
            this.InvalidateCookies(this.Request, this.Response);
            return this.RedirectToAction("Index","Home");
        }


        /// <summary>
        /// Invalidates the cookies.
        /// </summary>
        /// <param name="request">The request.</param>
        private void InvalidateCookies(HttpRequestBase request, HttpResponseBase response)
        {
            var requestCookyKeys = request.Cookies.AllKeys;

            response.Cookies.Clear();
            foreach (var key in requestCookyKeys)
            {
                response.Cookies.Add(new HttpCookie(key) { Expires = DateTime.Now.AddDays(-1) });
            }
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            //return File(fileContents, contentType, fileName);
            var result = this.File(fileContents, contentType, fileName);
            return result;
        }

        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return this.File(fileContents, contentType, fileName);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetMessagesCount()
        {
            var result = this.data.Messages.All().Where(x => x.ValidUntill >= DateTime.Now).Select(x=>x.MessageText).Count();
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult GetMessages([DataSourceRequest] DataSourceRequest request)
        {
            var result = this.data.Messages.All().Where(x => x.ValidUntill >= DateTime.Now).OrderByDescending(x=>x.CreatedOn).ToList();
            return this.Json(result.ToDataSourceResult(request, this.ModelState,Mapper.Map<MessageViewModel>));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetLastMessage()
        {
            var result = this.data.Messages.All().Where(x => x.ValidUntill >= DateTime.Now).OrderByDescending(x=>x.CreatedOn).FirstOrDefault();
            return this.Json(result,JsonRequestBehavior.AllowGet);
        }

        private bool IsPowerUser()
        {
            return this.UserProfile.UserRoles.Where(x => CommonConstants.PowerUsers.Any(y => y == x.Name)).Any();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetAllProducts([DataSourceRequest] DataSourceRequest request)
        {
            var products = this.data.Products.All().ToList();
            var productsView = Mapper.Map<IEnumerable<ProductViewModel>>(products);
            return this.Json(productsView.ToDataSourceResult(request, this.ModelState));
        }

        public ActionResult ValueMapper(int[] values)
        {
            var indices = new List<int>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var item in this.data.Products.All())
                {
                    if (values.Contains(item.Id))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return this.Json(indices, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetAllProcessUnits()
        {
            var products = this.data.ProcessUnits.All().ToList();
            var productsView = Mapper.Map<IEnumerable<ProcessUnitViewModel>>(products);
            return this.Json(productsView);
        }

        public ActionResult ProcessUnitValueMapper(int[] values)
        {
            var indices = new List<int>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var item in this.data.ProcessUnits.All())
                {
                    if (values.Contains(item.Id))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return this.Json(indices, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetAllTankStatuses()
        {
            var statuses = this.data.TankStatuses.All().ToList();
            var statusesView = Mapper.Map<IEnumerable<TankStatusViewModel>>(statuses);
            return this.Json(statusesView);
        }

        public ActionResult TankStatusesValueMapper(int[] values)
        {
            var indices = new List<int>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var item in this.data.TankStatuses.All())
                {
                    if (values.Contains(item.Id))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return this.Json(indices, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonthlyReportTypes()
        {
            List<MonthlyReportType> monthlyReportTypes = this.data.MonthlyReportTypes.All().ToList();
            IEnumerable<Areas.NomManagement.Models.ViewModels.MonthlyReportTypeViewModel> monthlyReportTypesView = Mapper.Map<IEnumerable<Areas.NomManagement.Models.ViewModels.MonthlyReportTypeViewModel>>(monthlyReportTypes);
            return this.Json(monthlyReportTypesView, JsonRequestBehavior.AllowGet);
        }
    }
}