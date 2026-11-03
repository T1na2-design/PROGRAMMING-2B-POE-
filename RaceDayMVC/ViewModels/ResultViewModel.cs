namespace RaceDayMVC.ViewModels
{
    public class ResultViewModel
    {
        public int ResultID { get; set; }
        public string? EventName { get; set; }
        public string? CategoryName { get; set; }
        public TimeSpan? FinishTime { get; set; }
        public int? OverallPosition { get; set; }
        public int? OverallTotal { get; set; }
        public int? GenderPosition { get; set; }
        public int? GenderTotal { get; set; }
        public int? CategoryPosition { get; set; }
        public int? CategoryTotal { get; set; }
        public decimal? Pace { get; set; }
    }
}
