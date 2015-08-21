namespace CollectingProductionDataSystem.Constants
{
    public static class PhdConnection
    {
        private static readonly string toleraneColumn = "Tolerance";
        private static readonly string hostNameColumn = "HostName";
        private static readonly string unitsColumn = "Units";
        private static readonly string confidenceColumn = "Confidence";
        private static readonly string tagNameColumn = "TagName";
        private static readonly string valueColumn = "Value";

        public static string ValueColumn
        {
            get
            {
                return valueColumn;
            }
        }

        public static string TagNameColumn
        {
            get
            {
                return tagNameColumn;
            }
        }

        public static string ConfidenceColumn
        {
            get
            {
                return confidenceColumn;
            }
        }

        public static string UnitsColumn
        {
            get
            {
                return unitsColumn;
            }
        }

        public static string HostNameColumn
        {
            get
            {
                return hostNameColumn;
            }
        }

        public static string ToleraneColumn
        {
            get
            {
                return toleraneColumn;
            }
        }
    }
}
