using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class Doctor
    {
        public int doctorId { get; set; }
        public string doctorImg { get; set; } = string.Empty;
        [Required(ErrorMessage = "First Name is required")]
        public string fName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string lName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string email { get; set; }
        [Required(ErrorMessage = "Contact number is required")]
        public string phone { get; set; }
        [Required(ErrorMessage = "Suite number is required")]
        public int suiteNumber { get; set; }
    }
}
