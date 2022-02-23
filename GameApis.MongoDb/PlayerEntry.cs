using GameApis.Shared.Players;
using MongoDB.Bson.Serialization.Attributes;

namespace GameApis.MongoDb;

public class PlayerEntry
{
    [BsonId]
    public InternalPlayerId InternalId { get; set; }
    public ExternalPlayerId ExternalId { get; set; }
    public string Name { get; set; } = default!;
}
