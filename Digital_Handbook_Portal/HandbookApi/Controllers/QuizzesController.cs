using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizzesController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public QuizzesController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        // GET: api/Quizzes
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Quiz>>>> GetQuizzes()
        {
            var quizzes = await _context.Quiz
                .Include(q => q.questions)
                .ThenInclude(q => q.options)
                .OrderByDescending(q => q.createdAt)
                .ToListAsync();

            return Ok(new ApiResponse<List<Quiz>> { Success = true, Data = quizzes });
        }

        // GET: api/Quizzes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Quiz>>> GetQuiz(int id)
        {
            var quiz = await _context.Quiz
                .Include(q => q.questions)
                .ThenInclude(q => q.options)
                .FirstOrDefaultAsync(q => q.quizId == id);

            if (quiz == null)
            {
                return NotFound(new ApiResponse<Quiz> { Success = false, Message = "Quiz not found." });
            }

            return Ok(new ApiResponse<Quiz> { Success = true, Data = quiz });
        }

        // POST: api/Quizzes
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Quiz>>> CreateQuiz([FromBody] Quiz quiz)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<Quiz> { Success = false, Message = "Invalid quiz data." });
            }

            quiz.createdAt = DateTime.UtcNow;
            if (quiz.questions != null)
            {
                quiz.totalQuestions = quiz.questions.Count;
            }

            _context.Quiz.Add(quiz);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Quiz> { Success = true, Message = "Quiz created successfully.", Data = quiz });
        }

        // PUT: api/Quizzes/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Quiz>>> UpdateQuiz(int id, [FromBody] Quiz quiz)
        {
            if (id != quiz.quizId)
            {
                return BadRequest(new ApiResponse<Quiz> { Success = false, Message = "Quiz ID mismatch." });
            }

            var existingQuiz = await _context.Quiz
                .Include(q => q.questions)
                .ThenInclude(q => q.options)
                .FirstOrDefaultAsync(q => q.quizId == id);

            if (existingQuiz == null)
            {
                return NotFound(new ApiResponse<Quiz> { Success = false, Message = "Quiz not found." });
            }

            // Update parent properties
            existingQuiz.title = quiz.title;
            existingQuiz.score = quiz.score;
            existingQuiz.passingScore = quiz.passingScore;
            existingQuiz.estimateTime = quiz.estimateTime;

            // Remove previous questions/options to sync modified graph
            _context.Set<QuizQuestion>().RemoveRange(existingQuiz.questions);

            if (quiz.questions != null)
            {
                existingQuiz.questions = quiz.questions.Select(q => new QuizQuestion
                {
                    questionText = q.questionText,
                    options = q.options != null
                        ? q.options.Select(o => new QuizOption
                        {
                            optionText = o.optionText,
                            isCorrect = o.isCorrect
                        }).ToList()
                        : new List<QuizOption>()
                }).ToList();

                existingQuiz.totalQuestions = existingQuiz.questions.Count;
            }

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Quiz> { Success = true, Message = "Quiz updated successfully.", Data = existingQuiz });
        }

        // DELETE: api/Quizzes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteQuiz(int id)
        {
            var quiz = await _context.Quiz
                .Include(q => q.questions)
                .ThenInclude(q => q.options)
                .FirstOrDefaultAsync(q => q.quizId == id);

            if (quiz == null)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Message = "Quiz not found.", Data = false });
            }

            _context.Quiz.Remove(quiz);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<bool> { Success = true, Message = "Quiz deleted successfully.", Data = true });
        }
    }
}