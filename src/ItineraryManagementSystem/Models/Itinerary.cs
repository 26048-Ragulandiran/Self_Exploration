using System.ComponentModel.DataAnnotations;

namespace ItineraryManagementSystem.Models
{
    public class Itinerary
    {
        [Key]
        public int Id { get; set; }
        public string Destination { get; set; } = string.Empty;
        public DateTime TravelDate { get; set; }
        public int DurationDays { get; set; }
    }
}