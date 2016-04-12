namespace CollectingProductionDataSystem.Web.Areas.Administration.ViewModels
{
    public class ProgressBarViewModel
    {
        public ProgressBarViewModel(int maxValueParam) : this(0, maxValueParam)
        {
        }

        public ProgressBarViewModel(int minValueParam, int maxValueParam)
        {
            this.MinValue = minValueParam;
            this.MaxValue = maxValueParam;
        }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }
    }
}