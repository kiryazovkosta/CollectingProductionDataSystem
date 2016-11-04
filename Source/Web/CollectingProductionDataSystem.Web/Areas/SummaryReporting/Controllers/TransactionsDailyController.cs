namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.Controllers
{
    using System.Data.Entity;
    using System.Diagnostics;
    using CollectingProductionDataSystem.Data.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers;
    using CollectingProductionDataSystem.Web.ViewModels.Transactions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using AutoMapper;
    using CollectingProductionDataSystem.Models.Transactions;
    using Resources = App_GlobalResources.Resources;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Application.TransactionsDailyDataServices;

    public class TransactionsDailyController : AreaBaseController
    {
        private readonly ITransactionsDailyDataService transactionsDailyData;

        public TransactionsDailyController(IProductionData dataParam, ITransactionsDailyDataService transactionDailyDataParam)
            : base(dataParam)
        {
            this.transactionsDailyData = transactionDailyDataParam;
        }

        [HttpGet]
        public ActionResult TransactionsDailyData()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ReadTransactionsDailyData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? flowDirection)
        {
            ValidateModelState(date, flowDirection);
            var kendoResult = new DataSourceResult();
            try
            {
                var dbResult = this.transactionsDailyData.ReadTransactionsDailyData(date.Value, flowDirection.Value);
                kendoResult = dbResult.ToDataSourceResult(request, ModelState);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
            }

            return Json(kendoResult);
        }

        private void ValidateModelState(DateTime? date, int? directionId)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }
            if (directionId == null || directionId.Value == CommonConstants.InputOutputDirection)
            {
                this.ModelState.AddModelError("direction", string.Format(Resources.ErrorMessages.Required, Resources.Layout.Direction));
            }
        }
    }
}