namespace CollectingProductionDataSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.TankDataServices;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNet.Identity;

    public class TestsController : BaseController
    {
        private readonly ITankDataKendoService tankData;
        public TestsController(IProductionData dataParam, ITankDataKendoService tankDataParam)
            : base(dataParam)
        {
            this.tankData = tankDataParam;
        }

        public ActionResult Index()
        {
            return View(this.UserProfile);
        }

        [Authorize]
        public ActionResult Test()
        {
            // add product
            var pr1 = new Product() { Name = "Added Product", ProductTypeId = 1 };
            this.data.Products.Add(pr1);

            //modify product
            var pr = this.data.Products.GetById(5);
            pr.Name = "Modified Product Name";
            this.data.Products.Update(pr);

            //delete product
            this.data.Products.Delete(4);

            this.data.SaveChanges(this.UserProfile.UserName);
            return View((object)"Success");
        }

        [Authorize]
        [HttpGet]
        public ActionResult TankChange()
        {
            var record = this.data.TanksData.All().FirstOrDefault(r => r.Id == 1);
            return View(record);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TankChange(TankData model)
        {
            if (ModelState.IsValid)
            {
                //this.data.TanksData.Update(model);
                this.data.SaveChanges(this.UserProfile.UserName);
                return RedirectToAction("TankChange");
            }

            return View(model);

        }

        [HttpGet]
        public ActionResult TankData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReadTank([DataSourceRequest]DataSourceRequest request, DateTime? date)
        {
            var dbResult = this.tankData.GetTankDataForDateTime(date);
            var kendoResult = dbResult.ToDataSourceResult(request, ModelState);
            kendoResult.Data = Mapper.Map<IEnumerable<TankData>, IEnumerable<TankDataViewModel>>((IEnumerable<TankData>)kendoResult.Data);
            return Json(kendoResult);
        }

        [HttpGet]

        public ActionResult CreateUser()
        {
            var user = new ApplicationUser()
            {
                UserName="Manual",
                Email = "manual@test.com",
                FirstName="Manualy",
                MiddleName = "Created",
                LastName = "User",
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = new PasswordHasher().HashPassword("12345678"),
                IsChangePasswordRequired=true,
            };

            data.Users.Add(user);
            var result = data.SaveChanges(UserProfile.UserName);
            if (result.IsValid)
            {
                return Content(string.Format("User: {0} was created successfully",user.FullName));
            }
            else 
            {
                return Content(string.Join("\n", result.EfErrors.Select(x => x.ErrorMessage)));                
            }
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

    }
}

