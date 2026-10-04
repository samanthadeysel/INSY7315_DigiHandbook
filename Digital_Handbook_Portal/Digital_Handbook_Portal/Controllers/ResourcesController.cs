using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Digital_Handbook_Portal.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResourcesController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("HandbookApi");
            _httpContextAccessor = httpContextAccessor;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("AdminToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // GET: Resources
        public async Task<IActionResult> Index(string? query)
        {
            string requestUri = string.IsNullOrWhiteSpace(query)
                ? "api/Resources"
                : $"api/Resources?query={Uri.EscapeDataString(query)}";

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<Resource>>>(requestUri);
            ViewData["CurrentFilter"] = query;
            return View(response?.Data ?? new List<Resource>());
        }

        // GET: Resources/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Resource>>($"api/Resources/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // GET: Resources/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Resources/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Resource resource)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("api/Resources", resource);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Failed to create resource entry via API.");
            }
            return View(resource);
        }

        // GET: Resources/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Resource>>($"api/Resources/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // POST: Resources/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Resource resource)
        {
            if (id != resource.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Resources/{id}", resource);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Failed to update resource entry via API.");
            }
            return View(resource);
        }

        // GET: Resources/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Resource>>($"api/Resources/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _httpClient.DeleteAsync($"api/Resources/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}