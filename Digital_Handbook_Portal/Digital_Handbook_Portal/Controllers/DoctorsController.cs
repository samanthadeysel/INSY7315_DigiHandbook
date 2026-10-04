using System.Net.Http.Headers;
using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Digital_Handbook_Portal.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DoctorsController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("HandbookApi");
            _httpContextAccessor = httpContextAccessor;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("AdminToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            try
            {
                var httpResponse = await _httpClient.GetAsync("api/Doctors");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return View(new List<Doctor>());
                }

                var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<List<Doctor>>>();
                return View(response?.Data ?? new List<Doctor>());
            }
            catch
            {
                return View(new List<Doctor>());
            }
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

            // Match model properties
            content.Add(new StringContent(doctor.fName ?? string.Empty), nameof(doctor.fName));
            content.Add(new StringContent(doctor.lName ?? string.Empty), nameof(doctor.lName));
            content.Add(new StringContent(doctor.email ?? string.Empty), nameof(doctor.email));
            content.Add(new StringContent(doctor.phone ?? string.Empty), nameof(doctor.phone));
            content.Add(new StringContent(doctor.suiteNumber.ToString()), nameof(doctor.suiteNumber));

            if (!string.IsNullOrWhiteSpace(doctor.doctorImg))
            {
                content.Add(new StringContent(doctor.doctorImg), nameof(doctor.doctorImg));
            }

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

            var errorDetails = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Failed to create doctor record via API: {errorDetails}");

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
        public async Task<IActionResult> Edit(int id, Doctor doctor, IFormFile? uploadFile)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(doctor.fName ?? string.Empty), nameof(doctor.fName));
            content.Add(new StringContent(doctor.lName ?? string.Empty), nameof(doctor.lName));
            content.Add(new StringContent(doctor.email ?? string.Empty), nameof(doctor.email));
            content.Add(new StringContent(doctor.phone ?? string.Empty), nameof(doctor.phone));
            content.Add(new StringContent(doctor.suiteNumber.ToString()), nameof(doctor.suiteNumber));

            if (!string.IsNullOrWhiteSpace(doctor.doctorImg))
            {
                content.Add(new StringContent(doctor.doctorImg), nameof(doctor.doctorImg));
            }

            if (uploadFile != null && uploadFile.Length > 0)
            {
                var fileContent = new StreamContent(uploadFile.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(uploadFile.ContentType);
                content.Add(fileContent, "imageFile", uploadFile.FileName);
            }

            var response = await _httpClient.PutAsync($"api/Doctors/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorDetails = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Failed to update doctor entry via API: {errorDetails}");
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