using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.Controllers;

namespace CollectingProductionDataSystem.Web.Areas.Documentation.Controllers
{
    public class HelpController : Controller
    {
        // GET: Documentation/Documentation
        public ActionResult Index(string nextAction)
        {
            ViewBag.NextAction = nextAction;
            return View();
        }
        public PartialViewResult ShiftData()
        {
            return PartialView();
        }
        public PartialViewResult ShiftDataMeasuringPosition()
        {
            return PartialView();
        }
        public PartialViewResult ShiftDataTank()
        {
            return PartialView();
        }
        public PartialViewResult DailyData()
        {
            return PartialView();
        }
        public PartialViewResult DailyDataMeasuringPosition()
        {
            return PartialView();
        }
        public PartialViewResult DailyDataExpedition()
        {
            return PartialView();
        }
        public PartialViewResult ShiftDataMeasuringPositionReport()
        {
            return PartialView();
        }
        public PartialViewResult SummaryReport()
        {
            return PartialView();
        }
        public PartialViewResult DailyDataPipeLines()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyData()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyDataIndoorPipeLines()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyDataTechPlants()
        {
            return PartialView();
        }
        public PartialViewResult DailyDataEnergyFlows()
        {
            return PartialView();
        }

        public PartialViewResult HelpNomenclature()
        {
            return PartialView();
        }
        public PartialViewResult ShiftDataHighwayPipelines()
        {
            return PartialView();
        }
        public PartialViewResult DailyDataStatusTank()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyDataHydroCarbons()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyDataFreshWater()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyDataCircularWater()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyDataPotableWater()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyDataHeatEnergy()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyDataChemicalClearingWater()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyDataElectricalEnergy()
        {
            return PartialView();
        }
        public PartialViewResult SummaryReportDescription()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyAir()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyNitrogen()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyDataChemicals()
        {
            return PartialView();
        }
        public PartialViewResult NomTypeProductDaily()
        {
            return PartialView();
        }
        public PartialViewResult NomDirection()
        {
            return PartialView();
        }
        public PartialViewResult NomCauseForEdit()
        {
            return PartialView();
        }
        public PartialViewResult NomTypeMaterial()
        {
            return PartialView();
        }
        public PartialViewResult NomMeasuringUnit()
        {
            return PartialView();
        }
        public PartialViewResult NomProduct()
        {
            return PartialView();
        }
        public PartialViewResult NomShift()
        {
            return PartialView();
        }
        public PartialViewResult NomTypeProduct()
        {
            return PartialView();
        }
        public PartialViewResult NomTypeProductShift()
        {
            return PartialView();
        }
        public PartialViewResult NomTypeTransport()
        {
            return PartialView();
        }
        public PartialViewResult NomPhdServers()
        {
            return PartialView();
        }
        public PartialViewResult NomSortMaterial()
        {
            return PartialView();
        }
        public PartialViewResult HelpProductionData()
        {
            return PartialView();
        }
        public PartialViewResult Plant()
        {
            return PartialView();
        }
        public PartialViewResult Production()
        {
            return PartialView();
        }
        public PartialViewResult Installation()
        {
            return PartialView();
        }
        public PartialViewResult MeasuringPositions()
        {
            return PartialView();
        }
        public PartialViewResult DailyMeasuringPositions()
        {
            return PartialView();
        }
        public PartialViewResult TeamDataFlows()
        {
            return PartialView();
        }
        public PartialViewResult Area()
        {
            return PartialView();
        }
        public PartialViewResult Park()
        {
            return PartialView();
        }
        public PartialViewResult TankConfig()
        {
            return PartialView();
        }
        public PartialViewResult TankMasterProduct()
        {
            return PartialView();
        }
        public PartialViewResult Ikunk()
        {
            return PartialView();
        }
        public PartialViewResult Zone()
        {
            return PartialView();
        }
        public PartialViewResult MeasuringPointConfig()
        {
            return PartialView();
        }
        public PartialViewResult TankStatus()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyReportType()
        {
            return PartialView();
        }
        public PartialViewResult UnitsMonthlyConfig()
        {
            return PartialView();
        }
        public PartialViewResult MonthlyFuel()
        {
            return PartialView();
        }

        public PartialViewResult UsersAdministration()
        {
            return PartialView();
        }

        public PartialViewResult RolesAdministration()
        {
            return PartialView();
        }

        public PartialViewResult ApplicationLogStatistics()
        {
            return PartialView();
        }

        public PartialViewResult UsersStatistics()
        {
            return PartialView();
        }

        public PartialViewResult GlobalMessagesStatistics()
        {
            return PartialView();
        }
        public PartialViewResult EnteredByUserValuesMessagesStatistics()
        {
            return PartialView();
        }
    }
}