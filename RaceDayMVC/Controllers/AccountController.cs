using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Services;
using RaceDayMVC.ViewModels;

namespace RaceDayMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApiService _apiService;

        public AccountController(ILogger<AccountController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Call API to authenticate user
                    var token = await _apiService.LoginAsync(model.Email, model.Password);

                    if (!string.IsNullOrEmpty(token))
                    {
                        // Store token and user info in session
                        HttpContext.Session.SetString("authToken", token);
                        HttpContext.Session.SetString("userEmail", model.Email);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during login");
                    ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
                }
            }

            return View(model);
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Call API to register user
                    var result = await _apiService.RegisterAsync(model);

                    if (result.Success)
                    {
                        // Redirect to login page on successful registration
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        // Add field-specific errors to ModelState if available
                        if (result.FieldErrors.Any())
                        {
                            foreach (var fieldError in result.FieldErrors)
                            {
                                ModelState.AddModelError(fieldError.Key, fieldError.Value);
                            }
                        }

                        // Add general errors to ModelState
                        if (result.GeneralErrors.Any())
                        {
                            foreach (var generalError in result.GeneralErrors)
                            {
                                ModelState.AddModelError(string.Empty, generalError);
                            }
                        }

                        // Add the main message if no other errors were added
                        if (!result.FieldErrors.Any() && !result.GeneralErrors.Any())
                        {
                            ModelState.AddModelError(string.Empty, result.Message ?? "Registration failed. Please try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during registration");
                    ModelState.AddModelError(string.Empty, $"An unexpected error occurred during registration: {ex.Message}");
                }
            }

            return View(model);
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
