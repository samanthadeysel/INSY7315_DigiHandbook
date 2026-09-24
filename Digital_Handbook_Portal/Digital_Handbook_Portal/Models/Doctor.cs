using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class Doctor
    {
        [Key]
        public int doctorId { get; set; }
        public string doctorImg { get; set; } = string.Empty;
        [Required(ErrorMessage = "First Name is required")]
        public string fName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last Name is required")]
        public string lName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required")]
        public string email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Contact number is required")]
        public string phone { get; set; } = string.Empty;
        [Required(ErrorMessage = "Suite number is required")]
        public int suiteNumber { get; set; }
    }
}
