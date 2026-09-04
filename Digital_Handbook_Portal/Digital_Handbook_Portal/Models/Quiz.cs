namespace Digital_Handbook_Portal.Models
{
    public class Quiz
    {
        public int quizId { get; set; }
        public string title { get; set; }
        public string questions { get; set; }
        public string answers { get; set; } = string.Empty;
        public int score { get; set; } = 0;
        public int totalQuestions { get; set; } = 0;
        public string estimateTime { get; set; }
    }
}
