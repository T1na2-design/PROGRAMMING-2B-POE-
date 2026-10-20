namespace RaceDayMVC.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public decimal EntryFee { get; set; }
        public int MaxParticipants { get; set; }
        public int RegisteredParticipants { get; set; }
    }
}
