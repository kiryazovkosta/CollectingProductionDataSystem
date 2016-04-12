namespace CollectingProductionDataSystem.Web.Areas.Administration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
    using Resources = App_GlobalResources.Resources;


    public class ForceLoadingViewModel
    {
        [Display(Name = "BeginDate", ResourceType = typeof(Resources.Layout))]
        public DateTime BeginDate { get; set; }

        [Display(Name = "EndDate", ResourceType = typeof(Resources.Layout))]
        public DateTime EndDate { get; set; }

        [Display(Name = "Shift", ResourceType = typeof(Resources.Layout))]
        public int ShiftId { get; set; }

        [Display(Name = "Factory", ResourceType = typeof(Resources.Layout))]
        public int FactoryId { get; set; }

        [Display(Name = "ProcessUnit", ResourceType = typeof(Resources.Layout))]
        public int ProcessUnitId{ get; set; }

        public ProgressViewModel Progress { get; set; }
    }
 
    /// <summary>
    /// 
    /// </summary>
    public class ProgressViewModel
    {
        public int ProgressMin { get; set; }

        public int ProgressMax { get; set; }

        public int ProgressCurrent { get; set; }
    }
}