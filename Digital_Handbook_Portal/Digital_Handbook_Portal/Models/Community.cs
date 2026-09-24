using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class Community
    {
        [Key]
        public int eventId { get; set; }

        public string title { get; set; }
        public string description { get; set; }
        public enum category
        {
            Social,
            Wellness,
            TeamBuilding
        }
        public DateTime eventDateTime { get; set; }
        public string location { get; set; }
    }
}
