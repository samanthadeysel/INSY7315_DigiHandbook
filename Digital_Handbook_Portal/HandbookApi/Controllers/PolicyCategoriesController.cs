using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PolicyCategoriesController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public PolicyCategoriesController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        // GET: api/PolicyCategories
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<PolicyCategory>>>> GetCategories()
        {
            var categories = await _context.PolicyCategory.OrderBy(c => c.categoryName).ToListAsync();
            return Ok(new ApiResponse<List<PolicyCategory>> { Success = true, Data = categories });
        }

        // GET: api/PolicyCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PolicyCategory>>> GetCategory(int id)
        {
            var category = await _context.PolicyCategory.FindAsync(id);
            if (category == null)
            {
                return NotFound(new ApiResponse<PolicyCategory> { Success = false, Message = "Policy category not found." });
            }

            return Ok(new ApiResponse<PolicyCategory> { Success = true, Data = category });
        }

        // POST: api/PolicyCategories
        [HttpPost]
        public async Task<ActionResult<ApiResponse<PolicyCategory>>> CreateCategory([FromBody] PolicyCategory category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<PolicyCategory> { Success = false, Message = "Invalid payload." });
            }

            _context.PolicyCategory.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<PolicyCategory> { Success = true, Message = "Policy category created successfully.", Data = category });
        }

        // PUT: api/PolicyCategories/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PolicyCategory>>> UpdateCategory(int id, [FromBody] PolicyCategory category)
        {
            if (id != category.categoryId)
            {
                return BadRequest(new ApiResponse<PolicyCategory> { Success = false, Message = "Category ID mismatch." });
            }

            var existingCategory = await _context.PolicyCategory.FindAsync(id);
            if (existingCategory == null)
            {
                return NotFound(new ApiResponse<PolicyCategory> { Success = false, Message = "Policy category not found." });
            }

            existingCategory.categoryName = category.categoryName;
            existingCategory.subCategory = category.subCategory;

            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<PolicyCategory> { Success = true, Message = "Policy category updated successfully.", Data = existingCategory });
        }

        // DELETE: api/PolicyCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCategory(int id)
        {
            var category = await _context.PolicyCategory.FindAsync(id);
            if (category == null)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Message = "Policy category not found.", Data = false });
            }

            _context.PolicyCategory.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<bool> { Success = true, Message = "Policy category deleted successfully.", Data = true });
        }
    }
}