using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Handbook_Portal.Models
{
    public class QuizResult
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [Required]
        public int QuizId { get; set; }

        [ForeignKey("QuizId")]
        public virtual Quiz? Quiz { get; set; }

        [Display(Name = "Score (%)")]
        public int ScorePercentage { get; set; }

        [Display(Name = "CPD Points Awarded")]
        public int CpdPointsAwarded { get; set; }

        [Display(Name = "Passed")]
        public bool IsPassed { get; set; }

        [Display(Name = "Completed At")]
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    }
}
