using System.ComponentModel.DataAnnotations;

namespace ItineraryManagementSystem.DTOs
{
    public class UpdateItineraryDto
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Destination is required")]
        [StringLength(100, ErrorMessage = "Destination max length is 100")]
        public string Destination { get; set; } = string.Empty;

        [Required(ErrorMessage = "Travel date is required")]
        public DateTime TravelDate { get; set; }

        [Range(1, 30, ErrorMessage = "Duration must be between 1 and 30 days")]
        public int DurationDays { get; set; }
    }
}
