using System.Text;
using System.Text.Json;
using RaceDayMVC.ViewModels;

namespace RaceDayMVC.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl;

        public ApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:5001";
        }

        /// <summary>
        /// Authenticates a user and returns a token if successful.
        /// </summary>
        public async Task<string?> LoginAsync(string email, string password)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var loginRequest = new { email, password };
                var content = new StringContent(
                    JsonSerializer.Serialize(loginRequest),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync($"{_apiBaseUrl}/api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("token", out var tokenElement))
                    {
                        return tokenElement.GetString();
                    }
                }

                return null;
            }
            catch (Exception)
            {
                // Log exception here if logging is configured
                return null;
            }
        }

        /// <summary>
        /// Registers a new user via the API.
        /// </summary>
        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var registerRequest = new
                {
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    email = model.Email,
                    phoneNumber = model.PhoneNumber,
                    idNumber = model.IDNumber,
                    gender = model.Gender,
                    password = model.Password,
                    confirmPassword = model.ConfirmPassword
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(registerRequest),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync($"{_apiBaseUrl}/api/auth/register", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Log exception here if logging is configured
                return false;
            }
        }

        /// <summary>
        /// Retrieves a list of racing events from the API.
        /// </summary>
        public async Task<List<object>> GetEventsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_apiBaseUrl}/api/events");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var events = JsonSerializer.Deserialize<List<object>>(jsonResponse);
                    return events ?? new List<object>();
                }

                return new List<object>();
            }
            catch (Exception)
            {
                // Log exception here if logging is configured
                return new List<object>();
            }
        }
    }
}
