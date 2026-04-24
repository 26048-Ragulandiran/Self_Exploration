using Grpc.Core;
using ItineraryManagementSystem.DTOs;
using ItineraryManagementSystem.Interfaces;
using ItineraryManagementSystem.Models;

namespace ItineraryManagementSystem.Grpc
{
    public class ItineraryGrpcService : ItineraryGrpc.ItineraryGrpcBase
    {
        private readonly IItineraryService _service;
        private readonly ILogger<ItineraryGrpcService> _logger;

        public ItineraryGrpcService(IItineraryService service, ILogger<ItineraryGrpcService> logger)
        {
            _service = service;
            _logger = logger;
        }

        public override async Task<GetAllResponse> GetAll(
            GetAllRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("gRPC GetAll called");

            var query = new ItineraryQueryParams
            {
                Destination = request.Destination,
                PageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber,
                PageSize = request.PageSize <= 0 ? 10 : request.PageSize,
                TravelDate = string.IsNullOrEmpty(request.TravelDate)
                    ? null
                    : DateTime.Parse(request.TravelDate)
            };

            var result = await _service.GetAsync(query);

            if (!result.IsSuccess || result.Data == null)
            {
                return new GetAllResponse
                {
                    TotalCount = 0
                };
            }

            var data = result.Data;

            var response = new GetAllResponse
            {
                TotalCount = data.TotalCount
            };

            response.Items.AddRange(data.Items.Select(x => new ItineraryResponse
            {
                Id = x.Id,
                Destination = x.Destination,
                TravelDate = x.TravelDate.ToString("yyyy-MM-dd"),
                DurationDays = x.DurationDays
            }));

            return response;
        }

        public override async Task<ItineraryResponse> GetById(GetByIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("gRPC GetById {Id}", request.Id);

            var result = await _service.GetByIdAsync(request.Id);

            if (!result.IsSuccess || result.Data == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, result.Message));
            }

            var dto = result.Data;

            return new ItineraryResponse
            {
                Id = dto.Id,
                Destination = dto.Destination,
                TravelDate = dto.TravelDate.ToString("yyyy-MM-dd"),
                DurationDays = dto.DurationDays
            };
        }

        public override async Task<ItineraryResponse> Create(CreateRequest request, ServerCallContext context)
        {
            _logger.LogInformation("gRPC Create {Destination}", request.Destination);

            var dto = new CreateItineraryDto
            {
                Destination = request.Destination,
                TravelDate = DateTime.Parse(request.TravelDate),
                DurationDays = request.DurationDays
            };

            var result = await _service.CreateAsync(dto);

            if (!result.IsSuccess || result.Data == null)
            {
                throw new RpcException(new Status(StatusCode.Internal, result.Message));
            }

            var entity = result.Data;

            return new ItineraryResponse
            {
                Id = entity.Id,
                Destination = entity.Destination,
                TravelDate = entity.TravelDate.ToString("yyyy-MM-dd"),
                DurationDays = entity.DurationDays
            };
        }

        public override async Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
        {
            _logger.LogInformation("gRPC Update {Id}", request.Id);

            var dto = new UpdateItineraryDto
            {
                Id = request.Id,
                Destination = request.Destination,
                TravelDate = DateTime.Parse(request.TravelDate),
                DurationDays = request.DurationDays
            };

            var result = await _service.UpdateAsync(request.Id, dto);

            return new UpdateResponse
            {
                Success = result.IsSuccess,
                Message = result.Message
            };
        }

        public override async Task<DeleteResponse> Delete(DeleteRequest request, ServerCallContext context)
        {
            _logger.LogInformation("gRPC Delete {Id}", request.Id);

            var result = await _service.DeleteAsync(request.Id);

            return new DeleteResponse
            {
                Success = result.IsSuccess,
                Message = result.Message
            };
        }
    }
}