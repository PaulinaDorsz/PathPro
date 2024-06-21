using System.ComponentModel.DataAnnotations;

namespace PathPro.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code must be at least 3 characters long.")]
        [MaxLength(10, ErrorMessage = "Code must be a maximum of 10 characters.")]
        public string Code { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Name must be a maximum of 20 characters")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
