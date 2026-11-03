using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Services;
using RaceDayMVC.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RaceDayMVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly ILogger<EventsController> _logger;
        private readonly ApiService _apiService;

        public EventsController(ILogger<EventsController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            try
            {
                var events = await _apiService.GetEventsAsync();
                return View(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching events");
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Fetch event details from API
                var eventDetails = await _apiService.GetEventDetailsAsync(id.Value);

                if (eventDetails == null)
                {
                    return NotFound();
                }

                return View(eventDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching event details for ID {id}");
                return NotFound();
            }
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            // Check if user is authenticated and has organiser role
            var userRole = HttpContext.Session.GetString("userRole");

            if (string.IsNullOrEmpty(userRole) || !userRole.Equals("Organiser", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventViewModel model)
        {
            // Check authorization
            var userRole = HttpContext.Session.GetString("userRole");
            if (string.IsNullOrEmpty(userRole) || !userRole.Equals("Organiser", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _apiService.CreateEventAsync(model);

                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to create event. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating event");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the event.");
                }
            }

            return View(model);
        }
    }
}
