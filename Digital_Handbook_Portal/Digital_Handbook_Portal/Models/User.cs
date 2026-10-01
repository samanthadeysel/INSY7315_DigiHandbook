using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }
        [Required(ErrorMessage = "Work email address is required")]
        [EmailAddress(ErrorMessage = "Please provide a valid email format")]
        [Display(Name = "Work Email")]
        public string email { get; set; }
        [Required(ErrorMessage = "A default password must be set")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [Display(Name = "Initial Password")]
        public string password { get; set; }
    }
}
