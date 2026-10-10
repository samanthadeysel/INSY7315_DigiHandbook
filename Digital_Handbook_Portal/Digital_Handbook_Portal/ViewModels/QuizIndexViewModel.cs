using Digital_Handbook_Portal.Models;

namespace Digital_Handbook_Portal.ViewModels
{
    public class QuizIndexViewModel
    {
        public class QuizIndexViewModel
        {
            public IEnumerable<Quiz> Quizzes { get; set; } = new List<Quiz>();
            public IEnumerable<QuizResultDisplayItem> QuizResults { get; set; } = new List<QuizResultDisplayItem>();
            public IEnumerable<string> DistinctUsers { get; set; } = new List<string>();
        }

        public class QuizResultDisplayItem
        {
            public int Id { get; set; }
            public string UserId { get; set; } = string.Empty;
            public string StaffName { get; set; } = string.Empty;
            public int QuizId { get; set; }
            public string QuizTitle { get; set; } = string.Empty;
            public int ScorePercentage { get; set; }
            public int CpdPointsAwarded { get; set; }
            public bool IsPassed { get; set; }
            public System.DateTime CompletedAt { get; set; }
        }
    }
}
