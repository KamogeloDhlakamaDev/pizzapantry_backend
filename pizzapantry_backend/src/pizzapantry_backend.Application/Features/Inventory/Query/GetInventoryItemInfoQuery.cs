using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Domain.Response;

namespace pizzapantry_backend.Application.Features.Inventory.Query
{
    public record GetInventoryItemInfoQuery(string ItemId) :
        IRequest<Result<OnSuccess<InventoryInfoResponse>, OnError>>;


    public class InventoryInfoResponse
    {
        public BasicInventoryItemInfoDto? InventoryItem { get; set; }
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
        public List<RecentAdjustmentDto> RecentAdjustments { get; set; } = [];
    }

    public class RecentAdjustmentDto
    {
        public string Reason { get; set; } = string.Empty;
        public string DateMade { get; set; } = string.Empty;
        public int AdjustmentAmount { get; set; }
    }
}