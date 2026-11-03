using System.Text;
using System.Text.Json;
using RaceDayMVC.ViewModels;

namespace RaceDayMVC.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl;
        private readonly MockDataService _mockDataService;

        public ApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:5001";
            _mockDataService = new MockDataService();
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
                    dateOfBirth = model.DateOfBirth,
                    role = model.Role,
                    password = model.Password,
                    confirmPassword = model.ConfirmPassword
                };

                Console.WriteLine($"[API Request] Sending registration request:");
                Console.WriteLine(JsonSerializer.Serialize(registerRequest));

                var content = new StringContent(
                    JsonSerializer.Serialize(registerRequest),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync($"{_apiBaseUrl}/api/auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[API Success] RegisterAsync succeeded with status code: {response.StatusCode}");
                }
                else
                {
                    Console.WriteLine($"[API Error] RegisterAsync failed with status code: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Error] Response: {errorContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Exception] RegisterAsync error: {ex.Message}");
                Console.WriteLine($"[Exception] Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Retrieves a list of racing events from the API.
        /// Falls back to mock data if API is unavailable.
        /// </summary>
        public async Task<List<EventViewModel>> GetEventsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_apiBaseUrl}/api/events");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // DEBUG: Output raw JSON to console
                    Console.WriteLine($"[API Response] Raw JSON from /api/events:");
                    Console.WriteLine(jsonResponse);
                    Console.WriteLine("---");

                    // Deserialize with case-insensitive property matching
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var events = JsonSerializer.Deserialize<List<EventViewModel>>(jsonResponse, options);

                    Console.WriteLine($"[Deserialization] Successfully deserialized {events?.Count ?? 0} events");

                    return events ?? new List<EventViewModel>();
                }

                Console.WriteLine($"[API Error] GetEventsAsync failed with status code: {response.StatusCode}");
                Console.WriteLine($"[Fallback] Using mock data instead");
                return _mockDataService.GetMockEvents();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Exception] GetEventsAsync error: {ex.Message}");
                Console.WriteLine($"[Exception] Stack trace: {ex.StackTrace}");
                Console.WriteLine($"[Fallback] Using mock data instead");
                return _mockDataService.GetMockEvents();
            }
        }

        /// <summary>
        /// Retrieves detailed information for a specific event from the API.
        /// Falls back to mock data if API is unavailable.
        /// </summary>
        public async Task<EventDetailsViewModel?> GetEventDetailsAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_apiBaseUrl}/api/events/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // DEBUG: Output raw JSON to console
                    Console.WriteLine($"[API Response] Raw JSON from /api/events/{id}:");
                    Console.WriteLine(jsonResponse);
                    Console.WriteLine("---");

                    // Deserialize with case-insensitive property matching
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var eventDetails = JsonSerializer.Deserialize<EventDetailsViewModel>(jsonResponse, options);

                    if (eventDetails != null)
                    {
                        Console.WriteLine($"[Deserialization] Successfully deserialized event: {eventDetails.Title}");
                    }

                    return eventDetails;
                }

                Console.WriteLine($"[API Error] GetEventDetailsAsync failed with status code: {response.StatusCode}");
                Console.WriteLine($"[Fallback] Using mock data for event ID {id}");
                return _mockDataService.GetMockEventDetails(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Exception] GetEventDetailsAsync error: {ex.Message}");
                Console.WriteLine($"[Exception] Stack trace: {ex.StackTrace}");
                Console.WriteLine($"[Fallback] Using mock data for event ID {id}");
                return _mockDataService.GetMockEventDetails(id);
            }
        }

        /// <summary>
        /// Creates a new event via the API.
        /// </summary>
        public async Task<bool> CreateEventAsync(CreateEventViewModel model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Get auth token from session/config
                var authToken = ""; // In a real app, this would come from HttpContext.Session

                // Create request object matching API schema
                var eventRequest = new
                {
                    title = model.Title,
                    description = model.Description,
                    eventDate = model.EventDate.ToString("yyyy-MM-dd"),
                    eventTime = model.EventTime,
                    venue = model.Venue,
                    city = model.City,
                    province = model.Province,
                    eventType = model.EventType,
                    distance = model.Distance,
                    distanceUnit = model.DistanceUnit,
                    maxParticipants = model.MaxParticipants
                };

                Console.WriteLine($"[API Request] Creating event: {model.Title}");
                Console.WriteLine($"[API Request] Payload: {JsonSerializer.Serialize(eventRequest)}");

                var content = new StringContent(
                    JsonSerializer.Serialize(eventRequest),
                    Encoding.UTF8,
                    "application/json");

                // Add authorization header if token exists
                if (!string.IsNullOrEmpty(authToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
                }

                var response = await client.PostAsync($"{_apiBaseUrl}/api/events", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[API Success] CreateEventAsync succeeded with status code: {response.StatusCode}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"[API Error] CreateEventAsync failed with status code: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Error] Response: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Exception] CreateEventAsync error: {ex.Message}");
                Console.WriteLine($"[Exception] Stack trace: {ex.StackTrace}");
                return false;
            }
        }
    }
}
