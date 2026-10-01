using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class Community
    {
        [Key]
        public int eventId { get; set; }

        [Required(ErrorMessage = "Event title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        [Display(Name = "Event Title")]
        public string title { get; set; }
        [Required(ErrorMessage = "Event description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Required(ErrorMessage = "Event category is required")]
        [Display(Name = "Category")]
        public EventCategory eventCategory { get; set; }

        [Required(ErrorMessage = "Date and time is required")]
        [Display(Name = "Date & Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy, HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime eventDateTime { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(150, ErrorMessage = "Location cannot exceed 150 characters")]
        [Display(Name = "Location / Venue")]
        public string location { get; set; }

        public enum EventCategory
        {
            Social,
            Wellness,
            TeamBuilding
        }
    }
}
