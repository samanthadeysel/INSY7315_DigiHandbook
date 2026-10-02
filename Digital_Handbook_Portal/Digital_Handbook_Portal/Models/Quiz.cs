using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Digital_Handbook_Portal.Models
{
    public class Quiz
    {
        [Key]
        public int quizId { get; set; }

        [Required(ErrorMessage = "Quiz title is required")]
        [StringLength(150, ErrorMessage = "Title cannot exceed 150 characters")]
        [Display(Name = "Quiz Title")]
        public string title { get; set; } = string.Empty;

        [Display(Name = "Total Points / Score")]
        public int score { get; set; } = 0;

        [Display(Name = "Total Questions")]
        public int totalQuestions { get; set; } = 0;

        [Required(ErrorMessage = "Estimated completion time is required")]
        [Display(Name = "Estimated Time")]
        public string estimateTime { get; set; } = string.Empty;

        [Display(Name = "Created At")]
        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Passing Score")]
        public int passingScore { get; set; } = 0;

        public virtual ICollection<QuizQuestion> questions { get; set; } = new List<QuizQuestion>();
    }

    public class QuizQuestion
    {
        [Key]
        public int questionId { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        [Display(Name = "Question")]
        public string questionText { get; set; } = string.Empty;

        public int quizId { get; set; }

        [JsonIgnore]
        public virtual Quiz? quiz { get; set; }

        public virtual ICollection<QuizOption> options { get; set; } = new List<QuizOption>();
    }

    public class QuizOption
    {
        [Key]
        public int optionId { get; set; }

        [Required(ErrorMessage = "Option text is required")]
        [Display(Name = "Option Text")]
        public string optionText { get; set; } = string.Empty;

        [Display(Name = "Is Correct Option")]
        public bool isCorrect { get; set; }

        public int questionId { get; set; }

        [JsonIgnore]
        public virtual QuizQuestion? question { get; set; }


    }
}