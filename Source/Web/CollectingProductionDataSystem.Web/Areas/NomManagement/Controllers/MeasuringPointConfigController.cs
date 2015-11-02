using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Transactions;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class MeasuringPointConfigController : GenericNomController<MeasuringPointConfig, MeasuringPointConfigViewModel>
    {
        public MeasuringPointConfigController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        public override ActionResult Index()
        {
            this.ViewData["zones"] = Mapper.Map<IEnumerable<ZoneViewModel>>(this.data.Zones.All().ToList());
            this.ViewData["directions"]= Mapper.Map<IEnumerable<DirectionViewModel>>(this.data.Directions.All().ToList());
            this.ViewData["transportTypes"] = Mapper.Map<IEnumerable<TransportTypeViewModel>>(this.data.TransportTypes.All().ToList());
            this.ViewData["relatedMeasuringPoint"] = Mapper.Map<IEnumerable<MeasuringPointConfig>, IEnumerable<RelatedMeasuringPointConfigsViewModel>>(this.data.MeasuringPointConfigs.All()).ToList();
            return base.Index();
        }
    }       
}