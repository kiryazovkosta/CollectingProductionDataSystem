namespace CollectingProductionDataSystem.Web.AppStart
{
    using System.Web.Mvc;
    
    public static class ViewEnginesConfig
    {
        /// <summary>
        /// Registers the view engines.
        /// </summary>
        /// <param name="engines">The engines.</param>
        public static void RegisterViewEngines(ViewEngineCollection engines)
        {
            engines.Clear();
            engines.Add(new RazorViewEngine());
        }
    }
}