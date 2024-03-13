using MongoDB.Bson.Serialization.Attributes;

namespace GameApis.MongoDb;

public class MongoEntry<TGameContext>
{
    [BsonId] public Guid Id { get; set; }

    public TGameContext GameContext { get; set; } = default!;

    public string CurrentState { get; set; } = default!;
}