namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    ﻿using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    public class UnitConfigController : GenericNomController<UnitConfig, UnitConfigViewModel>
    {
        public UnitConfigController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        public override ActionResult Index()
        {
            this.ViewData["materialTypes"] = Mapper.Map<IEnumerable<MaterialTypeViewModel>>(this.data.MaterialTypes.All().ToList());
            this.ViewData["processUnits"] = Mapper.Map<IEnumerable<ProcessUnitViewModel>>(this.data.ProcessUnits.All().ToList());
            this.ViewData["products"] = Mapper.Map<IEnumerable<ProductViewModel>>(this.data.Products.All().ToList());
            this.ViewData["measureUnits"] = Mapper.Map<IEnumerable<MeasureUnitViewModel>>(this.data.MeasureUnits.All().ToList());
            this.ViewData["directions"] = Mapper.Map<IEnumerable<DirectionViewModel>>(this.data.Directions.All().ToList());
            this.ViewData["shiftProductTypes"] = Mapper.Map<IEnumerable<ShiftProductTypeViewModel>>(this.data.ShiftProductTypes.All().ToList());
            this.ViewData["relatedUnits"] = Mapper.Map <IEnumerable<UnitConfig>, IEnumerable<RelatedUnitConfigsViewModel>>(this.data.UnitConfigs.All().ToList());
            return base.Index();
        }
    }
}
