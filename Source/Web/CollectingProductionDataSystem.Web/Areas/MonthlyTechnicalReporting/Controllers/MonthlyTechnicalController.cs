namespace CollectingProductionDataSystem.Web.Areas.MonthlyTechnicalReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.MonthlyTechnologicalDataServices;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.MonthlyTechnicalReporting.ViewModels;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using System.Threading.Tasks;
    using System.Web.UI;
    using Resources = App_GlobalResources.Resources;
    using Models.Productions;
    using Models.Productions.Technological;
    using System.Net;

    public class MonthlyTechnicalController : AreaBaseController
    {
        private const int HalfAnHour = 60 * 30;
        private readonly IMonthlyTechnicalDataService monthlyService;

        public MonthlyTechnicalController(IProductionData dataParam, IMonthlyTechnicalDataService monthlyServiceParam)
            : base(dataParam)
        {
            this.monthlyService = monthlyServiceParam;
        }

        [HttpGet]
        public ActionResult MonthlyTechnicalData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReadMonthlyTechnicalData([DataSourceRequest]DataSourceRequest request, int? factoryId, DateTime? date)
        {
            ValidateModelState(factoryId, date);

            if (!this.ModelState.IsValid)
            {
                DataSourceResult kendoResult = new List<MonthlyTechnicalViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }

            IEfStatus status = this.monthlyService.CheckIfAllMonthReportAreApproved(date.Value);
            if (!status.IsValid)
            {
                status.ToModelStateErrors(this.ModelState);
            }

            if (this.ModelState.IsValid)
            {
                //try
                {
                    IEnumerable<int> processUnits = new HashSet<int>();
                    if (IsPowerUser())
                    {
                        processUnits = this.data.ProcessUnits.All().Where(x => x.FactoryId == factoryId).Select(x => x.Id);
                    }
                    else
                    {
                        List<int> processUnitsForFactory = this.data.ProcessUnits.All().Where(x => x.FactoryId == factoryId).Select(x => x.Id).ToList();
                        processUnits = this.UserProfile.ProcessUnits.ToList().Where(p => processUnitsForFactory.Contains(p.Id)).Select(x => x.Id);
                    }
                    IEnumerable<MonthlyTechnicalReportDataDto> dbResult = this.monthlyService.ReadMonthlyTechnologicalDataAsync(date.Value, processUnits.ToArray());
                    IEnumerable<MonthlyTechnicalViewModel> vmResult = Mapper.Map<IEnumerable<MonthlyTechnicalViewModel>>(dbResult);
                    DataSourceResult kendoResult = vmResult.ToDataSourceResult(request, ModelState);
                    //return Json(kendoResult);
                    JsonResult output = Json(kendoResult, JsonRequestBehavior.AllowGet);
                    output.MaxJsonLength = int.MaxValue;
                    return output;
                }
            }
            else
            {
                DataSourceResult kendoResult = new List<MonthlyTechnicalViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }

        private void ValidateModelState(int? factoryId, DateTime? date)
        {
            if (factoryId == null)
            {
                this.ModelState.AddModelError("factoryId", string.Format(Resources.ErrorMessages.Required, Resources.Layout.ChooseFactory));
            }

            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsConfirmed(DateTime date)
        {
            return Json(new { IsConfirmed = true });
        }

        private bool IsPowerUser()
        {
            return UserProfile.UserRoles.Where(x => CommonConstants.PowerUsers.Any(y => y == x.Name)).Any();
        }

        private bool IsMonthlyTechnologicalReportWriter()
        {
            return UserProfile.UserRoles.Where(x => CommonConstants.MonthlyTechnologicalReportWriterUsers.Any(y => y == x.Name)).Any();
        }

        private bool IsMonthlyTechnologicalApprover()
        {
            return UserProfile.UserRoles.Where(x => CommonConstants.MonthlyTechnologicalApproverUsers.Any(y => y == x.Name)).Any();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetFactoryName([DataSourceRequest]DataSourceRequest request, int? factoryId)
        {
            if (!factoryId.HasValue)
            {
                this.ModelState.AddModelError("", "factory id");
            }

            if (this.ModelState.IsValid)
            {
                Factory factory = this.data.Factories.All().Where(x => x.Id == factoryId).FirstOrDefault();
                if (factory != null)
                {
                    return Json(new { factoryName = factory.FullName });
                }

                return Json(new { factoryName = string.Empty });
            }
            else
            {
                return Json(new { factoryName = string.Empty });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetExportData([DataSourceRequest]DataSourceRequest request, int? factoryId, DateTime? date)
        {
            if (!factoryId.HasValue)
            {
                this.ModelState.AddModelError("", "factory id");
            }

            if (!date.HasValue)
            {
                this.ModelState.AddModelError("", "month");
            }

            IEfStatus status = this.monthlyService.CheckIfAllMonthReportAreApproved(date.Value);
            if (!status.IsValid)
            {
                status.ToModelStateErrors(this.ModelState);
            }

            if (this.ModelState.IsValid)
            {
                Factory factory = this.data.Factories.All().Where(x => x.Id == factoryId).FirstOrDefault();
                if (factory != null)
                {
                    MonthlyTechnologicalReportsData reportData = this.data.MonthlyTechnologicalReportsDatas.All().Where(x => x.FactoryId == factoryId && x.Month == date).FirstOrDefault();
                    var creatorName = reportData == null ? string.Empty : this.UserProfile.FullName;
                    var occupation = reportData == null ? string.Empty : this.UserProfile.Occupation;
                    var dateOfCreation = reportData?.ModifiedOn;
                    var isExsisting = reportData != null;
                    var isApproved = reportData?.Approved;
                    var reportText = reportData == null ? string.Empty : reportData.Message;
                    var isMonthlyTechnologicalReportWriter = IsMonthlyTechnologicalReportWriter();
                    var isMonthlyTechnologicalApprover = IsMonthlyTechnologicalApprover();
                    var isPowerUser = IsPowerUser();

                    return Json(new {
                        factoryName = factory.FullName,
                        creatorName = creatorName,
                        occupation = occupation,
                        dateOfCreation = dateOfCreation,
                        isExsisting = isExsisting,
                        isApproved = isApproved,
                        reportText = reportText,
                        isValid = true,
                        isMonthlyTechnologicalReportWriter = isMonthlyTechnologicalReportWriter,
                        isMonthlyTechnologicalApprover = isMonthlyTechnologicalApprover,
                        isPowerUser = isPowerUser,
                    });
                }

                return Json(new { factoryName = string.Empty });
            }
            else
            {
                return Json(new {
                    factoryName = string.Empty,
                    creatorName = string.Empty,
                    occupation = string.Empty,
                    dateOfCreation = DateTime.Today,
                    isExsisting = false,
                    isApproved = false,
                    reportText = string.Empty,
                    isValid = false,
                    isMonthlyTechnologicalReportWriter = false,
                    isMonthlyTechnologicalApprover = false,
                    isPowerUser = false,
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveReport([DataSourceRequest]DataSourceRequest request, int? factoryId, DateTime? date, string reportText)
        {
            if (ModelState.IsValid)
            {
                MonthlyTechnologicalReportsData reportData = this.data.MonthlyTechnologicalReportsDatas.All().Where(x => x.FactoryId == factoryId && x.Month == date).FirstOrDefault();
                if (reportData != null && reportData.Approved)
                {
                    ModelState.AddModelError("", "Описанието на технологичният отчет е вече потвърден. Корекции не са разрешени.");
                }

                if (ModelState.IsValid)
                {
                    if (reportData == null)
                    {
                        var record = new MonthlyTechnologicalReportsData
                        {
                            FactoryId = factoryId.Value,
                            Month = date.Value,
                            Message = reportText
                        };
                        this.data.MonthlyTechnologicalReportsDatas.Add(record);
                    }
                    else
                    {
                        reportData.Message = reportText;
                        this.data.MonthlyTechnologicalReportsDatas.Update(reportData);
                    }

                    IEfStatus result = this.data.SaveChanges(this.UserProfile.UserName);
                    return Json(new { IsConfirmed = result.IsValid }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    var errors = GetErrorListFromModelState(ModelState);
                    return Json(new { data = new { errors = errors } });
                }
            }
            else
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                var errors = GetErrorListFromModelState(ModelState);
                return Json(new { data = new { errors = errors } });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmReport([DataSourceRequest]DataSourceRequest request, int? factoryId, DateTime? date, string reportText)
        {
            if (string.IsNullOrWhiteSpace(reportText))
            {
                ModelState.AddModelError("reportText", "не е въведено описание на технологичният отчет.");
            }

            if (ModelState.IsValid)
            {
                MonthlyTechnologicalReportsData reportData = this.data.MonthlyTechnologicalReportsDatas.All().Where(x => x.FactoryId == factoryId && x.Month == date).FirstOrDefault();
                if (reportData != null && reportData.Approved)
                {
                    ModelState.AddModelError("", "Описанието на технологичният отчет е вече потвърден. Корекции не са разрешени.");
                }

                if (ModelState.IsValid)
                {
                    if (reportData == null)
                    {
                        var record = new MonthlyTechnologicalReportsData
                        {
                            FactoryId = factoryId.Value,
                            Month = date.Value,
                            Approved = true,
                            ApprovedBy = this.UserProfile.UserName,
                        };
                        this.data.MonthlyTechnologicalReportsDatas.Add(record);
                    }
                    else
                    {
                        reportData.Approved = true;
                        reportData.ApprovedBy = this.UserProfile.UserName;
                        this.data.MonthlyTechnologicalReportsDatas.Update(reportData);
                    }

                    IEfStatus result = this.data.SaveChanges(this.UserProfile.UserName);
                    return Json(new { IsConfirmed = result.IsValid }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    List<string> errors = GetErrorListFromModelState(ModelState);
                    return Json(new { data = new { errors = errors } });
                }
            }
            else
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                List<string> errors = GetErrorListFromModelState(ModelState);
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