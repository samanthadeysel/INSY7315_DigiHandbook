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

        // Hardcoded Policy Categories mapped to IDs
        private static readonly List<SelectListItem> HardcodedCategories = new()
        {
            new SelectListItem { Value = "1", Text = "General & Administrative" },
            new SelectListItem { Value = "2", Text = "Clinical & Patient Care" },
            new SelectListItem { Value = "3", Text = "Human Resources" },
            new SelectListItem { Value = "4", Text = "Health & Safety" },
            new SelectListItem { Value = "5", Text = "IT & Data Security" },
            new SelectListItem { Value = "6", Text = "Compliance & Ethics" }
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
                ViewBag.ErrorMessage = $"API Request Failed: {errorContent}";
                return View(new List<Policy>());
            }

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<List<Policy>>>();
            return View(response?.Data ?? new List<Policy>());
        }

        // READ DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Policy>>($"api/Policies/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // GET: Policies/Create
        public IActionResult Create()
        {
            ViewBag.Categories = HardcodedCategories;
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

            int selectedCategoryId = policy.categoryId > 0 ? policy.categoryId : 1;
            content.Add(new StringContent(selectedCategoryId.ToString()), nameof(policy.categoryId));

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
                    var errorObj = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<Policy>>(
                        errorDetails,
                        new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (!string.IsNullOrWhiteSpace(errorObj?.Message))
                    {
                        errorMessage = errorObj.Message;
                    }
                }
                catch
                {

                }
            }

            ModelState.AddModelError(string.Empty, $"Failed to create policy: {errorMessage}");

            ViewBag.Categories = HardcodedCategories;
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
        public async Task<IActionResult> Edit(int id, Policy policy, IFormFile? uploadFile)
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

            if (uploadFile != null && uploadFile.Length > 0)
            {
                var fileContent = new StreamContent(uploadFile.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(uploadFile.ContentType);
                content.Add(fileContent, "uploadFile", uploadFile.FileName);
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