using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class Policy
    {
        [Key]
        public int policyId { get; set; }

        [Required(ErrorMessage = "Policy Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Summary is required")]
        [StringLength(500, ErrorMessage = "Summary must be no more than 500 characters")]
        public string contentSummary { get; set; }

        [StringLength(50, ErrorMessage = "General category must be no more than 50 characters")]
        public string generalCategory { get; set; } //eg: Nursing policies, HR policies, etc

        [StringLength(50, ErrorMessage = "Specific category must be no more than 50 characters")]
        public string specificCategory { get; set; }  //eg: Nursing policies -> Patient care, HR policies -> Leave policies, etc
        public string fileUrl { get; set; }
    }
}
