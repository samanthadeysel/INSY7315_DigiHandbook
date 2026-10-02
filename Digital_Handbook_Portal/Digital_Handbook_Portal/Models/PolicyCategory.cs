using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Digital_Handbook_Portal.Models
{
    public class PolicyCategory
    {
        [Key]
        public int categoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50)]
        [Display(Name = "Category Name")]
        public string categoryName { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Department / Subcategory")]
        public string? subCategory { get; set; }

        [JsonIgnore]
        public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();
    }
}