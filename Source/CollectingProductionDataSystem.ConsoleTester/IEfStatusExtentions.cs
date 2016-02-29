using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using CollectingProductionDataSystem.Data.Contracts;

namespace CollectingProductionDataSystem.ConsoleTester
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