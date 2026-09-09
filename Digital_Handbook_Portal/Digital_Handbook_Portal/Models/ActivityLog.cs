using System;
using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class ActivityLog
    {
        [Key]
        public int logId { get; set; }

        public string sessionId { get; set; } = string.Empty;

        public string pageVisited { get; set; } = string.Empty;

        public int durationInSeconds { get; set; }

        public DateTime timestampEntered { get; set; } = DateTime.UtcNow;
    }
}
