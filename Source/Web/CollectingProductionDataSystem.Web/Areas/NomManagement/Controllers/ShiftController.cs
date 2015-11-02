namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;

    public class ShiftController : GenericNomController<Shift, ShiftViewModel>
    {
        public ShiftController(IProductionData dataParam)
            : base(dataParam)
        {
           
        }
    }
}