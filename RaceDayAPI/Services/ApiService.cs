using System.Text;
using System.Text.Json;

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
