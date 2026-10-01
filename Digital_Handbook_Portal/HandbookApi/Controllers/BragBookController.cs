using HandbookApi.DTOs;
using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BragBookController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public BragBookController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<BragBook>>>> GetBragPosts()
        {
            var posts = await _context.BragBook.OrderByDescending(b => b.datePosted).ToListAsync();
            return Ok(new ApiResponse<List<BragBook>> { Success = true, Data = posts });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<BragBook>>> CreateBragPost([FromBody] BragBook post)
        {
            if (post == null)
            {
                return BadRequest(new ApiResponse<BragBook> { Success = false, Message = "Invalid post data." });
            }

            post.datePosted = DateTime.Now;
            _context.BragBook.Add(post);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<BragBook> { Success = true, Message = "Brag post published successfully.", Data = post });
        }
    }
}