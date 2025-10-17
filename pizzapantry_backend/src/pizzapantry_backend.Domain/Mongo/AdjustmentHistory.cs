using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pizzapantry_backend.Domain.Mongo
{
    public class AdjustmentHistory
    {
        [BsonId]
        public ObjectId AdjustmentId { get; set; }
        public required string ItemId { get; set; }
        public int Quanity { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}