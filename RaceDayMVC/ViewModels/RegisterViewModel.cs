using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "ID number is required.")]
        [StringLength(13, MinimumLength = 8, ErrorMessage = "ID number must be between 8 and 13 characters.")]
        public string IDNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", 
            ErrorMessage = "Password must contain uppercase, lowercase, and numeric characters.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emergency contact name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Emergency contact name must be between 2 and 100 characters.")]
        public string EmergencyContactName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emergency contact number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string EmergencyContactNumber { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Medical conditions cannot exceed 500 characters.")]
        public string? MedicalConditions { get; set; }

        [StringLength(100, ErrorMessage = "Club affiliation cannot exceed 100 characters.")]
        public string? ClubAffiliation { get; set; }
    }
}
