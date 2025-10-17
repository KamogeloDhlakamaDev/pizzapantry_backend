using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using pizzapantry_backend.Application.Features.AdjustItem.Query;
using pizzapantry_backend.Application.Features.AdjustItem.Repository;
using pizzapantry_backend.Domain.Mongo;
using Serilog;

namespace pizzapantry_backend.Infrastructure.Repositories
{
    public class AdjustItemRepository : IAdjustItemRespositry
    {
        public readonly MongoClient _client;
        public readonly IMongoDatabase database;
        public IMongoDatabaseSettings _settings;
        private readonly IMongoCollection<Item> _itemCollection;
        private readonly IMongoCollection<AdjustmentHistory> _adjustmentCollection;

        public AdjustItemRepository(IMongoDatabaseSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            database = _client.GetDatabase(settings.DatabaseName);
            _settings = settings;
            _itemCollection = database.GetCollection<Item>(settings.InventoryCollectionName);
            _adjustmentCollection = database.GetCollection<AdjustmentHistory>(settings.AdjustedHistoryCollectionName);
        }

        public async Task<bool> AdjustItemQuanty(AdjustmentHistory adjustItem)
        {
            try
            {
                await _adjustmentCollection.InsertOneAsync(adjustItem);
                return true;
            }
            catch (MongoWriteException ex)
            {
                Log.Error($"Mongo write error: {ex.Message}");
                return false;
            }
        }

        public async Task<ItemToAdjustDto?> GetItemToAdjust(string itemId)
        {

            try
            {
                var item = await _itemCollection
                    .Find(i => i.ItemId == ObjectId.Parse(itemId))
                    .FirstOrDefaultAsync();

                if (item == null)
                    return null;

                var dto = new ItemToAdjustDto
                {
                    ItemId = item.ItemId.ToString(),
                    SKU = item.SKU,
                    ItemName = item.ItemName,
                    CurrentStock = item.CurrentQuanity,
                    MinimumStock = item.MinimumQuantity
                };

                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching item to adjust: {ex.Message}");
                return null;
            }
        }
    }
}