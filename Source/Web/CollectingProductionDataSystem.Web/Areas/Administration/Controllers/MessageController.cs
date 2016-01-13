using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.UtilityEntities;
using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
using CollectingProductionDataSystem.Web.Hubs;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    public class MessageController : AreaBaseController
    {
        public MessageController(IProductionData dataParam) 
            :base(dataParam)
        { }

        [HttpGet]
        public ActionResult PublishMessage() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, MessageInputModel message)
        {
            if (ModelState.IsValid)
            {
                var dbMessage = Mapper.Map<Message>(message);
                this.data.Messages.Add(dbMessage);
                this.data.SaveChanges("Test");
                MessagesHub.DisplayNewMessage(dbMessage);
            }

            return Json(new[] { message }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request) 
        {
            var result = data.Messages.AllWithDeleted().OrderByDescending(x=>x.CreatedOn).ToList();
            return Json(result.ToDataSourceResult(request, ModelState,Mapper.Map<MessageInputModel>));
        }
    }
}