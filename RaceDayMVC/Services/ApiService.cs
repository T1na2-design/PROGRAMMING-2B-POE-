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
        /// Registers a new user via the API and returns detailed result information.
        /// </summary>
        public async Task<RegistrationResult> RegisterAsync(RegisterViewModel model)
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
                    return RegistrationResult.SuccessResult();
                }
                else
                {
                    Console.WriteLine($"[API Error] RegisterAsync failed with status code: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Error] Response: {errorContent}");

                    // Parse the error response to extract detailed error messages
                    return RegistrationResult.FromApiResponse(errorContent, (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Exception] RegisterAsync error: {ex.Message}");
                Console.WriteLine($"[Exception] Stack trace: {ex.StackTrace}");
                return RegistrationResult.FailureResult("An unexpected error occurred during registration. Please try again.");
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
        /// <summary>
        /// Creates a new event via the API.
        /// Returns the newly created Event ID on success, or 0 if failed.
        /// </summary>
        public async Task<int> CreateEventAsync(CreateEventViewModel model, string? authToken = null)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

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

                    // Try to extract the event ID from the response
                    try
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                        // Try to deserialize as an object with EventID/Id property
                        using var jsonDoc = System.Text.Json.JsonDocument.Parse(responseContent);
                        var root = jsonDoc.RootElement;

                        if (root.TryGetProperty("eventId", out var eventIdElement) || 
                            root.TryGetProperty("id", out eventIdElement))
                        {
                            if (eventIdElement.TryGetInt32(out int eventId))
                            {
                                Console.WriteLine($"[API Response] New Event ID: {eventId}");
                                return eventId;
                            }
                        }

                        // If we can't extract the ID, return a positive value to indicate success
                        // (The caller will need to handle this gracefully)
                        Console.WriteLine("[API Response] Could not extract EventID from response, returning 1");
                        return 1;
                    }
                    catch (Exception parseEx)
                    {
                        Console.WriteLine($"[Warning] Could not parse event ID from response: {parseEx.Message}");
                        // Return 1 to indicate success but ID couldn't be extracted
                        return 1;
                    }
                }
                else
                {
                    Console.WriteLine($"[API Error] CreateEventAsync failed with status code: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Error] Response: {errorContent}");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Exception] CreateEventAsync error: {ex.Message}");
                Console.WriteLine($"[Exception] Stack trace: {ex.StackTrace}");
                return 0;
            }
        }

        /// <summary>
        /// Enrolls a participant in an event category via the API.
        /// </summary>
        public async Task<bool> EnrolAsync(int categoryId, string? authToken = null)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Create enrolment request
                var enrolmentRequest = new { categoryId };

                Console.WriteLine($"[API Request] Enrolling participant in category {categoryId}");
                Console.WriteLine($"[API Request] Payload: {JsonSerializer.Serialize(enrolmentRequest)}");

                var content = new StringContent(
                    JsonSerializer.Serialize(enrolmentRequest),
                    Encoding.UTF8,
                    "application/json");

                // Add authorization header if token exists
                if (!string.IsNullOrEmpty(authToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
                }

                var response = await client.PostAsync($"{_apiBaseUrl}/api/enrolments", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[API Success] EnrolAsync succeeded with status code: {response.StatusCode}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"[API Error] EnrolAsync failed with status code: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Error] Response: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Exception] EnrolAsync error: {ex.Message}");
                Console.WriteLine($"[Exception] Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Retrieves the list of enrolments for the logged-in user.
        /// </summary>
        public async Task<List<EnrolmentViewModel>?> GetEnrolmentsAsync(string? authToken = null)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                Console.WriteLine($"[API Request] Fetching enrolments from {_apiBaseUrl}/api/enrolments");

                // Add authorization header if token exists
                if (!string.IsNullOrEmpty(authToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
                }

                var response = await client.GetAsync($"{_apiBaseUrl}/api/enrolments");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Success] GetEnrolmentsAsync response: {content}");

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var enrolments = JsonSerializer.Deserialize<List<EnrolmentViewModel>>(content, options);

                    return enrolments ?? new List<EnrolmentViewModel>();
                }
                else
                {
                    Console.WriteLine($"[API Error] GetEnrolmentsAsync failed with status code: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Error] Response: {errorContent}");
                    return new List<EnrolmentViewModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Exception] GetEnrolmentsAsync error: {ex.Message}");
                Console.WriteLine($"[Exception] Stack trace: {ex.StackTrace}");
                return new List<EnrolmentViewModel>();
            }
        }

        /// <summary>
        /// Retrieves the list of race results for the logged-in user.
        /// </summary>
        public async Task<List<ResultViewModel>?> GetResultsAsync(string? authToken = null)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                Console.WriteLine($"[API Request] Fetching results from {_apiBaseUrl}/api/results");

                // Add authorization header if token exists
                if (!string.IsNullOrEmpty(authToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
                }

                var response = await client.GetAsync($"{_apiBaseUrl}/api/results");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Success] GetResultsAsync response: {content}");

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var results = JsonSerializer.Deserialize<List<ResultViewModel>>(content, options);

                    return results ?? new List<ResultViewModel>();
                }
                else
                {
                    Console.WriteLine($"[API Error] GetResultsAsync failed with status code: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Error] Response: {errorContent}");
                    return new List<ResultViewModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Exception] GetResultsAsync error: {ex.Message}");
                Console.WriteLine($"[Exception] Stack trace: {ex.StackTrace}");
                return new List<ResultViewModel>();
            }
        }

        /// <summary>
        /// Captures/submits a race result for a participant enrolment.
        /// </summary>
        public async Task<bool> CaptureResultAsync(CaptureResultViewModel model, string? authToken = null)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Create result request
                var resultRequest = new
                {
                    model.EnrolmentID,
                    model.FinishTime,
                    model.OverallPosition,
                    model.GenderPosition,
                    model.CategoryPosition,
                    model.Pace
                };

                Console.WriteLine($"[API Request] Capturing result for enrolment {model.EnrolmentID}");
                Console.WriteLine($"[API Request] Payload: {JsonSerializer.Serialize(resultRequest)}");

                var content = new StringContent(
                    JsonSerializer.Serialize(resultRequest),
                    Encoding.UTF8,
                    "application/json");

                // Add authorization header if token exists
                if (!string.IsNullOrEmpty(authToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
                }

                var response = await client.PostAsync($"{_apiBaseUrl}/api/results", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[API Success] CaptureResultAsync succeeded with status code: {response.StatusCode}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"[API Error] CaptureResultAsync failed with status code: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[API Error] Response: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Exception] CaptureResultAsync error: {ex.Message}");
                Console.WriteLine($"[Exception] Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Uploads a banner image for an event to the API.
        /// </summary>
        public async Task<bool> UploadBannerAsync(int eventId, IFormFile file, string? authToken = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    Console.WriteLine("[Validation] UploadBannerAsync: No file provided");
                    return false;
                }

                var client = _httpClientFactory.CreateClient();

                Console.WriteLine($"[API Request] Uploading banner for event {eventId}, file: {file.FileName}");

                // Create multipart form data content
                using (var content = new MultipartFormDataContent())
                {
                    // Add file to the request
                    using (var stream = file.OpenReadStream())
                    {
                        var fileContent = new StreamContent(stream);
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");
                        content.Add(fileContent, "file", file.FileName);
                    }

                    // Add authorization header if token exists
                    if (!string.IsNullOrEmpty(authToken))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
                    }

                    var response = await client.PostAsync($"{_apiBaseUrl}/api/events/{eventId}/upload-banner", content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"[API Success] UploadBannerAsync succeeded with status code: {response.StatusCode}");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"[API Error] UploadBannerAsync failed with status code: {response.StatusCode}");
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"[API Error] Response: {errorContent}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Exception] UploadBannerAsync error: {ex.Message}");
                Console.WriteLine($"[Exception] Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Tests the API connection and returns a status message.
        /// </summary>
        public async Task<string> TestConnectionAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_apiBaseUrl}/api/Events");

                if (response.IsSuccessStatusCode)
                {
                    return "✅ API is connected and responding";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return "✅ API is responding (authentication required)";
                }
                else
                {
                    return $"⚠️ API returned status: {response.StatusCode}";
                }
            }
            catch (HttpRequestException ex)
            {
                return $"❌ Failed to connect to API: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"❌ Error testing API connection: {ex.Message}";
            }
        }
    }
}
