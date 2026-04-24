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

        public async Task<Response<PagedResult<ItineraryDto>>> GetAsync(ItineraryQueryParams query)
        {
            _logger.LogInformation("Fetching itineraries with query {@Query}", query);

            var (items, totalCount) = await _itineraryRepository.GetAsync(query);

            var dtoItems = items.Select(item => new ItineraryDto
            {
                Id = item.Id,
                Destination = item.Destination,
                TravelDate = item.TravelDate,
                DurationDays = item.DurationDays
            });

            var result = new PagedResult<ItineraryDto>
            {
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                Items = dtoItems
            };

            if (!dtoItems.Any())
            {
                _logger.LogWarning("No itineraries found for query {@Query}", query);

                return new Response<PagedResult<ItineraryDto>>
                {
                    IsSuccess = false,
                    Message = "No itinerary found",
                    Data = result
                };
            }

            return new Response<PagedResult<ItineraryDto>>
            {
                IsSuccess = true,
                Message = "Fetched successfully",
                Data = result
            };
        }

        public async Task<Response<ItineraryDto>> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching itinerary by Id {Id}", id);

            var data = await _itineraryRepository.GetByIdAsync(id);

            if (data is null)
            {
                _logger.LogWarning("Itinerary not found for Id {Id}", id);

                return new Response<ItineraryDto>
                {
                    IsSuccess = false,
                    Message = $"Itinerary {id} not found"
                };
            }

            var dto = new ItineraryDto
            {
                Id = data.Id,
                Destination = data.Destination,
                TravelDate = data.TravelDate,
                DurationDays = data.DurationDays
            };

            return new Response<ItineraryDto>
            {
                IsSuccess = true,
                Data = dto
            };
        }

        public async Task<Response<ItineraryDto>> CreateAsync(CreateItineraryDto dto)
        {
            _logger.LogInformation("Creating itinerary for {Destination}", dto.Destination);

            var entity = new Itinerary
            {
                Destination = dto.Destination,
                TravelDate = dto.TravelDate,
                DurationDays = dto.DurationDays
            };

            var created = await _itineraryRepository.CreateAsync(entity);

            var resultDto = new ItineraryDto
            {
                Id = created.Id,
                Destination = created.Destination,
                TravelDate = created.TravelDate,
                DurationDays = created.DurationDays
            };

            _logger.LogInformation("Itinerary created with Id {Id}", created.Id);

            return new Response<ItineraryDto>
            {
                IsSuccess = true,
                Message = "Created successfully",
                Data = resultDto
            };
        }

        public async Task<Response<bool>> UpdateAsync(int id, UpdateItineraryDto dto)
        {
            _logger.LogInformation("Updating itinerary Id {Id}", id);

            if (id != dto.Id)
            {
                _logger.LogWarning("ID mismatch RouteId={RouteId}, BodyId={BodyId}", id, dto.Id);

                return new Response<bool>
                {
                    IsSuccess = false,
                    Message = "ID mismatch",
                    Data = false
                };
            }

            var existing = await _itineraryRepository.GetByIdAsync(id);

            if (existing is null)
            {
                _logger.LogWarning("Update failed. Itinerary not found for Id {Id}", id);

                return new Response<bool>
                {
                    IsSuccess = false,
                    Message = $"Itinerary {id} not found",
                    Data = false
                };
            }

            existing.Destination = dto.Destination;
            existing.TravelDate = dto.TravelDate;
            existing.DurationDays = dto.DurationDays;

            var updated = await _itineraryRepository.UpdateAsync(existing);

            _logger.LogInformation("Update status {Status} for Id {Id}", updated, id);

            return new Response<bool>
            {
                IsSuccess = updated,
                Message = updated ? "Updated successfully" : "Update failed",
                Data = updated
            };
        }

        public async Task<Response<bool>> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting itinerary Id {Id}", id);

            var success = await _itineraryRepository.DeleteAsync(id);

            if (!success)
            {
                _logger.LogWarning("Delete failed for Id {Id}", id);

                return new Response<bool>
                {
                    IsSuccess = false,
                    Message = $"Itinerary {id} not found",
                    Data = false
                };
            }

            _logger.LogInformation("Itinerary deleted successfully for Id {Id}", id);

            return new Response<bool>
            {
                IsSuccess = true,
                Message = "Deleted successfully",
                Data = true
            };
        }
    }
}