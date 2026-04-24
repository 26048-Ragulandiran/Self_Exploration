using ItineraryManagementSystem.Common;
using ItineraryManagementSystem.DTOs;
using ItineraryManagementSystem.Interfaces;
using ItineraryManagementSystem.Models;

namespace ItineraryManagementSystem.Services
{
    public class ItineraryService : IItineraryService
    {
        private readonly IItineraryRepository _itineraryRepository;
        private readonly ILogger<ItineraryService> _logger;

        public ItineraryService(IItineraryRepository itineraryRepository, ILogger<ItineraryService> logger)
        {
            _itineraryRepository = itineraryRepository;
            _logger = logger;
        }

        public async Task<Response> GetAsync(ItineraryQueryParams query)
        {
            _logger.LogInformation("Fetching itineraries with query {@Query}", query);

            var (items, totalCount) = await _itineraryRepository.GetAsync(query);

            if (!items.Any())
            {
                _logger.LogWarning("No itineraries found for query {@Query}", query);

                return new Response
                {
                    IsSuccess = false,
                    Message = "No itinerary found",
                    Query = query
                };
            }

            var data = items.Select(x => new ItineraryDto
            {
                Id = x.Id,
                Destination = x.Destination,
                TravelDate = x.TravelDate,
                DurationDays = x.DurationDays
            });

            return new Response
            {
                IsSuccess = true,
                Message = "Fetched successfully",
                Data = new
                {
                    totalCount,
                    totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize),
                    query.PageNumber,
                    query.PageSize,
                    items = data
                }
            };
        }

        public async Task<Response> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching itinerary by Id {Id}", id);

            var data = await _itineraryRepository.GetByIdAsync(id);

            if (data == null)
            {
                _logger.LogWarning("Itinerary not found for Id {Id}", id);

                return new Response
                {
                    IsSuccess = false,
                    Message = $"Itinerary {id} not found"
                };
            }

            return new Response
            {
                IsSuccess = true,
                Data = new ItineraryDto
                {
                    Id = data.Id,
                    Destination = data.Destination,
                    TravelDate = data.TravelDate,
                    DurationDays = data.DurationDays
                }
            };
        }

        public async Task<Response> CreateAsync(CreateItineraryDto dto)
        {
            _logger.LogInformation("Creating itinerary for {Destination}", dto.Destination);

            var entity = new Itinerary
            {
                Destination = dto.Destination,
                TravelDate = dto.TravelDate,
                DurationDays = dto.DurationDays
            };

            var created = await _itineraryRepository.CreateAsync(entity);

            _logger.LogInformation("Itinerary created with Id {Id}", created.Id);

            return new Response
            {
                IsSuccess = true,
                Message = "Created successfully",
                Data = created
            };
        }

        public async Task<Response> UpdateAsync(int id, UpdateItineraryDto dto)
        {
            _logger.LogInformation("Updating itinerary Id {Id}", id);

            if (id != dto.Id)
            {
                _logger.LogWarning("ID mismatch for update. RouteId={Id}, BodyId={BodyId}", id, dto.Id);

                return new Response
                {
                    IsSuccess = false,
                    Message = "ID mismatch"
                };
            }

            var existing = await _itineraryRepository.GetByIdAsync(id);

            if (existing == null)
            {
                _logger.LogWarning("Update failed. Itinerary not found for Id {Id}", id);

                return new Response
                {
                    IsSuccess = false,
                    Message = $"Itinerary {id} not found"
                };
            }

            existing.Destination = dto.Destination;
            existing.TravelDate = dto.TravelDate;
            existing.DurationDays = dto.DurationDays;

            await _itineraryRepository.UpdateAsync(existing);

            _logger.LogInformation("Itinerary updated successfully for Id {Id}", id);

            return new Response
            {
                IsSuccess = true,
                Message = "Updated successfully"
            };
        }

        public async Task<Response> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting itinerary Id {Id}", id);

            var success = await _itineraryRepository.DeleteAsync(id);

            if (!success)
            {
                _logger.LogWarning("Delete failed. Itinerary not found for Id {Id}", id);

                return new Response
                {
                    IsSuccess = false,
                    Message = $"Itinerary {id} not found"
                };
            }

            _logger.LogInformation("Itinerary deleted successfully for Id {Id}", id);

            return new Response
            {
                IsSuccess = true,
                Message = "Deleted successfully"
            };
        }
    }
}