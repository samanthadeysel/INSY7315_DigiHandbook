using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Digital_Handbook_Portal.Models
{
    public class Doctor
    {
        [Key]
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("fName")]
        public string FName { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("lName")]
        public string LName { get; set; } = string.Empty;

        [JsonPropertyName("specialty")]
        public string Specialty { get; set; } = "General Practitioner";

        [Required]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("suiteNumber")]
        public string SuiteNumber { get; set; } = string.Empty;

        [JsonPropertyName("imageUrl")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("fullNameWithTitle")]
        public string FullNameWithTitle => $"Dr. {FName} {LName}".Trim();
    }
}