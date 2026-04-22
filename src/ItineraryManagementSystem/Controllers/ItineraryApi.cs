using ItineraryManagementSystem.Common;
using ItineraryManagementSystem.DTOs;
using ItineraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItineraryManagementSystem.Controllers
{
    /// <summary>
    /// Provides Api's for itinerary application.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ItineraryApi : ControllerBase
    {
        private readonly ItineraryDbContext _itineraryDbContext;

        public ItineraryApi(ItineraryDbContext itineraryDbContext)
        {
            _itineraryDbContext = itineraryDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetItineraries([FromQuery] ItineraryQueryParams query)
        {
            var itineraries = _itineraryDbContext.Itineraries.AsQueryable();

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

            var data = await itineraries
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new ItineraryDto
                {
                    Id = x.Id,
                    Destination = x.Destination,
                    TravelDate = x.TravelDate,
                    DurationDays = x.DurationDays
                })
                .ToListAsync();

            if (data.Count == 0)
            {
                return NotFound(new Response
                {
                    IsSuccess = false,
                    Message = $"No itinerary found for the request.",
                    Query = query
                });
            }

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "Itineraries fetched successfully",
                Data = new
                {
                    totalCount,
                    totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize),
                    pageNumber = query.PageNumber,
                    pageSize = query.PageSize,
                    items = data
                }
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _itineraryDbContext.Itineraries.FindAsync(id);

            if (data == null)
            {
                return NotFound(new Response
                {
                    IsSuccess = false,
                    Message = $"Itinerary {id} not found"
                });
            }

            var dto = new ItineraryDto
            {
                Id = data.Id,
                Destination = data.Destination,
                TravelDate = data.TravelDate,
                DurationDays = data.DurationDays
            };

            return Ok(new Response
            {
                IsSuccess = true,
                Data = dto
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateItineraryDto dto)
        {
            var entity = new Itinerary
            {
                Destination = dto.Destination,
                TravelDate = dto.TravelDate,
                DurationDays = dto.DurationDays
            };

            await _itineraryDbContext.Itineraries.AddAsync(entity);
            await _itineraryDbContext.SaveChangesAsync();

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "Created successfully",
                Data = entity
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateItineraryDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "ID mismatch"
                });
            }

            var existing = await _itineraryDbContext.Itineraries.FindAsync(id);

            if (existing == null)
            {
                return NotFound(new Response
                {
                    IsSuccess = false,
                    Message = $"Itinerary {id} not found"
                });
            }

            existing.Destination = dto.Destination;
            existing.TravelDate = dto.TravelDate;
            existing.DurationDays = dto.DurationDays;

            await _itineraryDbContext.SaveChangesAsync();

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "Updated successfully",
                Data = existing
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _itineraryDbContext.Itineraries.FindAsync(id);

            if (data == null)
            {
                return NotFound(new Response
                {
                    IsSuccess = false,
                    Message = $"Itinerary {id} not found"
                });
            }

            _itineraryDbContext.Itineraries.Remove(data);
            await _itineraryDbContext.SaveChangesAsync();

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "Deleted successfully"
            });
        }
    }
}