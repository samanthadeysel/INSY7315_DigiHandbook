using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Handbook_Portal;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/resources")]
    public class ResourcesApiController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public ResourcesApiController(Digital_Handbook_PortalContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.Resource.ToListAsync());
    }
}