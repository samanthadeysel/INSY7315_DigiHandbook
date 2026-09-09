using Microsoft.AspNetCore.Mvc;
using Digital_Handbook_Portal;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PoliciesApiController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public PoliciesApiController(Digital_Handbook_PortalContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Policy.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var policy = await _context.Policy.FindAsync(id);
            return policy == null ? NotFound() : Ok(policy);
        }
    }
}
