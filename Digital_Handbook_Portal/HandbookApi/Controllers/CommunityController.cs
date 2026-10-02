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

        // GET: api/Community
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Community>>>> GetCommunityEvents()
        {
            var events = await _context.Community.OrderByDescending(e => e.eventDateTime).ToListAsync();
            return Ok(new ApiResponse<List<Community>> { Success = true, Data = events });
        }

        // GET: api/Community/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Community>>> GetCommunityEvent(int id)
        {
            var communityEvent = await _context.Community.FindAsync(id);
            if (communityEvent == null)
            {
                return NotFound(new ApiResponse<Community> { Success = false, Message = "Community event not found." });
            }

            return Ok(new ApiResponse<Community> { Success = true, Data = communityEvent });
        }

        // POST: api/Community
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Community>>> CreateCommunityEvent([FromBody] Community community)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<Community> { Success = false, Message = "Invalid event payload." });
            }

            // Ensure DateTime is stored as UTC for PostgreSQL/Npgsql compatibility
            community.eventDateTime = DateTime.SpecifyKind(community.eventDateTime, DateTimeKind.Utc);

            _context.Community.Add(community);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Community> { Success = true, Message = "Community event created successfully.", Data = community });
        }

        // PUT: api/Community/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Community>>> UpdateCommunityEvent(int id, [FromBody] Community community)
        {
            if (id != community.eventId)
            {
                return BadRequest(new ApiResponse<Community> { Success = false, Message = "Event ID mismatch." });
            }

            var existingEvent = await _context.Community.FindAsync(id);
            if (existingEvent == null)
            {
                return NotFound(new ApiResponse<Community> { Success = false, Message = "Community event not found." });
            }

            existingEvent.title = community.title;
            existingEvent.description = community.description;
            existingEvent.eventCategory = community.eventCategory;
            existingEvent.eventDateTime = DateTime.SpecifyKind(community.eventDateTime, DateTimeKind.Utc);
            existingEvent.location = community.location;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Community> { Success = true, Message = "Community event updated successfully.", Data = existingEvent });
        }

        // DELETE: api/Community/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCommunityEvent(int id)
        {
            var communityEvent = await _context.Community.FindAsync(id);
            if (communityEvent == null)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Message = "Community event not found.", Data = false });
            }

            _context.Community.Remove(communityEvent);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<bool> { Success = true, Message = "Community event deleted successfully.", Data = true });
        }
    }
}