using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyTechnicalReporting.Models
{
    public class Approver
    {
        public string CreatorName { get; set; }

        public string Occupation { get; set; }

        public DateTime? DateOfCreation { get; set; }

    }
}