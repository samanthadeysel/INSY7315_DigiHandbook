using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Digital_Handbook_Portal.Controllers
{
    public class PolicyCategoriesController : Controller
    {
        private readonly HttpClient _httpClient;

        public PolicyCategoriesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HandbookApi");
        }

        // READ ALL
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<PolicyCategory>>>("api/PolicyCategories");
            return View(response?.Data ?? new List<PolicyCategory>());
        }

        // READ DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PolicyCategory>>($"api/PolicyCategories/{id}");
            if (response == null || !response.Success) return NotFound();

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
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PolicyCategory>>($"api/PolicyCategories/{id}");
            if (response == null || !response.Success) return NotFound();

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
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PolicyCategory>>($"api/PolicyCategories/{id}");
            if (response == null || !response.Success) return NotFound();

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