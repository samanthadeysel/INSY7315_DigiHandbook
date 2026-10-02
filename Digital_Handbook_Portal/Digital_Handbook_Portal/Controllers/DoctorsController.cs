using System.Net.Http.Headers;
using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Digital_Handbook_Portal.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly HttpClient _httpClient;

        public DoctorsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HandbookApi");
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<Doctor>>>("api/Doctors");
            return View(response?.Data ?? new List<Doctor>());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Doctor>>($"api/Doctors/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor, IFormFile? imageFile)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(doctor.fName ?? string.Empty), nameof(doctor.fName));
            content.Add(new StringContent(doctor.lName ?? string.Empty), nameof(doctor.lName));
            content.Add(new StringContent(doctor.email ?? string.Empty), nameof(doctor.email));
            content.Add(new StringContent(doctor.phone ?? string.Empty), nameof(doctor.phone));
            content.Add(new StringContent(doctor.suiteNumber.ToString()), nameof(doctor.suiteNumber));

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileContent = new StreamContent(imageFile.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                content.Add(fileContent, "imageFile", imageFile.FileName);
            }

            var response = await _httpClient.PostAsync("api/Doctors", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Failed to create doctor entry via API.");
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Doctor>>($"api/Doctors/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doctor doctor, IFormFile? imageFile)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(doctor.fName ?? string.Empty), nameof(doctor.fName));
            content.Add(new StringContent(doctor.lName ?? string.Empty), nameof(doctor.lName));
            content.Add(new StringContent(doctor.email ?? string.Empty), nameof(doctor.email));
            content.Add(new StringContent(doctor.phone ?? string.Empty), nameof(doctor.phone));
            content.Add(new StringContent(doctor.suiteNumber.ToString()), nameof(doctor.suiteNumber));

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileContent = new StreamContent(imageFile.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                content.Add(fileContent, "imageFile", imageFile.FileName);
            }

            var response = await _httpClient.PutAsync($"api/Doctors/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Failed to update doctor entry via API.");
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Doctor>>($"api/Doctors/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _httpClient.DeleteAsync($"api/Doctors/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}