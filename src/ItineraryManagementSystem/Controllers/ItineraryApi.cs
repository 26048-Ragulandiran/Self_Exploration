using ItineraryManagementSystem.DTOs;
using ItineraryManagementSystem.Interfaces;
using ItineraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItineraryManagementSystem.Controllers
{
    /// <summary>
    /// Provides Api's for itinerary application.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ItineraryApi : ControllerBase
    {
        private readonly IItineraryService _service;

        public ItineraryApi(IItineraryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetItineraries([FromQuery] ItineraryQueryParams query)
        {
            var result = await _service.GetAsync(query);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateItineraryDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateItineraryDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result.IsSuccess)
            {
                return result.Message == "ID mismatch"
                    ? BadRequest(result)
                    : NotFound(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}