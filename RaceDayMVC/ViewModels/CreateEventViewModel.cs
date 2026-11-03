using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.ViewModels
{
    public class CreateEventViewModel
    {
        [Required(ErrorMessage = "Event title is required.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 2000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event date is required.")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Event date must be in the future.")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Event time is required.")]
        [DataType(DataType.Time)]
        public string EventTime { get; set; } = string.Empty;

        [Required(ErrorMessage = "Venue is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Venue must be between 3 and 200 characters.")]
        public string Venue { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Province is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Province must be between 2 and 100 characters.")]
        public string Province { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event type is required.")]
        public string EventType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Distance is required.")]
        [Range(0.1, 999.9, ErrorMessage = "Distance must be between 0.1 and 999.9.")]
        public decimal Distance { get; set; }

        [Required(ErrorMessage = "Distance unit is required.")]
        public string DistanceUnit { get; set; } = "km";

        [Required(ErrorMessage = "Maximum participants is required.")]
        [Range(10, 100000, ErrorMessage = "Maximum participants must be between 10 and 100,000.")]
        public int MaxParticipants { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? BannerImage { get; set; }
    }

    /// <summary>
    /// Validation attribute for future dates.
    /// </summary>
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                return date > DateTime.Now;
            }
            return true;
        }
    }
}
