using System.Net.Http.Headers;
using System.Text.Json;
using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Digital_Handbook_Portal.Controllers
{
    public class PoliciesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

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
            var httpResponse = await _httpClient.GetAsync("api/Policies");

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorContent = await httpResponse.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = $"API Request Failed ({(int)httpResponse.StatusCode}): {errorContent}";
                return View(new List<Policy>());
            }

            var content = await httpResponse.Content.ReadAsStringAsync();

            try
            {
                var response = JsonSerializer.Deserialize<ApiResponse<List<Policy>>>(content, JsonOptions);
                return View(response?.Data ?? new List<Policy>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Parsing Error: {ex.Message}";
                return View(new List<Policy>());
            }
        }

        // READ DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var httpResponse = await _httpClient.GetAsync($"api/Policies/{id}");
            if (!httpResponse.IsSuccessStatusCode) return NotFound();

            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ApiResponse<Policy>>(content, JsonOptions);

            if (response == null || !response.Success || response.Data == null) return NotFound();

            return View(response.Data);
        }

        // GET: Policies/Create
        public async Task<IActionResult> Create()
        {
            await PopulateCategoryDropdownAsync();
            return View();
        }

        // POST: Policies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Policy policy, IFormFile? pdfFile)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(policy.Title ?? string.Empty), nameof(policy.Title));
            content.Add(new StringContent(policy.contentSummary ?? string.Empty), nameof(policy.contentSummary));
            content.Add(new StringContent(policy.specificCategory ?? string.Empty), nameof(policy.specificCategory));
            content.Add(new StringContent(policy.categoryId.ToString()), nameof(policy.categoryId));

            if (!string.IsNullOrWhiteSpace(policy.fileUrl))
            {
                content.Add(new StringContent(policy.fileUrl), nameof(policy.fileUrl));
            }

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

            var errorDetails = await response.Content.ReadAsStringAsync();
            string errorMessage = errorDetails;

            if (!string.IsNullOrWhiteSpace(errorDetails))
            {
                try
                {
                    var errorObj = JsonSerializer.Deserialize<ApiResponse<Policy>>(errorDetails, JsonOptions);
                    if (!string.IsNullOrWhiteSpace(errorObj?.Message))
                    {
                        errorMessage = errorObj.Message;
                    }
                }
                catch
                {
                    // Fallback to raw string error
                }
            }

            ModelState.AddModelError(string.Empty, $"Failed to create policy: {errorMessage}");

            await PopulateCategoryDropdownAsync(policy.categoryId);
            return View(policy);
        }

        // UPDATE (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var httpResponse = await _httpClient.GetAsync($"api/Policies/{id}");
            if (!httpResponse.IsSuccessStatusCode) return NotFound();

            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ApiResponse<Policy>>(content, JsonOptions);

            if (response == null || !response.Success || response.Data == null) return NotFound();

            await PopulateCategoryDropdownAsync(response.Data.categoryId);
            return View(response.Data);
        }

        // POST: Policies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Policy policy, IFormFile? uploadFile)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(policy.policyId.ToString()), nameof(policy.policyId));
            content.Add(new StringContent(policy.Title ?? string.Empty), nameof(policy.Title));
            content.Add(new StringContent(policy.contentSummary ?? string.Empty), nameof(policy.contentSummary));
            content.Add(new StringContent(policy.specificCategory ?? string.Empty), nameof(policy.specificCategory));
            content.Add(new StringContent(policy.categoryId.ToString()), nameof(policy.categoryId));

            if (!string.IsNullOrWhiteSpace(policy.fileUrl))
            {
                content.Add(new StringContent(policy.fileUrl), nameof(policy.fileUrl));
            }

            if (uploadFile != null && uploadFile.Length > 0)
            {
                var fileContent = new StreamContent(uploadFile.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(uploadFile.ContentType);
                content.Add(fileContent, "pdfFile", uploadFile.FileName);
            }

            var response = await _httpClient.PutAsync($"api/Policies/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorDetails = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Failed to update policy document via API: {errorDetails}");
            await PopulateCategoryDropdownAsync(policy.categoryId);
            return View(policy);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var httpResponse = await _httpClient.GetAsync($"api/Policies/{id}");
            if (!httpResponse.IsSuccessStatusCode) return NotFound();

            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<ApiResponse<Policy>>(content, JsonOptions);

            if (response == null || !response.Success || response.Data == null) return NotFound();

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
            try
            {
                var httpResponse = await _httpClient.GetAsync("api/PolicyCategories");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<ApiResponse<List<PolicyCategory>>>(content, JsonOptions);
                    var categories = response?.Data ?? new List<PolicyCategory>();
                    ViewBag.Categories = new SelectList(categories, "categoryId", "categoryName", selectedCategoryId);
                    return;
                }
            }
            catch
            {
                // Fallback on error
            }

            ViewBag.Categories = new SelectList(Enumerable.Empty<PolicyCategory>(), "categoryId", "categoryName");
        }
    }
}