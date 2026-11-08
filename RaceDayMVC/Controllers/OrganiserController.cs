using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Services;

namespace RaceDayMVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly ILogger<OrganiserController> _logger;
        private readonly ApiService _apiService;

        public OrganiserController(ILogger<OrganiserController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        // GET: Organiser/Dashboard
        public IActionResult Dashboard()
        {
            // Check if user is logged in and has Organiser role
            var userRole = HttpContext.Session.GetString("userRole");
            var userEmail = HttpContext.Session.GetString("userEmail");

            if (string.IsNullOrEmpty(userRole) || userRole != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["UserEmail"] = userEmail;
            return View();
        }

        // GET: Organiser/CreateEvent
        public IActionResult CreateEvent()
        {
            // Check if user is logged in and has Organiser role
            var userRole = HttpContext.Session.GetString("userRole");
            var userEmail = HttpContext.Session.GetString("userEmail");

            if (string.IsNullOrEmpty(userRole) || userRole != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["UserEmail"] = userEmail;
            return View();
        }

        // POST: Organiser/CreateEvent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(IFormCollection form)
        {
            var userRole = HttpContext.Session.GetString("userRole");
            if (string.IsNullOrEmpty(userRole) || userRole != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            // TODO: Implement event creation via API
            // For now, just return to dashboard
            return RedirectToAction("Dashboard");
        }
    }
}
