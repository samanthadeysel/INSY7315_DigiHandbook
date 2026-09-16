using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Handbook_Portal;
using Digital_Handbook_Portal.Models;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/bragbooks")]
    public class BragBooksApiController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public BragBooksApiController(Digital_Handbook_PortalContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.BragBook.OrderByDescending(b => b.datePosted).ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BragBook submission)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            submission.datePosted = DateTime.UtcNow;
            _context.BragBook.Add(submission);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Morale post logged successfully." });
        }
    }
}
