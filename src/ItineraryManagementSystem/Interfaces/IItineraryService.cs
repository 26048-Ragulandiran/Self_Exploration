using ItineraryManagementSystem.Common;
using ItineraryManagementSystem.DTOs;
using ItineraryManagementSystem.Models;

namespace ItineraryManagementSystem.Interfaces
{
    public interface IItineraryService
    {
        Task<Response> GetAsync(ItineraryQueryParams query);
        Task<Response> GetByIdAsync(int id);
        Task<Response> CreateAsync(CreateItineraryDto dto);
        Task<Response> UpdateAsync(int id, UpdateItineraryDto dto);
        Task<Response> DeleteAsync(int id);
    }
}