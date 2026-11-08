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
                    var (token, role) = await _apiService.LoginAsync(model.Email, model.Password);

                    if (!string.IsNullOrEmpty(token))
                    {
                        // Store token, email, and role in session
                        HttpContext.Session.SetString("authToken", token);
                        HttpContext.Session.SetString("userEmail", model.Email);
                        HttpContext.Session.SetString("userRole", role ?? "Participant");

                        Console.WriteLine($"[Login Success] User logged in: {model.Email}, Role: {role}");
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
                    // Set default values for optional fields
                    if (string.IsNullOrEmpty(model.MedicalConditions)) 
                        model.MedicalConditions = "None";
                    if (string.IsNullOrEmpty(model.ClubAffiliation)) 
                        model.ClubAffiliation = "None";

                    // Call API to register user
                    var result = await _apiService.RegisterAsync(model);

                    if (result == "Success")
                    {
                        // Redirect to login page on successful registration
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        // Show the exact API error on the screen
                        ModelState.AddModelError("", $"Registration failed: {result}");
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
