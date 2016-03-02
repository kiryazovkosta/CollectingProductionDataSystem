namespace CollectingProductionDataSystem.Web.Areas.ShiftReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.HighwayPipelinesDataServices;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Transactions.HighwayPipelines;
    using CollectingProductionDataSystem.Web.Areas.ShiftReporting.ViewModels;
    using CollectingProductionDataSystem.Web.Controllers;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Resources = App_GlobalResources.Resources;

    //[Authorize(Roles = "Administrator, ShiftReporter")]
    [Authorize(Roles = "Administrator,HighwayPipelinesDataReporter")]
    public class HighwayPipelinesController : BaseController
    {
        private readonly IHighwayPipelinesDataService highwayPipelinesService;

        public HighwayPipelinesController(IProductionData dataParam, IHighwayPipelinesDataService highwayPipelinesDataServiceParam) 
            : base(dataParam)
        {
            this.highwayPipelinesService = highwayPipelinesDataServiceParam;
        }

        [HttpGet]
        public ActionResult HighwayPipelinesData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReadHighwayPipelinesData([DataSourceRequest]DataSourceRequest request, DateTime? date)
        {
             ValidateModelState(date);

            if (this.ModelState.IsValid)
            {
                if (date.Value.Day == CommonConstants.FirstDayInMonth)
                {
                    if ( !this.data.HighwayPipelineDatas.All().Where(x => x.RecordTimestamp == date).Any())
                    {
                        var pipelinesConfigs = this.data.HighwayPipelineConfigs.All().ToList();
                        foreach (var pipelinesConfig in pipelinesConfigs)
                        {
                            this.data.HighwayPipelineDatas.Add(new HighwayPipelineData
                            {
                                RecordTimestamp = date.Value,
                                HighwayPipelineConfigId = pipelinesConfig.Id,
                                ProductName = pipelinesConfig.Product.Name,
                                ProductCode = pipelinesConfig.Product.Code,
                                Volume = 0,
                                Mass = 0,
                            });   
                        }

                        this.data.SaveChanges(this.UserProfile.UserName);
                    }
                }
                else
                {
                    {
                        if (!this.data.HighwayPipelineDatas.All().Where(x => x.RecordTimestamp == date).Any())
                        {
                            var highwayPipelinesLastData = this.highwayPipelinesService.ReadDataForDay(date.Value);
                            foreach (var item in highwayPipelinesLastData)
                            {
                                this.data.HighwayPipelineDatas.Add(new HighwayPipelineData
                                {
                                    RecordTimestamp = date.Value,
                                    HighwayPipelineConfigId = item.HighwayPipeline.Id,
                                    ProductName = item.Quantity.ProductName,
                                    ProductCode = item.Quantity.ProductCode,
                                    Volume = item.Quantity.Volume,
                                    Mass = item.Quantity.Mass,
                                });    
                            }

                            this.data.SaveChanges(this.UserProfile.UserName);
                        }
                    }
                }

                if (this.ModelState.IsValid)
                {
                    var highwayPipesData = this.data.HighwayPipelineDatas.All().Include(x => x.HighwayPipelineConfig).Where(x => x.RecordTimestamp == date).ToList();
                    var kendoPreparedResult = Mapper.Map<IEnumerable<HighwayPipelineData>, IEnumerable<HighwayPipelinesDataViewModel>>(highwayPipesData);
                    var kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
                    return Json(kendoResult);  
                }
                else
                {
                    var kendoResult = new List<HighwayPipelinesDataViewModel>().ToDataSourceResult(request, ModelState);
                    return Json(kendoResult);
                }
            }
            else
            {
                var kendoResult = new List<HighwayPipelinesDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }
 
        private void ValidateModelState(DateTime? date)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }
            if (date.HasValue && date.Value.CompareTo(DateTime.Today) > 0)
            {
                this.ModelState.AddModelError("date", Resources.Layout.UnitsDateSelectorFuture);
            }
            if (date != null && DateTime.Today.AddDays(1) < date.Value)
            {
                if (date == null)
                {
                    this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([DataSourceRequest]DataSourceRequest request, HighwayPipelinesDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var exsistRecord = this.data.HighwayPipelineDatas.All().Where(x => x.Id == model.Id).First();
                exsistRecord.Volume = model.Volume;
                exsistRecord.Mass = model.Mass;
                this.data.HighwayPipelineDatas.Update(exsistRecord);
                try
                    {
                        IEfStatus result = this.data.SaveChanges(UserProfile.UserName);
                        if (!result.IsValid)
                        {
                            result.ToModelStateErrors(this.ModelState);
                        }
                    }
                    catch (DbUpdateException)
                    {
                        this.ModelState.AddModelError("", "Записът не можа да бъде осъществен. Моля опитайте на ново!");
                    }
                    catch(Exception ex)
                    {
                        this.ModelState.AddModelError("", ex.ToString());
                    }
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsConfirmed([DataSourceRequest]DataSourceRequest request, DateTime? date)
        {
            ValidateModelState(date);

            if (this.ModelState.IsValid)
            {
                var approvedDay = this.data.HighwayPipelineApprovedDatas
                    .All()
                    .Where(u => u.RecordDate == date)
                    .FirstOrDefault();
                if (approvedDay == null)
                {
                    var status = this.highwayPipelinesService.CheckIfPreviousDaysAreReady(date.Value);
                    if (!status.IsValid)
                    {
                        return Json(new { IsConfirmed = true });    
                    }
                    return Json(new { IsConfirmed = false });
                }
                return Json(new { IsConfirmed = true });
            }
            else
            {
                return Json(new { IsConfirmed = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(DateTime date)
        {
            //ValidateModelAgainstReportPatameters(this.ModelState, model, Session["reportParams"]);

            if (!Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ComplitionCheckHighwayPipelineDeactivared"]))
            {
                //var status = this.dailyService.CheckIfPreviousDaysAreReady(model.processUnitId, model.date, CommonConstants.MaterialType);
                //if (!status.IsValid)
                //{
                //    status.ToModelStateErrors(this.ModelState);
                //}
            }

            if (ModelState.IsValid)
            {
                var confirmedDay = this.data.HighwayPipelineApprovedDatas
                   .All()
                   .Where(u => u.RecordDate == date)
                   .FirstOrDefault();
                if (confirmedDay == null)
                {
                    this.data.HighwayPipelineApprovedDatas.Add(
                        new HighwayPipelineApprovedData
                        {
                            RecordDate = date,
                            Approved = true
                        });

                    var result = this.data.SaveChanges(this.UserProfile.UserName);
                    return Json(new { IsConfirmed = result.IsValid }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("unitsapproveddata", "Дневните данни вече са потвърдени");
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var errors = GetErrorListFromModelState(ModelState);
                    return Json(new { data = new { errors = errors } });
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = GetErrorListFromModelState(ModelState);
                return Json(new { data = new { errors = errors } });
            }
        }

        private List<string> GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }
    }
}