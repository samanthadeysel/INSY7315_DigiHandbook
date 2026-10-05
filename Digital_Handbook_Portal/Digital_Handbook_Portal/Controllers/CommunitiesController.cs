using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Digital_Handbook_Portal.Controllers
{
    public class CommunitiesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommunitiesController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("HandbookApi");
            _httpContextAccessor = httpContextAccessor;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("AdminToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // GET: Communities
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<Community>>>("api/Community");
            return View(response?.Data ?? new List<Community>());
        }

        // GET: Communities/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Community>>($"api/Community/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // GET: Communities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Communities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Community community)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("api/Community", community);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Failed to create community event via API.");
            }
            return View(community);
        }

        // GET: Communities/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Community>>($"api/Community/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // POST: Communities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Community community)
        {
            if (id != community.eventId) return NotFound();

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Community/{id}", community);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Failed to update community event via API.");
            }
            return View(community);
        }

        // GET: Communities/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Community>>($"api/Community/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // POST: Communities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _httpClient.DeleteAsync($"api/Community/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}