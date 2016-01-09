namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    using System.Data.Entity;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Models.Transactions.HighwayPipelines;
    using CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using System.Diagnostics;

    public class HighwayPipelinesController : AreaBaseController
    {
        public HighwayPipelinesController(IProductionData dataParam) : base(dataParam)
        {

        }

        [HttpGet]
        public ActionResult HighwayPipelinesData()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ReadHighwayPipelinesData([DataSourceRequest]DataSourceRequest request, DateTime? date)
        {
            var kendoResult = new DataSourceResult();
            try
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
                var highwayPipesData = this.data.HighwayPipelineDatas.All().Include(x => x.HighwayPipelineConfig).Where(x => x.RecordTimestamp == date).ToList();
                var kendoPreparedResult = Mapper.Map<IEnumerable<HighwayPipelineData>, IEnumerable<HighwayPipelinesDataViewModel>>(highwayPipesData);
                kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
            }

            return Json(kendoResult);
        }

                [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([DataSourceRequest]DataSourceRequest request, HighwayPipelinesDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var newManualRecord = new HighwayPipelineData
                //{
                //    Id = model.Id,
                //    Value = model.UnitsManualDailyData.Value,
                //    EditReasonId = model.UnitsManualDailyData.EditReason.Id
                //};

                //var existRecord = this.data.UnitsManualDailyDatas.All().FirstOrDefault(x => x.Id == newManualRecord.Id);
                //if (existRecord == null)
                //{
                //    this.data.UnitsManualDailyDatas.Add(newManualRecord);
                //}
                //else
                //{
                //    UpdateRecord(existManualRecord, model);
                //}
                //try
                //{
                //    using (var transaction = new TransactionScope(TransactionScopeOption.Required, this.transantionOption))
                //    {
                //        var result = this.data.SaveChanges(UserProfile.UserName);
                //        if (!result.IsValid)
                //        {
                //            foreach (ValidationResult error in result.EfErrors)
                //            {
                //                this.ModelState.AddModelError(error.MemberNames.ToList()[0], error.ErrorMessage);
                //            }
                //        }

                //        var updatedRecords = this.dailyService.CalculateDailyDataForProcessUnit(model.UnitsDailyConfig.ProcessUnitId, model.RecordTimestamp, isRecalculate: true, editReasonId: model.UnitsManualDailyData.EditReason.Id);
                //        var status = UpdateResultRecords(updatedRecords, model.UnitsManualDailyData.EditReason.Id);

                //        if (!status.IsValid)
                //        {
                //            status.ToModelStateErrors(this.ModelState);
                //            //logger.Error()
                //        }
                //        else
                //        {
                //            transaction.Complete();
                //        }
                //    }
                //}
                //catch (DbUpdateException)
                //{
                //    this.ModelState.AddModelError("ManualValue", "Записът не можа да бъде осъществен. Моля опитайте на ново!");
                //}
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}