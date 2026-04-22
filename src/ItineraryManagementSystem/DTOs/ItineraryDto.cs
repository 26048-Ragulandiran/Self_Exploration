namespace ItineraryManagementSystem.DTOs
{
    public class ItineraryDto
    {
        public int Id { get; set; }
        public string Destination { get; set; } = string.Empty;
        public DateTime TravelDate { get; set; }
        public int DurationDays { get; set; }
    }
}
