using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class BragBook
    {
        [Key]
        public int bragId { get; set; }

        [Required(ErrorMessage = "Praise content is required")]
        [StringLength(1000, ErrorMessage = "Content cannot exceed 1000 characters")]
        [Display(Name = "Praise / Shoutout Content")]
        public string content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sender type is required")]
        [Display(Name = "Sender Attribution")]
        public string senderType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Recipient name is required")]
        [Display(Name = "Recipient Name")]
        public string recipientName { get; set; } = string.Empty;

        [Display(Name = "Image URL")]
        public string? imageUrl { get; set; }

        [Display(Name = "Date Posted")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy, HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime datePosted { get; set; } = DateTime.UtcNow;
    }
}