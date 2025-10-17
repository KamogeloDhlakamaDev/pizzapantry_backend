using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pizzapantry_backend.Application.Features.Inventory.Query;
using pizzapantry_backend.Domain.Mongo;

namespace pizzapantry_backend.Application.Features.Inventory.Repository
{
    public interface IInventoryRepository
    {
        Task<bool> CreateItem(Item createItem);
        Task<bool> UpdateItem(Item createItem);
        Task<Item?> GetInventoryItem(string itemId);
        Task<DetailedInventoryItemInfoDto?> GetDetailedInventoryInfo(string itemId);
        Task<List<ItemsDto>> GetInventoryItems();
        Task<BasicInventoryItemInfoDto?> GetInventoryItemInfo(string ItemId);

        Task<bool> RemoveItemStock(string ItemId);
    }
}