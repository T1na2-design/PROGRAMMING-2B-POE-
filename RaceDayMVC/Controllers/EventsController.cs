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
            var userEmail = HttpContext.Session.GetString("userEmail");
            if (string.IsNullOrEmpty(userRole) || !userRole.Equals("Organiser", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get auth token from session
                    var authToken = HttpContext.Session.GetString("authToken");

                    // Create the event and get the event ID
                    var eventId = await _apiService.CreateEventAsync(model, authToken);

                    if (eventId > 0)
                    {
                        // If banner image is provided, upload it
                        if (model.BannerImage != null && model.BannerImage.Length > 0)
                        {
                            try
                            {
                                var uploadResult = await _apiService.UploadBannerAsync(eventId, model.BannerImage, authToken);

                                if (uploadResult)
                                {
                                    _logger.LogInformation($"Organiser {userEmail} created event {eventId} with banner image");
                                }
                                else
                                {
                                    _logger.LogWarning($"Event {eventId} created but banner upload failed");
                                }
                            }
                            catch (Exception bannerEx)
                            {
                                _logger.LogWarning(bannerEx, $"Error uploading banner for event {eventId}");
                                // Don't fail the event creation if banner upload fails
                                // The event is already created successfully
                            }
                        }
                        else
                        {
                            _logger.LogInformation($"Organiser {userEmail} created event {eventId} without banner image");
                        }

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
