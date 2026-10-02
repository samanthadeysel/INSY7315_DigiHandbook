using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class Resource
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Resource title is required")]
        [StringLength(150, ErrorMessage = "Title cannot exceed 150 characters")]
        [Display(Name = "Resource Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Resource URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        [Display(Name = "Resource Link / URL")]
        public string ResourceUrl { get; set; } = string.Empty;

        [Display(Name = "Breadcrumb Path")]
        public string BreadcrumbPath { get; set; } = string.Empty;
    }
}