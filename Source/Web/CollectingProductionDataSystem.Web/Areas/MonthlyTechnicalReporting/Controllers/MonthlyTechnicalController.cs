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
    using System.Net;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Productions.Technological;
    using Models;
    using CollectingProductionDataSystem.Models.Identity;

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
        public JsonResult ReadMonthlyTechnicalData([DataSourceRequest] DataSourceRequest request, int? factoryId,
            DateTime? date)
        {
            ValidateModelState(factoryId, date);

            if (!this.ModelState.IsValid)
            {
                DataSourceResult kendoResult = new List<MonthlyTechnicalViewModel>().ToDataSourceResult(request,
                    ModelState);
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
                        processUnits = this.data.ProcessUnits.All()
                            .Where(x => x.FactoryId == factoryId)
                            .Select(x => x.Id);
                    }
                    else
                    {
                        List<int> processUnitsForFactory = this.data.ProcessUnits.All()
                            .Where(x => x.FactoryId == factoryId)
                            .Select(x => x.Id).ToList();
                        processUnits = this.UserProfile.ProcessUnits.ToList()
                            .Where(p => processUnitsForFactory.Contains(p.Id))
                            .Select(x => x.Id);
                    }
                    IEnumerable<MonthlyTechnicalReportDataDto> dbResult = this.monthlyService.ReadMonthlyTechnologicalDataAsync(date.Value, processUnits.ToArray());
                    IEnumerable<MonthlyTechnicalViewModel> vmResult = Mapper.Map<IEnumerable<MonthlyTechnicalViewModel>>(dbResult);
                    DataSourceResult kendoResult = vmResult.ToDataSourceResult(request, ModelState);
                    JsonResult output = Json(kendoResult, JsonRequestBehavior.AllowGet);
                    output.MaxJsonLength = int.MaxValue;
                    return output;
                }
            }
            else
            {
                DataSourceResult kendoResult = new List<MonthlyTechnicalViewModel>().ToDataSourceResult(request,
                    ModelState);
                return Json(kendoResult);
            }
        }

        private void ValidateModelState(int? factoryId, DateTime? date)
        {
            if (factoryId == null)
            {
                this.ModelState.AddModelError("factoryId",
                    string.Format(Resources.ErrorMessages.Required, Resources.Layout.ChooseFactory));
            }

            if (date == null)
            {
                this.ModelState.AddModelError("date",
                    string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsConfirmed(DateTime date)
        {
            return this.Json(new { IsConfirmed = true });
        }

        private bool IsPowerUser()
        {
            return this.UserProfile.UserRoles.Any(x => CommonConstants.PowerUsers.Any(y => y == x.Name));
        }

        private bool IsMonthlyTechnologicalReportWriter()
        {
            return
                this.UserProfile.UserRoles.Any(x => CommonConstants.MonthlyTechnologicalReportWriterUsers.Any(y => y == x.Name));
        }

        private bool IsMonthlyTechnologicalApprover()
        {
            return
                this.UserProfile.UserRoles.Any(x => CommonConstants.MonthlyTechnologicalApproverUsers.Any(y => y == x.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetFactoryName([DataSourceRequest] DataSourceRequest request, int? factoryId)
        {
            if (!factoryId.HasValue)
            {
                this.ModelState.AddModelError("", "factory id");
            }

            if (this.ModelState.IsValid)
            {
                Factory factory = this.data.Factories.All().FirstOrDefault(x => x.Id == factoryId);
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
        public ActionResult GetExportData(int factoryId, DateTime date)
        {
            IEfStatus status = this.monthlyService.CheckIfAllMonthReportAreApproved(date);
            if (!status.IsValid)
            {
                status.ToModelStateErrors(this.ModelState);
            }

            if (this.ModelState.IsValid)
            {
                Factory factory = this.data.Factories.All().FirstOrDefault(x => x.Id == factoryId);

                if (factory != null)
                {
                    MonthlyTechnologicalReportsData reportData =
                        this.data.MonthlyTechnologicalReportsDatas.All()
                        .FirstOrDefault(x => x.FactoryId == factoryId && x.Month == date);

                    //var exsistingTechData = this.data.Mon

                    Composer composer = this.GetComposer(reportData);
                    Approver approver = this.GetApprover(reportData);
                    bool isExsisting = reportData != null;
                    bool isComposed = reportData?.IsComposed ?? false;
                    bool isApproved = reportData?.IsApproved ?? false;
                    string reportText = reportData?.Message ?? string.Empty;
                    bool isMonthlyTechnologicalReportWriter = this.IsMonthlyTechnologicalReportWriter();
                    bool isMonthlyTechnologicalApprover = this.IsMonthlyTechnologicalApprover();
                    bool isPowerUser = this.IsPowerUser();

                    return this.Json(new
                    {
                        PdfExportDetails = new
                        {
                            //header info
                            FactoryName = factory.FullName,
                            Month = DateTime.Now,
                            MonthAsString = date.ToString("MMMM yyyy г."),

                            //footer table info
                            //first row
                            CreatorName = composer?.Name ?? "",
                            Occupation = composer?.Occupation ?? "",
                            DateOfCreation = composer?.Date,

                            //second row
                            ApproverName = approver?.Name,
                            ApproverOccupation = approver?.Occupation,
                            DateOfApprovement = approver?.Date
                        },
                        EditorContent = reportText,
                        IsExsisting = isExsisting,
                        IsComposed = isComposed,
                        IsApproved = isApproved,
                        IsValid = true,
                        IsMonthlyTechnologicalReportWriter = isMonthlyTechnologicalReportWriter,
                        IsMonthlyTechnologicalApprover = isMonthlyTechnologicalApprover,
                        IsPowerUser = isPowerUser

                    });
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, string.Format(Resources.ErrorMessages.NoFactoryError, factoryId));
                }
            }

            IEnumerable<string> allErrors = this.ModelState.Values.SelectMany(v => v.Errors).Select(x=>x.ErrorMessage);
            return this.Json(new {errors = allErrors});
        }

        private string GetUserName(ApplicationUser user)
        {
            string name = string.Empty;
            if (user != null)
            {
                name = $"{user.FirstName} {user.LastName}";
            }

            return name;
        }

        private Composer GetComposer(MonthlyTechnologicalReportsData reportData)
        {
            var composer = new Composer();
            if (reportData?.IsComposed == true)
            {
                ApplicationUser user = this.data.Users.All().FirstOrDefault(x => x.UserName == reportData.ComposedBy);
                composer.Name = this.GetUserName(user);
                composer.Occupation = user?.Occupation ?? string.Empty;
                composer.Date = reportData.ComposedOn;
            }

            return composer;
        }

        private Approver GetApprover(MonthlyTechnologicalReportsData reportData)
        {
            var approver = new Approver();
            if (reportData?.IsApproved == true)
            {
                ApplicationUser user = this.data.Users.All().FirstOrDefault(x => x.UserName == reportData.ApprovedBy);
                approver.Name = this.GetUserName(user);
                approver.Occupation = user?.Occupation ?? string.Empty;
                approver.Date = reportData.ApprovedOn;
            }
            return approver;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveReport([DataSourceRequest] DataSourceRequest request, int? factoryId, DateTime? date,
            string reportText)
        {
            if (this.ModelState.IsValid)
            {
                MonthlyTechnologicalReportsData reportData =
                    this.data.MonthlyTechnologicalReportsDatas.All()
                        .Where(x => x.FactoryId == factoryId && x.Month == date)
                        .FirstOrDefault();

                if (reportData?.IsComposed == true)
                {
                    ModelState.AddModelError("", "Описанието на технологичният отчет е вече съставено. Корекции не са разрешени.");
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
        public ActionResult ComposeReport([DataSourceRequest] DataSourceRequest request, int? factoryId, DateTime? date,
            string reportText)
        {
            if (string.IsNullOrWhiteSpace(reportText))
            {
                ModelState.AddModelError("reportText", "Не е въведено описание на технологичният отчет.");
            }

            if (ModelState.IsValid)
            {
                MonthlyTechnologicalReportsData reportData =
                    this.data.MonthlyTechnologicalReportsDatas.All()
                        .Where(x => x.FactoryId == factoryId && x.Month == date)
                        .FirstOrDefault();
                if (reportData != null && reportData.IsComposed)
                {
                    ModelState.AddModelError("", "Описанието на технологичният отчет е вече съставено. Корекции не са разрешени.");
                }

                if (ModelState.IsValid)
                {
                    if (reportData == null)
                    {
                        var record = new MonthlyTechnologicalReportsData
                        {
                            FactoryId = factoryId.Value,
                            Month = date.Value,
                            Message = reportText,
                            IsComposed = true,
                            ComposedBy = this.UserProfile.UserName,
                            ComposedOn = DateTime.Now,
                        };
                        this.data.MonthlyTechnologicalReportsDatas.Add(record);
                    }
                    else
                    {
                        reportData.Message = reportText;
                        reportData.IsComposed = true;
                        reportData.ComposedBy = this.UserProfile.UserName;
                        reportData.ComposedOn = DateTime.Now;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveReport([DataSourceRequest] DataSourceRequest request, int? factoryId, DateTime? date,
            string reportText)
        {
            if (string.IsNullOrWhiteSpace(reportText))
            {
                ModelState.AddModelError("reportText", "Не е въведено описание на технологичният отчет.");
            }

            if (ModelState.IsValid)
            {
                MonthlyTechnologicalReportsData reportData =
                    this.data.MonthlyTechnologicalReportsDatas.All()
                        .Where(x => x.FactoryId == factoryId && x.Month == date)
                        .FirstOrDefault();
                if (reportData == null)
                {
                    ModelState.AddModelError("", "Описанието на технологичният отчет не е съставено.");
                }

                if (reportData?.IsApproved == true)
                {
                    ModelState.AddModelError("", "Описанието на технологичният отчет е вече утвърдено. Корекции не са разрешени.");
                }

                if (ModelState.IsValid)
                {
                    reportData.Message = reportText;
                    reportData.IsApproved = true;
                    reportData.ApprovedBy = this.UserProfile.UserName;
                    reportData.ApprovedOn = DateTime.Now;
                    this.data.MonthlyTechnologicalReportsDatas.Update(reportData);
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