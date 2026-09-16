using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class Quiz
    {
        [Key]
        public int quizId { get; set; }

        public string title { get; set; }
        public int score { get; set; } = 0;
        public int totalQuestions { get; set; } = 0;
        public string estimateTime { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        public int passingScore { get; set; } = 0;
        //nav property because a quiz has many questions
        public virtual ICollection<QuizQuestion> questions { get; set; } = new List<QuizQuestion>();
    }

    public class QuizQuestion
    {
        [Key]
        public int questionId { get; set; }
        public string questionText { get; set; }
        //foreign key to quiz
        public int quizId { get; set; }
        //nav property because a question belongs to a quiz
        public virtual Quiz quiz { get; set; }

        //nav property because a question has many options
        public virtual ICollection<QuizOption> options { get; set; } = new List<QuizOption>();
    }

    public class QuizOption
    {
        [Key]
        public int optionId { get; set; }
        public string optionText { get; set; }
        public bool isCorrect { get; set; }
        //foreign key to question
        public int questionId { get; set; }
        //nav property because an option belongs to a question
        public virtual QuizQuestion question { get; set; }
    }
}
