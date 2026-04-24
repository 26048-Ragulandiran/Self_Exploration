using ItineraryManagementSystem.Common;
using ItineraryManagementSystem.DTOs;
using ItineraryManagementSystem.Models;

namespace ItineraryManagementSystem.Interfaces
{
    public interface IItineraryService
    {
        Task<Response<ItineraryDto>> CreateAsync(CreateItineraryDto dto);
        Task<Response<bool>> DeleteAsync(int id);
        Task<Response<PagedResult<ItineraryDto>>> GetAsync(ItineraryQueryParams query);
        Task<Response<ItineraryDto>> GetByIdAsync(int id);
        Task<Response<bool>> UpdateAsync(int id, UpdateItineraryDto dto);
    }
}