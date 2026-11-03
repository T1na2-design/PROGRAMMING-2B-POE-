namespace RaceDayMVC.ViewModels
{
    public class CaptureResultViewModel
    {
        public int EnrolmentID { get; set; }
        public TimeSpan? FinishTime { get; set; }
        public int? OverallPosition { get; set; }
        public int? GenderPosition { get; set; }
        public int? CategoryPosition { get; set; }
        public decimal? Pace { get; set; }
    }
}
