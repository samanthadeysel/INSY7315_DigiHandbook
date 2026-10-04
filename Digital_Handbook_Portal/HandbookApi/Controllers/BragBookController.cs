using Digital_Handbook_Portal.Models;
using HandbookApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BragBookController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;
        private readonly ICloudStorageService _storageService;

        public BragBookController(Digital_Handbook_PortalContext context, ICloudStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        // GET: api/BragBook
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<BragBook>>>> GetBragPosts()
        {
            var posts = await _context.BragBook.OrderByDescending(b => b.datePosted).ToListAsync();
            return Ok(new ApiResponse<List<BragBook>> { Success = true, Data = posts });
        }

        // GET: api/BragBook/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BragBook>>> GetBragPost(int id)
        {
            var post = await _context.BragBook.FindAsync(id);
            if (post == null)
            {
                return NotFound(new ApiResponse<BragBook> { Success = false, Message = "Brag post not found." });
            }

            return Ok(new ApiResponse<BragBook> { Success = true, Data = post });
        }

        // POST: api/BragBook
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<BragBook>>> CreateBragPost([FromForm] BragBook post, IFormFile? imageFile)
        {
            ModelState.Remove(nameof(BragBook.bragId));
            ModelState.Remove(nameof(BragBook.datePosted));
            ModelState.Remove(nameof(BragBook.imageUrl));

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new ApiResponse<BragBook> { Success = false, Message = $"Validation error: {errors}" });
            }

            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadedUrl = await _storageService.UploadFileAsync(imageFile, "bragbook");
                    post.imageUrl = uploadedUrl;
                }

                post.datePosted = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

                var newEntity = new BragBook
                {
                    content = post.content,
                    senderType = post.senderType,
                    recipientName = post.recipientName,
                    imageUrl = post.imageUrl,
                    datePosted = post.datePosted
                };

                _context.BragBook.Add(newEntity);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<BragBook> { Success = true, Message = "Brag post published successfully.", Data = newEntity });
            }
            catch (Exception ex)
            {
                var details = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, new ApiResponse<BragBook>
                {
                    Success = false,
                    Message = $"Database Exception: {details}"
                });
            }
        }

        // PUT: api/BragBook/5
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<BragBook>>> UpdateBragPost(int id, [FromForm] BragBook post, IFormFile? imageFile)
        {
            var existingPost = await _context.BragBook.FindAsync(id);
            if (existingPost == null)
            {
                return NotFound(new ApiResponse<BragBook> { Success = false, Message = "Brag post not found." });
            }

            existingPost.content = post.content;
            existingPost.senderType = post.senderType;
            existingPost.recipientName = post.recipientName;

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(existingPost.imageUrl))
                {
                    try
                    {
                        await _storageService.DeleteFileAsync(existingPost.imageUrl);
                    }
                    catch {  
                    }
                }

                string uploadedUrl = await _storageService.UploadFileAsync(imageFile, "bragbook");
                existingPost.imageUrl = uploadedUrl;
            }

            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<BragBook> { Success = true, Message = "Brag post updated successfully.", Data = existingPost });
        }

        // DELETE: api/BragBook/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteBragPost(int id)
        {
            var post = await _context.BragBook.FindAsync(id);
            if (post == null)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Message = "Brag post not found.", Data = false });
            }

            if (!string.IsNullOrWhiteSpace(post.imageUrl))
            {
                try
                {
                    await _storageService.DeleteFileAsync(post.imageUrl);
                }
                catch { 
                }
            }

            _context.BragBook.Remove(post);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<bool> { Success = true, Message = "Brag post deleted successfully.", Data = true });
        }
    }
}