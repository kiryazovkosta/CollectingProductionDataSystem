using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;

namespace CollectingProductionDataSystem.Web.Infrastructure.Extentions
{
    public static class IEfStatusExtentions
    {
        public static void ToModelStateErrors(this IEfStatus status, ModelStateDictionary modelState) 
        {
            foreach (var error in status.EfErrors) 
            {
                modelState.AddModelError(error.MemberNames.FirstOrDefault()?? string.Empty, error.ErrorMessage);
            }
        }
    }
}