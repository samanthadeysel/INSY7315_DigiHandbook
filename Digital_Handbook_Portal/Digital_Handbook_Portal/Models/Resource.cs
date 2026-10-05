using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Handbook_Portal.Models
{
    [Table("Resource")]
    public class Resource
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Resource title is required")]
        [StringLength(150, ErrorMessage = "Title cannot exceed 150 characters")]
        [Display(Name = "Resource Title")]
        [Column("Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        [Display(Name = "Category")]
        [Column("Category")]
        public string Category { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        [Column("Description")]
        public string? Description { get; set; }

        [Display(Name = "Resource Link / URL")]
        [Column("ResourceUrl")]
        public string? ResourceUrl { get; set; }

        [Display(Name = "Breadcrumb Path")]
        [Column("BreadcrumbPath")]
        public string? BreadcrumbPath { get; set; }
    }
}