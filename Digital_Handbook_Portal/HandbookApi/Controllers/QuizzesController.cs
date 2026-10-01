using HandbookApi.DTOs;
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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Quiz>>>> GetQuizzes()
        {
            var quizzes = await _context.Quiz
                .Include(q => q.questions)
                .ThenInclude(q => q.options)
                .ToListAsync();

            return Ok(new ApiResponse<List<Quiz>> { Success = true, Data = quizzes });
        }

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
    }
}