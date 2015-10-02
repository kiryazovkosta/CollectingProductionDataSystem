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
    public class FactoryController : AreaBaseController
    {
        public FactoryController(IProductionData dataParam)
            : base(dataParam)
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Factories_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Factory> factories = data.Factories.All();
            DataSourceResult result = factories.ToDataSourceResult(request, Mapper.Map<FactoryViewModel>);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Factories_Create([DataSourceRequest]DataSourceRequest request, FactoryViewModel factory)
        {
            if (ModelState.IsValid)
            {
                var entity = Mapper.Map<Factory>(factory);

                data.Factories.Add(entity);

                var result = data.SaveChanges(this.UserProfile.UserName);

                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }

                factory.Id = entity.Id;
            }

            return Json(new[] { factory }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Factories_Update([DataSourceRequest]DataSourceRequest request, FactoryViewModel factory)
        {
            if (ModelState.IsValid)
            {

                var dbEntity = data.Factories.GetById(factory.Id);

                if (dbEntity == null)
                {
                    ModelState.AddModelError("", string.Format(Resources.ErrorMessages.InvalidRecordUpdate, factory.Id));
                }
                else
                {
                    Mapper.Map(factory,dbEntity);

                    data.Factories.Update(dbEntity);

                    var result = data.SaveChanges(this.UserProfile.UserName);

                    if (!result.IsValid)
                    {
                        result.ToModelStateErrors(ModelState);
                    }
                }
            }

            return Json(new[] { factory }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Factories_Destroy([DataSourceRequest]DataSourceRequest request, FactoryViewModel factory)
        {
            if (ModelState.IsValid)
            {
                data.Factories.Delete(factory.Id);
                var result = data.SaveChanges(this.UserProfile.UserName);

                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }
            }

            return Json(new[] { factory }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}
