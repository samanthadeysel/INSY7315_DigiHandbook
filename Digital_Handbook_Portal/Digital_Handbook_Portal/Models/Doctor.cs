using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class Doctor
    {
        [Key]
        public int doctorId { get; set; }

        [Display(Name = "Doctor Image URL")]
        public string? doctorImg { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string fName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string lName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Phone Number")]
        public string phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Suite number is required")]
        [Range(1, 9999, ErrorMessage = "Suite number must be a positive integer")]
        [Display(Name = "Suite Number")]
        public int suiteNumber { get; set; }

        [Display(Name = "Doctor Name")]
        public string FullName => $"Dr. {fName} {lName}";
    }
}