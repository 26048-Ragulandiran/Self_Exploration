using Microsoft.EntityFrameworkCore;

namespace ItineraryManagementSystem.Models
{
    public class ItineraryDbContext : DbContext
    {
        public ItineraryDbContext(DbContextOptions<ItineraryDbContext> options)
        : base(options)
        {
        }

        public DbSet<Itinerary> Itineraries { get; set; }
    }
}