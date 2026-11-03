using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Services;

namespace RaceDayMVC.Controllers
{
    public class EnrolmentsController : Controller
    {
        private readonly ILogger<EnrolmentsController> _logger;
        private readonly ApiService _apiService;

        public EnrolmentsController(ILogger<EnrolmentsController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        // GET: Enrolments/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                // Check if user is logged in
                var userEmail = HttpContext.Session.GetString("userEmail");
                var userRole = HttpContext.Session.GetString("userRole");

                if (string.IsNullOrEmpty(userEmail))
                {
                    return RedirectToAction("Login", "Account");
                }

                // Get auth token from session
                var authToken = HttpContext.Session.GetString("authToken");

                // Fetch enrolments from API
                var enrolments = await _apiService.GetEnrolmentsAsync(authToken);

                // Store the user role in ViewBag for view to access
                ViewBag.UserRole = userRole;

                return View(enrolments ?? new List<RaceDayMVC.ViewModels.EnrolmentViewModel>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching enrolments");
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Enrolments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int categoryId, int eventId)
        {
            try
            {
                // Check if user is authenticated and is a Participant
                var userRole = HttpContext.Session.GetString("userRole");
                var userEmail = HttpContext.Session.GetString("userEmail");

                if (string.IsNullOrEmpty(userRole) || 
                    !userRole.Equals("Participant", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["ErrorMessage"] = "You must be logged in as a Participant to enrol in an event.";
                    return RedirectToAction("Login", "Account");
                }

                if (string.IsNullOrEmpty(userEmail))
                {
                    TempData["ErrorMessage"] = "User session is invalid. Please log in again.";
                    return RedirectToAction("Login", "Account");
                }

                // Get auth token from session
                var authToken = HttpContext.Session.GetString("authToken");

                // Call API to enrol participant
                var result = await _apiService.EnrolAsync(categoryId, authToken);

                if (result)
                {
                    TempData["SuccessMessage"] = "You have successfully enrolled in this category!";
                    _logger.LogInformation($"User {userEmail} enrolled in category {categoryId}");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to enrol in this category. Please try again.";
                    _logger.LogWarning($"Failed to enrol user {userEmail} in category {categoryId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enrolling participant in category {categoryId}");
                TempData["ErrorMessage"] = "An error occurred while processing your enrolment. Please try again.";
            }

            return RedirectToAction("Details", "Events", new { id = eventId });
        }
    }
}
