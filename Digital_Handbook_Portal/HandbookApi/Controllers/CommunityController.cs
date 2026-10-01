using HandbookApi.DTOs;
using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunityController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public CommunityController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Community>>>> GetCommunityEvents()
        {
            var events = await _context.Community.OrderByDescending(e => e.eventDateTime).ToListAsync();
            return Ok(new ApiResponse<List<Community>> { Success = true, Data = events });
        }
    }
}