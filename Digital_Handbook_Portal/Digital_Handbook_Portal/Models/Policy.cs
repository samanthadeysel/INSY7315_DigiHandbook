using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital_Handbook_Portal.Models
{
    public class Policy
    {
        [Key]
        public int policyId { get; set; }

        [Required(ErrorMessage = "Policy Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Summary is required")]
        [StringLength(500, ErrorMessage = "Summary must be no more than 500 characters")]
        public string contentSummary { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Specific category must be no more than 50 characters")]
        public string? specificCategory { get; set; }

        [Display(Name = "Document URL")]
        public string? fileUrl { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        [Display(Name = "Category")]
        public int categoryId { get; set; }

        [ForeignKey("categoryId")]
        public virtual PolicyCategory? Category { get; set; }

        //for doctor link
        [Display(Name = "Linked Doctor (Optional)")]
        public int? doctorId { get; set; }
        public Doctor? Doctor { get; set; }
    }
}