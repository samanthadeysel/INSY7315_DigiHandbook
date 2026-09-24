using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class BragBook
    {
        [Key]
        public int bragId { get; set; }

        public string content { get; set; }
        public string senderType { get; set; }
        public string recipientName { get; set; }
        public DateTime datePosted { get; set; }
    }
}
