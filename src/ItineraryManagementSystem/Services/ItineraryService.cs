using ItineraryManagementSystem.Common;
using ItineraryManagementSystem.DTOs;
using ItineraryManagementSystem.Interfaces;
using ItineraryManagementSystem.Models;

namespace ItineraryManagementSystem.Services
{
    public class ItineraryService : IItineraryService
    {
        private readonly IItineraryRepository _itineraryRepository;

        public ItineraryService(IItineraryRepository itineraryRepository)
        {
            _itineraryRepository = itineraryRepository;
        }

        public async Task<Response> GetAsync(ItineraryQueryParams query)
        {
            var (items, totalCount) = await _itineraryRepository.GetAsync(query);

            if (!items.Any())
            {
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
            var data = await _itineraryRepository.GetByIdAsync(id);

            if (data == null)
            {
                return new Response
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

            return new Response
            {
                IsSuccess = true,
                Data = dto
            };
        }

        public async Task<Response> CreateAsync(CreateItineraryDto dto)
        {
            var entity = new Itinerary
            {
                Destination = dto.Destination,
                TravelDate = dto.TravelDate,
                DurationDays = dto.DurationDays
            };

            var created = await _itineraryRepository.CreateAsync(entity);

            return new Response
            {
                IsSuccess = true,
                Message = "Created successfully",
                Data = created
            };
        }

        public async Task<Response> UpdateAsync(int id, UpdateItineraryDto dto)
        {
            if (id != dto.Id)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "ID mismatch"
                };
            }

            var existing = await _itineraryRepository.GetByIdAsync(id);

            if (existing == null)
            {
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

            return new Response
            {
                IsSuccess = true,
                Message = "Updated successfully"
            };
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var success = await _itineraryRepository.DeleteAsync(id);

            if (!success)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = $"Itinerary {id} not found"
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Deleted successfully"
            };
        }
    }
}
