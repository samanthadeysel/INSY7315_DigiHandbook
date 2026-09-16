using Microsoft.AspNetCore.Mvc;
using Digital_Handbook_Portal;
using Digital_Handbook_Portal.Models;
using HandbookApi.Models;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/telemetry")]
    public class TelemetryController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public TelemetryController(Digital_Handbook_PortalContext context) => _context = context;

        [HttpPost("log")]
        public async Task<IActionResult> LogActivity([FromBody] TelemetryLogRequest request)
        {
            var log = new ActivityLog
            {
                sessionId = request.SessionId,
                pageVisited = request.PageVisited,
                durationInSeconds = request.DurationInSeconds,
                timestampEntered = DateTime.UtcNow
            };

            _context.ActivityLog.Add(log);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Telemetry metric cached to remote storage layers." });
        }
    }
}
