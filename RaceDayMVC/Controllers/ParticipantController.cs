using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Services;

namespace RaceDayMVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ILogger<ParticipantController> _logger;
        private readonly ApiService _apiService;

        public ParticipantController(ILogger<ParticipantController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        // GET: Participant/MyRegistrations
        public IActionResult MyRegistrations()
        {
            // Check if user is logged in and has Participant role
            var userRole = HttpContext.Session.GetString("userRole");
            var userEmail = HttpContext.Session.GetString("userEmail");

            if (string.IsNullOrEmpty(userRole) || userRole != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["UserEmail"] = userEmail;
            return View();
        }
    }
}
