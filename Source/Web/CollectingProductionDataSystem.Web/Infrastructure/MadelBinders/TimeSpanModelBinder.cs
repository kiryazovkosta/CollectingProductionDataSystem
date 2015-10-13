using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Infrastructure.MadelBinders
{
    public class TimeSpanModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            var model = this.CreateModel(controllerContext,
                bindingContext, bindingContext.ModelType);
            bindingContext.ModelMetadata.Model = model;

            var targets = model.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => Attribute.IsDefined(p, typeof(TimeSpanComponentAttribute)));


            if (targets == null)
                throw new MemberAccessException(PropertyNotFound);

            foreach (var target in targets)
            {
                int hours = 0, minutes = 0, seconds = 0, days = 0;
                days = ParseTimeComponent(target.Name + "." + DaysKey, bindingContext);
                hours = ParseTimeComponent(target.Name + "." + HoursKey, bindingContext);
                minutes = ParseTimeComponent(target.Name + "." + MinutesKey, bindingContext);
                seconds = ParseTimeComponent(target.Name + "." + SecondsKey, bindingContext);

                target.SetValue(model, new TimeSpan(days, hours, minutes, seconds));
            }

            return base.BindModel(controllerContext, bindingContext);
        }

        public int ParseTimeComponent(string component,
            ModelBindingContext bindingContext)
        {
            int result = 0;
            var val = bindingContext.ValueProvider.GetValue(component);

            if (!int.TryParse(val.AttemptedValue, out result))
                bindingContext.ModelState.AddModelError(component,
                    String.Format(Resources.ErrorMessages.Required, component));

            // Again, this is important
            bindingContext.ModelState.SetModelValue(component, val);

            return result;
        }

        private readonly string DaysKey = "Days";
        private readonly string HoursKey = "Hours";
        private readonly string MinutesKey = "Minutes";
        private readonly string SecondsKey = "Seconds";
        private readonly string PropertyNotFound = "Could not bind to TimeSpan property.  Did you forget to decorate " +
                                                   "a property with TimeSpanComponentAttribute?";
    }
}