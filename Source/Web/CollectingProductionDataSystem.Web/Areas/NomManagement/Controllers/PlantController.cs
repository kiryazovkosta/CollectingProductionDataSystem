﻿using System;
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

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class PlantController : AreaBaseController
    {
        public PlantController(IProductionData dataParam)
            : base(dataParam)
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Plants_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Plant> plants = data.Plants.All();
            DataSourceResult result = plants.ToDataSourceResult(request, Mapper.Map<PlantViewModel>);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Plants_Create([DataSourceRequest]DataSourceRequest request, PlantViewModel plant)
        {
            if (ModelState.IsValid)
            {
                var entity = Mapper.Map<Plant>(plant);

                data.Plants.Add(entity);

                var result = data.SaveChanges(this.UserProfile.UserName);

                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }

                plant.Id = entity.Id;
            }

            return Json(new[] { plant }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Plants_Update([DataSourceRequest]DataSourceRequest request, PlantViewModel plant)
        {
            if (ModelState.IsValid)
            {

                var dbEntity = data.Plants.GetById(plant.Id);

                if (dbEntity == null)
                {
                    ModelState.AddModelError("", string.Format(Resources.ErrorMessages.InvalidRecordUpdate, plant.Id));
                }
                else
                {
                    Mapper.Map(plant,dbEntity);

                    data.Plants.Update(dbEntity);

                    var result = data.SaveChanges(this.UserProfile.UserName);

                    if (!result.IsValid)
                    {
                        result.ToModelStateErrors(ModelState);
                    }
                }
            }

            return Json(new[] { plant }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Plants_Destroy([DataSourceRequest]DataSourceRequest request, PlantViewModel plant)
        {
            if (ModelState.IsValid)
            {
                data.Plants.Delete(plant.Id);
                var result = data.SaveChanges(this.UserProfile.UserName);

                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }
            }

            return Json(new[] { plant }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}
