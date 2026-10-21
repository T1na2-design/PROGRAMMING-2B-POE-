namespace RaceDayMVC.ViewModels
{
    public class EventViewModel
    {
        public int EventId { get; set; }
        public int Id { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string RaceType { get; set; } = string.Empty;
        public decimal Distance { get; set; }
        public decimal EntryFee { get; set; }
        public int MaxParticipants { get; set; }
        public int RegisteredParticipants { get; set; }
        public int ParticipantCount { get; set; }
        public string OrganizerName { get; set; } = string.Empty;
        public string Status { get; set; } = "Upcoming";
        public string PrizePool { get; set; } = string.Empty;
    }
}
