using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pizzapantry_backend.Application.Features.AdjustItem.Query
{
    public record GetItemToAdjustInfoQuery(string ItemId);

    public class ItemToAdjustResponse
    {

        public ItemToAdjustDto? ItemToAdjust { get; set; }
    }

    public class ItemToAdjustDto
    {
        public required string ItemId { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
    }
}