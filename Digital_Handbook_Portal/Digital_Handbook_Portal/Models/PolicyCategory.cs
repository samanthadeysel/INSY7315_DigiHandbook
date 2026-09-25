using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class PolicyCategory
    {
        [Key]
        public int categoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50)]
        [Display(Name = "Category Name")]
        public string categoryName { get; set; } = string.Empty; // e.g., "Nursing Policies", "HR Policies"

        [StringLength(50)]
        [Display(Name = "Department / Subcategory")]
        public string subCategory { get; set; } = string.Empty; // e.g., "Patient Care", "Theatre & Pre-Op"

        // 1:N Navigation: One category has many policies
        public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();
    }
}
