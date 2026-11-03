namespace RaceDayMVC.ViewModels
{
    /// <summary>
    /// Result object for registration attempts with detailed error information.
    /// </summary>
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        /// <summary>
        /// Dictionary of field names and their associated error messages.
        /// Example: { "Email", "Email is already in use" }
        /// </summary>
        public Dictionary<string, string> FieldErrors { get; set; } = new();

        /// <summary>
        /// List of general error messages (not field-specific).
        /// </summary>
        public List<string> GeneralErrors { get; set; } = new();

        /// <summary>
        /// Creates a successful registration result.
        /// </summary>
        public static RegistrationResult SuccessResult(string message = "Registration successful. Please log in.")
        {
            return new RegistrationResult
            {
                Success = true,
                Message = message
            };
        }

        /// <summary>
        /// Creates a failed registration result with field errors.
        /// </summary>
        public static RegistrationResult FailureResult(string message, Dictionary<string, string>? fieldErrors = null, List<string>? generalErrors = null)
        {
            return new RegistrationResult
            {
                Success = false,
                Message = message,
                FieldErrors = fieldErrors ?? new(),
                GeneralErrors = generalErrors ?? new()
            };
        }

        /// <summary>
        /// Creates a failed registration result from an error response.
        /// </summary>
        public static RegistrationResult FromApiResponse(string responseContent, int statusCode)
        {
            try
            {
                var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                using var jsonDoc = System.Text.Json.JsonDocument.Parse(responseContent);
                var root = jsonDoc.RootElement;

                var result = new RegistrationResult { Success = false };

                // Try to extract message
                if (root.TryGetProperty("message", out var messageElement))
                {
                    result.Message = messageElement.GetString() ?? "Registration failed";
                }
                else if (root.TryGetProperty("error", out var errorElement))
                {
                    result.Message = errorElement.GetString() ?? "Registration failed";
                }
                else
                {
                    result.Message = "Registration failed. Please try again.";
                }

                // Try to extract field errors
                if (root.TryGetProperty("errors", out var errorsElement))
                {
                    if (errorsElement.ValueKind == System.Text.Json.JsonValueKind.Object)
                    {
                        foreach (var property in errorsElement.EnumerateObject())
                        {
                            var fieldName = property.Name;
                            if (property.Value.ValueKind == System.Text.Json.JsonValueKind.Array)
                            {
                                // If errors is an array, join them
                                var errorMessages = new List<string>();
                                foreach (var error in property.Value.EnumerateArray())
                                {
                                    var errorMsg = error.GetString();
                                    if (!string.IsNullOrEmpty(errorMsg))
                                        errorMessages.Add(errorMsg);
                                }
                                if (errorMessages.Any())
                                    result.FieldErrors[fieldName] = string.Join("; ", errorMessages);
                            }
                            else if (property.Value.ValueKind == System.Text.Json.JsonValueKind.String)
                            {
                                var errorMsg = property.Value.GetString();
                                if (!string.IsNullOrEmpty(errorMsg))
                                    result.FieldErrors[fieldName] = errorMsg;
                            }
                        }
                    }
                }

                // If no field errors, add to general errors
                if (!result.FieldErrors.Any() && root.TryGetProperty("details", out var detailsElement))
                {
                    if (detailsElement.ValueKind == System.Text.Json.JsonValueKind.String)
                    {
                        result.GeneralErrors.Add(detailsElement.GetString() ?? "");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Warning] Could not parse registration error response: {ex.Message}");
                return FailureResult("Registration failed. Please try again.");
            }
        }
    }
}
