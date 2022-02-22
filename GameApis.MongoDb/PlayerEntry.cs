using MongoDB.Bson.Serialization.Attributes;

namespace GameApis.MongoDb;

public class PlayerEntry
{
    [BsonId]
    public Guid InternalId { get; set; }
    public Guid ExternalId { get; set; }
    public string Name { get; set; } = default!;
}
