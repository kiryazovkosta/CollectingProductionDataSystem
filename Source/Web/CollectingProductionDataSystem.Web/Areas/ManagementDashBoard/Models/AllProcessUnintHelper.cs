using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.ManagementDashBoard.Models
{
    public static class AllProcessUnintHelper
    {
        public static MvcHtmlString AllProcessUnints(this HtmlHelper helper, AllProcessUnitProductionPlanViewModel model)
        {
            var htmlResult = new StringBuilder();

            htmlResult.AppendLine("<div class='container-fluid'>");

            foreach (var factory in model.Factories)
            {
                htmlResult.AppendLine("<div class='panel panel-danger'>");
                htmlResult.AppendFormat("<div class='panel-heading'>{0}</div>", factory.FullName);
                htmlResult.AppendLine();
                htmlResult.AppendLine("<div class='panel-body'>");
                htmlResult.AppendLine("<div class='row chart-tumb-row' style='width:100%; margin-top:5px;'>");
                foreach (var processUnit in factory.ProcessUnits)
                {
                    if (processUnit.HasDailyStatistics && !processUnit.IsDeleted)
                    {
                        htmlResult.AppendFormat("<div id='{0}-{1}' class='col-sm-3 pu-chart-holder' role='{2}'></div>", model.ElementPrefix, processUnit.Id, model.ElementRole);
                        htmlResult.AppendLine();
                    }
                }
                htmlResult.AppendLine("</div>");
                htmlResult.AppendLine("</div>");
                htmlResult.AppendLine("</div>");
            }
            htmlResult.AppendLine("</div>");

            return new MvcHtmlString(htmlResult.ToString());
        }
    }
}