using System.ComponentModel.DataAnnotations;

namespace Digital_Handbook_Portal.Models
{
    public class Policy
    {
        [Key]
        public int policyId { get; set; }

        public string Title { get; set; }
        public string contentSummary { get; set; }
        public string fileUrl { get; set; }
    }
}
