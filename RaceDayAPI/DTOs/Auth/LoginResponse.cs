namespace RaceDay.DTOs.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public UserResponse User { get; set; }
    }
}
