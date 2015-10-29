using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Transactions;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class IkunkController : GenericNomController<Ikunk, IkunkViewModel>
    {
        public IkunkController(IProductionData dataParam)
            : base(dataParam)
        {
        }
    }
}