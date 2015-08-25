using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    [Authorize(Roles="Administrator")]
    public class BaseController : Controller
    {
        
    }
}