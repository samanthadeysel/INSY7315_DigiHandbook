using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Digital_Handbook_Portal.Controllers
{
    public class PolicyCategoriesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public PolicyCategoriesController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("HandbookApi");
            _httpContextAccessor = httpContextAccessor;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("AdminToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // READ ALL
        public async Task<IActionResult> Index()
        {
            var httpResponse = await _httpClient.GetAsync("api/PolicyCategories");
            if (!httpResponse.IsSuccessStatusCode)
            {
                return View(new List<PolicyCategory>());
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ApiResponse<List<PolicyCategory>>>(content, JsonOptions);
            return View(response?.Data ?? new List<PolicyCategory>());
        }

        // READ DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var httpResponse = await _httpClient.GetAsync($"api/PolicyCategories/{id}");
            if (!httpResponse.IsSuccessStatusCode) return NotFound();

            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ApiResponse<PolicyCategory>>(content, JsonOptions);

            if (response == null || !response.Success || response.Data == null) return NotFound();

            return View(response.Data);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PolicyCategory policyCategory)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("api/PolicyCategories", policyCategory);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Failed to create policy category via API.");
            }
            return View(policyCategory);
        }

        // UPDATE (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var httpResponse = await _httpClient.GetAsync($"api/PolicyCategories/{id}");
            if (!httpResponse.IsSuccessStatusCode) return NotFound();

            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ApiResponse<PolicyCategory>>(content, JsonOptions);

            if (response == null || !response.Success || response.Data == null) return NotFound();

            return View(response.Data);
        }

        // UPDATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PolicyCategory policyCategory)
        {
            if (id != policyCategory.categoryId) return NotFound();

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"api/PolicyCategories/{id}", policyCategory);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Failed to update policy category via API.");
            }
            return View(policyCategory);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var httpResponse = await _httpClient.GetAsync($"api/PolicyCategories/{id}");
            if (!httpResponse.IsSuccessStatusCode) return NotFound();

            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ApiResponse<PolicyCategory>>(content, JsonOptions);

            if (response == null || !response.Success || response.Data == null) return NotFound();

            return View(response.Data);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _httpClient.DeleteAsync($"api/PolicyCategories/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}