using System.Net.Http.Headers;
using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;

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
        }

        /// <summary>
        /// </summary>
        private void ApplyAuthHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null; // Clear previous state
            var token = _httpContextAccessor.HttpContext?.Session.GetString("AdminToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // GET: Resources
        public async Task<IActionResult> Index()
        {
            try
            {
                ApplyAuthHeader();
                var httpResponse = await _httpClient.GetAsync("api/Resources");

                if (!httpResponse.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = $"API Error: {httpResponse.StatusCode}";
                    return View(new List<Resource>());
                }

                var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<List<Resource>>>();
                return View(response?.Data ?? new List<Resource>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Unable to connect to backend service: {ex.Message}";
                return View(new List<Resource>());
            }
        }

        // GET: Resources/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                ApplyAuthHeader();
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<Resource>>($"api/Resources/{id}");
                if (response == null || !response.Success) return NotFound();

                return View(response.Data);
            }
            catch
            {
                return NotFound();
            }
        }

        // GET: Resources/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Resources/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Resource resource, IFormFile? uploadFile)
        {
            // Remove validation for ResourceUrl if a file is uploaded
            if (uploadFile != null && uploadFile.Length > 0)
            {
                ModelState.Remove(nameof(resource.ResourceUrl));
            }

            if (!ModelState.IsValid)
            {
                return View(resource);
            }

            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(resource.Title ?? string.Empty), nameof(resource.Title));
            content.Add(new StringContent(resource.Category ?? string.Empty), nameof(resource.Category));
            content.Add(new StringContent(resource.Description ?? string.Empty), nameof(resource.Description));
            content.Add(new StringContent(resource.BreadcrumbPath ?? string.Empty), nameof(resource.BreadcrumbPath));

            if (!string.IsNullOrWhiteSpace(resource.ResourceUrl))
            {
                content.Add(new StringContent(resource.ResourceUrl), nameof(resource.ResourceUrl));
            }

            if (uploadFile != null && uploadFile.Length > 0)
            {
                var fileContent = new StreamContent(uploadFile.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(uploadFile.ContentType);
                content.Add(fileContent, "uploadFile", uploadFile.FileName);
            }

            ApplyAuthHeader();
            var response = await _httpClient.PostAsync("api/Resources", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorDetails = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Failed to create resource entry via API: {errorDetails}");
            return View(resource);
        }

        // GET: Resources/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                ApplyAuthHeader();
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<Resource>>($"api/Resources/{id}");
                if (response == null || !response.Success) return NotFound();

                return View(response.Data);
            }
            catch
            {
                return NotFound();
            }
        }

        // POST: Resources/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Resource resource, IFormFile? uploadFile)
        {
            if (id != resource.Id) return NotFound();

            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(resource.Title ?? string.Empty), nameof(resource.Title));
            content.Add(new StringContent(resource.Category ?? string.Empty), nameof(resource.Category));
            content.Add(new StringContent(resource.Description ?? string.Empty), nameof(resource.Description));
            content.Add(new StringContent(resource.BreadcrumbPath ?? string.Empty), nameof(resource.BreadcrumbPath));

            if (!string.IsNullOrWhiteSpace(resource.ResourceUrl))
            {
                content.Add(new StringContent(resource.ResourceUrl), nameof(resource.ResourceUrl));
            }

            if (uploadFile != null && uploadFile.Length > 0)
            {
                var fileContent = new StreamContent(uploadFile.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(uploadFile.ContentType);
                content.Add(fileContent, "uploadFile", uploadFile.FileName);
            }

            ApplyAuthHeader();
            var response = await _httpClient.PutAsync($"api/Resources/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorDetails = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Failed to update resource entry via API: {errorDetails}");
            return View(resource);
        }

        // GET: Resources/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ApplyAuthHeader();
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<Resource>>($"api/Resources/{id}");
                if (response == null || !response.Success) return NotFound();

                return View(response.Data);
            }
            catch
            {
                return NotFound();
            }
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ApplyAuthHeader();
            await _httpClient.DeleteAsync($"api/Resources/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
