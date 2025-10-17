using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Domain.Response;

namespace pizzapantry_backend.Application.Features.Inventory.Query
{
    public record GetInventoryItemsQuery:
        IRequest<Result<OnSuccess<InventoryItemsResponse>,OnError>>; 
        

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