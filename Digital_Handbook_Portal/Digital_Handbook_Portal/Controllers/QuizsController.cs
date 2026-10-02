using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Digital_Handbook_Portal.Controllers
{
    public class QuizsController : Controller
    {
        private readonly HttpClient _httpClient;

        public QuizsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HandbookApi");
        }

        // GET: Quizzes
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<Quiz>>>("api/Quizzes");
            return View(response?.Data ?? new List<Quiz>());
        }

        // GET: Quizzes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Quiz>>($"api/Quizzes/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // GET: Quizzes/Create
        public IActionResult Create()
        {
            var quiz = new Quiz
            {
                questions = new List<QuizQuestion>
                {
                    new QuizQuestion
                    {
                        options = new List<QuizOption>
                        {
                            new QuizOption(),
                            new QuizOption()
                        }
                    }
                }
            };

            return View(quiz);
        }

        // POST: Quizzes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Quiz quiz)
        {
            CleanAndPrepareQuiz(quiz);

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("api/Quizzes", quiz);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Failed to create quiz via API.");
            }

            return View(quiz);
        }

        // GET: Quizzes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Quiz>>($"api/Quizzes/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // POST: Quizzes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Quiz quiz)
        {
            if (id != quiz.quizId) return NotFound();

            CleanAndPrepareQuiz(quiz);

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Quizzes/{id}", quiz);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Failed to update quiz via API.");
            }

            return View(quiz);
        }

        // GET: Quizzes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<Quiz>>($"api/Quizzes/{id}");
            if (response == null || !response.Success) return NotFound();

            return View(response.Data);
        }

        // POST: Quizzes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _httpClient.DeleteAsync($"api/Quizzes/{id}");
            return RedirectToAction(nameof(Index));
        }

        private void CleanAndPrepareQuiz(Quiz quiz)
        {
            if (quiz.questions != null)
            {
                quiz.questions = quiz.questions
                    .Where(q => !string.IsNullOrWhiteSpace(q.questionText))
                    .ToList();

                foreach (var question in quiz.questions)
                {
                    if (question.options != null)
                    {
                        question.options = question.options
                            .Where(o => !string.IsNullOrWhiteSpace(o.optionText))
                            .ToList();
                    }
                }

                quiz.totalQuestions = quiz.questions.Count;
            }
        }
    }
}