using ItineraryManagementSystem.Interfaces;
using ItineraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

public class ItineraryRepository : IItineraryRepository
{
    private readonly ItineraryDbContext _itineraryDbContext;
    private readonly ILogger<ItineraryRepository> _logger;

    public ItineraryRepository(ItineraryDbContext context, ILogger<ItineraryRepository> logger)
    {
        _itineraryDbContext = context;
        _logger = logger;
    }

    public async Task<(IEnumerable<Itinerary>, int)> GetAsync(ItineraryQueryParams query)
    {
        _logger.LogInformation("Applying filters in repository");

        var itineraries = _itineraryDbContext.Itineraries.AsQueryable();

        if (!string.IsNullOrEmpty(query.Destination))
        {
            itineraries = itineraries.Where(x => x.Destination.Contains(query.Destination));
        }

        if (query.TravelDate.HasValue)
        {
            itineraries = itineraries.Where(x => x.TravelDate.Date == query.TravelDate.Value.Date);
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
        return await _itineraryDbContext.Itineraries.FindAsync(id);
    }

    public async Task<Itinerary> CreateAsync(Itinerary entity)
    {
        await _itineraryDbContext.Itineraries.AddAsync(entity);
        await _itineraryDbContext.SaveChangesAsync();

        _logger.LogInformation("Inserted itinerary with Id {Id}", entity.Id);

        return entity;
    }

    public async Task<bool> UpdateAsync(Itinerary entity)
    {
        _itineraryDbContext.Itineraries.Update(entity);
        var result = await _itineraryDbContext.SaveChangesAsync() > 0;

        _logger.LogInformation("Update operation status: {Result} for Id {Id}", result, entity.Id);

        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var data = await _itineraryDbContext.Itineraries.FindAsync(id);

        if (data == null)
        {
            _logger.LogWarning("Delete failed. Id {Id} not found", id);
            return false;
        }

        _itineraryDbContext.Itineraries.Remove(data);
        var result = await _itineraryDbContext.SaveChangesAsync() > 0;

        _logger.LogInformation("Delete operation status: {Result} for Id {Id}", result, id);

        return result;
    }
}