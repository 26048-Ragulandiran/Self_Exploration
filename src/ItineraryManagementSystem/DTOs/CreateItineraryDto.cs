using System.ComponentModel.DataAnnotations;

namespace ItineraryManagementSystem.DTOs
{
    public class CreateItineraryDto
    {
        [Required(ErrorMessage = "Destination is required")]
        [StringLength(100, ErrorMessage = "Destination cannot exceed 100 characters")]
        public string Destination { get; set; } = string.Empty;

        [Required(ErrorMessage = "Travel date is required")]
        public DateTime TravelDate { get; set; }

        [Range(1, 30, ErrorMessage = "Duration must be between 1 and 30 days")]
        public int DurationDays { get; set; }
    }
}
