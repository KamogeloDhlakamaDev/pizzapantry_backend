namespace Domain.Entities.Mongo
{
    public class MongoDatabaseSettings : IMongoDatabaseSettings
    {
        public required string InventoryCollectionName { get; set; }
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string AdjustedHistoryCollectionName { get; set; }

    }

    public interface IMongoDatabaseSettings
    {
        string InventoryCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string AdjustedHistoryCollectionName { get; set; }

    }
}
