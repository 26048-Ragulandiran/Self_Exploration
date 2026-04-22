namespace ItineraryManagementSystem.Models
{
    public class ItineraryQueryParams
    {
        public string? Destination { get; set; }
        public DateTime? TravelDate { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
