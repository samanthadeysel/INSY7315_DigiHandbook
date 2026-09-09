namespace HandbookApi.Models
{
    public class TelemetryLogRequest
    {
        public string SessionId { get; set; } = string.Empty;
        public string PageVisited { get; set; } = string.Empty;
        public int DurationInSeconds { get; set; }
    }
}
