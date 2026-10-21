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
                // TODO: Implement API call to fetch events
                // var events = await _apiService.GetEventsAsync();
                var events = new List<EventViewModel>();
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
                // TODO: Implement API call to fetch event details
                // var eventViewModel = await _apiService.GetEventAsync(id.Value);
                var eventViewModel = new EventViewModel();

                if (eventViewModel == null)
                {
                    return NotFound();
                }

                return View(eventViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching event details");
                return NotFound();
            }
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            // Check if user is authenticated and has organiser role
            var userEmail = HttpContext.Session.GetString("userEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Implement API call to create event
                    // var result = await _apiService.CreateEventAsync(model);
                    // if (result)
                    // {
                    //     return RedirectToAction(nameof(Index));
                    // }

                    ModelState.AddModelError(string.Empty, "Failed to create event. Please try again.");
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
