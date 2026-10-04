using System.Net.Http.Headers;
using Digital_Handbook_Portal.Models;
using Digital_Handbook_Portal;
using Microsoft.AspNetCore.Mvc;

namespace Digital_Handbook_Portal.Controllers
{
    public class BragBooksController : Controller
    {
        private readonly HttpClient _httpClient;
         private readonly IHttpContextAccessor _httpContextAccessor;
        public BragBooksController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("HandbookApi");
            _httpContextAccessor = httpContextAccessor;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("AdminToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // GET: BragBooks
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<BragBook>>>("api/BragBook");
            return View(response?.Data ?? new List<BragBook>());
        }

        // GET: BragBooks/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<BragBook>>($"api/BragBook/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // GET: BragBooks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BragBooks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BragBook bragBook, IFormFile? imageFile)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(bragBook.content ?? string.Empty), nameof(bragBook.content));
            content.Add(new StringContent(bragBook.senderType ?? string.Empty), nameof(bragBook.senderType));
            content.Add(new StringContent(bragBook.recipientName ?? string.Empty), nameof(bragBook.recipientName));

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileContent = new StreamContent(imageFile.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                content.Add(fileContent, "imageFile", imageFile.FileName);
            }

            var response = await _httpClient.PostAsync("api/BragBook", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Failed to create brag post via API.");
            return View(bragBook);
        }

        // GET: BragBooks/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<BragBook>>($"api/BragBook/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // POST: BragBooks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BragBook bragBook, IFormFile? imageFile)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(bragBook.content ?? string.Empty), nameof(bragBook.content));
            content.Add(new StringContent(bragBook.senderType ?? string.Empty), nameof(bragBook.senderType));
            content.Add(new StringContent(bragBook.recipientName ?? string.Empty), nameof(bragBook.recipientName));

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileContent = new StreamContent(imageFile.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                content.Add(fileContent, "imageFile", imageFile.FileName);
            }

            var response = await _httpClient.PutAsync($"api/BragBook/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Failed to update brag post via API.");
            return View(bragBook);
        }

        // GET: BragBooks/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<BragBook>>($"api/BragBook/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // POST: BragBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _httpClient.DeleteAsync($"api/BragBook/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}