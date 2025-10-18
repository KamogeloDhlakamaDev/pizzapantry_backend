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
    public record GetInventoryItemInfoQuery(string ItemId) :
        IRequest<Result<OnSuccess<InventoryInfoResponse>, OnError>>;

    public class GetInventoryItemInfoQueryHandler :
        IRequestHandler<GetInventoryItemInfoQuery, Result<OnSuccess<InventoryInfoResponse>, OnError>>
    {
        private readonly ILogger _logger;
        private readonly IInventoryRepository _inventoryRepository;

        public GetInventoryItemInfoQueryHandler(
            ILogger logger,
            IInventoryRepository inventoryRepository
        )
        {
            _logger = logger;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Result<OnSuccess<InventoryInfoResponse>, OnError>> Handle(GetInventoryItemInfoQuery request, CancellationToken cancellationToken = default)
        {
            BasicInventoryItemInfoDto? basicInventoryItemInfoDto = await _inventoryRepository.GetInventoryItemInfo(request.ItemId);

            if (basicInventoryItemInfoDto is null)
            {
                return new OnError(HttpStatusCode.BadRequest, error: "Could not get basic inventory info");
            }

            return new OnSuccess<InventoryInfoResponse>
            {
                Response = new InventoryInfoResponse()
                {
                    InventoryItemInfo = basicInventoryItemInfoDto
                },
                StatusCode = HttpStatusCode.OK
            };
        }
    }

    public class GetInventoryItemInfoQueryValidation : AbstractValidator<GetInventoryItemInfoQuery>
    {
        public GetInventoryItemInfoQueryValidation()
        {
            RuleFor(x => x.ItemId)
                .NotEmpty()
                .WithMessage("Item cannot be empty.");
        }
    }
    public class InventoryInfoResponse
    {
        public BasicInventoryItemInfoDto? InventoryItemInfo { get; set; }
    }

    public class BasicInventoryItemInfoDto
    {
        public string ItemName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public required string ItemId { get; set; }
        public string Status { get; set; } = string.Empty;
        public int Quanity { get; set; }
        public string Location { get; set; } = string.Empty;
        public double SellingPrice { get; set; }
        public string Description { get; set; } = string.Empty;
        public int MinimumQuantity { get; set; }
        public List<RecentAdjustmentDto> RecentAdjustments { get; set; } = [];
    }

    public class RecentAdjustmentDto
    {
        public string Reason { get; set; } = string.Empty;
        public string DateMade { get; set; } = string.Empty;
        public int AdjustmentAmount { get; set; }
    }
}