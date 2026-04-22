using ItineraryManagementSystem.Interfaces;
using ItineraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

public class ItineraryRepository : IItineraryRepository
{
    private readonly ItineraryDbContext _context;

    public ItineraryRepository(ItineraryDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Itinerary>, int)> GetAsync(ItineraryQueryParams query)
    {
        var itineraries = _context.Itineraries.AsQueryable();

        if (!string.IsNullOrEmpty(query.Destination))
        {
            itineraries = itineraries
                .Where(x => x.Destination.Contains(query.Destination));
        }

        if (query.TravelDate.HasValue)
        {
            itineraries = itineraries
                .Where(x => x.TravelDate.Date == query.TravelDate.Value.Date);
        }

        var totalCount = await itineraries.CountAsync();

        var items = await itineraries
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Itinerary?> GetByIdAsync(int id)
    {
        return await _context.Itineraries.FindAsync(id);
    }

    public async Task<Itinerary> CreateAsync(Itinerary entity)
    {
        await _context.Itineraries.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Itinerary entity)
    {
        _context.Itineraries.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var data = await _context.Itineraries.FindAsync(id);
        if (data == null) return false;

        _context.Itineraries.Remove(data);
        return await _context.SaveChangesAsync() > 0;
    }
}