using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class Resource
    {
        [Key]
        public string Title { get; set; }

        public string resourceType { get; set; }
        public string linkUrl { get; set; }
    }
}
