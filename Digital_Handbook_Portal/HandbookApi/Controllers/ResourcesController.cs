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

        // GET: api/Resources OR api/Resources?query=mental
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Resource>>>> GetResources([FromQuery] string? query)
        {
            var queryable = _context.Resource.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var lowercaseQuery = query.ToLower();
                queryable = queryable.Where(r => EF.Functions.Like(r.Title.ToLower(), $"%{lowercaseQuery}%") ||
                                                 EF.Functions.Like(r.Category.ToLower(), $"%{lowercaseQuery}%") ||
                                                 EF.Functions.Like(r.Description.ToLower(), $"%{lowercaseQuery}%"));
            }

            var resources = await queryable.OrderBy(r => r.Title).ToListAsync();
            return Ok(new ApiResponse<List<Resource>> { Success = true, Data = resources });
        }

        // GET: api/Resources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Resource>>> GetResource(int id)
        {
            var resource = await _context.Resource.FindAsync(id);
            if (resource == null)
            {
                return NotFound(new ApiResponse<Resource> { Success = false, Message = $"Resource with ID {id} was not found." });
            }

            return Ok(new ApiResponse<Resource> { Success = true, Data = resource });
        }

        // POST: api/Resources
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Resource>>> CreateResource([FromBody] Resource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<Resource> { Success = false, Message = "Invalid resource payload." });
            }

            _context.Resource.Add(resource);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Resource> { Success = true, Message = "Resource created successfully.", Data = resource });
        }

        // PUT: api/Resources/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Resource>>> UpdateResource(int id, [FromBody] Resource resource)
        {
            if (id != resource.Id)
            {
                return BadRequest(new ApiResponse<Resource> { Success = false, Message = "Resource ID mismatch." });
            }

            var existingResource = await _context.Resource.FindAsync(id);
            if (existingResource == null)
            {
                return NotFound(new ApiResponse<Resource> { Success = false, Message = "Resource not found." });
            }

            existingResource.Title = resource.Title;
            existingResource.Category = resource.Category;
            existingResource.Description = resource.Description;
            existingResource.ResourceUrl = resource.ResourceUrl;
            existingResource.BreadcrumbPath = resource.BreadcrumbPath;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Resource> { Success = true, Message = "Resource updated successfully.", Data = existingResource });
        }

        // DELETE: api/Resources/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteResource(int id)
        {
            var resource = await _context.Resource.FindAsync(id);
            if (resource == null)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Message = "Resource not found.", Data = false });
            }

            _context.Resource.Remove(resource);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<bool> { Success = true, Message = "Resource deleted successfully.", Data = true });
        }
    }
}