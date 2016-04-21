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
    }
}