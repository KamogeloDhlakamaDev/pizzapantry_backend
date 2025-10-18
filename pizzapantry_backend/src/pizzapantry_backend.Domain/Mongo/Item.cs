using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pizzapantry_backend.Domain.Mongo
{
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ItemId { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int CurrentQuanity { get; set; }
        public int MinimumQuantity { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public double SellingPrice { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}