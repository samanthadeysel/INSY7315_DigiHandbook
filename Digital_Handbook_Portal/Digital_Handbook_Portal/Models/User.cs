using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Digital_Handbook_Portal.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        [Required(ErrorMessage = "Work email address is required")]
        [EmailAddress(ErrorMessage = "Please provide a valid email format")]
        [Display(Name = "Work Email")]
        public string email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A default password must be set")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [Display(Name = "Initial Password")]
        public string password { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();
    }

    public class UserSession
    {
        [Key]
        public int SessionId { get; set; }

        public int userId { get; set; }

        [ForeignKey("userId")]
        [JsonIgnore]
        public virtual User? User { get; set; }

        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double TotalDurationSeconds { get; set; }

        public virtual ICollection<FragmentVisit> Visits { get; set; } = new List<FragmentVisit>();
    }

    public class FragmentVisit
    {
        [Key]
        public int VisitId { get; set; }

        public int SessionId { get; set; }
        public string FragmentName { get; set; } = string.Empty;
        public DateTime EnteredAt { get; set; }
        public DateTime ExitedAt { get; set; }
        public double DurationSeconds { get; set; }
    }

    public class LoginRequest
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}