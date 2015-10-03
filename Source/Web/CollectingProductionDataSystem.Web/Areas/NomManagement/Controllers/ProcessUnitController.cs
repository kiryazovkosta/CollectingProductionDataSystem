namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
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

    public class ProcessUnitController : AreaBaseController
    {
        public ProcessUnitController(IProductionData dataParam)
            : base(dataParam)
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessUnit_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<ProcessUnit> processUnits = data.ProcessUnits.All();
            DataSourceResult result = processUnits.ToDataSourceResult(request, Mapper.Map<ProcessUnitViewModel>);

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessUnit_Create([DataSourceRequest]DataSourceRequest request, ProcessUnitViewModel processUnit)
        {
            if (ModelState.IsValid)
            {
                var entity = Mapper.Map<ProcessUnit>(processUnit);

                data.ProcessUnits.Add(entity);

                var result = data.SaveChanges(this.UserProfile.UserName);

                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }

                processUnit.Id = entity.Id;
            }

            return Json(new[] { processUnit }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessUnit_Update([DataSourceRequest]DataSourceRequest request, ProcessUnitViewModel processUnit)
        {
            if (ModelState.IsValid)
            {

                var dbEntity = data.ProcessUnits.GetById(processUnit.Id);

                if (dbEntity == null)
                {
                    ModelState.AddModelError("", string.Format(Resources.ErrorMessages.InvalidRecordUpdate, processUnit.Id));
                }
                else
                {
                    Mapper.Map(processUnit, dbEntity);

                    data.ProcessUnits.Update(dbEntity);

                    var result = data.SaveChanges(this.UserProfile.UserName);

                    if (!result.IsValid)
                    {
                        result.ToModelStateErrors(ModelState);
                    }
                }
            }

            return Json(new[] { processUnit }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessUnit_Destroy([DataSourceRequest]DataSourceRequest request, ProcessUnitViewModel processUnit)
        {
            if (ModelState.IsValid)
            {
                data.ProcessUnits.Delete(processUnit.Id);
                var result = data.SaveChanges(this.UserProfile.UserName);

                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }
            }

            return Json(new[] { processUnit }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}
