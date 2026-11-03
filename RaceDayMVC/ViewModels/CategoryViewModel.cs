namespace RaceDayMVC.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Distance { get; set; }
        public string DistanceUnit { get; set; } = "km";
        public decimal EntryFee { get; set; }
        public DateTime StartTime { get; set; }
    }
}
