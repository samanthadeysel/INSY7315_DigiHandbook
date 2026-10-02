using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital_Handbook_Portal.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HandbookApi");
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login (Admin Portal Authentication)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                ViewBag.ErrorMessage = "Please enter both email and password.";
                return View(request);
            }

            var response = await _httpClient.PostAsJsonAsync("api/Auth/admin-login", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>();

                if (result != null && result.Success && result.Data != null)
                {
                    HttpContext.Session.SetString("AdminEmail", result.Data.Email);
                    HttpContext.Session.SetString("AdminName", result.Data.Name);
                    HttpContext.Session.SetString("AdminToken", result.Data.Token);

                    return RedirectToAction("Index", "Users");
                }
            }

            ViewBag.ErrorMessage = "Invalid admin credentials.";
            return View(request);
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}