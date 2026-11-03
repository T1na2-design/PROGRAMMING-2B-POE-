using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Services;
using RaceDayMVC.ViewModels;

namespace RaceDayMVC.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ILogger<ResultsController> _logger;
        private readonly ApiService _apiService;

        public ResultsController(ILogger<ResultsController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        // GET: Results/Index
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

                // Fetch results from API
                var results = await _apiService.GetResultsAsync(authToken);

                // Store the user role in ViewBag for view to access
                ViewBag.UserRole = userRole;

                return View(results ?? new List<ResultViewModel>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching results");
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Results/CaptureResult
        public async Task<IActionResult> CaptureResult(int enrolmentId)
        {
            try
            {
                // Check if user is an Organiser
                var userRole = HttpContext.Session.GetString("userRole");
                var userEmail = HttpContext.Session.GetString("userEmail");

                if (string.IsNullOrEmpty(userRole) || 
                    !userRole.Equals("Organiser", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["ErrorMessage"] = "Only organisers can capture results.";
                    return RedirectToAction("Login", "Account");
                }

                if (string.IsNullOrEmpty(userEmail))
                {
                    TempData["ErrorMessage"] = "User session is invalid. Please log in again.";
                    return RedirectToAction("Login", "Account");
                }

                // Create a new capture result form with the enrolment ID
                var model = new CaptureResultViewModel
                {
                    EnrolmentID = enrolmentId
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading capture result form for enrolment {enrolmentId}");
                TempData["ErrorMessage"] = "An error occurred while loading the capture result form.";
                return RedirectToAction("Index", "Enrolments");
            }
        }

        // POST: Results/CaptureResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CaptureResult(CaptureResultViewModel model)
        {
            try
            {
                // Check if user is an Organiser
                var userRole = HttpContext.Session.GetString("userRole");
                var userEmail = HttpContext.Session.GetString("userEmail");

                if (string.IsNullOrEmpty(userRole) || 
                    !userRole.Equals("Organiser", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["ErrorMessage"] = "Only organisers can capture results.";
                    return RedirectToAction("Login", "Account");
                }

                if (ModelState.IsValid)
                {
                    // Get auth token from session
                    var authToken = HttpContext.Session.GetString("authToken");

                    // Call API to capture result
                    var result = await _apiService.CaptureResultAsync(model, authToken);

                    if (result)
                    {
                        TempData["SuccessMessage"] = "Result captured successfully!";
                        _logger.LogInformation($"Organiser {userEmail} captured result for enrolment {model.EnrolmentID}");
                        return RedirectToAction("Index", "Enrolments");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to capture result. Please try again.");
                        _logger.LogWarning($"Failed to capture result for enrolment {model.EnrolmentID}");
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error capturing result for enrolment {model.EnrolmentID}");
                ModelState.AddModelError(string.Empty, "An error occurred while capturing the result.");
                return View(model);
            }
        }
    }
}
