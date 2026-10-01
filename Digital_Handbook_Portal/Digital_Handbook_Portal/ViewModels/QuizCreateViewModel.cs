using Digital_Handbook_Portal.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.ViewModels
{
    public class QuizCreateViewModel
    {
        public int QuizId { get; set; }

        [Required(ErrorMessage = "Quiz title is required.")]
        public string Title { get; set; } = string.Empty;

        [Range(0, 1000, ErrorMessage = "Score must be a positive number.")]
        public int Score { get; set; } = 100;

        [Required(ErrorMessage = "Estimated completion time is required.")]
        public string EstimateTime { get; set; } = "15 mins";

        [Range(0, 100, ErrorMessage = "Passing score percentage must be between 0 and 100.")]
        public int PassingScore { get; set; } = 80;

        public List<QuizQuestionViewModel> Questions { get; set; } = new List<QuizQuestionViewModel>();
    }

    public class QuizQuestionViewModel
    {
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Question text is required.")]
        public string QuestionText { get; set; } = string.Empty;

        public List<QuizOptionViewModel> Options { get; set; } = new List<QuizOptionViewModel>();
    }

    public class QuizOptionViewModel
    {
        public int OptionId { get; set; }

        [Required(ErrorMessage = "Option text is required.")]
        public string OptionText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}