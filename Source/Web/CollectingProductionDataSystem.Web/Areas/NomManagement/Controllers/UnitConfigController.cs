﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Data;
using CollectingProductionDataSystem.Data.Contracts;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class UnitConfigController : AreaBaseController
    {
        public UnitConfigController(IProductionData dataParam)
            : base(dataParam)
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnitConfigs_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<UnitConfig> unitconfigs = data.UnitConfigs.All();
            DataSourceResult result = unitconfigs.ToDataSourceResult(request, unitConfig => new
            {
                Id = unitConfig.Id,
                Code = unitConfig.Code,
                Position = unitConfig.Position,
                Name = unitConfig.Name,
                IsMaterial = unitConfig.IsMaterial,
                IsEnergy = unitConfig.IsEnergy,
                IsInspectionPoint = unitConfig.IsInspectionPoint,
                CollectingDataMechanism = unitConfig.CollectingDataMechanism,
                AggregateGroup = unitConfig.AggregateGroup,
                AggregateParameter = unitConfig.AggregateParameter,
                IsCalculated = unitConfig.IsCalculated,
                PreviousShiftTag = unitConfig.PreviousShiftTag,
                CurrentInspectionDataTag = unitConfig.CurrentInspectionDataTag,
                Notes = unitConfig.Notes,
                MaximumCost = unitConfig.MaximumCost,
                EstimatedDensity = unitConfig.EstimatedDensity,
                EstimatedPressure = unitConfig.EstimatedPressure,
                EstimatedTemperature = unitConfig.EstimatedTemperature,
                EstimatedCompressibilityFactor = unitConfig.EstimatedCompressibilityFactor,
                IsDeleted = unitConfig.IsDeleted,
                DeletedOn = unitConfig.DeletedOn,
                DeletedFrom = unitConfig.DeletedFrom,
                CreatedOn = unitConfig.CreatedOn,
                ModifiedOn = unitConfig.ModifiedOn,
                CreatedFrom = unitConfig.CreatedFrom,
                ModifiedFrom = unitConfig.ModifiedFrom,
                MaterialTypeId = unitConfig.MaterialTypeId,
                ProcessUnitId = unitConfig.ProcessUnitId,
                ProductId = unitConfig.ProductId,
                ProductTypeId = unitConfig.ProductTypeId,
                MeasureUnitId = unitConfig.MeasureUnitId,
                DirectionId = unitConfig.DirectionId
            });

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnitConfigs_Create([DataSourceRequest]DataSourceRequest request, UnitConfig unitConfig)
        {
            if (ModelState.IsValid)
            {
                var entity = new UnitConfig
                {
                    Code = unitConfig.Code,
                    Position = unitConfig.Position,
                    Name = unitConfig.Name,
                    IsMaterial = unitConfig.IsMaterial,
                    IsEnergy = unitConfig.IsEnergy,
                    IsInspectionPoint = unitConfig.IsInspectionPoint,
                    CollectingDataMechanism = unitConfig.CollectingDataMechanism,
                    AggregateGroup = unitConfig.AggregateGroup,
                    AggregateParameter = unitConfig.AggregateParameter,
                    IsCalculated = unitConfig.IsCalculated,
                    PreviousShiftTag = unitConfig.PreviousShiftTag,
                    CurrentInspectionDataTag = unitConfig.CurrentInspectionDataTag,
                    Notes = unitConfig.Notes,
                    MaximumCost = unitConfig.MaximumCost,
                    EstimatedDensity = unitConfig.EstimatedDensity,
                    EstimatedPressure = unitConfig.EstimatedPressure,
                    EstimatedTemperature = unitConfig.EstimatedTemperature,
                    EstimatedCompressibilityFactor = unitConfig.EstimatedCompressibilityFactor,
                    IsDeleted = unitConfig.IsDeleted,
                    DeletedOn = unitConfig.DeletedOn,
                    DeletedFrom = unitConfig.DeletedFrom,
                    CreatedOn = unitConfig.CreatedOn,
                    ModifiedOn = unitConfig.ModifiedOn,
                    CreatedFrom = unitConfig.CreatedFrom,
                    ModifiedFrom = unitConfig.ModifiedFrom,
                    MaterialTypeId = unitConfig.MaterialTypeId,
                    ProcessUnitId = unitConfig.ProcessUnitId,
                    ProductId = unitConfig.ProductId,
                    ProductTypeId = unitConfig.ProductTypeId,
                    MeasureUnitId = unitConfig.MeasureUnitId,
                    DirectionId = unitConfig.DirectionId
                    
                };

                data.UnitConfigs.Add(entity);
                var result = data.SaveChanges(this.UserProfile.UserName);
                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }
                unitConfig.Id = entity.Id;
            }

            return Json(new[] { unitConfig }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnitConfigs_Update([DataSourceRequest]DataSourceRequest request, UnitConfig unitConfig)
        {
            if (ModelState.IsValid)
            {
                var entity = new UnitConfig
                {
                    Id = unitConfig.Id,
                    Code = unitConfig.Code,
                    Position = unitConfig.Position,
                    Name = unitConfig.Name,
                    IsMaterial = unitConfig.IsMaterial,
                    IsEnergy = unitConfig.IsEnergy,
                    IsInspectionPoint = unitConfig.IsInspectionPoint,
                    CollectingDataMechanism = unitConfig.CollectingDataMechanism,
                    AggregateGroup = unitConfig.AggregateGroup,
                    AggregateParameter = unitConfig.AggregateParameter,
                    IsCalculated = unitConfig.IsCalculated,
                    PreviousShiftTag = unitConfig.PreviousShiftTag,
                    CurrentInspectionDataTag = unitConfig.CurrentInspectionDataTag,
                    Notes = unitConfig.Notes,
                    MaximumCost = unitConfig.MaximumCost,
                    EstimatedDensity = unitConfig.EstimatedDensity,
                    EstimatedPressure = unitConfig.EstimatedPressure,
                    EstimatedTemperature = unitConfig.EstimatedTemperature,
                    EstimatedCompressibilityFactor = unitConfig.EstimatedCompressibilityFactor,
                    IsDeleted = unitConfig.IsDeleted,
                    DeletedOn = unitConfig.DeletedOn,
                    DeletedFrom = unitConfig.DeletedFrom,
                    CreatedOn = unitConfig.CreatedOn,
                    ModifiedOn = unitConfig.ModifiedOn,
                    CreatedFrom = unitConfig.CreatedFrom,
                    ModifiedFrom = unitConfig.ModifiedFrom,
                    MaterialTypeId = unitConfig.MaterialTypeId,
                    ProcessUnitId = unitConfig.ProcessUnitId,
                    ProductId = unitConfig.ProductId,
                    ProductTypeId = unitConfig.ProductTypeId,
                    MeasureUnitId = unitConfig.MeasureUnitId,
                    DirectionId = unitConfig.DirectionId
                };

                data.UnitConfigs.Update(entity);
                var result = data.SaveChanges(this.UserProfile.UserName);

                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }
            }

            return Json(new[] { unitConfig }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnitConfigs_Destroy([DataSourceRequest]DataSourceRequest request, UnitConfig unitConfig)
        {
            if (ModelState.IsValid)
            {
                var entity = new UnitConfig
                {
                    Id = unitConfig.Id,
                    Code = unitConfig.Code,
                    Position = unitConfig.Position,
                    Name = unitConfig.Name,
                    IsMaterial = unitConfig.IsMaterial,
                    IsEnergy = unitConfig.IsEnergy,
                    IsInspectionPoint = unitConfig.IsInspectionPoint,
                    CollectingDataMechanism = unitConfig.CollectingDataMechanism,
                    AggregateGroup = unitConfig.AggregateGroup,
                    AggregateParameter = unitConfig.AggregateParameter,
                    IsCalculated = unitConfig.IsCalculated,
                    PreviousShiftTag = unitConfig.PreviousShiftTag,
                    CurrentInspectionDataTag = unitConfig.CurrentInspectionDataTag,
                    Notes = unitConfig.Notes,
                    MaximumCost = unitConfig.MaximumCost,
                    EstimatedDensity = unitConfig.EstimatedDensity,
                    EstimatedPressure = unitConfig.EstimatedPressure,
                    EstimatedTemperature = unitConfig.EstimatedTemperature,
                    EstimatedCompressibilityFactor = unitConfig.EstimatedCompressibilityFactor,
                    IsDeleted = unitConfig.IsDeleted,
                    DeletedOn = unitConfig.DeletedOn,
                    DeletedFrom = unitConfig.DeletedFrom,
                    CreatedOn = unitConfig.CreatedOn,
                    ModifiedOn = unitConfig.ModifiedOn,
                    CreatedFrom = unitConfig.CreatedFrom,
                    ModifiedFrom = unitConfig.ModifiedFrom,
                    MaterialTypeId = unitConfig.MaterialTypeId,
                    ProcessUnitId = unitConfig.ProcessUnitId,
                    ProductId = unitConfig.ProductId,
                    ProductTypeId = unitConfig.ProductTypeId,
                    MeasureUnitId = unitConfig.MeasureUnitId,
                    DirectionId = unitConfig.DirectionId
                };

                data.UnitConfigs.Delete(entity);
                var result = data.SaveChanges(this.UserProfile.UserName);

                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }
            }

            return Json(new[] { unitConfig }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}
