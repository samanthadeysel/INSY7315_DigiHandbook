using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        public string email { get; set; }
        public string password { get; set; }
    }
}
