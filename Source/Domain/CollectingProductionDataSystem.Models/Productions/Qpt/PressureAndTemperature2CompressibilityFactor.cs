namespace CollectingProductionDataSystem.Models.Productions.Qpt
{
    public partial class PressureAndTemperature2CompressibilityFactor
    {
        public int Id { get; set; }
        public decimal Pressure { get; set; }
        public decimal Temperature { get; set; }
        public decimal CompressibilityFactor { get; set; }
    }
}
