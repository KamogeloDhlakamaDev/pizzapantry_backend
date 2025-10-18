using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Domain.Response;
using pizzapantry_backend.Application.Features.Inventory.Repository;

namespace pizzapantry_backend.Application.Features.Inventory.Query
{
    public record GetInventoryItemDetailedInfoQuery(string itemId) :
        IRequest<Result<OnSuccess<InventoryItemDetailInfoResponse>, OnError>>;

    public sealed class GetInventoryItemDetailedInfoQueryHandler :
        IRequestHandler<GetInventoryItemDetailedInfoQuery, Result<OnSuccess<InventoryItemDetailInfoResponse>, OnError>>
    {
        private readonly ILogger _logger;
        private readonly IInventoryRepository _inventoryRepository;

        public GetInventoryItemDetailedInfoQueryHandler(
            ILogger logger,
            IInventoryRepository inventoryRepository
        )
        {
            _inventoryRepository = inventoryRepository;
            _logger = logger;
        }

        public async Task<Result<OnSuccess<InventoryItemDetailInfoResponse>, OnError>> Handle(GetInventoryItemDetailedInfoQuery request, CancellationToken cancellationToken = default)
        {
            try
            {
                DetailedInventoryItemInfoDto? detailedInventoryItemInfoDto = await _inventoryRepository.GetDetailedInventoryInfo(request.itemId);

                if (detailedInventoryItemInfoDto is null)
                {
                    return new OnError(HttpStatusCode.BadRequest, error: "Could not find detailed inventory item info");
                }

                return new OnSuccess<InventoryItemDetailInfoResponse>
                {
                    Response = new InventoryItemDetailInfoResponse()
                    {
                        InventoryDetailInfo = detailedInventoryItemInfoDto
                    },
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new OnError(HttpStatusCode.InternalServerError, error: "Could not inventory info.");
            }
        }
    }
    public class GetInventoryItemDetailedInfoQueryValidation : AbstractValidator<GetInventoryItemDetailedInfoQuery>
    {
        public GetInventoryItemDetailedInfoQueryValidation()
        {
            RuleFor(x => x.itemId)
                .NotEmpty()
                .WithMessage("Item Id cannot be empty ");
        }
    }
    public class DetailedInventoryItemInfoDto
    {
        public string ItemName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public required string ItemId { get; set; }
        public string Status { get; set; } = string.Empty;
        public int Quanity { get; set; }
        public int MinimumQuantity { get; set; }
        public string Location { get; set; } = string.Empty;
        public double SellingPrice { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class InventoryItemDetailInfoResponse
    {
        public DetailedInventoryItemInfoDto? InventoryDetailInfo { get; set; }
    }
}