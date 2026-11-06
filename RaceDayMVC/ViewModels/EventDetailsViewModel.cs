namespace RaceDayMVC.ViewModels
{
    public class EventDetailsViewModel
    {
        public int EventID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Venue { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public decimal Distance { get; set; }
        public string DistanceUnit { get; set; } = "km";
        public int? MaxParticipants { get; set; }
        public string Status { get; set; } = "Upcoming";
        public string? BannerUrl { get; set; }
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
