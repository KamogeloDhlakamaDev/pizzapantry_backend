using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Domain.Response;
using pizzapantry_backend.Application.Features.Inventory.Repository;

namespace pizzapantry_backend.Application.Features.Inventory.Query
{
    public record GetInventoryItemsQuery :
        IRequest<Result<OnSuccess<InventoryItemsResponse>, OnError>>;

    public class GetInventoryItemsQueryHandler :
        IRequestHandler<GetInventoryItemsQuery, Result<OnSuccess<InventoryItemsResponse>, OnError>>
    {
        private readonly ILogger _logger;
        private readonly IInventoryRepository _inventoryRepository;

        public GetInventoryItemsQueryHandler(
            ILogger logger,
            IInventoryRepository inventoryRepository
        )
        {
            _logger = logger;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Result<OnSuccess<InventoryItemsResponse>, OnError>> Handle(GetInventoryItemsQuery request, CancellationToken cancellationToken = default)
        {
            try
            {
                List<ItemsDto> itemsDtos = await _inventoryRepository.GetInventoryItems();
                return new OnSuccess<InventoryItemsResponse>
                {
                    StatusCode = HttpStatusCode.OK,
                    Response = new InventoryItemsResponse()
                    {
                        Items = itemsDtos
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return new OnError(HttpStatusCode.BadRequest, error: "Could not get the inventory items.");
            }
        }
    }

    public class GetInventoryItemsQueryValidation : AbstractValidator<GetInventoryItemsQuery>
    {
        public GetInventoryItemsQueryValidation()
        {

        }
    }


    public class InventoryItemsResponse
    {
        public List<ItemsDto> Items { get; set; } = [];
    }

    public class ItemsDto
    {
        public string ItemName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public required string ItemId { get; set; }
        public string Status { get; set; } = string.Empty;
        public int Quanity { get; set; }
        public string Location { get; set; } = string.Empty;
        public double SellingPrice { get; set; }
    }
}