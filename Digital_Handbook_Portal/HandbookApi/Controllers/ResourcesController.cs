using HandbookApi.DTOs;
using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public ResourcesController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Resource>>>> GetResources()
        {
            var resources = await _context.Resource.ToListAsync();
            return Ok(new ApiResponse<List<Resource>> { Success = true, Data = resources });
        }
    }
}