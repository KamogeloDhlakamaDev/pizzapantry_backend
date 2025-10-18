using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using pizzapantry_backend.Application.Features.Inventory.Query;
using pizzapantry_backend.Application.Features.Inventory.Repository;
using pizzapantry_backend.Domain.Mongo;
using Serilog;

namespace pizzapantry_backend.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        public readonly MongoClient _client;
        public readonly IMongoDatabase database;
        public IMongoDatabaseSettings _settings;
        private readonly IMongoCollection<Item> _itemCollection;
        private readonly IMongoCollection<AdjustmentHistory> _adjustmentCollection;


        public InventoryRepository(IMongoDatabaseSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            database = _client.GetDatabase(settings.DatabaseName);
            _settings = settings;
            _itemCollection = database.GetCollection<Item>(settings.InventoryCollectionName);
            _adjustmentCollection = database.GetCollection<AdjustmentHistory>(settings.AdjustedHistoryCollectionName);
        }

        public async Task<bool> CreateItem(Item createItem)
        {
            try
            {
                await _itemCollection.InsertOneAsync(createItem);
                return true;
            }
            catch (MongoWriteException ex)
            {
                Log.Error($"Mongo write error: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> UpdateItem(Item updatedItem)
        {
            try
            {
                var filter = Builders<Item>.Filter.Eq(i => i.ItemId, updatedItem.ItemId);
                var update = Builders<Item>.Update
                    .Set(i => i.ItemName, updatedItem.ItemName)
                    .Set(i => i.SKU, updatedItem.SKU)
                    .Set(i => i.Category, updatedItem.Category)
                    .Set(i => i.CurrentQuanity, updatedItem.CurrentQuanity)
                    .Set(i => i.MinimumQuantity, updatedItem.MinimumQuantity)
                    .Set(i => i.Location, updatedItem.Location)
                    .Set(i => i.Description, updatedItem.Description)
                    .Set(i => i.SellingPrice, updatedItem.SellingPrice);

                var result = await _itemCollection.UpdateOneAsync(filter, update);

                return result.MatchedCount > 0;
            }
            catch (Exception ex)
            {
                Log.Error($"Mongo update error: {ex.Message}");
                return false;
            }
        }



        public async Task<Item?> GetInventoryItem(string itemId)
        {
            try
            {
                Item? item = await _itemCollection
                    .Find(i => i.ItemId.ToString() == itemId)
                    .FirstOrDefaultAsync();

                if (item is null)
                {
                    return null;
                }

                return item;
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving item {itemId}: {ex.Message}");
                return null;
            }
        }

        public async Task<List<ItemsDto>> GetInventoryItems()
        {
            try
            {
                var items = await _itemCollection.Find(_ => true).ToListAsync();

                var itemsOrderd = items.OrderByDescending(x => x.CreatedOn);

                var itemsDto = itemsOrderd.Select(i => new ItemsDto
                {
                    ItemId = i.ItemId.ToString(),
                    ItemName = i.ItemName,
                    SKU = i.SKU,
                    Category = i.Category,
                    Quanity = i.CurrentQuanity,
                    Location = i.Location,
                    SellingPrice = i.SellingPrice,
                    Status = GetStockStatus(i.CurrentQuanity, i.MinimumQuantity)
                }).ToList();

                return itemsDto;
            }
            catch (Exception ex)
            {
                Log.Error($"Error fetching inventory items: {ex.Message}");
                return new List<ItemsDto>();
            }
        }
        private string GetStockStatus(int current, int minimum)
        {
            if (current <= minimum / 2) return "Critical";
            if (current < minimum) return "Low Stock";
            return "Good";
        }

        public async Task<BasicInventoryItemInfoDto?> GetInventoryItemInfo(string itemId)
        {

            try
            {
                var item = await _itemCollection
                    .Find(i => i.ItemId.ToString() == itemId)
                    .FirstOrDefaultAsync();

                if (item == null)
                    return null;

                var adjustments = await _adjustmentCollection
                    .Find(a => a.ItemId == itemId)
                    .SortByDescending(a => a.CreatedOn)
                    .ToListAsync();

                var recentAdjustments = adjustments.Select(a => new RecentAdjustmentDto
                {
                    Reason = a.Reason ?? string.Empty,
                    AdjustmentAmount = a.Quantity,
                    DateMade = a.CreatedOn.ToString("MMM dd, yyyy 'at' h:mm tt")
                }).ToList();

                var dto = new BasicInventoryItemInfoDto
                {
                    ItemId = item.ItemId.ToString(),
                    ItemName = item.ItemName,
                    SKU = item.SKU,
                    Category = item.Category,
                    Quanity = item.CurrentQuanity,
                    Location = item.Location,
                    SellingPrice = item.SellingPrice,
                    Description = item.Description,
                    Status = GetStockStatus(item.CurrentQuanity, item.MinimumQuantity),
                    RecentAdjustments = recentAdjustments,
                    MinimumQuantity = item.MinimumQuantity
                };

                return dto;
            }
            catch (Exception ex)
            {
                Log.Error($"Error fetching item info: {ex.Message}");
                return null;
            }
        }

        public async Task<DetailedInventoryItemInfoDto?> GetDetailedInventoryInfo(string itemId)
        {
            try
            {
                var item = await _itemCollection
                    .Find(i => i.ItemId.ToString() == itemId)
                    .FirstOrDefaultAsync();

                if (item == null)
                    return null;

                var itemInfo = new DetailedInventoryItemInfoDto
                {
                    ItemId = item.ItemId.ToString(),
                    ItemName = item.ItemName,
                    SKU = item.SKU,
                    Category = item.Category,
                    Quanity = item.CurrentQuanity,
                    Location = item.Location,
                    SellingPrice = item.SellingPrice,
                    Description = item.Description,
                    MinimumQuantity = item.MinimumQuantity,
                    Status = GetStockStatus(item.CurrentQuanity, item.MinimumQuantity)
                };

                return itemInfo;
            }
            catch (Exception ex)
            {
                Log.Error($"Error fetching detailed inventory info: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> RemoveItemStock(string itemId)
        {
            try
            {
                var filter = Builders<Item>.Filter.Eq(i => i.ItemId, itemId);

                var update = Builders<Item>.Update.Set(i => i.CurrentQuanity, 0);

                var result = await _itemCollection.UpdateOneAsync(filter, update);

                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Log.Error($"Error removing item stock: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteItem(string itemId)
        {

            try
            {
                
                var filter = Builders<Item>.Filter.Eq(i => i.ItemId, itemId);

                var result = await _itemCollection.DeleteOneAsync(filter);


                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting item {itemId}: {ex.Message}");
                return false;
            }
        }

    }
}