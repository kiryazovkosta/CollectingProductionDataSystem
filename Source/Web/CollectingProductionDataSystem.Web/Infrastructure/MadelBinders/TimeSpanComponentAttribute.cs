namespace CollectingProductionDataSystem.Web.Infrastructure.MadelBinders
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TimeSpanComponentAttribute : Attribute
    {
        public override bool Match(object obj)
        {
            return obj.GetType() == typeof(TimeSpan);
        }
    }
}