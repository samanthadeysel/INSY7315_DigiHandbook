using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Handbook_Portal;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/community")]
    public class CommunityApiController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public CommunityApiController(Digital_Handbook_PortalContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetActiveEvents() =>
            Ok(await _context.Community.OrderBy(e => e.eventDateTime).ToListAsync());
    }
}
