using ItineraryManagementSystem.Models;

namespace ItineraryManagementSystem.Interfaces
{
    public interface IItineraryRepository
    {
        Task<Itinerary> CreateAsync(Itinerary entity);
        Task<bool> DeleteAsync(int id);
        Task<(IEnumerable<Itinerary>, int)> GetAsync(ItineraryQueryParams query);
        Task<Itinerary?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Itinerary entity);
    }
}