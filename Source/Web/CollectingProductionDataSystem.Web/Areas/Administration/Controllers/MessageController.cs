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
using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
using CollectingProductionDataSystem.Web.Infrastructure.HubAuthomation;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    public class MessageController : AreaBaseController
    {
        private readonly MessagesPublisher messanger;
        public MessageController(IProductionData dataParam) 
            :base(dataParam)
        {
            this.messanger = DependencyResolver.Current.GetService<MessagesPublisher>();//MessagesPublisher.GetInstance(dataParam);
        }

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
                this.data.SaveChanges(this.UserProfile.UserName);
                message.Id = dbMessage.Id;
            }

            return Json(new[] { message }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, MessageInputModel message)
        {

             if (ModelState.IsValid)
            {

                var dbEntity = this.data.Messages.GetById(message.Id);

                if (dbEntity == null)
                {
                    ModelState.AddModelError("", string.Format(Resources.ErrorMessages.InvalidRecordUpdate, message.Id));
                }
                else
                {
                    Mapper.Map(message, dbEntity);

                    this.data.Messages.Update(dbEntity);

                    var result = data.SaveChanges(this.UserProfile.UserName);

                    if (!result.IsValid)
                    {
                        result.ToModelStateErrors(ModelState);
                    }
                }
            }

            return Json(new[] { message }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, MessageInputModel message)
        {
            if (ModelState.IsValid)
            {
                this.data.Messages.Delete(message.Id);
                var result = data.SaveChanges(this.UserProfile.UserName);

                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }
            }

            MessagesHub.GetActualMessagesCount();
            return Json(new[] { message }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request) 
        {
            var result = data.Messages.AllWithDeleted().OrderByDescending(x=>x.CreatedOn).ToList();
            return Json(result.ToDataSourceResult(request, ModelState,Mapper.Map<MessageInputModel>));
        }
    }
}