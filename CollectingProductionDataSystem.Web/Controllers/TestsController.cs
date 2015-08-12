using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data;
using CollectingProductionDataSystem.Models;
using CollectingProductionDataSystem.Web.AppStart;
using Microsoft.AspNet.Identity.Owin;

namespace CollectingProductionDataSystem.Web.Controllers
{
    public class TestsController : BaseController
    {
        public TestsController(IProductionData dataParam) : base(dataParam) 
        { }

        public ActionResult Index()
        {
            return View(this.UserProfile);
        }

        [Authorize]
        public ActionResult Test() 
        {
            this.data.Products.Delete(2);
            var pr = this.data.Products.GetById(3);
            pr.Name = "Modified Product Name";
            this.data.Products.Update(pr);
            this.data.SaveChanges(this.UserProfile.User.UserName);
            return View((object)"Success");
        }

    }

}