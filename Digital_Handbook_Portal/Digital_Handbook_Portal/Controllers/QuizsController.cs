using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Digital_Handbook_Portal.Controllers
{
    public class QuizsController : Controller
    {
        private readonly Digital_Handbook_PortalContext _context;

        public QuizsController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        // READ ALL: Includes questions and options for full object graph visibility
        public async Task<IActionResult> Index()
        {
            var quizzes = await _context.Quiz
                .Include(q => q.questions)
                .ThenInclude(q => q.options)
                .ToListAsync();

            return View(quizzes);
        }

        // READ DETAILS: Fetches full hierarchy (Quiz -> Questions -> Options)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var quiz = await _context.Quiz
                .Include(q => q.questions)
                .ThenInclude(q => q.options)
                .FirstOrDefaultAsync(m => m.quizId == id);

            if (quiz == null) return NotFound();

            return View(quiz);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            var quiz = new Quiz
            {
                // Pre-populate with 1 default question and options for initial rendering
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

        // CREATE (POST): Takes nested questions and options from the form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Quiz quiz)
        {
            // Remove empty/invalid questions or options if any were posted unpopulated
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

            if (ModelState.IsValid)
            {
                quiz.createdAt = DateTime.Now;
                _context.Add(quiz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(quiz);
        }

        // UPDATE (GET): Loads full nested hierarchy into the form
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var quiz = await _context.Quiz
                .Include(q => q.questions)
                .ThenInclude(q => q.options)
                .FirstOrDefaultAsync(m => m.quizId == id);

            if (quiz == null) return NotFound();

            return View(quiz);
        }

        // UPDATE (POST): Updates Quiz metadata as well as nested Questions & Options
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Quiz quiz)
        {
            if (id != quiz.quizId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch existing quiz from DB including questions and options
                    var existingQuiz = await _context.Quiz
                        .Include(q => q.questions)
                        .ThenInclude(q => q.options)
                        .FirstOrDefaultAsync(q => q.quizId == id);

                    if (existingQuiz == null) return NotFound();

                    // Update Quiz Scalar properties
                    existingQuiz.title = quiz.title;
                    existingQuiz.score = quiz.score;
                    existingQuiz.passingScore = quiz.passingScore;
                    existingQuiz.estimateTime = quiz.estimateTime;

                    // Remove existing nested questions and replace with updated set
                    _context.Set<QuizQuestion>().RemoveRange(existingQuiz.questions);

                    if (quiz.questions != null)
                    {
                        existingQuiz.questions = quiz.questions
                            .Where(q => !string.IsNullOrWhiteSpace(q.questionText))
                            .Select(q => new QuizQuestion
                            {
                                questionText = q.questionText,
                                options = q.options != null
                                    ? q.options.Where(o => !string.IsNullOrWhiteSpace(o.optionText)).ToList()
                                    : new List<QuizOption>()
                            }).ToList();

                        existingQuiz.totalQuestions = existingQuiz.questions.Count;
                    }

                    _context.Update(existingQuiz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Quiz.Any(e => e.quizId == quiz.quizId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(quiz);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var quiz = await _context.Quiz
                .Include(q => q.questions)
                .FirstOrDefaultAsync(m => m.quizId == id);

            if (quiz == null) return NotFound();

            return View(quiz);
        }

        // DELETE (POST): Deleting a Quiz automatically cascades and removes its Questions and Options
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quiz = await _context.Quiz
                .Include(q => q.questions)
                .ThenInclude(q => q.options)
                .FirstOrDefaultAsync(q => q.quizId == id);

            if (quiz != null)
            {
                _context.Quiz.Remove(quiz);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}