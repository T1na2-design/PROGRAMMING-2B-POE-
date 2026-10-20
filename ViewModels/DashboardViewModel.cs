namespace RaceDayMVC.ViewModels
{
    public class DashboardViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public List<EventViewModel> AvailableEvents { get; set; } = new();
        public List<EventViewModel> RegisteredEvents { get; set; } = new();
    }
}
