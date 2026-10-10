namespace Digital_Handbook_Portal.ViewModels
{
    public class UserCpdSummaryViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalCpdPoints { get; set; }
        public int TotalQuizzesAttempted { get; set; }
        public int TotalQuizzesPassed { get; set; }
    }
}
