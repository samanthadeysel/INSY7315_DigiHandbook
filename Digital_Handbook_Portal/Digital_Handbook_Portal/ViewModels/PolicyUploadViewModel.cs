using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.ViewModels
{
    public class PolicyUploadViewModel
    {
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Policy title is required.")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content summary is required.")]
        public string ContentSummary { get; set; } = string.Empty;

        public string? SpecificCategory { get; set; }

        [Required(ErrorMessage = "Please select a policy category.")]
        public int CategoryId { get; set; }

        public string? ExistingFileUrl { get; set; }

        [Display(Name = "Upload Policy Document (PDF)")]
        public IFormFile? PdfFile { get; set; }
    }
}