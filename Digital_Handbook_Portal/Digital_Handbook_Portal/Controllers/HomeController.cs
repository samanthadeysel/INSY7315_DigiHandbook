using System.Diagnostics;
using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Digital_Handbook_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HandbookApi");
        }

        public async Task<IActionResult> Index()
        {
            var policiesTask = GetEntityCountAsync<Policy>("api/Policies"); //plural because end points are plural
            var quizzesTask = GetEntityCountAsync<Quiz>("api/Quizzes");
            var doctorsTask = GetEntityCountAsync<Doctor>("api/Doctors");
            var eventsTask = GetEntityCountAsync<Community>("api/Community");

            await Task.WhenAll(policiesTask, quizzesTask, doctorsTask, eventsTask);

            ViewBag.TotalPolicies = await policiesTask;
            ViewBag.TotalQuizzes = await quizzesTask;
            ViewBag.TotalDoctors = await doctorsTask;
            ViewBag.TotalEvents = await eventsTask;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<int> GetEntityCountAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<T>>>(endpoint);
                return response?.Data?.Count ?? 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}