using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Mongo;
using MongoDB.Driver;

namespace pizzapantry_backend.Infrastructure.Services
{
    public class InventoryDbService : IInventoryDBService
    {
        public readonly MongoClient _client;
        public readonly IMongoDatabase database;
        public IMongoDatabaseSettings _settings;
        // private readonly IMongoCollection<FormDetails> _formCollection;
        // private readonly IMongoCollection<Submissions> _formDataCollection;

        public InventoryDbService(IMongoDatabaseSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            database = _client.GetDatabase(settings.DatabaseName);
            _settings = settings;

        }

    }
}