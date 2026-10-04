using System.Net.Http.Headers;
using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Digital_Handbook_Portal.Controllers
{
    public class PoliciesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PoliciesController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
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
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<Policy>>>("api/Policies");
            return View(response?.Data ?? new List<Policy>());
        }

        // READ DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Policy>>($"api/Policies/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // CREATE (GET)
        public async Task<IActionResult> Create()
        {
            await PopulateCategoryDropdownAsync();
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Policy policy, IFormFile? pdfFile)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(policy.Title ?? string.Empty), nameof(policy.Title));
            content.Add(new StringContent(policy.contentSummary ?? string.Empty), nameof(policy.contentSummary));
            content.Add(new StringContent(policy.specificCategory ?? string.Empty), nameof(policy.specificCategory));
            content.Add(new StringContent(policy.categoryId.ToString()), nameof(policy.categoryId));

            if (pdfFile != null && pdfFile.Length > 0)
            {
                var fileContent = new StreamContent(pdfFile.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(pdfFile.ContentType);
                content.Add(fileContent, "pdfFile", pdfFile.FileName);
            }

            var response = await _httpClient.PostAsync("api/Policies", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Failed to create policy document via API.");
            await PopulateCategoryDropdownAsync(policy.categoryId);
            return View(policy);
        }

        // UPDATE (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Policy>>($"api/Policies/{id}");
            if (response == null || !response.Success) return NotFound();

            await PopulateCategoryDropdownAsync(response.Data?.categoryId);
            return View(response.Data);
        }

        // UPDATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Policy policy, IFormFile? pdfFile)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(policy.Title ?? string.Empty), nameof(policy.Title));
            content.Add(new StringContent(policy.contentSummary ?? string.Empty), nameof(policy.contentSummary));
            content.Add(new StringContent(policy.specificCategory ?? string.Empty), nameof(policy.specificCategory));
            content.Add(new StringContent(policy.categoryId.ToString()), nameof(policy.categoryId));

            if (pdfFile != null && pdfFile.Length > 0)
            {
                var fileContent = new StreamContent(pdfFile.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(pdfFile.ContentType);
                content.Add(fileContent, "pdfFile", pdfFile.FileName);
            }

            var response = await _httpClient.PutAsync($"api/Policies/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Failed to update policy document via API.");
            await PopulateCategoryDropdownAsync(policy.categoryId);
            return View(policy);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Policy>>($"api/Policies/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _httpClient.DeleteAsync($"api/Policies/{id}");
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateCategoryDropdownAsync(int? selectedCategoryId = null)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<PolicyCategory>>>("api/PolicyCategories");
            var categories = response?.Data ?? new List<PolicyCategory>();
            ViewBag.Categories = new SelectList(categories, "categoryId", "categoryName", selectedCategoryId);
        }
    }
}